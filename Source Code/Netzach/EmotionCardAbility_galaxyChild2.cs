using System;
using LOR_DiceSystem;
using Battle.CameraFilter;
using System.Collections.Generic;
using Sound;
using UnityEngine;
using System.Collections;

namespace EmotionalFix
{
    public class EmotionCardAbility_galaxyChild2 : EmotionCardAbilityBase
    {
        private Battle.CreatureEffect.CreatureEffect _effect;
        private List<Battle.CreatureEffect.CreatureEffect> _damagedEffects = new List<Battle.CreatureEffect.CreatureEffect>();
        public override void OnBreakState()
        {
            int num = (int)((double)this._owner.breakDetail.GetDefaultBreakGauge() * 0.35);
            if (num >= 20)
                num = 20;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
            {
                if (alive == this._owner || alive.IsBreakLifeZero())
                    break;
                alive.TakeBreakDamage(num);
                alive.view.BreakDamaged(num, BehaviourDetail.Penetrate, this._owner, AtkResist.Normal);
                Battle.CreatureEffect.CreatureEffect creatureEffect = SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("4/GalaxyBoy_Damaged", 1f, alive.view, (BattleUnitView)null, 2f);
                creatureEffect.SetLayer("Character");
                this._damagedEffects.Add(creatureEffect);
                creatureEffect.gameObject.SetActive(false);
                creatureEffect.SetLayer("Effect");
            }
            this._effect = this.MakeEffect("4/GalaxyBoy_Dust", destroyTime: 3f);
            this._effect?.gameObject.SetActive(false);
            this._effect?.AttachEffectLayer();
            SoundEffectPlayer.PlaySound("Creature/GalaxyBoy_Cry");
        }
        public override void OnPrintEffect(BattleDiceBehavior behavior)
        {
            if (!(bool)(UnityEngine.Object)this._effect)
                return;
            this._effect.gameObject.SetActive(true);
            this._effect = (Battle.CreatureEffect.CreatureEffect)null;
            foreach (Component damagedEffect in this._damagedEffects)
                damagedEffect.gameObject.SetActive(true);
            this._damagedEffects.Clear();
        }
    }
}
