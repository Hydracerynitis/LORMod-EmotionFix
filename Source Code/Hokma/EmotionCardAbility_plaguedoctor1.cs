using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using HarmonyLib;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_plaguedoctor1 : EmotionCardAbilityBase
    {
        public static Dictionary<UnitBattleDataModel, int> WhiteNightClock;
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            WhiteNightClock.Add(_owner.UnitData, 0);
            if (_owner.faction == Faction.Player)
            {
                int cardId = 1100019;
                if (SearchEmotion(_owner, "WhiteNight_Red") != null)
                    cardId = 1100020;
                _owner.allyCardDetail.AddNewCard(cardId);
            }
            if (_owner.faction == Faction.Enemy)
            {
                if (_owner.bufListDetail.GetActivatedBufList().Find(x => x is PlagueDoctor) != null)
                    return;
                _owner.bufListDetail.AddBuf(new PlagueDoctor());
            }
        }
        public override void OnWaveStart()
        {
            if (_owner.faction == Faction.Player)
            {
                int cardId = 1100019;
                if (SearchEmotion(_owner, "WhiteNight_Red") != null)
                    cardId = 1100020;
                _owner.allyCardDetail.AddNewCard(cardId);
            }
            if (_owner.faction == Faction.Enemy)
            {
                if (_owner.bufListDetail.GetActivatedBufList().Find(x => x is PlagueDoctor) != null)
                    return;
                _owner.bufListDetail.AddBuf(new PlagueDoctor());
            }
        }
        public class PlagueDoctor : BattleUnitBuf
        {
            private int bless;
            private List<BattleUnitModel> patient;
            public override int SpeedDiceNumAdder() => bless;
            public override void Init(BattleUnitModel model)
            {
                Init(model);
                patient = new List<BattleUnitModel>();
            }
            public override void OnRoundStart()
            {
                patient.Clear();
                List<BattleUnitModel> alive = BattleObjectManager.instance.GetAliveList(_owner.faction).FindAll(x => x != _owner);
                for (int i = 0; i < 2; i++)
                {
                    if (alive.Count <= 0)
                        break;
                    BattleUnitModel victim = RandomUtil.SelectOne<BattleUnitModel>(alive);
                    patient.Add(victim);
                    alive.Remove(victim);
                }
                bless = patient.Count;
            }
            public override void OnDrawCard()
            {
                base.OnDrawCard();
                for (int i = 1; i < bless + 1; i++)
                {
                    DiceCardXmlInfo bless = ItemXmlDataList.instance.GetCardItem(1108401).Copy(true);
                    bless.Script = "BlessingPlague";
                    if (SearchEmotion(_owner, "WhiteNight_Red_Enemy") != null)
                        bless.Script = "BlessingPlagueUpGraded";
                    int num = EmotionCardAbility_plaguedoctor1.WhiteNightClock[_owner.UnitData] + i;
                    bless.DiceBehaviourList[0].Dice = num;
                    bless.DiceBehaviourList[0].Min = num;
                    BattleDiceCardModel card = BattleDiceCardModel.CreatePlayingCard(bless);
                    card.temporary = true;
                    List<BattleDiceCardModel> hand = typeof(BattleAllyCardDetail).GetField("_cardInHand", AccessTools.all).GetValue(_owner.allyCardDetail) as List<BattleDiceCardModel>;
                    hand.Add(card);
                }
            }
            public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int idx)
            {
                if (card.GetID() != 1108401 || patient.Count <= 0)
                    return base.ChangeAttackTarget(card, idx);
                BattleUnitModel target = patient[0];
                patient.Remove(target);
                return target;
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
        public void Destroy()
        {
            EmotionCardAbility_plaguedoctor1.WhiteNightClock.Remove(_owner.UnitData);
            if (_owner.faction == Faction.Player)
            {
                _owner.allyCardDetail.ExhaustCard(1100019);
                _owner.allyCardDetail.ExhaustCard(1100020);
            }
            if (_owner.faction == Faction.Enemy)
            {
                BattleUnitBuf doctor = _owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is PlagueDoctor));
                if (doctor != null)
                    doctor.Destroy();                
            }
        }
    }
}
