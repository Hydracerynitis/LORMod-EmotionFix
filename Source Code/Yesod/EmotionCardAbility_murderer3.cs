using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_murderer3 : EmotionCardAbilityBase
    {
        private static int BrkDmg => RandomUtil.Range(3, 7);
        private static int BrkDmg_Enemy => RandomUtil.Range(2, 5);
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null) 
                return;
            if (behavior.DiceVanillaValue < behavior.GetDiceMax())
                return;
            if (this._owner.faction == Faction.Enemy)
            {
                target.battleCardResultLog?.SetCreatureAbilityEffect("2/Murderer_Emotion_Thud", 1.5f);
                target.TakeBreakDamage(BrkDmg_Enemy, DamageType.Attack,this._owner);
            }
            if(this._owner.faction==Faction.Player && behavior.Detail == BehaviourDetail.Hit)
            {
                target.battleCardResultLog?.SetCreatureAbilityEffect("2/Murderer_Emotion_Thud", 1.5f);
                target.TakeBreakDamage(BrkDmg,DamageType.Attack, this._owner);
            }
        }
    }
}
