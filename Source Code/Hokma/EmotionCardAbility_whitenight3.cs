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
            if (!this._effect)
            {
                this._effect = true;
                if ((UnityEngine.Object)this._aura == (UnityEngine.Object)null)
                    this._aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("9_H/FX_IllusionCard_9_H_Power", 1f, this._owner.view, this._owner.view)?.gameObject;
            }
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
            {
                if (alive != this._owner)
                {
                    BattleUnitBuf_Emotion_WhiteNight_Mighty whiteNightMighty = new BattleUnitBuf_Emotion_WhiteNight_Mighty(this._owner);
                    alive.bufListDetail.AddBuf(whiteNightMighty);
                }
            }
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this.DestroyAura();
            this._aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("9_H/FX_IllusionCard_9_H_Power", 1f, this._owner.view, this._owner.view)?.gameObject;
        }

        public override void OnDie(BattleUnitModel killer)
        {
            base.OnDie(killer);
            this.DestroyAura();
        }
        public override void OnEndBattlePhase()
        {
            base.OnEndBattlePhase();
            DestroyAura();
        }
        public void DestroyAura()
        {
            if (!((UnityEngine.Object)this._aura != (UnityEngine.Object)null))
                return;
            UnityEngine.Object.Destroy((UnityEngine.Object)this._aura);
            this._aura = (GameObject)null;
        }
        public class BattleUnitBuf_Emotion_WhiteNight_Mighty : BattleUnitBuf
        {
            private BattleUnitModel _god;
            public override bool Hide => true;
            public BattleUnitBuf_Emotion_WhiteNight_Mighty(BattleUnitModel god) => this._god = god;
            private int absorb
            {
                get
                {
                    if (this._owner.faction == Faction.Enemy)
                        return 3;
                    else if (this._god?.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is DiceCardSelfAbility_whiteNightEgo.BattleUnitBuf_whiteNight)) != null)
                        return 5;
                    else
                        return 2;
                }
            }
            public override void OnDieOtherUnit(BattleUnitModel unit)
            {
                base.OnDieOtherUnit(unit);
                if (unit == null || unit != this._god)
                    return;
                this.Destroy();
            }
            public override double ChangeDamage(BattleUnitModel attacker, double dmg)
            {
                if (dmg > (double)absorb)
                    return base.ChangeDamage(attacker, dmg);
                this._owner.RecoverHP(Mathf.RoundToInt((float)dmg));
                return 0.0;
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }
    }
}
