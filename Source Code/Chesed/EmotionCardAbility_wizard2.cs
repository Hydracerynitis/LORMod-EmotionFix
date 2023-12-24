using LOR_DiceSystem;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

namespace EmotionalFix
{
    public class EmotionCardAbility_wizard2: EmotionCardAbilityBase
    {
        private static List<WizardBufType> _remainedList = new List<WizardBufType>();
        private WizardBufType _selectedBuf;

        public override void OnSelectEmotion()
        {
            if (_owner.faction == Faction.Player)
            {
                _owner.personalEgoDetail.AddCard(1100023);
                _owner.personalEgoDetail.AddCard(1100024);
                _owner.personalEgoDetail.AddCard(1100025);
                _owner.personalEgoDetail.AddCard(1100026);
                _owner.personalEgoDetail.AddCard(1100027);
                return;
            }
            _remainedList.Clear();
            if(!CheckBuff(WizardBufType.Potion))
                _remainedList.Add(WizardBufType.Potion);
            if (!CheckBuff(WizardBufType.Pocket))
                _remainedList.Add(WizardBufType.Pocket);
            if (!CheckBuff(WizardBufType.Heart))
                _remainedList.Add(WizardBufType.Heart);
            if (!CheckBuff(WizardBufType.Home))
                _remainedList.Add(WizardBufType.Home);
            if (!CheckBuff(WizardBufType.Change))
                _remainedList.Add(WizardBufType.Change);
            WizardBufType bufType = RandomUtil.SelectOne<WizardBufType>(_remainedList);
            switch (bufType)
            {
                case WizardBufType.Potion:
                    _owner.bufListDetail.AddBuf(new BattleUnitBuf_potion());
                    break;
                case WizardBufType.Pocket:
                    _owner.bufListDetail.AddBuf(new BattleUnitBuf_pocket());
                    break;
                case WizardBufType.Heart:
                    _owner.bufListDetail.AddBuf(new BattleUnitBuf_heart());
                    break;
                case WizardBufType.Home:
                    _owner.bufListDetail.AddBuf(new BattleUnitBuf_home());
                    break;
                case WizardBufType.Change:
                    _owner.bufListDetail.AddBuf(new BattleUnitBuf_change());
                    break;
            }
            _selectedBuf = bufType;
        }
        public override void OnWaveStart()
        {
            if (_owner.faction == Faction.Player)
            {
                _owner.personalEgoDetail.AddCard(1100023);
                _owner.personalEgoDetail.AddCard(1100024);
                _owner.personalEgoDetail.AddCard(1100025);
                _owner.personalEgoDetail.AddCard(1100026);
                _owner.personalEgoDetail.AddCard(1100027);
                return;
            }
            switch (_selectedBuf)
            {
                case WizardBufType.Potion:
                    _owner.bufListDetail.AddBuf(new BattleUnitBuf_potion());
                    break;
                case WizardBufType.Pocket:
                    _owner.bufListDetail.AddBuf(new BattleUnitBuf_pocket());
                    break;
                case WizardBufType.Heart:
                    _owner.bufListDetail.AddBuf(new BattleUnitBuf_heart());
                    break;
                case WizardBufType.Home:
                    _owner.bufListDetail.AddBuf(new BattleUnitBuf_home());
                    break;
                case WizardBufType.Change:
                    _owner.bufListDetail.AddBuf(new BattleUnitBuf_change());
                    break;
            }
        }
        public bool CheckBuff(WizardBufType bufType)
        {
            foreach(BattleUnitModel ally in BattleObjectManager.instance.GetAliveList(_owner.faction))
            {
                switch (bufType)
                {
                    case WizardBufType.Potion:
                        if (ally.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_potion)) != null)
                            return true;
                        break;
                    case WizardBufType.Pocket:
                        if (ally.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_pocket)) != null)
                            return true;
                        break;
                    case WizardBufType.Heart:
                        if (ally.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_heart)) != null)
                            return true;
                        break;
                    case WizardBufType.Home:
                        if (ally.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_home)) != null)
                            return true;
                        break;
                    case WizardBufType.Change:
                        if (ally.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_change)) != null)
                            return true;
                        break;
                }
            }
            return false;
        }
        public enum WizardBufType
        {
            Potion,
            Pocket,
            Heart,
            Home,
            Change,
        }
        public class BattleUnitBuf_potion : BattleUnitBuf
        {
            public override string keywordId => "Wizard_potion";
            public override string keywordIconId => "Wizard_Courage";
            public BattleUnitBuf_potion() => stack = 0;
            private int round = 0;
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                if(_owner.faction==Faction.Player)
                    SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Gift", 1f, owner.view, owner.view, 2f);
            }
            public override void OnRoundStart()
            {
                round += 1;
                switch (round)
                {
                    case 1:
                        _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 2);
                        break;
                    case 2:
                        _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 2);
                        break;
                    case 3:
                        _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak, 2);
                        break;
                    case 4:
                        _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak, 2);
                        round = 0;
                        break;
                }
            }
        }
        public void Destroy()
        {
            if (_owner.faction == Faction.Player)
            {
                foreach(BattleUnitModel ally in BattleObjectManager.instance.GetAliveList(Faction.Player))
                {
                    if (ally.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_pocket)) is BattleUnitBuf_pocket pocket)
                        pocket.Destroy();
                    if (ally.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_potion)) is BattleUnitBuf_potion potion)
                        potion.Destroy();
                    if (ally.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_heart)) is BattleUnitBuf_heart heart)
                        heart.Destroy();
                    if (ally.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_home)) is BattleUnitBuf_home home)
                        home.Destroy();
                    if (ally.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_change)) is BattleUnitBuf_change change)
                        change.Destroy();
                }
            }
            if (_owner.faction == Faction.Enemy)
            {
                if (_owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_pocket)) is BattleUnitBuf_pocket pocket)
                    pocket.Destroy();
                if (_owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_potion)) is BattleUnitBuf_potion potion)
                    potion.Destroy();
                if (_owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_heart)) is BattleUnitBuf_heart heart)
                    heart.Destroy();
                if (_owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_home)) is BattleUnitBuf_home home)
                    home.Destroy();
                if (_owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_change)) is BattleUnitBuf_change change)
                    change.Destroy();
            }
        }
        public class BattleUnitBuf_pocket : BattleUnitBuf
        {
            public override string keywordId => "Wizard_poket";
            public override string keywordIconId => "Wizard_Scarecrow_Debuf";
            private int Pow => RandomUtil.Range(1, 2);
            public BattleUnitBuf_pocket() => stack = 0;
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                if (_owner.faction == Faction.Player)
                    SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Gift", 1f, owner.view, owner.view, 2f);
            }
            public override void OnDrawCard()
            {
                int count = _owner.allyCardDetail.GetHand().Count;
                BattleDiceCardModel Card = _owner.allyCardDetail.GetHand()[count-1];
                _owner.allyCardDetail.AddNewCard(Card.GetID()).XmlData.optionList.Add(CardOption.ExhaustOnUse);
            }
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = -Pow
                });
            }
        }
        public class BattleUnitBuf_heart : BattleUnitBuf
        {
            public override string keywordId => stack==0? "Wizard_heart": "Wizard_heart_Deactivate";
            public override string keywordIconId => "Lumberjack_Piece_Final";
            public BattleUnitBuf_heart() => stack = 0;
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                if (_owner.faction == Faction.Player)
                    SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Gift", 1f, owner.view, owner.view, 2f);
            }
            public override void OnRoundStart()
            {
                if (stack > 0)
                    return;
                stack = _owner.cardSlotDetail.GetMaxPlayPoint() - _owner.cardSlotDetail.PlayPoint;
                _owner.cardSlotDetail.RecoverPlayPoint(_owner.cardSlotDetail.GetMaxPlayPoint());
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                if (stack > 0)
                {
                    _owner.cardSlotDetail.SetRecoverPoint(0);
                    stack -= 1;
                    return;
                }
                _owner.cardSlotDetail.SetRecoverPointDefault();
            }
        }
        public class BattleUnitBuf_home : BattleUnitBuf
        {
            public override string keywordId => "Wizard_home";
            public override string keywordIconId => "WayBackHome_Super_Cancel";
            private int Pow => RandomUtil.Range(1, 3);
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                if (_owner.faction == Faction.Player)
                    SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Gift", 1f, owner.view, owner.view, 2f);
            }
            public BattleUnitBuf_home()
            {
                stack = 0;
                TargetList = new List<BattleUnitModel>();
            }
            private List<BattleUnitModel> TargetList;
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                TargetList.Clear();
            }
            public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
            {
                base.OnUseCard(card);
                BattleUnitModel target = card.target;
                if (target == null)
                    return;
                if (TargetList.Contains(target))
                {
                    card.ApplyDiceStatBonus(DiceMatch.AllAttackDice,new DiceStatBonus()
                    {
                        power = -Pow
                    });
                }
                else
                {
                    card.ApplyDiceStatBonus(DiceMatch.AllAttackDice, new DiceStatBonus()
                    {
                        power = Pow
                    });
                    TargetList.Add(target);
                }
            }
        }
        public class BattleUnitBuf_change : BattleUnitBuf
        {
            public override string keywordId => "Wizard_change";

            public override string keywordIconId => "Ozma_LibrarianToPumpkin";

            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                if (_owner.faction == Faction.Player)
                    SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Gift", 1f, owner.view, owner.view, 2f);
            }
            public BattleUnitBuf_change() => stack = 0;

            public override void OnRoundStart()
            {
                BattleUnitModel ally = RandomUtil.SelectOne<BattleUnitModel>(BattleObjectManager.instance.GetAliveList(_owner.faction));
                ally.RecoverHP((int)((double)_owner.MaxHp * 0.3));
                ally.breakDetail.RecoverBreak((int)((double)_owner.breakDetail.GetDefaultBreakGauge() * 0.4));
                if (ally.IsBreakLifeZero())
                {
                    ally.breakDetail.nextTurnBreak = false;
                    ally.breakDetail.breakLife = 1;
                }

                ally.allyCardDetail.DiscardACardRandomlyByAbility(1);
                ally.allyCardDetail.AddNewCard(9907421).exhaust = true;
            }
        }
    }
}
