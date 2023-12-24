using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using HarmonyLib;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_whitenight3 : EmotionCardAbilityBase
    {
        private bool _effect;
        private GameObject _aura;
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            _effect = false;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (!_effect)
            {
                _effect = true;
                if (_aura == null)
                    _aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("9_H/FX_IllusionCard_9_H_Power", 1f, _owner.view, _owner.view)?.gameObject;
            }
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(_owner.faction))
            {
                if (alive != _owner)
                {
                    BattleUnitBuf_Emotion_WhiteNight_Mighty whiteNightMighty = new BattleUnitBuf_Emotion_WhiteNight_Mighty(_owner);
                    alive.bufListDetail.AddBuf(whiteNightMighty);
                }
            }
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            DestroyAura();
            _aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("9_H/FX_IllusionCard_9_H_Power", 1f, _owner.view, _owner.view)?.gameObject;
        }

        public override void OnDie(BattleUnitModel killer)
        {
            base.OnDie(killer);
            DestroyAura();
        }
        public override void OnEndBattlePhase()
        {
            base.OnEndBattlePhase();
            DestroyAura();
        }
        public void DestroyAura()
        {
            if (!(_aura != null))
                return;
            UnityEngine.Object.Destroy(_aura);
            _aura = (GameObject)null;
        }
        public class BattleUnitBuf_Emotion_WhiteNight_Mighty : BattleUnitBuf
        {
            private BattleUnitModel _god;
            public override bool Hide => true;
            public BattleUnitBuf_Emotion_WhiteNight_Mighty(BattleUnitModel god) => _god = god;
            private int absorb
            {
                get
                {
                    if (_owner.faction == Faction.Enemy)
                        return 3;
                    else if (_god?.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is DiceCardSelfAbility_whiteNightEgo.BattleUnitBuf_whiteNight)) != null)
                        return 5;
                    else
                        return 2;
                }
            }
            public override void OnDieOtherUnit(BattleUnitModel unit)
            {
                base.OnDieOtherUnit(unit);
                if (unit == null || unit != _god)
                    return;
                Destroy();
            }
            public override double ChangeDamage(BattleUnitModel attacker, double dmg)
            {
                if (dmg > (double)absorb)
                    return base.ChangeDamage(attacker, dmg);
                _owner.RecoverHP(Mathf.RoundToInt((float)dmg));
                return 0.0;
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
    }
}
