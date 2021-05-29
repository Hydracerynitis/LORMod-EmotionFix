using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_nosferatu2 : EmotionCardAbilityBase
    {
        private bool _trigger;
        public override void OnKill(BattleUnitModel target)
        {
            base.OnKill(target);
            if (this._owner.faction==Faction.Player && target.bufListDetail.GetActivatedBuf(KeywordBuf.Bleeding) == null)
                return;
            target.battleCardResultLog?.SetNewCreatureAbilityEffect("6_G/FX_IllusionCard_6_G_TeathATK");
            target.battleCardResultLog?.SetCreatureAbilityEffect("6/Nosferatu_Emotion_BloodDrain");
            target.battleCardResultLog?.SetCreatureEffectSound("Creature/Nosferatu_Change");
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
                alive.RecoverHP(10);
            this._trigger = true;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (!_trigger)
                return;
            this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 2);
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction).FindAll(x=> x!=this._owner))
                alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak, 2);
            this._trigger = false;
        }
    }
}

