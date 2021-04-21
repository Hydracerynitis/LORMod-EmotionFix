using System;
using Battle.CreatureEffect;
using LOR_DiceSystem;
using UI;
using Sound;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_bloodbath3 : EmotionCardAbilityBase
    {
        private BattleUnitModel _target;
        private int _stack;
        public override int GetCounter() => this._stack;
        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            base.OnRollDice(behavior);
            if (!this.IsAttackDice(behavior.Detail) || this._target == null || this._target == behavior.card.target)
                return;
            if (this._target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BloodBath_HandDebuf)) is BloodBath_HandDebuf bloodBathHandDebuf)
                this._target.bufListDetail.RemoveBuf(bloodBathHandDebuf);
            this._target = null;
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            if (this._target != behavior.card.target)
            {
                this._target = behavior.card.target;
                this._stack = 1;
                this._target.bufListDetail.AddBuf(new BloodBath_HandDebuf());
                if (!(this._target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BloodBath_HandDebuf)) is BloodBath_HandDebuf bloodBathHandDebuf))
                    return;
                bloodBathHandDebuf.OnHit();
            }
            else
            {
                ++this._stack;
                if (this._target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BloodBath_HandDebuf)) is BloodBath_HandDebuf bloodBathHandDebuf)
                    bloodBathHandDebuf.OnHit();
                if (this._stack < 3)
                    return;
                this.Ability();
            }
        }

        private void Ability()
        {
            if (this._target == null)
                return;
            if(this._owner.faction==Faction.Player)
                this._target.TakeBreakDamage(RandomUtil.Range(3, 10), DamageType.Emotion,this._owner);
            if(this._owner.faction==Faction.Enemy)
                this._target.TakeBreakDamage(RandomUtil.Range(3, 12), DamageType.Emotion,this._owner);
            if (this._target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BloodBath_HandDebuf)) is BloodBath_HandDebuf bloodBathHandDebuf)
                this._target.bufListDetail.RemoveBuf(bloodBathHandDebuf);
            this._target.battleCardResultLog?.SetCreatureAbilityEffect("0/BloodyBath_PaleHand_Hit", 3f);
            this._target = null;
            this._stack = 0;
        }
        public void Destroy()
        {
            if (this._target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BloodBath_HandDebuf)) is BloodBath_HandDebuf bloodBathHandDebuf)
                this._target.bufListDetail.RemoveBuf(bloodBathHandDebuf);
            this._target = null;
            this._stack = 0;
        }

        public class BloodBath_HandDebuf : BattleUnitBuf
        {
            protected override string keywordId => "Ability/BloodBath_Hand";
            public BloodBath_HandDebuf() => this.stack = 0;
            public void OnHit() => ++this.stack;
        }
    }
}
