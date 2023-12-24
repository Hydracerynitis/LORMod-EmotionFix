using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_snowqueen1 : EmotionCardAbilityBase
    {
        private static bool Prob => (double)RandomUtil.valueForProb < 0.5;
        private static int Bind => RandomUtil.Range(1, 3);
        private static int Dmg => RandomUtil.Range(2, 4);
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || !Prob)
                return;
            target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Binding, Bind, _owner);
            if (target.bufListDetail.GetReadyBufList().Find(x => x is BattleUnitBuf_Emotion_Snowqueen_Aura) != null)
                return;
            target.bufListDetail.AddReadyBuf(new BattleUnitBuf_Emotion_Snowqueen_Aura());
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || !IsAttackDice(behavior.Detail) || target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x.bufType == KeywordBuf.Binding)) == null)
                return;
            int bonus = Dmg;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                dmg = bonus,
                breakDmg =bonus
            });
            target.battleCardResultLog?.SetNewCreatureAbilityEffect("0_K/FX_IllusionCard_0_K_SnowUnATK", 2f);
            target.battleCardResultLog?.SetCreatureEffectSound("Creature/SnowQueen_Atk");
        }
        public class BattleUnitBuf_Emotion_Snowqueen_Aura : BattleUnitBuf
        {
            private GameObject aura;
            public override bool Hide => true;
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (_owner == null)
                    return;
                aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("0_K/FX_IllusionCard_0_K_SnowAura", 1f, _owner.view, _owner.view)?.gameObject;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
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
                if (!(aura != null))
                    return;
                UnityEngine.Object.Destroy(aura.gameObject);
                aura = (GameObject)null;
            }
        }
    }
}
