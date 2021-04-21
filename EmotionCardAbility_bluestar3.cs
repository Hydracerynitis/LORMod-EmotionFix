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
    public class EmotionCardAbility_bluestar3 : EmotionCardAbilityBase
    {
        private int round;
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            round = 3;
            this._owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_BlueStar_SoundBuf());
            this._owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_BlueStar_SoundBuf_Cool(round));

        }

        public override void OnWaveStart()
        {
            base.OnWaveStart();
            if(round==3)
                this._owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_BlueStar_SoundBuf());
            this._owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_BlueStar_SoundBuf_Cool(round));
        }
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            round--;
            if (round <= 0)
                round = 3;
        }
        public void Destroy()
        {
            BattleUnitBuf Buff = this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_BlueStar_SoundBuf));
            if (Buff != null)
                Buff.Destroy();
            Buff = this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_BlueStar_SoundBuf_Cool));
            if (Buff != null)
                Buff.Destroy();
        }
        public class BattleUnitBuf_Emotion_BlueStar_SoundBuf : BattleUnitBuf
        {
            protected override string keywordId => "Emotion_BlueStar_SoundBuf";

            private int BDmg => RandomUtil.Range(2, 4);

            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                this.stack = 0;
            }
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (behavior == null)
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    breakDmg = this.BDmg
                });
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }
        public class BattleUnitBuf_Emotion_BlueStar_SoundBuf_Cool : BattleUnitBuf
        {
            protected override string keywordId => "Emotion_BlueStar_SoundBuf_Cool";
            public BattleUnitBuf_Emotion_BlueStar_SoundBuf_Cool(int cooldown)
            {
                this.Init(this._owner);
                this.stack = cooldown;
                if (stack >= 3)
                    stack = 3;
            }
            public override void OnRoundEndTheLast()
            {
                base.OnRoundEndTheLast();
                --this.stack;
                if (this.stack > 0)
                    return;
                this.stack = 3;
                this._owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_BlueStar_SoundBuf());
            }
        }
    }
}
