using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using System.IO;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_redShoes3 : EmotionCardAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if (this._owner.faction == Faction.Enemy)
            {
                if(RandomUtil.valueForProb>0.4)
                    return;
                SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/RedShoes_SlashHit")?.SetGlobalPosition(this._owner.view.WorldPosition);
                this._owner.battleCardResultLog.SetAttackEffectFilter(typeof(ImageFilter_ColorBlend_Red));
                BattleUnitModel target = behavior.card.target;
                if (!(target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is DeepBleed)) is DeepBleed DeepBleed))
                {
                    DeepBleed deepBleed = new DeepBleed();
                    target.bufListDetail.AddBuf(deepBleed);
                    deepBleed.reserve += 1;
                }
                else
                    DeepBleed.reserve += 1;
            }
            if (this._owner.faction == Faction.Player)
            {
                if (behavior.Detail!=BehaviourDetail.Slash)
                    return;
                SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/RedShoes_SlashHit")?.SetGlobalPosition(this._owner.view.WorldPosition);
                this._owner.battleCardResultLog.SetAttackEffectFilter(typeof(ImageFilter_ColorBlend_Red));
                BattleUnitModel target = behavior.card.target;
                if (!(target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is DeepBleed)) is DeepBleed DeepBleed))
                {
                    DeepBleed deepBleed = new DeepBleed();
                    target.bufListDetail.AddBuf(deepBleed);
                    deepBleed.reserve+=1;
                }
                else
                    DeepBleed.reserve += 1;
            }
        }
        public class DeepBleed: BattleUnitBuf
        {
            public override bool Hide => this.stack==0;
            protected override string keywordId => "DeepBleed";
            protected override string keywordIconId => "Bleeding";
            public int reserve;
            public override bool independentBufIcon => true;
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
            public override void AfterDiceAction(BattleDiceBehavior behavior)
            {
                if (!this.IsAttackDice(behavior.Detail))
                    return;
                if (!this._owner.IsImmune(this.bufType))
                {
                    if (this.stack == 0)
                        return;
                    this._owner.TakeDamage(this.stack);
                    if (this._owner.Book.GetBookClassInfoId() != 150013)
                        this._owner.battleCardResultLog?.AddBufEffect("BufEffect_Bleeding");
                }
            }
            public override void OnRoundEnd()
            {
                stack = 0;
                if (reserve == 0)
                    Destroy();
                stack += reserve;
                reserve = 0;
            }
        }
    }
}
