using System;
using Battle.CreatureEffect;
using LOR_DiceSystem;
using UI;
using Sound;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_bloodbath3 : EmotionCardAbilityBase
    {
        private BattleUnitModel _target;
        private int _stack;
        public override int GetCounter() => _stack;
        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            base.OnRollDice(behavior);
            if (!IsAttackDice(behavior.Detail) || _target == null || _target == behavior.card.target)
                return;
            if (_target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BloodBath_HandDebuf)) is BloodBath_HandDebuf bloodBathHandDebuf)
                _target.bufListDetail.RemoveBuf(bloodBathHandDebuf);
            _target = null;
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            if (_target != behavior.card.target)
            {
                _target = behavior.card.target;
                _stack = 1;
                if(_owner.faction==Faction.Player)
                    _target.bufListDetail.AddBuf(new BloodBath_HandDebuf());
                else
                    _target.bufListDetail.AddBuf(new BloodBath_HandDebuf_Enemy());
                if (!(_target.bufListDetail.GetActivatedBufList().Find((x => x is BloodBath_HandDebuf)) is BloodBath_HandDebuf bloodBathHandDebuf))
                    return;
                bloodBathHandDebuf.OnHit();
            }
            else
            {
                ++_stack;
                if (_target.bufListDetail.GetActivatedBufList().Find((x => x is BloodBath_HandDebuf)) is BloodBath_HandDebuf bloodBathHandDebuf)
                    bloodBathHandDebuf.OnHit();
                if (_stack < 3)
                    return;
                Ability();
            }
        }

        private void Ability()
        {
            if (_target == null)
                return;
            if(_owner.faction==Faction.Player)
                _target.TakeBreakDamage(RandomUtil.Range(3, 10), DamageType.Emotion,_owner);
            if(_owner.faction==Faction.Enemy)
                _target.TakeBreakDamage(RandomUtil.Range(3, 12), DamageType.Emotion,_owner);
            if (_target.bufListDetail.GetActivatedBufList().Find((x => x is BloodBath_HandDebuf)) is BloodBath_HandDebuf bloodBathHandDebuf)
                _target.bufListDetail.RemoveBuf(bloodBathHandDebuf);
            _target.battleCardResultLog?.SetCreatureAbilityEffect("0/BloodyBath_PaleHand_Hit", 3f);
            _target = null;
            _stack = 0;
        }
        public void Destroy()
        {
            if (_target == null)
                return;
            if (_target.bufListDetail.GetActivatedBufList().Find((x => x is BloodBath_HandDebuf)) is BloodBath_HandDebuf bloodBathHandDebuf)
                _target.bufListDetail.RemoveBuf(bloodBathHandDebuf);
            _target = null;
            _stack = 0;
        }

        public class BloodBath_HandDebuf : BattleUnitBuf
        {
            public override string keywordIconId => "BloodBath_Hand";
            public override string keywordId => "Bloodbath_Hands";
            public BloodBath_HandDebuf() => stack = 0;
            public void OnHit() => ++stack;
        }
        public class BloodBath_HandDebuf_Enemy : BloodBath_HandDebuf
        {
            public override string keywordId => "BloodBath_Hand_Enemy";
            public override string keywordIconId => "Ability/BloodBath_Hand";
        }

    }
}
