using System;
using LOR_XML;
using System.IO;
using UnityEngine;
using BaseMod;
using System.Collections.Generic;

namespace EmotionalFix
{
    public class PassiveAbility_668: PassiveAbilityBase
    {
        private bool WhiteNight;
        private int Collect1;
        private int Collect2;
        private bool Collect3;
        public static List<UnitBattleDataModel> LevelUped;
        public PassiveAbility_668(BattleUnitModel model,bool Trigger)
        {
            this.Init(model);
            this.name = Singleton<PassiveDescXmlList>.Instance.GetName(668);
            this.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(668);
            this.rare = Rarity.Unique;
            Collect1 = this.owner.emotionDetail.PassiveList.FindAll((Predicate<BattleEmotionCardModel>)(x => x.XmlInfo.EmotionLevel == 1)).Count;
            Collect2 = this.owner.emotionDetail.PassiveList.FindAll((Predicate<BattleEmotionCardModel>)(x => x.XmlInfo.EmotionLevel == 2)).Count;
            Collect3 = this.owner.emotionDetail.PassiveList.Exists((Predicate<BattleEmotionCardModel>)(x => x.XmlInfo.EmotionLevel == 3));
            WhiteNight = Trigger;
        }
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            if (LevelUped.Contains(self.UnitData))
                return;
            List<BattleUnitModel> player = BattleObjectManager.instance.GetAliveList(Faction.Player);
            int level = 0;
            foreach (BattleUnitModel librarian in player)
            {
                level += librarian.emotionDetail.EmotionLevel;
            }
            level = level / player.Count;
            self.emotionDetail.SetEmotionLevel(level);
            self.cardSlotDetail.RecoverPlayPoint(self.cardSlotDetail.GetMaxPlayPoint());
            self.OnRoundStartOnlyUI();
            LevelUped.Add(self.UnitData);
        }
        public override void OnRoundEndTheLast()
        {
            if (this.owner.emotionDetail.EmotionLevel >= 1 && Collect1<1)
            {
                if (WhiteNight)
                    this.owner.emotionDetail.ApplyEmotionCard(Singleton<EmotionCardXmlList>.Instance.GetData(90911, SephirahType.None));
                else
                {
                    string name = RandomUtil.SelectOne<EmotionCardXmlInfo>(EmotionFixInitializer.emotion1).Name + "_Enemy";
                    EmotionCardXmlInfo emotion = EmotionFixInitializer.enermy.Find((Predicate<EmotionCardXmlInfo>)(x => x.Name == name));
                    //emotion = RandomUtil.SelectOne<EmotionCardXmlInfo>(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.None, 10, 1));
                    if (emotion == null)
                        return;
                    //emotion = Singleton<EmotionCardXmlList>.Instance.GetData(90813, SephirahType.None);
                    this.owner.emotionDetail.ApplyEmotionCard(emotion);
                    Debug.Log(owner.UnitData.unitData.name + " receive " + name);
                }
                Collect1 += 1;
            }
            if (this.owner.emotionDetail.EmotionLevel >= 2 && Collect1 < 2)
            {
                string name = RandomUtil.SelectOne<EmotionCardXmlInfo>(EmotionFixInitializer.emotion1).Name + "_Enemy";
                while (SearchEmotion(owner, name) != null)
                    name = RandomUtil.SelectOne<EmotionCardXmlInfo>(EmotionFixInitializer.emotion1).Name + "_Enemy";
                EmotionCardXmlInfo emotion = EmotionFixInitializer.enermy.Find((Predicate<EmotionCardXmlInfo>)(x => x.Name == name));
                //emotion = RandomUtil.SelectOne<EmotionCardXmlInfo>(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.None, 10, 1));
                if (emotion == null)
                    return;
                //emotion = Singleton<EmotionCardXmlList>.Instance.GetData(90813, SephirahType.None);
                this.owner.emotionDetail.ApplyEmotionCard(emotion);
                Debug.Log(owner.UnitData.unitData.name + " receive " + name);
                Collect1 += 1;
            }
            if (this.owner.emotionDetail.EmotionLevel >= 3 && Collect2<1)
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
                    //emotion = Singleton<EmotionCardXmlList>.Instance.GetData(90913, SephirahType.None);
                    this.owner.emotionDetail.ApplyEmotionCard(emotion);
                    Debug.Log(owner.UnitData.unitData.name + " receive " + name);
                }
                Collect2 += 1;
            }
            if (this.owner.emotionDetail.EmotionLevel >= 4 && Collect2 < 2)
            {
                string name = RandomUtil.SelectOne<EmotionCardXmlInfo>(EmotionFixInitializer.emotion2).Name + "_Enemy";
                while (SearchEmotion(owner, name) != null)
                    name = RandomUtil.SelectOne<EmotionCardXmlInfo>(EmotionFixInitializer.emotion1).Name + "_Enemy";
                EmotionCardXmlInfo emotion = EmotionFixInitializer.enermy.Find((Predicate<EmotionCardXmlInfo>)(x => x.Name == name));
                //emotion = RandomUtil.SelectOne<EmotionCardXmlInfo>(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.None, 10, 2));
                if (emotion == null)
                    return;
                //emotion = Singleton<EmotionCardXmlList>.Instance.GetData(90813, SephirahType.None);
                this.owner.emotionDetail.ApplyEmotionCard(emotion);
                Debug.Log(owner.UnitData.unitData.name + " receive " + name);
                Collect2 += 1;
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
