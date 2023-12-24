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
                if (_clock == null)
                    _clock = SingletonBehavior<BattleManagerUI>.Instance.EffectLayer.GetComponentInChildren<Silence_Emotion_Clock>();
                if (_clock == null)
                {
                    Silence_Emotion_Clock original = Resources.Load<Silence_Emotion_Clock>("Prefabs/Battle/CreatureEffect/8/Silence_Emotion_Clock");
                    if (original != null)
                    {
                        Silence_Emotion_Clock silenceEmotionClock = UnityEngine.Object.Instantiate<Silence_Emotion_Clock>(original);
                        silenceEmotionClock.gameObject.transform.SetParent(SingletonBehavior<BattleManagerUI>.Instance.EffectLayer);
                        silenceEmotionClock.gameObject.transform.localPosition = new Vector3(0.0f, 800f, 0.0f);
                        silenceEmotionClock.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                    }
                    _clock = original;
                }
                return _clock;
            }
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            Init();
        }
        private void Init()
        {
            _elapsed = 0.0f;
            _bTimeLimitOvered = false;
            rolled = false;
        }
        public override void OnFixedUpdateInWaitPhase(float delta)
        {
            base.OnFixedUpdateInWaitPhase(delta);
            if (!rolled || _bTimeLimitOvered)
                return;
            Clock?.Run(_elapsed);
            _elapsed += delta;
            if ((double)_elapsed < 30.0 || SingletonBehavior<BattleCamManager>.Instance.IsCamIsReturning)
                return;
            Trigger = false;
            TriggerEnemy = true;
            _bTimeLimitOvered = true;
            Clock?.OnStartUnitMoving();
            if (!damaged)
            {
                SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Clock_StopCard");
                if (_owner.faction == Faction.Player)
                    _owner.TakeDamage((int)(_owner.hp * 0.15), DamageType.Emotion);
                damaged = true;
            }
        }
        public override void OnAfterRollSpeedDice()
        {
            base.OnAfterRollSpeedDice();
            Init();
            rolled = true;
            Trigger = true;
            TriggerEnemy = false;
            damaged = false;
            Clock?.OnStartRollSpeedDice();
            _elapsed = 0.0f;
        }
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            Clock?.OnStartUnitMoving();
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            rolled = false;
        }
        public void Destroy()
        {
            try
            {
                if (!(_clock != null))
                    return;
                UnityEngine.Object.Destroy(_clock.gameObject);
                _clock = null;
            }
            catch
            {

            }

        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if ((_owner.faction == Faction.Player && Trigger) || (_owner.faction == Faction.Enemy && TriggerEnemy))
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = Pow
                });
            }
        }
    }
}
