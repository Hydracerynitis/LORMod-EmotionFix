using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;

namespace EmotionalFix
{
    public class EmotionCardAbility_bluetree2 : EmotionCardAbilityBase
    {
        private bool triggered;
        private int Dmg => RandomUtil.Range(3, 7);
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            this.triggered = false;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
            {
                if (alive.IsBreakLifeZero())
                {
                    this.triggered = true;
                    alive.TakeDamage(this.Dmg, DamageType.Emotion, this._owner);
                }
            }
            if (!this.triggered)
                return;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
                alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 1, this._owner);
        }
    }
}
