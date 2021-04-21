using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_knightofdespair2 : EmotionCardAbilityBase
    {
        private int stack;
        private int tempStack;
        private SpriteFilter_Despair _filter;
        private static int Power => RandomUtil.Range(1, 3);
        private static int DmgUp => RandomUtil.Range(1, 3);
        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            base.OnDieOtherUnit(unit);
            if (unit.faction != this._owner.faction)
                return;
            ++this.stack;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStartOnce();
            if (this.tempStack > 0)
            {
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, Power);
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.DmgUp, DmgUp);
                if (!ifNilil(this._owner))
                {
                    int num = (int)((double)this._owner.breakDetail.GetDefaultBreakGauge() * 0.25);
                    this._owner.breakDetail.LoseBreakGauge(num);
                    this._owner.view.BreakDamaged(num, BehaviourDetail.Penetrate, this._owner, AtkResist.Normal);
                }
                if (tempStack >= 2 && this._owner.faction==Faction.Player)
                {
                    if (!(this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_knightofdespair3.Tear)) is EmotionCardAbility_knightofdespair3.Tear tear))
                    {
                        EmotionCardAbility_knightofdespair3.Tear Tear = new EmotionCardAbility_knightofdespair3.Tear
                        {
                            stack=1,
                            reserve= 1
                        };
                        this._owner.bufListDetail.AddBuf(Tear);
                    }
                    else
                    {
                        tear.stack += 1;
                        tear.reserve+= 1;
                    }
                }
                if ((UnityEngine.Object)this._filter == (UnityEngine.Object)null)
                {
                    this._filter = new GameObject().AddComponent<SpriteFilter_Despair>();
                    this._filter.Init("EmotionCardFilter/KnightOfDespair_Gaho", true, 1f);
                }
            }
            this.tempStack = 0;
        }
        public void Destroy()
        {
            if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_knightofdespair3.Tear)) is EmotionCardAbility_knightofdespair3.Tear Tear)
                Tear.Destroy();
        }
        public bool ifNilil(BattleUnitModel owner)
        {
            return this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_clownofnihil3.BattleUnitBuf_Emotion_Nihil)) != null;
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            if ((UnityEngine.Object)this._filter != (UnityEngine.Object)null)
            {
                this._filter.ManualDestroy();
                this._filter = (SpriteFilter_Despair)null;
            }
            this.tempStack = this.stack;
            this.stack = 0;
        }
    }
}
