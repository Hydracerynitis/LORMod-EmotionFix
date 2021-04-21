using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_redhood1 : EmotionCardAbilityBase
    {
        private BattleUnitModel _target;
        private static int Dmg => RandomUtil.Range(2, 6);
        public override void OnWaveStart()
        {
            _target=null;
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || target.faction == this._owner.faction || (_target != null && !_target.IsDead()))
                return;
            _target = target;
            target.bufListDetail.AddBuf(new BattleUnitBuf_redhood_prey());
        }
        public override void OnSelectEmotion()
        {
           _target=null;
        }
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            base.BeforeGiveDamage(behavior);
            BattleUnitModel target = behavior.card?.target;
            if (target == null || target != this._target)
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                dmg = Dmg
            });
        }
        public void Destroy()
        {
            if (_target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_redhood_prey)) is BattleUnitBuf_redhood_prey prey)
                prey.Destroy();
        }
        public class BattleUnitBuf_redhood_prey : BattleUnitBuf
        {
            protected override string keywordId => "RedHood_Hunt";
            protected override string keywordIconId => "Redhood_Target";
        }
    }
}
