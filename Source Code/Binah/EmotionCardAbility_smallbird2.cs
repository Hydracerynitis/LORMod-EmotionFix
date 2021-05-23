using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_smallbird2 : EmotionCardAbilityBase
    {
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            if (this._owner.history.takeDamageAtOneRound > 0)
                return;
            GiveBuf();
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (behavior.TargetDice == null)
                return;
            behavior.TargetDice.ApplyDiceStatBonus(new DiceStatBonus()
            {
                power = -1
            });
        }
        private void GiveBuf()
        {
            if (this._owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Emotion_SmallBird_Buri) != null)
                return;
            this._owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_SmallBird_Buri());
        }

        public class BattleUnitBuf_Emotion_SmallBird_Buri : BattleUnitBuf
        {
            private int Dmg => RandomUtil.Range(2, 4);
            protected override string keywordId => "Smallbird_Beak";
            protected override string keywordIconId => "SmallBird_Emotion_Buri";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                this.stack = 0;
            }
            public override void BeforeGiveDamage(BattleDiceBehavior behavior)
            {
                base.BeforeGiveDamage(behavior);
                int dmg = Dmg;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    dmg = dmg,
                    breakDmg = dmg
                });
                behavior.card.target.battleCardResultLog?.SetNewCreatureAbilityEffect("8_B/FX_IllusionCard_8_B_Attack", 2f);
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }
    }
}
