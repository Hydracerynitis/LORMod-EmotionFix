using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;
using System.IO;

namespace EmotionalFix
{
    public class EmotionCardAbility_silence1 : EmotionCardAbilityBase
    {
        private bool damaged;
        private bool rolled;
        private float _elapsed;
        private bool _bTimeLimitOvered;
        private bool Trigger;
        private bool TriggerEnemy;
        private Silence_Emotion_Clock _clock;
        private static int Pow => RandomUtil.Range(1, 2);
        private Silence_Emotion_Clock Clock
        {
            get
            {
                if ((UnityEngine.Object)this._clock == (UnityEngine.Object)null)
                    this._clock = SingletonBehavior<BattleManagerUI>.Instance.EffectLayer.GetComponentInChildren<Silence_Emotion_Clock>();
                if ((UnityEngine.Object)this._clock == (UnityEngine.Object)null)
                {
                    Silence_Emotion_Clock original = Resources.Load<Silence_Emotion_Clock>("Prefabs/Battle/CreatureEffect/8/Silence_Emotion_Clock");
                    if ((UnityEngine.Object)original != (UnityEngine.Object)null)
                    {
                        Silence_Emotion_Clock silenceEmotionClock = UnityEngine.Object.Instantiate<Silence_Emotion_Clock>(original);
                        silenceEmotionClock.gameObject.transform.SetParent(SingletonBehavior<BattleManagerUI>.Instance.EffectLayer);
                        silenceEmotionClock.gameObject.transform.localPosition = new Vector3(0.0f, 800f, 0.0f);
                        silenceEmotionClock.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                    }
                    this._clock = original;
                }
                return this._clock;
            }
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this.Init();
        }
        private void Init()
        {
            this._elapsed = 0.0f;
            this._bTimeLimitOvered = false;
            this.rolled = false;
        }
        public override void OnFixedUpdateInWaitPhase(float delta)
        {
            base.OnFixedUpdateInWaitPhase(delta);
            if (!this.rolled || this._bTimeLimitOvered)
                return;
            this.Clock?.Run(this._elapsed);
            this._elapsed += delta;
            if ((double)this._elapsed < 30.0 || SingletonBehavior<BattleCamManager>.Instance.IsCamIsReturning)
                return;
            this.Trigger = false;
            this.TriggerEnemy = true;
            this._bTimeLimitOvered = true;
            this.Clock?.OnStartUnitMoving();
            if (!damaged)
            {
                SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Clock_StopCard");
                if (this._owner.faction == Faction.Player)
                    this._owner.TakeDamage((int)(this._owner.hp * 0.15), DamageType.Emotion);
                damaged = true;
            }
        }
        public override void OnAfterRollSpeedDice()
        {
            base.OnAfterRollSpeedDice();
            this.Init();
            this.rolled = true;
            this.Trigger = true;
            this.TriggerEnemy = false;
            this.damaged = false;
            this.Clock?.OnStartRollSpeedDice();
            this._elapsed = 0.0f;
        }
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            this.Clock?.OnStartUnitMoving();
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            this.rolled = false;
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if ((this._owner.faction == Faction.Player && Trigger) || (this._owner.faction == Faction.Enemy && TriggerEnemy))
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = Pow
                });
            }
        }
    }
}
