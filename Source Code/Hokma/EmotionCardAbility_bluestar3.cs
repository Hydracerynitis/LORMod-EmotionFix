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
        private SoundEffectPlayer _loop;
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            round = 3;
            this._owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_BlueStar_SoundBuf());
            this._owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_BlueStar_SoundBuf_Cool(round));

        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_BlueStar_SoundBuf)) == null)
                return;
            Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/9_H/FX_IllusionCard_9_H_Voice");
            if (!((UnityEngine.Object)original != (UnityEngine.Object)null))
                return;
            Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
            if (!((UnityEngine.Object)creatureEffect?.gameObject.GetComponent<AutoDestruct>() == (UnityEngine.Object)null))
                return;
            AutoDestruct autoDestruct = creatureEffect?.gameObject.AddComponent<AutoDestruct>();
            if (!((UnityEngine.Object)autoDestruct != (UnityEngine.Object)null))
                return;
            autoDestruct.time = 5f;
            autoDestruct.DestroyWhenDisable();
            SoundEffectPlayer.PlaySound("Creature/BlueStar_Atk");
            SingletonBehavior<BattleSoundManager>.Instance.EndBgm();
            if (!((UnityEngine.Object)this._loop == (UnityEngine.Object)null))
                return;
            this._loop = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/BlueStar_Bgm", true, parent: SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject.transform);
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
            DestroyLoopSound();
        }
        public override void OnEndBattlePhase()
        {
            base.OnEndBattlePhase();
            this.Destroy();
        }
        public override void OnDie(BattleUnitModel killer)
        {
            base.OnDie(killer);
            Destroy();
        }
        private void DestroyLoopSound()
        {
            if (!((UnityEngine.Object)this._loop != (UnityEngine.Object)null))
                return;
            SingletonBehavior<BattleSoundManager>.Instance.StartBgm();
            this._loop.ManualDestroy();
            this._loop = (SoundEffectPlayer)null;
        }
        public void Destroy()
        {
            DestroyLoopSound();
            BattleUnitBuf Buff = this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_BlueStar_SoundBuf));
            if (Buff != null)
                Buff.Destroy();
            Buff = this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_BlueStar_SoundBuf_Cool));
            if (Buff != null)
                Buff.Destroy();
        }
        public class BattleUnitBuf_Emotion_BlueStar_SoundBuf : BattleUnitBuf
        {
            public override string keywordId => "Emotion_BlueStar_SoundBuf";

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
            public override string keywordId => "Emotion_BlueStar_SoundBuf_Cool";
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
