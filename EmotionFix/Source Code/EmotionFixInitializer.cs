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
using UI;
using TMPro;

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
        public static DifficultyOption DifficultyUI = null;
        private static bool HasLoadEmotion = false;
        public override void OnInitializeMod()
        {
            Harmony harmony = new Harmony("Hydracerynitis.EmotionFix");
            modPath = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
            harmony.PatchAll(typeof(EmotionFixInitializer));
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
                diff = ReadDifficulty();
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
        [HarmonyPatch(typeof(UIOptionWindow),nameof(UIOptionWindow.Open))]
        [HarmonyPostfix]
        public static void UIOptionWindow_Open(UIOptionWindow __instance)
        {
            if (DifficultyUI == null)
            {
                DifficultyUI = new DifficultyOption();
                TMP_Dropdown new_dropdown = UnityEngine.Object.Instantiate<TMP_Dropdown>(__instance.languageDropdown, __instance.languageDropdown.transform.parent);
                new_dropdown.transform.localPosition += new Vector3(0f, -200f);
                GameObject textObject= __instance.root.transform.GetChild(4).GetChild(0).GetChild(3).gameObject;
                TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();
                InitText(text, textObject, "EF_difficulty_title1", -190f);
                InitText(text, textObject, "EF_difficulty_title2", -210f);
                DifficultyUI.DifficultyDropDown = new_dropdown;
            }
            diff = ReadDifficulty();
            DifficultyUI.Init(AbleChangeDiff());
        }
        public static bool AbleChangeDiff()
        {
            UIPhase gameState = UI.UIController.Instance.CurrentUIPhase;
            return !(gameState == UIPhase.DUMMY);
        }
        [HarmonyPatch(typeof(UIOptionWindow), nameof(UIOptionWindow.ApplyAndClose))]
        [HarmonyPostfix]
        public static void UIOptionWindow_ApplyAndClose(UIOptionWindow __instance)
        {
            if (DifficultyUI == null)
                return;
            diff = DifficultyUI.ApplySetting();
            File.WriteAllText(modPath + "/Difficulty.txt", DifficultyTextPrefix +" "+ diff.ToString());
        }
        private static string DifficultyTextPrefix = "Current EmotionFix's Difficulty (None, Easy, Normal, Hard):";
        public static void InitText(TextMeshProUGUI original, GameObject originalObject,string key,float downwardVector)
        {
            TextMeshProUGUI new_text = UnityEngine.Object.Instantiate<TextMeshProUGUI>(original, originalObject.transform.parent);
            new_text.color=original.color;
            new_text.transform.localPosition += new Vector3(0f, downwardVector);
            UITextDataLoader textloader = new_text.transform.gameObject.GetComponent<UITextDataLoader>();
            textloader.key = key;
        }
        public static Difficulty ReadDifficulty()
        {
            Difficulty Dif = Difficulty.Normal;
            try
            {
                Debug.Log("Difficulty input found");
                foreach (string str in File.ReadAllLines(modPath + "/Difficulty.txt"))
                {
                    string text = str.Trim();
                    if (!text.StartsWith(DifficultyTextPrefix))
                        continue;
                    int i = text.IndexOf(DifficultyTextPrefix);
                    text = text.Remove(i, DifficultyTextPrefix.Length).Trim();
                    return (Difficulty) Enum.Parse(typeof(Difficulty), text);  
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
            list.AddRange(EmotionCardXmlList.Instance.GetDataList(SephirahType.Malkuth, 7, emotionlevel));
            list.AddRange(EmotionCardXmlList.Instance.GetDataList(SephirahType.Yesod, 7, emotionlevel));
            list.AddRange(EmotionCardXmlList.Instance.GetDataList(SephirahType.Hod , 7, emotionlevel));
            list.AddRange(EmotionCardXmlList.Instance.GetDataList(SephirahType.Netzach , 7, emotionlevel));
            list.AddRange(EmotionCardXmlList.Instance.GetDataList(SephirahType.Tiphereth , 7, emotionlevel));
            list.AddRange(EmotionCardXmlList.Instance.GetDataList(SephirahType.Gebura , 7, emotionlevel));
            list.AddRange(EmotionCardXmlList.Instance.GetDataList(SephirahType.Chesed , 7, emotionlevel));
            list.AddRange(EmotionCardXmlList.Instance.GetDataList(SephirahType.Binah , 7, emotionlevel));
            list.AddRange(EmotionCardXmlList.Instance.GetDataList(SephirahType.Hokma , 7, emotionlevel));
            list.AddRange(EmotionCardXmlList.Instance.GetDataList(SephirahType.Keter , 7, emotionlevel));
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
            226769011,226769012,226769013,226769014,226769015,226769016,226769017,226769018,226769019,226769020, //永远亭
            226769021,226769022,226769023,226769024,226769025,226769026,226769027,226769028,226769029,226769030,
            226769031,226769032,226769033,226769034,226769035,226769036,226769037,226769038,226769039,226769040,
            226769041,226769042,226769043,226769044,226769045,226769046,226769047,226769048,226769049,226769050,
            226769011,226769012,226769013,226769014,226769015,226769016,226769017,226769018,226769059,226769060,

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

    public enum EmotionBundle
    {
        None,
        Clown,
        Whitenight,
        Gift
    }
    public enum Difficulty
    {
        None,
        Easy,
        Normal,
        Hard
    }
}
