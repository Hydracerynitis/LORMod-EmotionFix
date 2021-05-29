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
    public class EmotionCardAbility_bluestar1 : EmotionCardAbilityBase
    {
        private int damage => RandomUtil.Range(2, 4);
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            base.BeforeGiveDamage(behavior);
            if (behavior == null)
                return;
            int dmg = damage;
            this._owner.LoseHp(dmg);
            this._owner.view.Damaged(dmg, BehaviourDetail.None, dmg, this._owner);
            this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("9_H/FX_IllusionCard_9_H_Martyr", 3f);
            SoundEffectPlayer.PlaySound("Creature/BlueStar_In");
            double ratio = 1 - (this._owner.hp / this._owner.MaxHp);
            double breakrate = ratio * 10 / 9;
            if (breakrate >= 1)
                breakrate = 1;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                breakRate = (int)(breakrate * 100)
            });
            behavior.card.target.battleCardResultLog?.SetNewCreatureAbilityEffect("9_H/FX_IllusionCard_9_H_MartyrExplo", 3f);
        }
    }
}
