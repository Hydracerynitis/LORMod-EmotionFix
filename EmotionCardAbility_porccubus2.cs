using System;
using LOR_DiceSystem;
using Battle.CameraFilter;
using System.Collections.Generic;
using Sound;
using UnityEngine;
using System.Collections;

namespace EmotionalFix
{
    public class EmotionCardAbility_porccubus2 : EmotionCardAbilityBase
    {
        private int cnt;
        private static bool Prob => (double)RandomUtil.valueForProb < 0.5;
        private static int BrkDmg => RandomUtil.Range(2, 7);
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            this.cnt = 0;
        }
        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            if (this.cnt >= 2 || !Prob)
                return;
            this._owner.battleCardResultLog?.SetTakeDamagedEvent(new BattleCardBehaviourResult.BehaviourEvent(this.Filter));
            this._owner.battleCardResultLog?.SetEmotionAbility(true, this._emotionCard, 0, ResultOption.Default, 2);
            this._owner.cardSlotDetail.RecoverPlayPoint(1);
            this._owner.allyCardDetail.DrawCards(1);
            this._owner.TakeBreakDamage(BrkDmg, DamageType.Emotion,this._owner);
            ++this.cnt;
            this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Porccu_Hit");
            this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Porccu_Nodmg");
        }
        public void Filter() => new GameObject().AddComponent<SpriteFilter_Porccubus>().Init("EmotionCardFilter/Porccubus_Filter", false);
    }
}
