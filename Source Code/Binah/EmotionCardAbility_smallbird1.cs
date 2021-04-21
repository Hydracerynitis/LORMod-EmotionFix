using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_smallbird1 : EmotionCardAbilityBase
    {
        private bool dmged;
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this.dmged = false;
        }
        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            base.BeforeTakeDamage(attacker, dmg);
            if (this._owner.IsImmuneDmg() || this._owner.IsInvincibleHp(null))
                return false;
            if (this._owner.faction == Faction.Enemy && dmg < (int)((double)this._owner.MaxHp * 0.02))
                return false;
            this.dmged = true;
            return false;
        }
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            if (this.dmged)
                this._owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_SmallBird_Punish());
            this.dmged = false;
        }
        public class BattleUnitBuf_Emotion_SmallBird_Punish : BattleUnitBuf
        {
            private static int Pow => RandomUtil.Range(2, 4);
            protected override string keywordId => "SmallBird_Punishment";
            protected override string keywordIconId => "SmallBird_Emotion_Punish";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                this.stack = 0;
            }
            public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
            {
                base.OnUseCard(curCard);
                curCard.ApplyDiceStatBonus(DiceMatch.NextDice, new DiceStatBonus()
                {
                    power = Pow
                });
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }
    }
}
