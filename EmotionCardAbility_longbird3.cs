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
            this._owner.battleCardResultLog?.SetEmotionAbility(true, this._emotionCard, 0, ResultOption.Sign, 3);
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                min = -3,
                max = +3
            });
        }
    }
}
