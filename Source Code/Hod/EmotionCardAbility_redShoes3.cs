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
                if (this.stack == 0)
                    return;
                this._owner.TakeDamage(this.stack,DamageType.Buf);
                this._owner.battleCardResultLog?.SetAfterActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.PrintEffect));
                if (this._owner.Book.GetBookClassInfoId() != 150013)
                    this._owner.battleCardResultLog?.AddBufEffect("BufEffect_Bleeding");
            }
            private void PrintEffect()
            {
                GameObject gameObject = Util.LoadPrefab("Battle/DiceAttackEffects/New/FX/DamageDebuff/FX_DamageDebuff_Blooding");
                if (!((UnityEngine.Object)gameObject != (UnityEngine.Object)null) || !((UnityEngine.Object)this._owner?.view != (UnityEngine.Object)null))
                    return;
                gameObject.transform.parent = this._owner.view.camRotationFollower;
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localScale = Vector3.one;
                gameObject.transform.localRotation = Quaternion.identity;
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
