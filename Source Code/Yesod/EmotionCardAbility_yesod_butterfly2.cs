using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix.Yesod
{
    public class EmotionCardAbility_yesod_butterfly2 : EmotionCardAbilityBase
    {
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            DiceEffectManager.Instance.CreateNewFXCreatureEffect("2_Y/FX_IllusionCard_2_Y_Fly", 1f, _owner.view, _owner.view, 2f);
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || !IsAttackDice(behavior.Detail))
                return;
            double Ratio = (target.breakDetail.GetDefaultBreakGauge() - target.breakDetail.breakGauge) / target.breakDetail.GetDefaultBreakGauge() - 0.25;
            int dmg = (int)(Ratio * 14);
            if (dmg <= 2)
                dmg = 2;
            if (dmg >= 7)
                dmg = 7;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                breakDmg = dmg
            });
        }

        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            behavior?.card?.target?.battleCardResultLog?.SetCreatureAbilityEffect("2/ButterflyEffect_White", 1f);
            _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/ButterFlyMan_Atk_White");
        }
    }
}
