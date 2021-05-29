using System;
using LOR_DiceSystem;
using Battle.CameraFilter;
using System.Collections.Generic;
using Sound;
using UnityEngine;
using System.Collections;

namespace EmotionalFix
{
    public class EmotionCardAbility_porccubus : EmotionCardAbilityBase
    {
        private BattleUnitModel _target;
        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            base.OnRollDice(behavior);
            if (!this.IsAttackDice(behavior.Detail) || this._target == null || this._target == behavior.card.target || this._owner.faction==Faction.Player)
                return;
            if (this._target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_Porccubus_Happy_Enemy)) is BattleUnitBuf_Emotion_Porccubus_Happy_Enemy happy)
                this._target.bufListDetail.RemoveBuf(happy);
            this._target = null;
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            BattleUnitModel target = behavior.card?.target;
            if (target == null)
                return;
            if (this._owner.faction == Faction.Enemy)
            {
                _target = target;
                if (!(target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_Porccubus_Happy_Enemy)) is BattleUnitBuf_Emotion_Porccubus_Happy_Enemy happy))
                {
                    BattleUnitBuf_Emotion_Porccubus_Happy_Enemy Happy = new BattleUnitBuf_Emotion_Porccubus_Happy_Enemy();
                    target.bufListDetail.AddBuf(Happy);
                    Happy.Add();
                }
                else
                    happy.Add();
            }
            if (behavior.Detail == BehaviourDetail.Penetrate && this._owner.faction == Faction.Player)
            {
                if (!(target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_Porccubus_Happy)) is BattleUnitBuf_Emotion_Porccubus_Happy happy))
                {
                    BattleUnitBuf_Emotion_Porccubus_Happy Happy = new BattleUnitBuf_Emotion_Porccubus_Happy();
                    target.bufListDetail.AddBuf(Happy);
                    Happy.Add();
                }
                else
                    happy.Add();
            }
        }
        public void Destroy()
        {
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList())
            {
                if (unit.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_Porccubus_Happy)) is BattleUnitBuf_Emotion_Porccubus_Happy happy)
                    happy.Destroy();
                if (unit.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_Porccubus_Happy_Enemy)) is BattleUnitBuf_Emotion_Porccubus_Happy_Enemy happyEnemy)
                    happyEnemy.Destroy();
            }
        }
        public void Filter() => new GameObject().AddComponent<SpriteFilter_Porccubus_Special>().Init("EmotionCardFilter/Porccubus_Filter", false, 1f);

        public class BattleUnitBuf_Emotion_Porccubus_Happy : BattleUnitBuf
        {
            private static int Dmg => RandomUtil.Range(2, 7);
            protected override string keywordId => "Porccubus_Happy";
            public void Add()
            {
                ++this.stack;
                if (this.stack < 4)
                {
                    this._owner.battleCardResultLog?.SetCreatureAbilityEffect("3/Porccubuss_Delight", 1f);
                    this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Porccu_Penetrate");
                    return;
                }

                this._owner.TakeDamage(Dmg);
                this._owner.TakeBreakDamage(Dmg);
                this.stack = 0;
                this.Destroy();
            }
        }
        public class BattleUnitBuf_Emotion_Porccubus_Happy_Enemy : BattleUnitBuf
        {
            private static int Dmg => RandomUtil.Range(3, 7);
            protected override string keywordId => "Porccubus_Happy_Enemy";
            protected override string keywordIconId => "Porccubus_Happy";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                this.stack = 0;
            }
            public void Add()
            {
                ++this.stack;
                if (this.stack < 3)
                {
                    this._owner.battleCardResultLog?.SetCreatureAbilityEffect("3/Porccubuss_Delight", 1f);
                    this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Porccu_Penetrate");
                    return;
                }

                this._owner.TakeDamage(Dmg);
                this._owner.TakeBreakDamage(Dmg);
                this.stack = 0;
                this.Destroy();
            }
        }
    }
}
