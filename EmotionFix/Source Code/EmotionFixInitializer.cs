using System;
using System.Collections.Generic;
using Sound;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using LOR_DiceSystem;
using UnityEngine;
using EmotionalFix;

namespace EmotionalFix
{
    [HarmonyPatch]
    public class EmotionFixInitializer: ModInitializer
    {
        public static Difficulty diff;
        public static string modPath;
        public static bool WhiteNightTrigger;
        public static bool ClownTrigger;
        public static bool GiftTrigger;
        public static List<BattleUnitModel> enemylist = new List<BattleUnitModel>();
        public static List<EmotionCardXmlInfo> emotion1 = new List<EmotionCardXmlInfo>();
        public static List<EmotionCardXmlInfo> emotion2 = new List<EmotionCardXmlInfo>();
        public static List<EmotionCardXmlInfo> emotion3 = new List<EmotionCardXmlInfo>();
        public static List<EmotionCardXmlInfo> enemy = new List<EmotionCardXmlInfo>();
        public static List<UnitBattleDataModel> LevelUped = new List<UnitBattleDataModel>();
        public override void OnInitializeMod()
        {
            Harmony harmony = new Harmony("Hydracerynitis.EmotionFix");
            modPath = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
            harmony.PatchAll(typeof(EmotionFixInitializer));
/*            InitConfig("EmotionFix", EmotionalFixConfig.Instance);*/
        }
        [HarmonyPatch(typeof(EmotionCardXmlList),nameof(EmotionCardXmlList.GetEnemyEmotionNeutralCardList))]
        [HarmonyPostfix]
        public static void EmotionCardXmlList_GetEnemyEmotionNeutralCardList(ref List<EmotionCardXmlInfo> __result)
        {
            __result.Clear();
        }
        [HarmonyPatch(typeof(StageController),nameof(StageController.StartBattle))]
        [HarmonyPostfix]
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
                enemy = EmotionCardXmlList.Instance.GetDataList_enemy(SephirahType.None);
                enemylist.Clear();
                TriggerReset();
                if (!GiftTrigger && RandomUtil.valueForProb <= 0.02)
                    GiftTrigger = true;
            }
        }
        private static void TriggerReset()
        {
            WhiteNightTrigger = false;
            ClownTrigger = false;
            GiftTrigger= false;
        }
        [HarmonyPatch(typeof(StageController),nameof(StageController.GameOver))]
        [HarmonyPostfix]
        public static void StageController_GameOver()
        {
            PassiveAbility_668.LevelUped.Clear();
            //EmotionCardAbility_hokma_plaguedoctor1.WhiteNightClock.Clear();
            TriggerReset();
        }
        [HarmonyPatch(typeof(StageController),nameof(StageController.RoundStartPhase_System))]
        [HarmonyPostfix]
        public static void StageController_RoundStartPhase_System()
        {
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
        [HarmonyPatch(typeof(PassiveAbility_170331),nameof(PassiveAbility_170331.SpeedDiceNumAdder))]
        [HarmonyPrefix]
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
                }
                Debug.Log("Your Difficulty is " + Dif.ToString());
            }
            catch(Exception ex)
            {
                File.WriteAllText(Application.dataPath + "/Mods/DifficultyError.txt", ex.Message+"\n"+ex.StackTrace);
            }
            return Dif;
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
            foreach (PassiveAbilityBase P in passiveList)
            {
                if (ExcluededPassive.Contains(P.GetType().ToString()))
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
            PassiveAbilityBase passive = null;
            switch (diff)
            {
                case (Difficulty.Easy):
                    passive=new PassiveAbility_666(unit, EB);
                    break;
                case (Difficulty.Normal):
                    passive = new PassiveAbility_667(unit, EB);
                    break;
                case (Difficulty.Hard):
                    passive = new PassiveAbility_668(unit, EB);
                break;
            }
            if(passive!=null)
                unit.passiveDetail._passiveList.Add(passive);
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
