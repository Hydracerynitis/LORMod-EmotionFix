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
        public override void OnRoundStart()
        {
            if (!(this._owner.bufListDetail.GetActivatedBufList().Exists((Predicate<BattleUnitBuf>)(x => x is Eager))))
            {
                this._owner.bufListDetail.AddBuf(new Eager());
                SoundEffectPlayer.PlaySound("Creature/RedShoes_On");
            }
            List<BattleUnitModel> alive = BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player);
            foreach(BattleUnitModel member in alive)
            {
                if (member.bufListDetail.GetActivatedBufList().Exists((Predicate<BattleUnitBuf>)(x => x is TheChosen)))
                    return;
            }
            RandomUtil.SelectOne<BattleUnitModel>(alive).bufListDetail.AddBuf(new TheChosen());
        }
        public override void OnSelectEmotion()
        {
            this._owner.bufListDetail.AddBuf(new Eager());
            SoundEffectPlayer.PlaySound("Creature/RedShoes_On");
        }
        public void Destroy()
        {
            if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Eager)) is Eager shoe)
                shoe.Destroy();
            foreach(BattleUnitModel enemy in BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
            {
                if (enemy.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is TheChosen)) is TheChosen  target)
                    target.Destroy();
            }
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
