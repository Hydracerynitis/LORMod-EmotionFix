using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_longbird2 : EmotionCardAbilityBase
    {
        private bool Prob => RandomUtil.valueForProb <= 0.5;
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
            {
                if (!Prob)
                    continue;
                BattleUnitBuf buf = alive.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_LongBird_Emotion_Sin));
                if (buf == null)
                {
                    buf = new BattleUnitBuf_LongBird_Emotion_Sin();
                    alive.bufListDetail.AddBuf(buf);
                }
                ++buf.stack;
            }
        }
        public void Destroy()
        {
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList())
            {
                if (unit.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_LongBird_Emotion_Sin)) is BattleUnitBuf_LongBird_Emotion_Sin sin)
                    sin.Destroy();
            }
        }
        public class BattleUnitBuf_LongBird_Emotion_Sin : BattleUnitBuf
        {
            private bool triggered;
            protected override string keywordId => "Sin_AbnormalityCard";
            protected override string keywordIconId => "Sin_Abnormality";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                this.stack = 0;
            }
            public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
            {
                base.OnUseCard(card);
                this.triggered = false;
            }
            public override void OnSuccessAttack(BattleDiceBehavior behavior)
            {
                base.OnSuccessAttack(behavior);
                BattleUnitModel target = behavior.card?.target;
                if (target == null || this.triggered || this.stack <= 0)
                    return;
                --this.stack;
                this.triggered = true;
                BattleUnitBuf buf = target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_LongBird_Emotion_Sin));
                if (buf == null)
                {
                    buf = new BattleUnitBuf_LongBird_Emotion_Sin();
                    target.bufListDetail.AddBuf(buf);
                }
                ++buf.stack;
            }
            public override void OnRoundEndTheLast()
            {
                base.OnRoundEndTheLast();
                if (this.stack < 4)
                    return;
                this._owner.TakeDamage(Mathf.RoundToInt((float)this._owner.MaxHp * 0.1f), DamageType.Buf,this._owner);
                SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("8_B/FX_IllusionCard_8_B_Judgement", 1f, this._owner.view, this._owner.view, 3f);
                SoundEffectPlayer.PlaySound("Creature/LongBird_Down");
                this.Destroy();
            }
        }
    }
}
