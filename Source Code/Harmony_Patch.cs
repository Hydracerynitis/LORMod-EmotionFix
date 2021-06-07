using System;
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
        public static Difficulty diff;
        public static string modPath;
        public static bool Hastrigger;
        public static List<BattleUnitModel> enemylist;
        public static List<EmotionCardXmlInfo> emotion1;
        public static List<EmotionCardXmlInfo> emotion2;
        public static List<EmotionCardXmlInfo> emotion3;
        public static List<EmotionCardXmlInfo> enermy;
        public Harmony_Patch()
        {
            Harmony harmony = new Harmony("Hydracerynitis.EmotionFix");
            modPath = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
            Debug.ModPatchDebug();
            EmotionCardAbility_bossbird4.Summation = new List<BattleDiceCardModel>();
            EmotionCardAbility_clownofnihil2.Clown = new List<UnitBattleDataModel>();
            EmotionCardAbility_plaguedoctor1.WhiteNightClock = new Dictionary<UnitBattleDataModel, int>();
            PassiveAbility_668.LevelUped = new List<UnitBattleDataModel>();
            enemylist = new List<BattleUnitModel>();
            MethodInfo method1 = typeof(Harmony_Patch).GetMethod("EmotionCardXmlList_GetEnemyEmotionNeutralCardList");
            MethodInfo method2 = typeof(EmotionCardXmlList).GetMethod("GetEnemyEmotionNeutralCardList", AccessTools.all);
            try
            {
                HarmonyMethod postfix1 = new HarmonyMethod(method1);
                harmony.Patch((MethodBase)method2, postfix: postfix1);
                Debug.Log("Patch " + method1.Name + " Succeed");
            }
            catch(Exception ex)
            {
                Debug.Error("HP_" + method1.Name, ex);
            }
            MethodInfo method3 = typeof(Harmony_Patch).GetMethod("StageController_StartBattle");
            MethodInfo method4 = typeof(StageController).GetMethod("StartBattle", AccessTools.all);
            try
            {
                HarmonyMethod postfix2 = new HarmonyMethod(method3);
                harmony.Patch((MethodBase)method4, postfix: postfix2);
                Debug.Log("Patch " + method3.Name + " Succeed");
            }
            catch(Exception ex)
            {
                Debug.Error("HP_" + method3.Name, ex);
            }
            MethodInfo method5 = typeof(Harmony_Patch).GetMethod("StageController_EndBattlePhase");
            MethodInfo method6 = typeof(StageController).GetMethod("EndBattlePhase", AccessTools.all);
            try
            {
                HarmonyMethod postfix3 = new HarmonyMethod(method5);
                harmony.Patch((MethodBase)method6, postfix: postfix3);
                Debug.Log("Patch " + method5.Name + " Succeed");
            }
            catch(Exception ex)
            {
                Debug.Error("HP_" + method5.Name, ex);
            }
            MethodInfo method7 = typeof(Harmony_Patch).GetMethod("StageController_GameOver");
            MethodInfo method8 = typeof(StageController).GetMethod("GameOver", AccessTools.all);
            try
            {
                HarmonyMethod postfix4 = new HarmonyMethod(method7);
                harmony.Patch((MethodBase)method8, postfix: postfix4);
                Debug.Log("Patch " + method7.Name + " Succeed");
            }
            catch(Exception ex)
            {
                Debug.Error("HP_" + method7.Name, ex);
            }
            MethodInfo method9 = typeof(Harmony_Patch).GetMethod("Decay_OnRoundEnd");
            MethodInfo method10 = typeof(BattleUnitBuf_Decay).GetMethod("OnRoundEnd", AccessTools.all);
            try
            {
                HarmonyMethod prefix1 = new HarmonyMethod(method9);
                harmony.Patch((MethodBase)method10, prefix: prefix1);
                Debug.Log("Patch " + method9.Name + " Succeed");
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + method9.Name, ex);
            }
            MethodInfo method11 = typeof(Harmony_Patch).GetMethod("BattleDiceBehavior_UpdateDiceFinalValue");
            MethodInfo method12 = typeof(BattleDiceBehavior).GetMethod("UpdateDiceFinalValue", AccessTools.all);
            try
            {
                HarmonyMethod prefix2 = new HarmonyMethod(method11);
                harmony.Patch((MethodBase)method12, prefix: prefix2);
                Debug.Log("Patch " + method11.Name + " Succeed");
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + method11.Name, ex);
            }
            MethodInfo method13 = typeof(Harmony_Patch).GetMethod("BattleUnitBuf_Alriune_Debuf_OnRoundEndTheLast");
            MethodInfo method14 = typeof(BattleUnitBuf_Alriune_Debuf).GetMethod("OnRoundEndTheLast", AccessTools.all);
            try
            {
                HarmonyMethod prefix3 = new HarmonyMethod(method13);
                harmony.Patch((MethodBase)method14, prefix: prefix3);
                Debug.Log("Patch " + method13.Name + " Succeed");
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + method13.Name, ex);
            }//RoundStartPhase_System
            MethodInfo method15 = typeof(Harmony_Patch).GetMethod("StageController_RoundStartPhase_System");
            MethodInfo method16 = typeof(StageController).GetMethod("RoundStartPhase_System", AccessTools.all);
            try
            {
                HarmonyMethod postfix5 = new HarmonyMethod(method15);
                harmony.Patch((MethodBase)method16, postfix: postfix5);
                Debug.Log("Patch " + method15.Name + " Succeed");
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + method15.Name, ex);
            }

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
                enemylist.Clear();
                Hastrigger = false;
                diff = DifficultyTweak();
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
                {
                    if (EmotionCardAbility_clownofnihil2.Clown.Contains(alive.UnitData))
                        alive.bufListDetail.AddBuf(new EmotionCardAbility_clownofnihil2.Clear());
                }
            }
        }
        public static void StageController_EndBattlePhase()
        {
            EmotionCardAbility_bossbird4.ClearCard();
        }
        public static void StageController_GameOver()
        {
            PassiveAbility_668.LevelUped.Clear();
            EmotionCardAbility_clownofnihil2.Clown.Clear();
            EmotionCardAbility_plaguedoctor1.WhiteNightClock.Clear();
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
        public static bool BattleUnitBuf_Alriune_Debuf_OnRoundEndTheLast(BattleUnitBuf_Alriune_Debuf __instance, ref int ___reserve, BattleUnitModel ____owner)
        {
            ____owner.TakeBreakDamage(__instance.stack, DamageType.Buf);
            __instance.stack= __instance.stack * 2 / 3;
            __instance.stack += ___reserve;
            ___reserve = 0;
            if (__instance.stack > 0)
                return false;
            __instance.Destroy();
            return false;
        }
        public static void StageController_RoundStartPhase_System()
        {
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
            {
                if (alive.faction == Faction.Enemy)
                {
                    if (ExcludedEnemyID.Contains(alive.UnitData.unitData.EnemyUnitId))
                        continue;
                    if (ExcludedBookID.Contains(alive.Book.GetBookClassInfoId()))
                        continue;
                    if (enemylist.Contains(alive))
                        continue;
                    AssignPassive(alive);
                }
            }
        }
        public static Difficulty DifficultyTweak()
        {
            Difficulty Dif = Difficulty.Normal;
            Debug.PathDebug("/Difficulty.txt", PathType.File);
            Debug.Log("Difficulty input found");
            foreach(string str in File.ReadAllLines(modPath + "/Difficulty.txt"))
            {
                string text = str.Trim();
                if (!text.StartsWith("Choose Your Difficulty (Casual, Normal, Hard, Brutal):"))
                    continue;
                int i = text.IndexOf("Choose Your Difficulty (Casual, Normal, Hard, Brutal):");
                text = text.Remove(i, "Choose Your Difficulty (Casual, Normal, Hard, Brutal):".Length);
                if (text.Contains("Casual"))
                {
                    Dif = Difficulty.Easy;
                    break;
                }
                if (text.Contains("Normal"))
                {
                    Dif = Difficulty.Normal;
                    break;
                }
                if (text.Contains("Hard"))
                {
                    Dif = Difficulty.Hard;
                    break;
                }
                if (text.Contains("Brutal"))
                {
                    Dif = Difficulty.Brutal;
                    break;
                }
            }
            Debug.Log("Your Difficulty is " + Dif.ToString());
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
        public static void AssignPassive(BattleUnitModel unit)
        {
            List<PassiveAbilityBase> passiveList = unit.passiveDetail.PassiveList;
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
            switch (diff)
            {
                case (Difficulty.Easy):
                    break;
                case (Difficulty.Normal):
                    passiveList.Add(new PassiveAbility_666(unit, trigger));
                    typeof(BattleUnitPassiveDetail).GetField("_passiveList", AccessTools.all).SetValue(unit.passiveDetail, passiveList);
                    break;
                case (Difficulty.Hard):
                    passiveList.Add(new PassiveAbility_667(unit, trigger));
                    typeof(BattleUnitPassiveDetail).GetField("_passiveList", AccessTools.all).SetValue(unit.passiveDetail, passiveList);
                    break;
                case (Difficulty.Brutal):
                    passiveList.Add(new PassiveAbility_668(unit, trigger));
                    typeof(BattleUnitPassiveDetail).GetField("_passiveList", AccessTools.all).SetValue(unit.passiveDetail, passiveList);
                    break;
            }
            enemylist.Add(unit);
            Debug.Log("Passive is Added to " + unit.UnitData.unitData.name);
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
