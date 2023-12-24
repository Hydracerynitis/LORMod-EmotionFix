using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;

namespace EmotionalFix
{
    public class EmotionCardAbility_bossbird3 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            if (_owner.faction == Faction.Player)
            {
                _owner.bufListDetail.AddBuf(new SmallBird());
                GameObject gameObject = Util.LoadPrefab("Battle/CreatureEffect/FinalBattle/BinahFinalBattle_ImageFilter");
                if (!(gameObject != null))
                    return;
                Creature_Final_Binah_ImageFilter component = gameObject?.GetComponent<Creature_Final_Binah_ImageFilter>();
                if (component != null)
                    component.Init(2);
                gameObject.AddComponent<AutoDestruct>().time = 10f;
            }
            if (_owner.faction == Faction.Enemy)
                _owner.bufListDetail.AddBuf(new Smallbird_Enemy());
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            if (_owner.faction == Faction.Player)
                _owner.bufListDetail.AddBuf(new SmallBird());
            if (_owner.faction == Faction.Enemy)
                _owner.bufListDetail.AddBuf(new Smallbird_Enemy());
        }
        public override StatBonus GetStatBonus()
        {
            if (_owner.faction == Faction.Player)
                return new StatBonus() { hpRate = -50 };
            return base.GetStatBonus();
        }
        public class SmallBird : BattleUnitBuf
        {
            public override string keywordIconId => "ApocalypseBird_SmallBeak";
            public override string keywordId => "Smallbird";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (behavior.TargetDice == null || !IsAttackDice(behavior.TargetDice.Detail))
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = RandomUtil.Range(1, 3)
                });
            }
        }
        public void Destroy()
        {
            BattleUnitBuf Buff = _owner.bufListDetail.GetActivatedBufList().Find(x => x is SmallBird);
            if (Buff != null)
                Buff.Destroy();
            Buff = _owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Smallbird_Enemy));
            if (Buff != null)
                Buff.Destroy();
            SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(_owner, _owner.faction, _owner.hp, _owner.breakDetail.breakGauge);
        }
        public class Smallbird_Enemy : BattleUnitBuf
        {
            public override string keywordIconId => "ApocalypseBird_SmallBeak";
            public override string keywordId => "Smallbird_Enemy";
            private List<BattleDiceCardModel> _cards = new List<BattleDiceCardModel>();
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
            public override void OnRoundStart()
            {
                BattleUnitModel alive = RandomUtil.SelectOne<BattleUnitModel>(BattleObjectManager.instance.GetAliveList(Faction.Player));
                List<BattleDiceCardModel> hand = alive.allyCardDetail.GetHand();
                int num = 0;
                while (num < 2)
                {
                    ++num;
                    if (hand.Count > 0)
                    {
                        BattleDiceCardModel battleDiceCardModel = RandomUtil.SelectOne<BattleDiceCardModel>(hand);
                        battleDiceCardModel.AddCost(1);
                        _cards.Add(battleDiceCardModel);
                        hand.Remove(battleDiceCardModel);
                    }
                }
            }
            public override void OnRoundEnd() => ResetCardsCost();
            public override void Destroy()
            {
                base.Destroy();
                ResetCardsCost();
            }
            private void ResetCardsCost()
            {
                if (_cards.Count <= 0)
                    return;
                foreach (BattleDiceCardModel card in _cards)
                    card.AddCost(-1);
                _cards.Clear();
            }
        }
    }
}
