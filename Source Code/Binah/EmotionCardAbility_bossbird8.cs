using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using HarmonyLib;
using System.Reflection;
using System.Collections.Generic;
using System.IO;

namespace EmotionalFix
{
    public class EmotionCardAbility_bossbird8 : EmotionCardAbilityBase
    {
        private BattleUnitModel _target;
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            _target = null;
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || GetAliveCreature() != null)
                return;
            _target = target;
            target.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_BossBird_Creature());
            target.battleCardResultLog?.SetNewCreatureAbilityEffect("8_B/FX_IllusionCard_8_B_Terrable_Start", 2f);
            target.battleCardResultLog?.SetCreatureEffectSound("Creature/Bossbird_Bossbird_Stab");
            target.battleCardResultLog?.SetCreatureEffectSound("Creature/Bossbird_StoryFilter_Dead");
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (_target != null && !_target.IsDead())
            {
                _target.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_BossBird_Creature());
                return;
            }
            _target =null;
        }
        private BattleUnitModel GetAliveCreature() => BattleObjectManager.instance.GetAliveList(Faction.Player).Find((Predicate<BattleUnitModel>)(x => x.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(y => y is BattleUnitBuf_Emotion_BossBird_Creature)) != null)) ?? _target;
        public class BattleUnitBuf_Emotion_BossBird_Creature : BattleUnitBuf
        {
            private Battle.CreatureEffect.CreatureEffect _aura;
            private int ReduceDmg => RandomUtil.Range(2, 4);
            private int ReduceBreakDmg => RandomUtil.Range(2, 4);
            private int AddDmg => RandomUtil.Range(2, 4);
            public override string keywordIconId => "ApocalypseBird_Apocalypse";
            public override string keywordId => "ApocalypseBird_Peace";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    dmg = -ReduceDmg,
                    breakDmg = -ReduceBreakDmg
                });
            }
            public override int GetBreakDamageReduction(BehaviourDetail behaviourDetail)
            {
                return -AddDmg;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
            public override int GetDamageReduction(BattleDiceBehavior behavior)
            {
                return -AddDmg;
            }
            public override void OnDie()
            {
                base.OnDie();
                Destroy();
            }
            public override void Destroy()
            {
                base.Destroy();
                DestroyAura();
            }
            public void DestroyAura()
            {
                if (!(_aura != null))
                    return;
                UnityEngine.Object.Destroy(_aura.gameObject);
                _aura = (Battle.CreatureEffect.CreatureEffect)null;
            }
        }
    }
}
