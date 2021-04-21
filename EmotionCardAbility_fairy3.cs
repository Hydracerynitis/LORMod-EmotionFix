using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_fairy3 : EmotionCardAbilityBase
    {
        private int Threshold => Mathf.Min((int)((double)this._owner.MaxHp * 0.1), 12);
        private int HealThreshold => Mathf.Min((int)((double)this._owner.MaxHp * 0.2), 24);
        private int _healing;
        private bool _hungry;
        public override void OnSelectEmotion()
        {
            _healing = 0;
        }
        public override void CheckDmg(int dmg, BattleUnitModel target)
        {
            int heal = (int)((double)dmg / 2);
            if (_healing >= HealThreshold)
                return;
            if (heal + _healing >= HealThreshold)
                heal = heal + _healing - HealThreshold;
            _healing += heal;
            this._owner.RecoverHP(heal);
        }
        public override void OnRoundStart()
        {
            _hungry = false;
            if (_healing < Threshold)
            {
                int dmg = (int)((double)this._owner.MaxHp * 0.1);
                this._owner.TakeDamage(Mathf.Min(dmg, 12));
                SoundEffectPlayer.PlaySound("Creature/Fairy_QueenEat");
                SoundEffectPlayer.PlaySound("Creature/Fairy_QueenChange");
                this.SetFilter("1/Fairy_Filter");
                _hungry = true;
            }
            _healing = 0;
        }
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            if (!_hungry)
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                dmgRate = 30
            });
        }
    }
}
