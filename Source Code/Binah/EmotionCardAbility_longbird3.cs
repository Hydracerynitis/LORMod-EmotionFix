using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_longbird3 : EmotionCardAbilityBase
    {
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            _owner.battleCardResultLog?.SetEmotionAbility(true, _emotionCard, 0, ResultOption.Sign, 3);
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                min = -3,
                max = +5
            });
        }
        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            if (behavior.DiceVanillaValue > behavior.GetDiceMin() && behavior.DiceVanillaValue<behavior.GetDiceMax())
                return;
            base.OnRollDice(behavior);
            _owner.battleCardResultLog?.SetNewCreatureAbilityEffect("8_B/FX_IllusionCard_8_B_Judgement", 3f);
            _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/LongBird_Stun");
        }
    }
}
