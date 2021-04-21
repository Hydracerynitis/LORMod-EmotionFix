using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_smallbird3 : EmotionCardAbilityBase
    {
        private int Dmg => RandomUtil.Range(2, 5);
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            BattleUnitModel target = behavior.card.target;
            if (target == null || target.IsDead() || target.history.damageAtOneRound <= 0)
                return;
            target.TakeDamage(Dmg);
        }
    }
}
