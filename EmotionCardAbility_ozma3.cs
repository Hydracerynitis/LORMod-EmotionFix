using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_ozma3 : EmotionCardAbilityBase
    {
        private bool _activated;
        public override void OnRoundStart()
        {
            if (this._activated)
            {
                this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_ozma3.BattleUnitBuf_ozmaReviveCheck))?.Destroy();
            }
            else
            {
                if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_ozma3.BattleUnitBuf_ozmaReviveCheck)) != null)
                    return;
                this._owner.bufListDetail.AddBuf((BattleUnitBuf)new EmotionCardAbility_ozma3.BattleUnitBuf_ozmaReviveCheck());
            }
        }
        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            if (this._activated || (double)this._owner.hp > (double)dmg)
                return false;
            this._activated = true;
            this._owner.RecoverHP((int)(double)this._owner.MaxHp);
            this._owner.breakDetail.RecoverBreakLife(this._owner.MaxBreakLife);
            this._owner.breakDetail.nextTurnBreak = false;
            this._owner.breakDetail.RecoverBreak(this._owner.breakDetail.GetDefaultBreakGauge());
            this._owner.cardSlotDetail.LosePlayPoint(this._owner.cardSlotDetail.GetMaxPlayPoint());
            return true;
        }
        public void Destroy()
        {
            if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_ozmaReviveCheck)) is BattleUnitBuf_ozmaReviveCheck revive)
                revive.Destroy();
        }
        public class BattleUnitBuf_ozmaReviveCheck : BattleUnitBuf
        {
            protected override string keywordId => "Ozma_revive";
            protected override string keywordIconId => "Ozma_AwakenPumpkin";
            public BattleUnitBuf_ozmaReviveCheck() => this.stack = 0;
        }
    }
}
