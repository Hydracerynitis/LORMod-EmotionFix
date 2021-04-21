using System;
using LOR_XML;

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
            Collect1 = this.owner.emotionDetail.PassiveList.Exists((Predicate<BattleEmotionCardModel>)(x => x.XmlInfo.EmotionLevel == 1));
            Collect2 = this.owner.emotionDetail.PassiveList.Exists((Predicate<BattleEmotionCardModel>)(x => x.XmlInfo.EmotionLevel == 2));
            Collect3 = this.owner.emotionDetail.PassiveList.Exists((Predicate<BattleEmotionCardModel>)(x => x.XmlInfo.EmotionLevel == 3));
            WhiteNight = Trigger;
        }
        public override void OnRoundEndTheLast()
        {

            if (this.owner.emotionDetail.EmotionLevel >= 1 && !Collect1)
            {
                if (WhiteNight)
                    this.owner.emotionDetail.ApplyEmotionCard(Singleton<EmotionCardXmlList>.Instance.GetData(90911, SephirahType.None));
                else
                {
                    string name = RandomUtil.SelectOne<EmotionCardXmlInfo>(Harmony_Patch.emotion1).Name + "_Enemy";
                    EmotionCardXmlInfo emotion = Harmony_Patch.enermy.Find((Predicate<EmotionCardXmlInfo>)(x => x.Name == name));
                    //emotion = RandomUtil.SelectOne<EmotionCardXmlInfo>(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.None, 10, 1));
                    if (emotion == null)
                        return;
                    //emotion = Singleton<EmotionCardXmlList>.Instance.GetData(90813, SephirahType.None);
                    this.owner.emotionDetail.ApplyEmotionCard(emotion);
                }
                Collect1 = true;
            }
            if (this.owner.emotionDetail.EmotionLevel >= 3 && !Collect2)
            {
                if (WhiteNight)
                    this.owner.emotionDetail.ApplyEmotionCard(Singleton<EmotionCardXmlList>.Instance.GetData(90912, SephirahType.None));
                else
                {
                    string name = RandomUtil.SelectOne<EmotionCardXmlInfo>(Harmony_Patch.emotion2).Name + "_Enemy";
                    EmotionCardXmlInfo emotion = Harmony_Patch.enermy.Find((Predicate<EmotionCardXmlInfo>)(x => x.Name == name));
                    //emotion = RandomUtil.SelectOne<EmotionCardXmlInfo>(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.None, 10, 2));
                    if (emotion == null)
                        return;
                    //emotion = Singleton<EmotionCardXmlList>.Instance.GetData(90913, SephirahType.None);
                    this.owner.emotionDetail.ApplyEmotionCard(emotion);
                }
                Collect2 = true;
            }
            if (this.owner.emotionDetail.EmotionLevel >= 5 && !Collect3)
            {
                if (WhiteNight)
                    this.owner.emotionDetail.ApplyEmotionCard(Singleton<EmotionCardXmlList>.Instance.GetData(90915, SephirahType.None));
                else
                {
                    string name = RandomUtil.SelectOne<EmotionCardXmlInfo>(Harmony_Patch.emotion3).Name + "_Enemy";
                    EmotionCardXmlInfo emotion = Harmony_Patch.enermy.Find((Predicate<EmotionCardXmlInfo>)(x => x.Name == name));
                    //emotion = RandomUtil.SelectOne<EmotionCardXmlInfo>(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.None, 10, 3));
                    if (emotion == null)
                        return;
                    //emotion = Singleton<EmotionCardXmlList>.Instance.GetData(90914, SephirahType.None);
                    this.owner.emotionDetail.ApplyEmotionCard(emotion);
                }
                Collect3 = true;
            }
        }
        public override void OnDie()
        {
            base.OnDie();
            Harmony_Patch.Hastrigger = false;
        }
    }
}
