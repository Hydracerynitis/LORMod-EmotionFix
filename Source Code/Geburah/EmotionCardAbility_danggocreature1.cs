using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_danggocreature1 : EmotionCardAbilityBase
    {
        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            if (unit == this._owner)
                return;
            int Hp = (int)((double)this._owner.MaxHp * 0.2);
            if (Hp >= 30)
                Hp = 30;
            this._owner.RecoverHP(Hp);
            int Break= (int)((double)this._owner.breakDetail.GetDefaultBreakGauge() * 0.2);
            if (Break >= 30)
                Break = 30;
            this._owner.breakDetail.RecoverBreak(Break);
            if (this._owner.IsBreakLifeZero())
            {
                this._owner.breakDetail.nextTurnBreak = false;
                this._owner.RecoverBreakLife(1);
            }
            this._owner.cardSlotDetail.RecoverPlayPoint(1);
        }
    }
}
