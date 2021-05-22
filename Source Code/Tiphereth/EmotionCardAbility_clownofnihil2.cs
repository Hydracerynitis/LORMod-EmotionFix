using System;
using System.Reflection;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_clownofnihil2 : EmotionCardAbilityBase
    {
        public static List<UnitBattleDataModel> Clown = new List<UnitBattleDataModel>();
        public override void OnSelectEmotion()
        {
            try
            {
                base.OnSelectEmotion();
                if (Clown.Contains(this._owner.UnitData))
                    return;
                this._owner.bufListDetail.AddBuf(new Clear());
                Clown.Add(this._owner.UnitData);
                Debug.Log(string.Format("虚无buff添加给{0}成功", this._owner.UnitData.unitData.name));
            }
            catch(Exception ex)
            {
                Debug.Error("ClownBuffDuplicate", ex);
            }

        }
        public class Clear : BattleUnitBuf
        {
            protected override string keywordIconId => "Fusion";
            protected override string keywordId => "RandomEmotion";
            private int level1;
            private int level2;
            private int level3;
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                //SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("5_T/FX_IllusionCard_5_T_Void", 1f, this._owner.view, this._owner.view, 2f);
            }
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                level1 = 0;
                level2 = 0;
                level3 = 0;
            }
            public override void OnRoundEndTheLast()
            {
                try
                {
                    base.OnRoundEndTheLast();
                    foreach (BattleEmotionCardModel emotion in this._owner.emotionDetail.PassiveList)
                    {
                        switch (emotion.XmlInfo.EmotionLevel)
                        {
                            case 1:
                                level1 += 1;
                                break;
                            case 2:
                                level2 += 1;
                                break;
                            case 3:
                                level3 += 1;
                                break;
                        }
                        foreach (EmotionCardAbilityBase ability in emotion.GetAbilityList())
                        {
                            MethodInfo destroy = ability.GetType().GetMethod("Destroy");
                            if (destroy != null)
                                try
                                {
                                    destroy.Invoke(ability, new object[] { });
                                }
                                catch(Exception ex)
                                {
                                    Debug.Error(ability.GetType().Name + "Destroy", ex);
                                }

                        }
                    }
                    this._owner.emotionDetail.PassiveList.Clear();
                    for (; level1 > 0; level1--)
                    {
                        if (this._owner.faction == Faction.Player)
                        {
                            EmotionCardXmlInfo emotion = RandomUtil.SelectOne<EmotionCardXmlInfo>(Harmony_Patch.emotion1);
                            if (this._owner.emotionDetail.CheckPassiveDuplicate(emotion))
                            {
                                level1 += 1;
                                continue;
                            }
                            this._owner.emotionDetail.ApplyEmotionCard(emotion);
                            Debug.Log(string.Format("添加新的情感卡{0}成功", emotion.Name));
                        }
                        if (this._owner.faction == Faction.Enemy)
                        {
                            string name = RandomUtil.SelectOne<EmotionCardXmlInfo>(Harmony_Patch.emotion1).Name + "_Enemy";
                            EmotionCardXmlInfo emotion = Harmony_Patch.enermy.Find((Predicate<EmotionCardXmlInfo>)(x => x.Name == name));
                            //emotion = RandomUtil.SelectOne<EmotionCardXmlInfo>(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.None, 10, 1));
                            if (this._owner.emotionDetail.CheckPassiveDuplicate(emotion))
                            {
                                level1 += 1;
                                continue;
                            }
                            this._owner.emotionDetail.ApplyEmotionCard(emotion);
                        }
                    }
                    for (; level2 > 0; level2--)
                    {
                        if (this._owner.faction == Faction.Player)
                        {
                            EmotionCardXmlInfo emotion = RandomUtil.SelectOne<EmotionCardXmlInfo>(Harmony_Patch.emotion2);
                            if (this._owner.emotionDetail.CheckPassiveDuplicate(emotion))
                            {
                                level2 += 1;
                                continue;
                            }
                            this._owner.emotionDetail.ApplyEmotionCard(emotion);
                            Debug.Log(string.Format("添加新的情感卡{0}成功", emotion.Name));
                        }
                        if (this._owner.faction == Faction.Enemy)
                        {
                            string name = RandomUtil.SelectOne<EmotionCardXmlInfo>(Harmony_Patch.emotion2).Name + "_Enemy";
                            EmotionCardXmlInfo emotion = Harmony_Patch.enermy.Find((Predicate<EmotionCardXmlInfo>)(x => x.Name == name));
                            //emotion = RandomUtil.SelectOne<EmotionCardXmlInfo>(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.None, 10, 2));
                            if (this._owner.emotionDetail.CheckPassiveDuplicate(emotion))
                            {
                                level2 += 1;
                                continue;
                            }
                            this._owner.emotionDetail.ApplyEmotionCard(emotion);
                        }
                    }
                    for (; level3 > 0; level3--)
                    {
                        if (this._owner.faction == Faction.Player)
                        {
                            EmotionCardXmlInfo emotion = RandomUtil.SelectOne<EmotionCardXmlInfo>(Harmony_Patch.emotion3);
                            if (this._owner.emotionDetail.CheckPassiveDuplicate(emotion))
                            {
                                level3 += 1;
                                continue;
                            }
                            this._owner.emotionDetail.ApplyEmotionCard(emotion);
                            Debug.Log(string.Format("添加新的情感卡{0}成功", emotion.Name));
                        }
                        if (this._owner.faction == Faction.Enemy)
                        {
                            string name = RandomUtil.SelectOne<EmotionCardXmlInfo>(Harmony_Patch.emotion3).Name + "_Enemy";
                            EmotionCardXmlInfo emotion = Harmony_Patch.enermy.Find((Predicate<EmotionCardXmlInfo>)(x => x.Name == name));
                            //emotion = RandomUtil.SelectOne<EmotionCardXmlInfo>(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.None, 10, 3));
                            if (this._owner.emotionDetail.CheckPassiveDuplicate(emotion))
                            {
                                level3 += 1;
                                continue;
                            }
                            this._owner.emotionDetail.ApplyEmotionCard(emotion);
                        }
                    }
                }
                catch(Exception ex)
                {
                    Debug.Error("ClownAddBuff", ex);
                }

            }
        }
    }
}
