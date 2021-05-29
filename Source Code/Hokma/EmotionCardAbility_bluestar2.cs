﻿using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;

namespace EmotionalFix
{
    public class EmotionCardAbility_bluestar2 : EmotionCardAbilityBase
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
                    this._owner.TakeDamage(this.Dmg, DamageType.Emotion, this._owner);
                    SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("9_H/FX_IllusionCard_9_H_JudgementExplo", 1f, alive.view, alive.view, 2f);
                    SoundEffectPlayer.PlaySound("Creature/BlueStar_Suicide");
                }
            }
            if (!this.triggered)
                return;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
                alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 1, this._owner);
        }
    }
}
