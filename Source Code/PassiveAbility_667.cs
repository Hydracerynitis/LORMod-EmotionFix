using System;
using System.Collections.Generic;
using LOR_XML;
using UnityEngine;
using BaseMod;

namespace EmotionalFix
{
    public class PassiveAbility_667: PassiveAbilityBase
    {
        private bool WhiteNight;
        private int Collect1;
        private int Collect2;
        private bool Collect3;
        public PassiveAbility_667(BattleUnitModel model,bool Trigger)
        {
            Init(model);
            name = Singleton<PassiveDescXmlList>.Instance.GetName(667);
            desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(667);
            rare = Rarity.Unique;
            Collect1 = owner.emotionDetail.PassiveList.FindAll((Predicate<BattleEmotionCardModel>)(x => x.XmlInfo.EmotionLevel == 1)).Count;
            Collect2 = owner.emotionDetail.PassiveList.FindAll((Predicate<BattleEmotionCardModel>)(x => x.XmlInfo.EmotionLevel == 2)).Count;
            Collect3 = owner.emotionDetail.PassiveList.Exists((Predicate<BattleEmotionCardModel>)(x => x.XmlInfo.EmotionLevel == 3));
            WhiteNight = Trigger;
        }
        public override void OnRoundEndTheLast()
        {
            if (owner.emotionDetail.EmotionLevel >= 1 && Collect1<1)
            {
                if (WhiteNight)
                    owner.emotionDetail.ApplyEmotionCard(Singleton<EmotionCardXmlList>.Instance.GetData(90911, SephirahType.None));
                else
                {
                    string name = RandomUtil.SelectOne<EmotionCardXmlInfo>(EmotionFixInitializer.emotion1).Name + "_Enemy";
                    EmotionCardXmlInfo emotion = EmotionFixInitializer.enermy.Find((Predicate<EmotionCardXmlInfo>)(x => x.Name == name));
                    //emotion = RandomUtil.SelectOne<EmotionCardXmlInfo>(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.None, 10, 1));
                    if (emotion == null)
                        return;
                    //emotion = Singleton<EmotionCardXmlList>.Instance.GetData(90813, SephirahType.None);
                    owner.emotionDetail.ApplyEmotionCard(emotion);
                    Debug.Log(owner.UnitData.unitData.name + " receive " + name);
                }
                Collect1 += 1;
            }
            if (owner.emotionDetail.EmotionLevel >= 2 && Collect1 < 2)
            {
                string name = RandomUtil.SelectOne<EmotionCardXmlInfo>(EmotionFixInitializer.emotion1).Name + "_Enemy";
                while (SearchEmotion(owner, name) != null)
                    name = RandomUtil.SelectOne<EmotionCardXmlInfo>(EmotionFixInitializer.emotion1).Name + "_Enemy";
                EmotionCardXmlInfo emotion = EmotionFixInitializer.enermy.Find((Predicate<EmotionCardXmlInfo>)(x => x.Name == name));
                //emotion = RandomUtil.SelectOne<EmotionCardXmlInfo>(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.None, 10, 1));
                if (emotion == null)
                    return;
                //emotion = Singleton<EmotionCardXmlList>.Instance.GetData(90813, SephirahType.None);
                owner.emotionDetail.ApplyEmotionCard(emotion);
                Debug.Log(owner.UnitData.unitData.name + " receive " + name);
                Collect1 += 1;
            }
            if (owner.emotionDetail.EmotionLevel >= 3 && Collect2<1)
            {
                if (WhiteNight)
                    owner.emotionDetail.ApplyEmotionCard(Singleton<EmotionCardXmlList>.Instance.GetData(90912, SephirahType.None));
                else
                {
                    string name = RandomUtil.SelectOne<EmotionCardXmlInfo>(EmotionFixInitializer.emotion2).Name + "_Enemy";
                    EmotionCardXmlInfo emotion = EmotionFixInitializer.enermy.Find((Predicate<EmotionCardXmlInfo>)(x => x.Name == name));
                    //emotion = RandomUtil.SelectOne<EmotionCardXmlInfo>(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.None, 10, 2));
                    if (emotion == null)
                        return;
                    //emotion = Singleton<EmotionCardXmlList>.Instance.GetData(90913, SephirahType.None);
                    owner.emotionDetail.ApplyEmotionCard(emotion);
                    Debug.Log(owner.UnitData.unitData.name + " receive " + name);
                }
                Collect2 += 1;
            }
            if (owner.emotionDetail.EmotionLevel >= 4 && Collect2 < 2)
            {
                string name = RandomUtil.SelectOne<EmotionCardXmlInfo>(EmotionFixInitializer.emotion2).Name + "_Enemy";
                while (SearchEmotion(owner, name) != null)
                    name = RandomUtil.SelectOne<EmotionCardXmlInfo>(EmotionFixInitializer.emotion1).Name + "_Enemy";
                EmotionCardXmlInfo emotion = EmotionFixInitializer.enermy.Find((Predicate<EmotionCardXmlInfo>)(x => x.Name == name));
                //emotion = RandomUtil.SelectOne<EmotionCardXmlInfo>(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.None, 10, 2));
                if (emotion == null)
                    return;
                //emotion = Singleton<EmotionCardXmlList>.Instance.GetData(90813, SephirahType.None);
                owner.emotionDetail.ApplyEmotionCard(emotion);
                Debug.Log(owner.UnitData.unitData.name + " receive " + name);
                Collect2 += 1;
            }
            if (owner.emotionDetail.EmotionLevel >= 5 && !Collect3)
            {
                if (WhiteNight)
                    owner.emotionDetail.ApplyEmotionCard(Singleton<EmotionCardXmlList>.Instance.GetData(90915, SephirahType.None));
                else
                {
                    string name = RandomUtil.SelectOne<EmotionCardXmlInfo>(EmotionFixInitializer.emotion3).Name + "_Enemy";
                    EmotionCardXmlInfo emotion = EmotionFixInitializer.enermy.Find((Predicate<EmotionCardXmlInfo>)(x => x.Name == name));
                    //emotion = RandomUtil.SelectOne<EmotionCardXmlInfo>(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.None, 10, 3));
                    if (emotion == null)
                        return;
                    //emotion = Singleton<EmotionCardXmlList>.Instance.GetData(90914, SephirahType.None);
                    owner.emotionDetail.ApplyEmotionCard(emotion);
                }
                Collect3 = true;
            }
        }
        private BattleEmotionCardModel SearchEmotion(BattleUnitModel owner, string Name)
        {
            List<BattleEmotionCardModel> emotion = owner.emotionDetail.PassiveList;
            foreach (BattleEmotionCardModel card in emotion)
            {
                if (card.XmlInfo.Name == Name)
                    return card;
            }
            return null;
        }
        public override void OnDie()
        {
            base.OnDie();
            EmotionFixInitializer.Hastrigger = false;
        }
    }
}
