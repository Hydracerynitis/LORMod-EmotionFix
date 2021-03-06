using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_lumberjack3 : EmotionCardAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            behavior.card.target.battleCardResultLog?.SetNewCreatureAbilityEffect("7_C/FX_IllusionCard_7_C_Bloodmeet", 2f);
            behavior.card.target.battleCardResultLog?.SetCreatureEffectSound("Creature/WoodMachine_Kill");
            this._owner.cardSlotDetail.RecoverPlayPoint(1);
        }
        public override void OnRoundEnd()
        {
            if (this._owner.cardSlotDetail.PlayPoint < this._owner.cardSlotDetail.GetMaxPlayPoint())
            {
                this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Weak, 2);
                this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Disarm, 2);
            }
        }
    }
}
