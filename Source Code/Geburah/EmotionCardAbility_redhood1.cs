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
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            if ((_target != null && !_target.IsDead()) || curCard.target.faction == _owner.faction || curCard.GetDiceBehaviorList().Find(x => x.Type == BehaviourType.Atk) == null)
                return;
            _target = curCard.target;
            _target.bufListDetail.AddBuf(new BattleUnitBuf_redhood_prey());
            _target.battleCardResultLog?.SetNewCreatureAbilityEffect("6_G/FX_IllusionCard_6_G_Hunted", 1.5f);
            _target.battleCardResultLog?.SetCreatureEffectSound("Creature/RedHood_Gun");
        }
        public override void OnWaveStart()
        {
            _target=null;
        }
        public override void OnSelectEmotion()
        {
           _target=null;
        }
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            base.BeforeGiveDamage(behavior);
            BattleUnitModel target = behavior.card?.target;
            if (target == null || target != _target)
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
            public override string keywordId => "RedHood_Hunt";
            public override string keywordIconId => "Redhood_Target";
        }
    }
}
