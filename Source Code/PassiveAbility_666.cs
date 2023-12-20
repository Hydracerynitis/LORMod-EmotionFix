using System;
using LOR_XML;
using BaseMod;
using UnityEngine;

namespace EmotionalFix
{
    public class PassiveAbility_666: PassiveAbilityBase
    {
        private bool WhiteNight;
        private bool Collect1;
        private bool Collect2;
        private bool Collect3;
        public PassiveAbility_666(BattleUnitModel model,bool Trigger)
        {
            this.Init(model);
            this.name = Singleton<PassiveDescXmlList>.Instance.GetName(666);
            this.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(666);
            this.rare = Rarity.Unique;
            Collect1 = this.owner.emotionDetail.PassiveList.Exists(x => x.XmlInfo.EmotionLevel == 1);
            Collect2 = this.owner.emotionDetail.PassiveList.Exists(x => x.XmlInfo.EmotionLevel == 2);
            Collect3 = this.owner.emotionDetail.PassiveList.Exists(x => x.XmlInfo.EmotionLevel == 3);
            WhiteNight = Trigger;
        }
        public override void OnRoundEndTheLast()
        {

            if (owner.emotionDetail.EmotionLevel >= 1 && !Collect1)
            {
                if (WhiteNight)
                    this.owner.emotionDetail.ApplyEmotionCard(Singleton<EmotionCardXmlList>.Instance.GetData(90911, SephirahType.None));
                else
                {
                    string name = RandomUtil.SelectOne<EmotionCardXmlInfo>(EmotionFixInitializer.emotion1).Name + "_Enemy";
                    EmotionCardXmlInfo emotion = EmotionFixInitializer.enermy.Find(x => x.Name == name);
                    //emotion = RandomUtil.SelectOne<EmotionCardXmlInfo>(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.None, 10, 1));
                    if (emotion == null)
                        return;
                    //emotion = Singleton<EmotionCardXmlList>.Instance.GetData(90813, SephirahType.None);
                    owner.emotionDetail.ApplyEmotionCard(emotion);
                    Debug.Log(owner.UnitData.unitData.name + " receive " + name);
                }
                Collect1 = true;
            }
            if (this.owner.emotionDetail.EmotionLevel >= 3 && !Collect2)
            {
                if (WhiteNight)
                    this.owner.emotionDetail.ApplyEmotionCard(Singleton<EmotionCardXmlList>.Instance.GetData(90912, SephirahType.None));
                else
                {
                    string name = RandomUtil.SelectOne<EmotionCardXmlInfo>(EmotionFixInitializer.emotion2).Name + "_Enemy";
                    EmotionCardXmlInfo emotion = EmotionFixInitializer.enermy.Find((Predicate<EmotionCardXmlInfo>)(x => x.Name == name));
                    //emotion = RandomUtil.SelectOne<EmotionCardXmlInfo>(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.None, 10, 2));
                    if (emotion == null)
                        return;
                    //emotion = Singleton<EmotionCardXmlList>.Instance.GetData(90515, SephirahType.None);
                    owner.emotionDetail.ApplyEmotionCard(emotion);
                    Debug.Log(owner.UnitData.unitData.name + " receive " + name);
                }
                Collect2 = true;
            }
            if (this.owner.emotionDetail.EmotionLevel >= 5 && !Collect3)
            {
                if (WhiteNight)
                    this.owner.emotionDetail.ApplyEmotionCard(Singleton<EmotionCardXmlList>.Instance.GetData(90915, SephirahType.None));
                else
                {
                    string name = RandomUtil.SelectOne<EmotionCardXmlInfo>(EmotionFixInitializer.emotion3).Name + "_Enemy";
                    EmotionCardXmlInfo emotion = EmotionFixInitializer.enermy.Find((Predicate<EmotionCardXmlInfo>)(x => x.Name == name));
                    //emotion = RandomUtil.SelectOne<EmotionCardXmlInfo>(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.None, 10, 3));
                    if (emotion == null)
                        return;
                    //emotion = Singleton<EmotionCardXmlList>.Instance.GetData(90914, SephirahType.None);
                    this.owner.emotionDetail.ApplyEmotionCard(emotion);
                    Debug.Log(owner.UnitData.unitData.name + " receive " + name);
                }
                Collect3 = true;
            }
        }
        public override void OnDie()
        {
            base.OnDie();
            EmotionFixInitializer.Hastrigger = false;
        }
    }
}
