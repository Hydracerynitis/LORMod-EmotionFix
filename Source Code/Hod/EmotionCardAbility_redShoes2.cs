using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_redShoes2 : EmotionCardAbilityBase
    {
        private List<CreatureEffect_FaceAttacher> _faceEffect = new List<CreatureEffect_FaceAttacher>();
        public override void OnRoundStart()
        {
            if (!(this._owner.bufListDetail.GetActivatedBufList().Exists(x => x is Eager)))
            {
                this._owner.bufListDetail.AddBuf(new Eager());
                SoundEffectPlayer.PlaySound("Creature/RedShoes_On");
                CreatureEffect_FaceAttacher effectFaceAttacher = this.MakeFaceEffect(this._owner.view);
                effectFaceAttacher.SetLayer("Character");
                if (!(bool)(UnityEngine.Object)effectFaceAttacher)
                    return;
                this._faceEffect.Add(effectFaceAttacher);
            }
            List<BattleUnitModel> alive = BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player);
            foreach(BattleUnitModel member in alive)
            {
                if (member.bufListDetail.GetActivatedBufList().Exists(x => x is TheChosen))
                    return;
            }
            RandomUtil.SelectOne<BattleUnitModel>(alive).bufListDetail.AddBuf(new TheChosen());
        }
        public CreatureEffect_FaceAttacher MakeFaceEffect(BattleUnitView target)
        {
            CreatureEffect_FaceAttacher effectFaceAttacher = this.MakeEffect("3/RedShoes_Attract", apply: false) as CreatureEffect_FaceAttacher;
            effectFaceAttacher.AttachTarget(target);
            return effectFaceAttacher;
        }
        public override void OnSelectEmotion()
        {
            this._owner.bufListDetail.AddBuf(new Eager());
            SoundEffectPlayer.PlaySound("Creature/RedShoes_On");
        }
        public override void OnLayerChanged(string layerName)
        {
            foreach (Battle.CreatureEffect.CreatureEffect creatureEffect in this._faceEffect)
                creatureEffect.SetLayer(layerName);
        }
        public void Destroy()
        {
            if (this._owner.bufListDetail.GetActivatedBufList().Find(x => x is Eager) is Eager shoe)
                shoe.Destroy();
            foreach(BattleUnitModel enemy in BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
            {
                if (enemy.bufListDetail.GetActivatedBufList().Find((x => x is TheChosen)) is TheChosen  target)
                    target.Destroy();
            }
            foreach (CreatureEffect_FaceAttacher effectFaceAttacher in this._faceEffect)
                effectFaceAttacher?.ManualDestroy();
            this._faceEffect.Clear();
        }
        public class Eager: BattleUnitBuf
        {
            public override bool Hide => true;
            public override List<BattleUnitModel> GetFixedTarget()
            {
                List<BattleUnitModel> choose = BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player).FindAll((Predicate<BattleUnitModel>)(x => x.bufListDetail.GetActivatedBufList().Exists((Predicate<BattleUnitBuf>)(y => y is TheChosen))));
                if (choose.Count == 0)
                    return base.GetFixedTarget();
                return choose;
            }
            public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int currentSlotOrder)
            {
                List<BattleUnitModel> choose = BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy: Faction.Player).FindAll((Predicate<BattleUnitModel>)(x => x.bufListDetail.GetActivatedBufList().Exists((Predicate<BattleUnitBuf>)(y => y is TheChosen))));
                if (choose.Count == 0)
                    return base.ChangeAttackTarget(card,currentSlotOrder);
                return RandomUtil.SelectOne<BattleUnitModel>(choose);
            }
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (behavior.card.target.bufListDetail.GetActivatedBufList().Exists((Predicate<BattleUnitBuf>)(x => x is TheChosen)))
                    this._owner.battleCardResultLog.SetAttackEffectFilter(typeof(ImageFilter_ColorBlend));
            }
            public override void OnSuccessAttack(BattleDiceBehavior behavior)
            {
                if (behavior.card.target.bufListDetail.GetActivatedBufList().Exists((Predicate<BattleUnitBuf>)(x => x is TheChosen)))
                    behavior.card.target.TakeDamage(RandomUtil.Range(3, 8));
            }
        }
        public class TheChosen: BattleUnitBuf
        {
            protected override string keywordId => "TheChosen";
            protected override string keywordIconId => "CopiousBleeding";
        }
    }
}
