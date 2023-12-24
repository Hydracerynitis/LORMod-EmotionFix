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
        private static int Power => RandomUtil.Range(2, 3);
        private static int DmgUp => RandomUtil.Range(2, 3);
        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            base.OnDieOtherUnit(unit);
            if (unit.faction != _owner.faction)
                return;
            ++stack;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStartOnce();
            if (tempStack > 0)
            {
                _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, Power);
                _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.DmgUp, DmgUp);
                if (!ifNilil(_owner))
                {
                    int num = (int)((double)_owner.breakDetail.GetDefaultBreakGauge() * 0.25);
                    _owner.breakDetail.LoseBreakGauge(num);
                    _owner.view.BreakDamaged(num, BehaviourDetail.Penetrate, _owner, AtkResist.Normal);
                }
                if (tempStack >= 2 && _owner.faction==Faction.Player)
                {
                    if (!(_owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_knightofdespair3.Tear)) is EmotionCardAbility_knightofdespair3.Tear tear))
                    {
                        EmotionCardAbility_knightofdespair3.Tear Tear = new EmotionCardAbility_knightofdespair3.Tear
                        {
                            stack=1,
                            reserve= 1
                        };
                        _owner.bufListDetail.AddBuf(Tear);
                    }
                    else
                    {
                        tear.stack += 1;
                        tear.reserve+= 1;
                    }
                }
                if (_filter == null)
                {
                    _filter = new GameObject().AddComponent<SpriteFilter_Despair>();
                    _filter.Init("EmotionCardFilter/KnightOfDespair_Gaho", true, 1f);
                }
            }
            tempStack = 0;
        }
        public void Destroy()
        {
            if (_owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_knightofdespair3.Tear)) is EmotionCardAbility_knightofdespair3.Tear Tear)
                Tear.Destroy();
            DestroyFilter();
        }
        public bool ifNilil(BattleUnitModel owner)
        {
            return _owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_clownofnihil3.BattleUnitBuf_Emotion_Nihil)) != null;
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            DestroyFilter();
            tempStack = stack;
            stack = 0;
        }
        public override void OnEndBattlePhase()
        {
            base.OnEndBattlePhase();
            DestroyFilter();
        }
        public void DestroyFilter()
        {
            if (_filter == null)
                return;
            _filter.ManualDestroy();
            _filter = (SpriteFilter_Despair)null;
        }
    }
}
