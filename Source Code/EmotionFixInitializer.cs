using System;
using System.Collections.Generic;
using Sound;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using LOR_DiceSystem;
using UnityEngine;
using EmotionalFix.Hokma;

namespace EmotionalFix
{
    public class EmotionFixInitializer: ModInitializer
    {
        public static Difficulty diff;
        public static string modPath;
        public static bool WhiteNightTrigger;
        public static bool ClownTrigger;
        public static bool GiftTrigger;
        public static List<BattleUnitModel> enemylist;
        public static List<EmotionCardXmlInfo> emotion1;
        public static List<EmotionCardXmlInfo> emotion2;
        public static List<EmotionCardXmlInfo> emotion3;
        public static List<EmotionCardXmlInfo> enermy;
        public override void OnInitializeMod()
        {
            Harmony harmony = new Harmony("Hydracerynitis.EmotionFix");
            modPath = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
            enemylist = new List<BattleUnitModel>();
            MethodInfo method1 = typeof(EmotionFixInitializer).GetMethod("EmotionCardXmlList_GetEnemyEmotionNeutralCardList");
            MethodInfo method2 = typeof(EmotionCardXmlList).GetMethod("GetEnemyEmotionNeutralCardList", AccessTools.all);
            try
            {
                HarmonyMethod postfix1 = new HarmonyMethod(method1);
                harmony.Patch((MethodBase)method2, postfix: postfix1);
                Debug.Log("Patch " + method1.Name + " Succeed");
            }
            catch
            {

            }
            MethodInfo method3 = typeof(EmotionFixInitializer).GetMethod("StageController_StartBattle");
            MethodInfo method4 = typeof(StageController).GetMethod("StartBattle", AccessTools.all);
            try
            {
                HarmonyMethod postfix2 = new HarmonyMethod(method3);
                harmony.Patch((MethodBase)method4, postfix: postfix2);
                Debug.Log("Patch " + method3.Name + " Succeed");
            }
            catch
            {

            }
           
            MethodInfo method7 = typeof(EmotionFixInitializer).GetMethod("StageController_GameOver");
            MethodInfo method8 = typeof(StageController).GetMethod("GameOver", AccessTools.all);
            try
            {
                HarmonyMethod postfix4 = new HarmonyMethod(method7);
                harmony.Patch((MethodBase)method8, postfix: postfix4);
                Debug.Log("Patch " + method7.Name + " Succeed");
            }
            catch
            {

            }
            //RoundStartPhase_System
            MethodInfo method15 = typeof(EmotionFixInitializer).GetMethod("StageController_RoundStartPhase_System");
            MethodInfo method16 = typeof(StageController).GetMethod("RoundStartPhase_System", AccessTools.all);
            try
            {
                HarmonyMethod postfix5 = new HarmonyMethod(method15);
                harmony.Patch((MethodBase)method16, postfix: postfix5);
                Debug.Log("Patch " + method15.Name + " Succeed");
            }
            catch 
            {

            }
            // Help Project Moon Fix their game
            MethodInfo method17 = typeof(EmotionFixInitializer).GetMethod("PassiveAbility_170331_SpeedDiceNumAdder");
            MethodInfo method18 = typeof(PassiveAbility_170331).GetMethod("SpeedDiceNumAdder", AccessTools.all);
            try
            {
                HarmonyMethod prefix4 = new HarmonyMethod(method17);
                harmony.Patch((MethodBase)method18, prefix: prefix4);
                Debug.Log("Patch " + method17.Name + " Succeed");
            }
            catch 
            {

            }
/*            InitConfig("EmotionFix", EmotionalFixConfig.Instance);*/
        }
        public static void EmotionCardXmlList_GetEnemyEmotionNeutralCardList(ref List<EmotionCardXmlInfo> __result)
        {
            __result.Remove(EmotionCardXmlList.Instance.GetData(1, SephirahType.None));
            __result.Remove(EmotionCardXmlList.Instance.GetData(4, SephirahType.None));
        }
        public static void StageController_StartBattle(StageType ____stageType)
        {
            if (____stageType == StageType.Invitation)
            {
                emotion1 = LoadEmotion(1);
                emotion2 = LoadEmotion(2);
                emotion3 = LoadEmotion(3);
                diff = DifficultyTweak();
                if (diff >= Difficulty.Hard)
                {
                    emotion1.Remove(EmotionCardXmlList.Instance.GetData(3, SephirahType.Tiphereth));
                    emotion1.Remove(EmotionCardXmlList.Instance.GetData(5, SephirahType.Tiphereth));
                    emotion2.Remove(EmotionCardXmlList.Instance.GetData(9, SephirahType.Tiphereth));
                    emotion2.Remove(EmotionCardXmlList.Instance.GetData(10, SephirahType.Tiphereth));
                }
                emotion3.Remove(EmotionCardXmlList.Instance.GetData(15, SephirahType.Tiphereth));
                emotion1.Remove(EmotionCardXmlList.Instance.GetData(11, SephirahType.Hokma));
                emotion2.Remove(EmotionCardXmlList.Instance.GetData(14, SephirahType.Chesed));
                emotion2.Remove(EmotionCardXmlList.Instance.GetData(12, SephirahType.Hokma));
                emotion3.Remove(EmotionCardXmlList.Instance.GetData(15, SephirahType.Hokma));
                enermy = EmotionCardXmlList.Instance.GetDataList_enemy(SephirahType.None);
                enemylist.Clear();
                TriggerReset();
            }
        }
        private static void TriggerReset()
        {
            WhiteNightTrigger = false;
            ClownTrigger = false;
            GiftTrigger= false;
        }
        public static void StageController_GameOver()
        {
            PassiveAbility_668.LevelUped.Clear();
            EmotionCardAbility_hokma_plaguedoctor1.WhiteNightClock.Clear();
            TriggerReset();
        }
        public static void StageController_RoundStartPhase_System()
        {
            if (!GiftTrigger && RandomUtil.valueForProb <= 0.02)
                GiftTrigger = true;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
            {
                if (ExcludedEnemyID.Exists(x => alive.UnitData.unitData.EnemyUnitId == x))
                    continue;
                if (ExcludedBookID.Exists(x => alive.Book.GetBookClassInfoId() == x))
                    continue;
                if (enemylist.Contains(alive))
                    continue;
                AssignPassive(alive);
            }
        }
        public static bool PassiveAbility_170331_SpeedDiceNumAdder(ref int __result)
        {
            __result=- 1;
            return false;
        }
        public static Difficulty DifficultyTweak()
        {
            Difficulty Dif = Difficulty.Normal;
            try
            {
                Debug.Log("Difficulty input found");
                foreach (string str in File.ReadAllLines(modPath + "/Difficulty.txt"))
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
            }
            catch(Exception ex)
            {
                File.WriteAllText(Application.dataPath + "/Mods/DifficultyError.txt", ex.Message+"\n"+ex.StackTrace);
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
            list.AddRange(EmotionCardXmlList.Instance.GetDataList(SephirahType.Malkuth, LibraryModel.Instance.GetFloor(SephirahType.Malkuth).Level, emotionlevel));
            list.AddRange(EmotionCardXmlList.Instance.GetDataList(SephirahType.Yesod, LibraryModel.Instance.GetFloor(SephirahType.Yesod ).Level, emotionlevel));
            list.AddRange(EmotionCardXmlList.Instance.GetDataList(SephirahType.Hod , LibraryModel.Instance.GetFloor(SephirahType.Hod ).Level, emotionlevel));
            list.AddRange(EmotionCardXmlList.Instance.GetDataList(SephirahType.Netzach , LibraryModel.Instance.GetFloor(SephirahType.Netzach).Level, emotionlevel));
            list.AddRange(EmotionCardXmlList.Instance.GetDataList(SephirahType.Tiphereth , LibraryModel.Instance.GetFloor(SephirahType.Tiphereth ).Level, emotionlevel));
            list.AddRange(EmotionCardXmlList.Instance.GetDataList(SephirahType.Gebura , LibraryModel.Instance.GetFloor(SephirahType.Gebura ).Level, emotionlevel));
            list.AddRange(EmotionCardXmlList.Instance.GetDataList(SephirahType.Chesed , LibraryModel.Instance.GetFloor(SephirahType.Chesed ).Level, emotionlevel));
            list.AddRange(EmotionCardXmlList.Instance.GetDataList(SephirahType.Binah , LibraryModel.Instance.GetFloor(SephirahType.Binah ).Level, emotionlevel));
            list.AddRange(EmotionCardXmlList.Instance.GetDataList(SephirahType.Hokma , LibraryModel.Instance.GetFloor(SephirahType.Hokma ).Level, emotionlevel));
            list.AddRange(EmotionCardXmlList.Instance.GetDataList(SephirahType.Keter , LibraryModel.Instance.GetFloor(SephirahType.Keter ).Level, emotionlevel));
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
            EmotionBundle EB = EmotionBundle.None;
            if (GiftTrigger)
                EB = EmotionBundle.Gift;
            if (!WhiteNightTrigger && RandomUtil.valueForProb <= 0.02) 
            { 
                WhiteNightTrigger = true;
                EB = EmotionBundle.Whitenight;
            }
            if (EB==EmotionBundle.None && diff>=Difficulty.Hard && !ClownTrigger && RandomUtil.valueForProb <= 0.02)
            {
                ClownTrigger = true;
                EB = EmotionBundle.Clown;
            }
            switch (diff)
            {
                case (Difficulty.Easy):
                    break;
                case (Difficulty.Normal):
                    passiveList.Add(new PassiveAbility_666(unit, EB));
                    typeof(BattleUnitPassiveDetail).GetField("_passiveList", AccessTools.all).SetValue(unit.passiveDetail, passiveList);
                    break;
                case (Difficulty.Hard):
                    passiveList.Add(new PassiveAbility_667(unit, EB));
                    typeof(BattleUnitPassiveDetail).GetField("_passiveList", AccessTools.all).SetValue(unit.passiveDetail, passiveList);
                    break;
                case (Difficulty.Brutal):
                    passiveList.Add(new PassiveAbility_668(unit, EB));
                    typeof(BattleUnitPassiveDetail).GetField("_passiveList", AccessTools.all).SetValue(unit.passiveDetail, passiveList);
                    break;
            }
            enemylist.Add(unit);
            Debug.Log("Passive is Added to " + unit.UnitData.unitData.name);
        }
/*        internal void InitConfig(string name, object config)
        {
            List<String> assembly = new List<String>();
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                assembly.Add(a.GetName().Name);
            }
            if (assembly.Contains("ConfigAPI"))
            {
                var tempInstance = new ConfigHandler();
                tempInstance.Init(name, config);
            }
        }*/
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
/*        public class EmotionalFixConfig
        {
            public int Difficulty = 1;
            public static EmotionalFixConfig Instance = new EmotionalFixConfig();
        }
        internal class ConfigHandler
        {
            internal void Init(string name, object config)
            {
                ConfigAPI.Init(name, config);
            }
        }*/
    }
}
