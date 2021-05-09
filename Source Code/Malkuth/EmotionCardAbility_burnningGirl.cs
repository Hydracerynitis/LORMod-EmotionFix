using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_burnningGirl: EmotionCardAbilityBase
    {
        private Battle.CreatureEffect.CreatureEffect _effect;
        public override void OnStartCardAction(BattlePlayingCardDataInUnitModel curCard)
        {
            if (this._owner.faction != Faction.Player)
                return;
            if ((double)this._owner.hp > (double)this._owner.MaxHp * 0.2)
                return;
            int num = Mathf.Min((int)((double)this._owner.MaxHp * 0.3), 36);
            List<BattleUnitModel> alive = BattleObjectManager.instance.GetAliveList(Faction.Enemy);
            foreach (BattleUnitModel unit in alive)
                unit.TakeDamage(num);
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/MachGirl_Explosion")?.SetGlobalPosition(this._owner.view.WorldPosition);
            this._owner.Die();
            SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(this._owner, this._owner.faction, 0.0f, this._owner.breakDetail.breakGauge);
            this._effect = this.MakeEffect("1/MatchGirl_Footfall", destroyTime: 2f, apply: false);
            this._effect.AttachEffectLayer();
            try
            {
                this._owner.view.StartDeadEffect(false);
            }
            catch
            {

            }
        }
        public override void OnRoundStart()
        {
            if (this._owner.faction != Faction.Enemy)
                return;
            this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Binding, 3);
        }
        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            if (this._owner.faction != Faction.Enemy)
                return;
            if ((double)dmg < (double)this._owner.MaxHp * 0.25)
                return;
            atkDice.owner.TakeDamage(dmg);
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/MachGirl_Explosion")?.SetGlobalPosition(this._owner.view.WorldPosition);
            SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(atkDice.owner, atkDice.owner.faction, atkDice.owner.hp, atkDice.owner.breakDetail.breakGauge);
            this._effect = this.MakeEffect("1/MatchGirl_Footfall", destroyTime: 2f, apply: false);
            this._effect.AttachEffectLayer(); 
        }
        public override void OnPrintEffect(BattleDiceBehavior behavior) => this._effect = (Battle.CreatureEffect.CreatureEffect)null;
        public override void OnSelectEmotion() => SoundEffectPlayer.PlaySound("Creature/MatchGirl_Cry");
    }
}
