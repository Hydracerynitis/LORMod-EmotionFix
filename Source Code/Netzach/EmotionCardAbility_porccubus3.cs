using System;
using LOR_DiceSystem;
using Battle.CameraFilter;
using System.Collections.Generic;
using Sound;
using UnityEngine;
using System.Collections;

namespace EmotionalFix
{
    public class EmotionCardAbility_porccubus3 : EmotionCardAbilityBase
    {
        private static int RecoverBP => RandomUtil.Range(2, 4);
        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            this._owner.battleCardResultLog?.SetTakeDamagedEvent(new BattleCardBehaviourResult.BehaviourEvent(this.Filter));
            if (this._owner.IsBreakLifeZero())
                return;
            int recoverBp = RecoverBP;
            this._owner.battleCardResultLog?.SetEmotionAbility(false, this._emotionCard, 0, ResultOption.Default, Array.Empty<int>());
            this._owner.breakDetail.RecoverBreak(recoverBp);
            this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Porccu_Hit");
        }
        public void Filter() => new GameObject().AddComponent<SpriteFilter_Porccubus>().Init("EmotionCardFilter/Porccubus_Filter", false);
        public override int GetDamageReduction(BattleDiceBehavior behavior) => -RandomUtil.Range(1, 3);
    }
}
