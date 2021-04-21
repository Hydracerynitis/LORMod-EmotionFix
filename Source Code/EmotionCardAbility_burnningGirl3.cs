using System;
using System.Collections.Generic;
using UnityEngine;
using LOR_DiceSystem;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_burnningGirl3: EmotionCardAbilityBase
    {
        private double DmgRate => RandomUtil.Range(3,8);
        private int Burn => RandomUtil.Range(2, 5);
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            if (this._owner.faction != Faction.Player)
                return;
            this._owner.bufListDetail.AddBuf(new BattleUnitBuf_burningGirl_Ember_New());
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            if (this._owner.faction != Faction.Player)
                return;
            BattleUnitBuf battleUnitBuf = this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_burningGirl_Ember_New));
            if (battleUnitBuf == null)
            {
                this._owner.bufListDetail.AddBuf(new BattleUnitBuf_burningGirl_Ember_New());
            }
            else
            {
                List<int> intList = battleUnitBuf is BattleUnitBuf_burningGirl_Ember_New burningGirlEmberNew ? burningGirlEmberNew.TargetIDs() : (List<int>)null;
                foreach (BattleDiceCardModel battleDiceCardModel in this._owner.allyCardDetail.GetAllDeck())
                {
                    if (intList.Contains(battleDiceCardModel.GetID()))
                    {
                        battleDiceCardModel.AddBuf(new BattleDiceCardBuf_Emotion_BurningGirl());
                        battleDiceCardModel.SetAddedIcon("Burning_Match", -1);
                    }
                }
            }
        }
        public override void OnRoundStart()
        {
            if (this._owner.faction != Faction.Enemy)
                return;
            int num = (int)((double)this._owner.MaxHp * DmgRate * 0.01);
            this._owner.LoseHp(num);
            this._owner.view.Damaged(num, BehaviourDetail.Hit,num, this._owner);
            SingletonBehavior<DiceEffectManager>.Instance.CreateBufEffect("BufEffect_Burn", this._owner.view);
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if (this._owner.faction != Faction.Enemy)
                return;
            behavior.card.target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, Burn, this._owner);
        }
        public void Destroy()
        {
            if (this._owner.faction != Faction.Player)
                return;
            BattleUnitBuf battleUnitBuf = this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_burningGirl_Ember_New));
            foreach(BattleDiceCardModel card in this._owner.allyCardDetail.GetAllDeck())
            {
                if(card.GetBufList().Find((Predicate<BattleDiceCardBuf>)(x => x is BattleDiceCardBuf_Emotion_BurningGirl)) is BattleDiceCardBuf_Emotion_BurningGirl match)
                {
                    card.RemoveAddedIcon("Burning_Match", -1);
                    match.Destroy();
                }
            }
            battleUnitBuf.Destroy();
        }
        public class BattleUnitBuf_burningGirl_Ember_New : BattleUnitBuf
        {
            private bool _triggered;
            private int _max;
            private List<int> _targetIDs = new List<int>();
            protected override string keywordId => "BurningGirl_Ember";
            protected override string keywordIconId => "Burning_Match";
            public BattleUnitBuf_burningGirl_Ember_New() => this.stack = 0;
            public override void AfterDiceAction(BattleDiceBehavior behavior)
            {
                base.AfterDiceAction(behavior);
                if (!this.CheckCondition(behavior))
                    return;
                if (this.stack >= 4 && (double)RandomUtil.valueForProb < 0.25)
                {
                    this._triggered = true;
                    this._max = behavior.GetDiceMax();
                    this._owner.TakeDamage(this._max);
                }
                else
                    this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/MatchGirl_NoBoom");
            }
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (!this.CheckCondition(behavior))
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    max = this.stack
                });
            }
            public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
            {
                base.OnUseCard(card);
                if (card == null || card.isKeepedCard)
                    return;
                if (this._targetIDs.Count < 2)
                {
                    this._targetIDs.Add(card.card.GetID());
                    foreach (BattleDiceCardModel battleDiceCardModel in this._owner.allyCardDetail.GetAllDeck())
                    {
                        if (battleDiceCardModel.GetID() == card.card.GetID())
                        {
                            battleDiceCardModel.AddBuf(new BattleDiceCardBuf_Emotion_BurningGirl());
                            battleDiceCardModel.SetAddedIcon("Burning_Match");
                        }
                    }
                }
                if (!this._targetIDs.Contains(card.card.GetID()))
                    return;
                ++this.stack;
            }
            public override void OnSuccessAttack(BattleDiceBehavior behavior)
            {
                base.OnSuccessAttack(behavior);
                if (!this.CheckCondition(behavior))
                    return;
                int stack = this.stack;
                if (stack <= 0)
                    return;
                behavior.card?.target?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, stack, this._owner);
            }
            public override void OnRollDiceInRecounter()
            {
                base.OnRollDiceInRecounter();
                if (!this._triggered)
                    return;
                SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("1/MatchGirl_Footfall", 1f, this._owner.view, (BattleUnitView)null, 2f).AttachEffectLayer();
                SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/MatchGirl_Explosion")?.SetGlobalPosition(this._owner.view.WorldPosition);
                this._triggered = false;
            }
            private bool CheckCondition(BattleDiceBehavior behavior)
            {
                BattlePlayingCardDataInUnitModel card = behavior?.card;
                return card != null && this._targetIDs.Contains(card.card.GetID()) && this.IsFirstDice(behavior);
            }
            private bool IsFirstDice(BattleDiceBehavior behavior) => behavior != null && behavior.Index == 0;
            public List<int> TargetIDs() => this._targetIDs;
        }
        public class BattleDiceCardBuf_Emotion_BurningGirl : BattleDiceCardBuf
        {
        }
    }
}
