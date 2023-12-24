using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix.Hod
{
    public class EmotionCardAbility_hod_redShoes2 : EmotionCardAbilityBase
    {
        private List<CreatureEffect_FaceAttacher> _faceEffect = new List<CreatureEffect_FaceAttacher>();
        public override void OnRoundStart()
        {
            if (!(_owner.bufListDetail.GetActivatedBufList().Exists(x => x is Eager)))
            {
                _owner.bufListDetail.AddBuf(new Eager());
                SoundEffectPlayer.PlaySound("Creature/RedShoes_On");
                CreatureEffect_FaceAttacher effectFaceAttacher = MakeFaceEffect(_owner.view);
                effectFaceAttacher.SetLayer("Character");
                _faceEffect.Add(effectFaceAttacher);
            }
            List<BattleUnitModel> alive = BattleObjectManager.instance.GetAliveList_opponent(_owner.faction);
            if (alive.Exists(x => x.bufListDetail.HasBuf<TheChosen>()))
                return;
            RandomUtil.SelectOne(alive).bufListDetail.AddBuf(new TheChosen());
        }
        public CreatureEffect_FaceAttacher MakeFaceEffect(BattleUnitView target)
        {
            CreatureEffect_FaceAttacher effectFaceAttacher = MakeEffect("3/RedShoes_Attract", apply: false) as CreatureEffect_FaceAttacher;
            effectFaceAttacher.AttachTarget(target);
            return effectFaceAttacher;
        }
        public override void OnSelectEmotion()
        {
            _owner.bufListDetail.AddBuf(new Eager());
            SoundEffectPlayer.PlaySound("Creature/RedShoes_On");
        }
        public override void OnLayerChanged(string layerName)
        {
            foreach (Battle.CreatureEffect.CreatureEffect creatureEffect in _faceEffect)
                creatureEffect.SetLayer(layerName);
        }
        public class Eager: BattleUnitBuf
        {
            public override bool Hide => true;
            public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int currentSlotOrder)
            {
                List<BattleUnitModel> choose = BattleObjectManager.instance.GetAliveList_opponent(_owner.faction).FindAll(x => x.bufListDetail.GetActivatedBufList().Exists(y => y is TheChosen));
                if (choose.Count == 0)
                    return base.ChangeAttackTarget(card,currentSlotOrder);
                return RandomUtil.SelectOne(choose);
            }
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (behavior.card.target.bufListDetail.GetActivatedBufList().Exists(x => x is TheChosen))
                    _owner.battleCardResultLog.SetAttackEffectFilter(typeof(ImageFilter_ColorBlend));
            }
            public override void OnSuccessAttack(BattleDiceBehavior behavior)
            {
                if (behavior.card.target.bufListDetail.GetActivatedBufList().Exists(x => x is TheChosen))
                    behavior.card.target.TakeDamage(RandomUtil.Range(3, 8));
            }
        }
        public class TheChosen: BattleUnitBuf
        {
            public override string keywordId => "EF_TheChosen";
            public override string keywordIconId => "CopiousBleeding";
        }
    }
}
