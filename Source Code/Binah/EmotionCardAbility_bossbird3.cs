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
            if (this._owner.faction == Faction.Player)
            {
                this._owner.bufListDetail.AddBuf(new SmallBird());
                GameObject gameObject = Util.LoadPrefab("Battle/CreatureEffect/FinalBattle/BinahFinalBattle_ImageFilter");
                if (!((UnityEngine.Object)gameObject != (UnityEngine.Object)null))
                    return;
                Creature_Final_Binah_ImageFilter component = gameObject?.GetComponent<Creature_Final_Binah_ImageFilter>();
                if ((UnityEngine.Object)component != (UnityEngine.Object)null)
                    component.Init(2);
                AutoDestruct autoDestruct = gameObject.AddComponent<AutoDestruct>();
                autoDestruct.time = 5f;
                autoDestruct.DestroyWhenDisable();
            }
            if (this._owner.faction == Faction.Enemy)
                this._owner.bufListDetail.AddBuf(new Smallbird_Enemy());
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            if (this._owner.faction == Faction.Player)
                this._owner.bufListDetail.AddBuf(new SmallBird());
            if (this._owner.faction == Faction.Enemy)
                this._owner.bufListDetail.AddBuf(new Smallbird_Enemy());
        }
        public override StatBonus GetStatBonus()
        {
            if (this._owner.faction == Faction.Player)
                return new StatBonus() { hpRate = -50 };
            return base.GetStatBonus();
        }
        public class SmallBird : BattleUnitBuf
        {
            protected override string keywordIconId => "ApocalypseBird_SmallBeak";
            protected override string keywordId => "Smallbird";
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
            BattleUnitBuf Buff = this._owner.bufListDetail.GetActivatedBufList().Find(x => x is SmallBird);
            if (Buff != null)
                Buff.Destroy();
            Buff = this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Smallbird_Enemy));
            if (Buff != null)
                Buff.Destroy();
            SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(this._owner, this._owner.faction, this._owner.hp, this._owner.breakDetail.breakGauge);
        }
        public class Smallbird_Enemy : BattleUnitBuf
        {
            protected override string keywordIconId => "ApocalypseBird_SmallBeak";
            protected override string keywordId => "Smallbird_Enemy";
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
                        this._cards.Add(battleDiceCardModel);
                        hand.Remove(battleDiceCardModel);
                    }
                }
            }
            public override void OnRoundEnd() => this.ResetCardsCost();
            public override void Destroy()
            {
                base.Destroy();
                this.ResetCardsCost();
            }
            private void ResetCardsCost()
            {
                if (this._cards.Count <= 0)
                    return;
                foreach (BattleDiceCardModel card in this._cards)
                    card.AddCost(-1);
                this._cards.Clear();
            }
        }
    }
}
