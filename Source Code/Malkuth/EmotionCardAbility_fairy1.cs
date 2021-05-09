using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_fairy1 : EmotionCardAbilityBase
    {
        private int _takendmg;
        private int DmgThreshold => Mathf.Min((int)((double)this._owner.MaxHp * 0.25),30);
        private int _count;
        private CreatureEffect_Anim _effect;
        private bool _hit;
        private bool _destroy;
        private int _hitCount;
        public override void OnSelectEmotion()
        {
            try
            {
                this._effect = this.MakeEffect("1/Fairy_Gluttony") as CreatureEffect_Anim;
                this._effect?.SetLayer("Character");
                SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Fariy_Special")?.SetGlobalPosition(this._owner.view.WorldPosition);
            }
            catch 
            {

            }
        }
        public override void OnRoundEnd()
        {
            if (this._count < 3)
            {
                int num = Mathf.Min((int)((double)this._owner.MaxHp * 0.2), 24);
                this._owner.RecoverHP(num);
                ++this._count;
                if ((UnityEngine.Object)this._effect != (UnityEngine.Object)null)
                    this._effect.SetTrigger("Recover");
            }
            if (this._count != 3)
                return;
            if ((UnityEngine.Object)this._effect != (UnityEngine.Object)null)
                this._effect.SetTrigger("Disappear");
            this._effect = (CreatureEffect_Anim)null;
        }
        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            if (this._takendmg >= DmgThreshold || this._count >= 3)
                return false;
            this._takendmg+=dmg;
            if (this._takendmg >= DmgThreshold)
            {
                this._count = 5;
                int dmg1 = this._owner.LoseHp(Mathf.Min((int)this._owner.hp * 25 / 100, 30));
                this._owner.battleCardResultLog?.SetDamageTaken(dmg1, dmg, BehaviourDetail.Slash);
                this._owner.battleCardResultLog?.SetEmotionAbility(true, this._emotionCard, 1, ResultOption.Default, dmg1);
                this._destroy = true;
                if ((bool)(UnityEngine.Object)this._effect)
                    this.ApplyCreatureEffect((Battle.CreatureEffect.CreatureEffect)this._effect);
            }
            else
            {
                this._hit = true;
                ++this._hitCount;
                if ((bool)(UnityEngine.Object)this._effect)
                    this.ApplyCreatureEffect((Battle.CreatureEffect.CreatureEffect)this._effect);
            }
            this._owner?.battleCardResultLog?.SetCreatureEffectSound("Creature/Fairy_Dead");
            return false;
        }

        public override void OnPrintEffect(BattleDiceBehavior behavior)
        {
            if (this._hit)
            {
                if ((UnityEngine.Object)this._effect != (UnityEngine.Object)null)
                    this._effect.SetTrigger("Hit");
                --this._hitCount;
                if (this._hitCount == 0)
                    this._hit = false;
            }
            if (!this._destroy)
                return;
            this._destroy = false;
            if ((UnityEngine.Object)this._effect != (UnityEngine.Object)null)
                this._effect.SetTrigger("Disappear");
            this._effect = (CreatureEffect_Anim)null;
        }

        public override void OnLayerChanged(string layerName)
        {
            if (!((UnityEngine.Object)this._effect == (UnityEngine.Object)null))
                return;
            this._effect.SetLayer(layerName);
        }
    }
}
