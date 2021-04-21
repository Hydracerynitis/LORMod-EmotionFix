﻿using System;
using System.Collections.Generic;
using Sound;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using LOR_DiceSystem;
using UnityEngine;

namespace EmotionalFix
{
    public class Harmony_Patch
    {
        public static bool Hastrigger;
        public static List<EmotionCardXmlInfo> emotion1;
        public static List<EmotionCardXmlInfo> emotion2;
        public static List<EmotionCardXmlInfo> emotion3;
        public static List<EmotionCardXmlInfo> enermy;
        public Harmony_Patch()
        {
            Harmony harmony = new Harmony("Hydracerynitis.EmotionFix");
            MethodInfo method1 = typeof(Harmony_Patch).GetMethod("EmotionCardXmlList_GetEnemyEmotionNeutralCardList");
            MethodInfo method2 = typeof(EmotionCardXmlList).GetMethod("GetEnemyEmotionNeutralCardList", AccessTools.all);
            HarmonyMethod postfix1 = new HarmonyMethod(method1);
            harmony.Patch((MethodBase)method2, postfix: postfix1);
            MethodInfo method3 = typeof(Harmony_Patch).GetMethod("StageController_StartBattle");
            MethodInfo method4 = typeof(StageController).GetMethod("StartBattle", AccessTools.all);
            HarmonyMethod postfix2 = new HarmonyMethod(method3);
            harmony.Patch((MethodBase)method4, postfix: postfix2);
            MethodInfo method5 = typeof(Harmony_Patch).GetMethod("StageController_EndBattlePhase");
            MethodInfo method6 = typeof(StageController).GetMethod("EndBattlePhase", AccessTools.all);
            HarmonyMethod postfix3 = new HarmonyMethod(method5);
            harmony.Patch((MethodBase)method6, postfix: postfix3);
            MethodInfo method7 = typeof(Harmony_Patch).GetMethod("StageController_GameOver");
            MethodInfo method8 = typeof(StageController).GetMethod("GameOver", AccessTools.all);
            HarmonyMethod postfix4 = new HarmonyMethod(method7);
            harmony.Patch((MethodBase)method8, postfix: postfix4);
            //MethodInfo method9 = typeof(Harmony_Patch).GetMethod("BattleDiceBehavior_get_DiceMin_Pre");
            //MethodInfo method10 = typeof(BattleDiceBehavior).GetMethod("get_DiceMin", AccessTools.all);
            //HarmonyMethod prefix1 = new HarmonyMethod(method9);
            //harmony.Patch((MethodBase)method10, prefix: prefix1);
            MethodInfo method11 = typeof(Harmony_Patch).GetMethod("Decay_OnRoundEnd");
            MethodInfo method12 = typeof(BattleUnitBuf_Decay).GetMethod("OnRoundEnd", AccessTools.all);
            HarmonyMethod prefix2 = new HarmonyMethod(method11);
            harmony.Patch((MethodBase)method12, prefix: prefix2);
            MethodInfo method13 = typeof(Harmony_Patch).GetMethod("BattleDiceBehavior_UpdateDiceFinalValue");
            MethodInfo method14 = typeof(BattleDiceBehavior).GetMethod("UpdateDiceFinalValue", AccessTools.all);
            HarmonyMethod prefix3 = new HarmonyMethod(method13);
            harmony.Patch((MethodBase)method14, prefix: prefix3);
            EmotionCardAbility_bossbird4.Summation = new List<BattleDiceCardModel>();
            EmotionCardAbility_bossbird7.Change = new List<BattleDiceCardModel>();
            EmotionCardAbility_clownofnihil2.Clown = new List<UnitBattleDataModel>();
            EmotionCardAbility_plaguedoctor1.WhiteNightClock=new Dictionary<UnitBattleDataModel, int>();
            PassiveAbility_668.Leveluped = new List<UnitBattleDataModel>();
        }
        public static void EmotionCardXmlList_GetEnemyEmotionNeutralCardList(ref List<EmotionCardXmlInfo> __result)
        {
            __result.Remove(Singleton<EmotionCardXmlList>.Instance.GetData(1, SephirahType.None));
            __result.Remove(Singleton<EmotionCardXmlList>.Instance.GetData(4, SephirahType.None));
        }
        public static void StageController_StartBattle(StageType ____stageType)
        {
            if (____stageType == StageType.Invitation)
            {
                emotion1 = LoadEmotion(1);
                emotion2 = LoadEmotion(2);
                emotion3 = LoadEmotion(3);
                emotion1.Remove(Singleton<EmotionCardXmlList>.Instance.GetData(11, SephirahType.Hokma));
                emotion2.Remove(Singleton<EmotionCardXmlList>.Instance.GetData(12, SephirahType.Hokma));
                emotion3.Remove(Singleton<EmotionCardXmlList>.Instance.GetData(15, SephirahType.Hokma));
                enermy = Singleton<EmotionCardXmlList>.Instance.GetDataList_enemy(SephirahType.None);
                Hastrigger = false;
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
                {
                    if (alive.faction == Faction.Enemy)
                    {
                        if (ExcludedEnemyID.Contains(alive.UnitData.unitData.EnemyUnitId))
                            continue;
                        if (ExcludedBookID.Contains(alive.Book.GetBookClassInfoId()))
                            continue;
                        List<PassiveAbilityBase> passiveList = alive.passiveDetail.PassiveList;
                        foreach (PassiveAbilityBase passive in passiveList)
                        {
                            if (ExcluededPassive.Contains(passive.GetType().ToString()))
                                continue;
                        }
                        bool trigger = false;
                        if (!Hastrigger)
                        {
                            trigger = RandomUtil.valueForProb <= 0.01;
                            if (trigger)
                                Hastrigger = true;
                        }
                        switch (DifficultyTweak())
                        {
                            case (Difficulty.Easy):
                                break;
                            case (Difficulty.Normal):
                                passiveList.Add(new PassiveAbility_666(alive, trigger));
                                typeof(BattleUnitPassiveDetail).GetField("_passiveList", AccessTools.all).SetValue(alive.passiveDetail, passiveList);
                                break;
                            case (Difficulty.Hard):
                                passiveList.Add(new PassiveAbility_667(alive, trigger));
                                typeof(BattleUnitPassiveDetail).GetField("_passiveList", AccessTools.all).SetValue(alive.passiveDetail, passiveList);
                                break;
                            case (Difficulty.Brutal):
                                passiveList.Add(new PassiveAbility_668(alive, trigger));
                                typeof(BattleUnitPassiveDetail).GetField("_passiveList", AccessTools.all).SetValue(alive.passiveDetail, passiveList);
                                break;
                        }
                    }
                    if (EmotionCardAbility_clownofnihil2.Clown.Contains(alive.UnitData))
                        alive.bufListDetail.AddBuf(new EmotionCardAbility_clownofnihil2.Clear());
                }
            }
        }
        public static void StageController_EndBattlePhase()
        {
            EmotionCardAbility_bossbird4.ClearCard();
            EmotionCardAbility_bossbird7.ClearCard();
        }
        public static void StageController_GameOver()
        {
            EmotionCardAbility_clownofnihil2.Clown.Clear();
            EmotionCardAbility_plaguedoctor1.WhiteNightClock.Clear();
            PassiveAbility_668.Leveluped.Clear();
            Hastrigger = false;
        }
        public static bool Decay_OnRoundEnd(BattleUnitBuf_Decay __instance,BattleUnitModel ____owner,ref int ___reserve)
        {
            if (!____owner.IsImmune(KeywordBuf.Decay))
            {
                ____owner.TakeDamage(__instance.stack);
                SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Angry_Decay");
            }
            if (__instance.stack > 0)
                __instance.stack=0;
            __instance.stack += ___reserve;
            ___reserve = 0;
            if (__instance.stack <= 0)
                 __instance.Destroy();
            return false;
        }
        public static bool BattleDiceBehavior_UpdateDiceFinalValue(int ____diceResultValue,ref int ____diceFinalResultValue)
        {
            ____diceFinalResultValue = Mathf.Max(1, ____diceResultValue);
            return true;
        }
        public static Difficulty DifficultyTweak()
        {
            Difficulty Dif = Difficulty.Normal;
            foreach(string str in File.ReadAllLines(Application.dataPath + "/BaseMods/EmotionFIx/Mod介绍.txt"))
            {
                string text = str.Trim();
                if (!text.StartsWith("请选择你的难度"))
                    continue;
                if (text.Contains("至福乐土"))
                {
                    Dif = Difficulty.Easy;
                    break;
                }
                if (text.Contains("水仙花平原"))
                {
                    Dif = Difficulty.Normal;
                    break;
                }
                if (text.Contains("塔耳塔罗斯"))
                {
                    Dif = Difficulty.Hard;
                    break;
                }
                if (text.Contains("冥王神殿"))
                {
                    Dif = Difficulty.Brutal;
                    break;
                }
            }
            return Dif;
        }
        public enum Difficulty
        {
            Easy,
            Normal,
            Hard,
            Brutal
        }
        public static List<EmotionCardXmlInfo> LoadEmotion(int emotionlevel)
        {
            List<EmotionCardXmlInfo> list=new List<EmotionCardXmlInfo>();
            list.AddRange(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.Malkuth, LibraryModel.Instance.GetFloor(SephirahType.Malkuth).Level, emotionlevel));
            list.AddRange(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.Yesod, LibraryModel.Instance.GetFloor(SephirahType.Yesod ).Level, emotionlevel));
            list.AddRange(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.Hod , LibraryModel.Instance.GetFloor(SephirahType.Hod ).Level, emotionlevel));
            list.AddRange(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.Netzach , LibraryModel.Instance.GetFloor(SephirahType.Netzach).Level, emotionlevel));
            list.AddRange(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.Tiphereth , LibraryModel.Instance.GetFloor(SephirahType.Tiphereth ).Level, emotionlevel));
            list.AddRange(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.Gebura , LibraryModel.Instance.GetFloor(SephirahType.Gebura ).Level, emotionlevel));
            list.AddRange(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.Chesed , LibraryModel.Instance.GetFloor(SephirahType.Chesed ).Level, emotionlevel));
            list.AddRange(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.Binah , LibraryModel.Instance.GetFloor(SephirahType.Binah ).Level, emotionlevel));
            list.AddRange(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.Hokma , LibraryModel.Instance.GetFloor(SephirahType.Hokma ).Level, emotionlevel));
            list.AddRange(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.Keter , LibraryModel.Instance.GetFloor(SephirahType.Keter ).Level, emotionlevel));
            return list;
        }
        public static List<int> ExcludedBookID => new List<int>() 
        { 
            2739666, 2739006, //安眠
            43510001, 43510002, 43510003, 43510004, 43511005, //黑鸦
            27925201, 27925202, 27925203, //秘法师1
            27925214,27925215,27925216, //秘法师2
            27925205,27925206,27925207,27925208,27925209, //秘法师3
        };
        public static List<int> ExcludedEnemyID => new List<int>()
        {
            61017371 //克隆
        };
        public static List<string> ExcluededPassive => new List<string>() 
        {
            "PassiveAbility_2731154" //安眠   
        };       
    }
}