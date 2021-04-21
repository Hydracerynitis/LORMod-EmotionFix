using System;
using LOR_DiceSystem;
using Battle.CameraFilter;
using System.Collections.Generic;
using Sound;
using UnityEngine;
using System.Collections;

namespace EmotionalFix
{
    public class EmotionCardAbility_fragmentSpace2 : EmotionCardAbilityBase
    {
        private List<BattleUnitModel> victim = new List<BattleUnitModel>();
        private Battle.CreatureEffect.CreatureEffect _hitEffect;
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (this._owner.faction == Faction.Enemy && victim.Count != 0 && victim.Contains(behavior.card.target))
                return;
            if (behavior.Detail != BehaviourDetail.Penetrate && this._owner.faction==Faction.Player)
                return;
            this._hitEffect = this.MakeEffect("4/Fragment_Hit", destroyTime: 1f);
            this._hitEffect?.gameObject.SetActive(false);
            if (behavior.card.target == null)
                return;
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Cosmos_Hit")?.SetGlobalPosition(behavior.card.target.view.WorldPosition);
        }

        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if (behavior.Detail != BehaviourDetail.Penetrate && this._owner.faction == Faction.Player)
                return;
            behavior.card.target.TakeBreakDamage((int)((double)behavior.card.target.breakDetail.breakGauge * 0.15));
            if (this._owner.faction == Faction.Enemy)
                victim.Add(behavior.card.target);
        }
        public override void OnRoundEnd()
        {
            victim.Clear();
        }
        public override void OnPrintEffect(BattleDiceBehavior behavior)
        {
            if (!(bool)(UnityEngine.Object)this._hitEffect)
                return;
            this._hitEffect.gameObject.SetActive(true);
            this._hitEffect = (Battle.CreatureEffect.CreatureEffect)null;
        }
    }
}
