using System;
using LOR_XML;
using BaseMod;
using UnityEngine;

namespace EmotionalFix
{
    public class PassiveAbility_666: PassiveAbilityBase
    {
        private EmotionBundle bundle = EmotionBundle.None;
        private bool Collect1;
        private bool Collect2;
        private bool Collect3;
        public PassiveAbility_666(BattleUnitModel model,EmotionBundle emotionBundle)
        {
            Init(model);
            name = Singleton<PassiveDescXmlList>.Instance.GetName(666);
            desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(666);
            rare = Rarity.Unique;
            Collect1 = owner.emotionDetail.PassiveList.Exists(x => x.XmlInfo.EmotionLevel == 1);
            Collect2 = owner.emotionDetail.PassiveList.Exists(x => x.XmlInfo.EmotionLevel == 2);
            Collect3 = owner.emotionDetail.PassiveList.Exists(x => x.XmlInfo.EmotionLevel == 3);
            if (emotionBundle == EmotionBundle.Clown)
                return;
            bundle = emotionBundle;
        }
        public override void OnRoundEndTheLast()
        {

            if (owner.emotionDetail.EmotionLevel >= 1 && !Collect1)
            {
                if (bundle==EmotionBundle.Whitenight)
                    owner.emotionDetail.ApplyEmotionCard(EmotionCardXmlList.Instance.GetData(90911, SephirahType.None));
                else
                {
                    string name = RandomUtil.SelectOne(EmotionFixInitializer.emotion1).Name + "_Enemy";
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
            if (owner.emotionDetail.EmotionLevel >= 3 && !Collect2)
            {
                if (bundle == EmotionBundle.Whitenight)
                    owner.emotionDetail.ApplyEmotionCard(EmotionCardXmlList.Instance.GetData(90912, SephirahType.None));
                if(bundle==EmotionBundle.Gift)
                    owner.emotionDetail.ApplyEmotionCard(EmotionCardXmlList.Instance.GetData(90714, SephirahType.None));
                else
                {
                    string name = RandomUtil.SelectOne(EmotionFixInitializer.emotion2).Name + "_Enemy";
                    EmotionCardXmlInfo emotion = EmotionFixInitializer.enermy.Find(x => x.Name == name);
                    //emotion = RandomUtil.SelectOne<EmotionCardXmlInfo>(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.None, 10, 2));
                    if (emotion == null)
                        return;
                    //emotion = Singleton<EmotionCardXmlList>.Instance.GetData(90515, SephirahType.None);
                    owner.emotionDetail.ApplyEmotionCard(emotion);
                    Debug.Log(owner.UnitData.unitData.name + " receive " + name);
                }
                Collect2 = true;
            }
            if (owner.emotionDetail.EmotionLevel >= 5 && !Collect3)
            {
                if (bundle == EmotionBundle.Whitenight)
                    owner.emotionDetail.ApplyEmotionCard(EmotionCardXmlList.Instance.GetData(90915, SephirahType.None));
                else
                {
                    string name = RandomUtil.SelectOne(EmotionFixInitializer.emotion3).Name + "_Enemy";
                    EmotionCardXmlInfo emotion = EmotionFixInitializer.enermy.Find(x => x.Name == name);
                    //emotion = RandomUtil.SelectOne<EmotionCardXmlInfo>(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.None, 10, 3));
                    if (emotion == null)
                        return;
                    //emotion = Singleton<EmotionCardXmlList>.Instance.GetData(90914, SephirahType.None);
                    owner.emotionDetail.ApplyEmotionCard(emotion);
                    Debug.Log(owner.UnitData.unitData.name + " receive " + name);
                }
                Collect3 = true;
            }
        }
    }
}
