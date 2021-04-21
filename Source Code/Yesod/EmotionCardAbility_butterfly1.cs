using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_butterfly1 : EmotionCardAbilityBase
    {
        private bool trigger;
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            this.trigger = false;
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || !this.IsAttackDice(behavior.Detail))
                return;
            double Ratio = ((double)target.MaxHp - (double)target.hp) / (double)target.MaxHp - 0.25;
            if (Ratio > 0.25)
                this.trigger = true;
            int dmg = (int)(Ratio * 14);
            if (dmg <= 0)
                dmg = 0;
            if (dmg >= 7)
                dmg = 7;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                dmg = dmg
            });
            this.trigger = true;
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            if (!this.trigger)
                return;
            behavior?.card?.target?.battleCardResultLog?.SetCreatureAbilityEffect("2/ButterflyEffect_Black", 1f);
            this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/ButterFlyMan_Atk_Black");
            this.trigger = false;
        }
    }
}
