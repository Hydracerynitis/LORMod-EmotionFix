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
        private EmotionBundle bundle = EmotionBundle.None;
        private int Collect1;
        private int Collect2;
        private bool Collect3;
        public static List<UnitBattleDataModel> LevelUped= new List<UnitBattleDataModel>();
        public PassiveAbility_668(BattleUnitModel model, EmotionBundle emotionBundle)
        {
            Init(model);
            name = Singleton<PassiveDescXmlList>.Instance.GetName(668);
            desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(668);
            rare = Rarity.Unique;
            Collect1 = owner.emotionDetail.PassiveList.FindAll(x => x.XmlInfo.EmotionLevel == 1).Count;
            Collect2 = owner.emotionDetail.PassiveList.FindAll(x => x.XmlInfo.EmotionLevel == 2).Count;
            Collect3 = owner.emotionDetail.PassiveList.Exists(x => x.XmlInfo.EmotionLevel == 3);
            bundle = emotionBundle;
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
            if (owner.emotionDetail.EmotionLevel >= 1 && Collect1<1)
            {
                if (bundle == EmotionBundle.Clown)
                    owner.emotionDetail.ApplyEmotionCard(EmotionCardXmlList.Instance.GetData(90503, SephirahType.None));
                if (bundle == EmotionBundle.Whitenight)
                    owner.emotionDetail.ApplyEmotionCard(EmotionCardXmlList.Instance.GetData(90911, SephirahType.None));
                else
                {
                    string name = RandomUtil.SelectOne(EmotionFixInitializer.emotion1).Name + "_Enemy";
                    EmotionCardXmlInfo emotion = EmotionFixInitializer.enemy.Find(x => x.Name == name);
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
                if (bundle == EmotionBundle.Clown)
                    owner.emotionDetail.ApplyEmotionCard(EmotionCardXmlList.Instance.GetData(90505, SephirahType.None));
                else
                {
                    string name = RandomUtil.SelectOne<EmotionCardXmlInfo>(EmotionFixInitializer.emotion1).Name + "_Enemy";
                    while (Helper.SearchEmotion(owner, name) != null)
                        name = RandomUtil.SelectOne<EmotionCardXmlInfo>(EmotionFixInitializer.emotion1).Name + "_Enemy";
                    EmotionCardXmlInfo emotion = EmotionFixInitializer.enemy.Find((Predicate<EmotionCardXmlInfo>)(x => x.Name == name));
                    //emotion = RandomUtil.SelectOne<EmotionCardXmlInfo>(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.None, 10, 1));
                    if (emotion == null)
                        return;
                    //emotion = Singleton<EmotionCardXmlList>.Instance.GetData(90813, SephirahType.None);
                    owner.emotionDetail.ApplyEmotionCard(emotion);
                    Debug.Log(owner.UnitData.unitData.name + " receive " + name);
                    Collect1 += 1;
                }
            }
            if (owner.emotionDetail.EmotionLevel >= 3 && Collect2<1)
            {
                if (bundle == EmotionBundle.Clown)
                    owner.emotionDetail.ApplyEmotionCard(EmotionCardXmlList.Instance.GetData(90509, SephirahType.None));
                if (bundle == EmotionBundle.Whitenight)
                    owner.emotionDetail.ApplyEmotionCard(EmotionCardXmlList.Instance.GetData(90912, SephirahType.None));
                if (bundle == EmotionBundle.Gift)
                    owner.emotionDetail.ApplyEmotionCard(EmotionCardXmlList.Instance.GetData(90714, SephirahType.None));
                else
                {
                    string name = RandomUtil.SelectOne<EmotionCardXmlInfo>(EmotionFixInitializer.emotion2).Name + "_Enemy";
                    EmotionCardXmlInfo emotion = EmotionFixInitializer.enemy.Find((Predicate<EmotionCardXmlInfo>)(x => x.Name == name));
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
                if (bundle == EmotionBundle.Clown)
                    owner.emotionDetail.ApplyEmotionCard(EmotionCardXmlList.Instance.GetData(90510, SephirahType.None));
                else
                {
                    string name = RandomUtil.SelectOne<EmotionCardXmlInfo>(EmotionFixInitializer.emotion2).Name + "_Enemy";
                    while (Helper.SearchEmotion(owner, name) != null)
                        name = RandomUtil.SelectOne<EmotionCardXmlInfo>(EmotionFixInitializer.emotion2).Name + "_Enemy";
                    EmotionCardXmlInfo emotion = EmotionFixInitializer.enemy.Find((Predicate<EmotionCardXmlInfo>)(x => x.Name == name));
                    //emotion = RandomUtil.SelectOne<EmotionCardXmlInfo>(Singleton<EmotionCardXmlList>.Instance.GetDataList(SephirahType.None, 10, 2));
                    if (emotion == null)
                        return;
                    //emotion = Singleton<EmotionCardXmlList>.Instance.GetData(90813, SephirahType.None);
                    owner.emotionDetail.ApplyEmotionCard(emotion);
                    Debug.Log(owner.UnitData.unitData.name + " receive " + name);
                    Collect2 += 1;
                }
            }
            if (owner.emotionDetail.EmotionLevel >= 5 && !Collect3)
            {
                if (bundle == EmotionBundle.Clown)
                    owner.emotionDetail.ApplyEmotionCard(EmotionCardXmlList.Instance.GetData(90515, SephirahType.None));
                if (bundle == EmotionBundle.Whitenight)
                    owner.emotionDetail.ApplyEmotionCard(EmotionCardXmlList.Instance.GetData(90915, SephirahType.None));
                else
                {
                    string name = RandomUtil.SelectOne<EmotionCardXmlInfo>(EmotionFixInitializer.emotion3).Name + "_Enemy";
                    EmotionCardXmlInfo emotion = EmotionFixInitializer.enemy.Find((Predicate<EmotionCardXmlInfo>)(x => x.Name == name));
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
