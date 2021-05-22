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
            target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Binding, Bind, this._owner);
            if (target.bufListDetail.GetReadyBufList().Find(x => x is BattleUnitBuf_Emotion_Snowqueen_Aura) != null)
                return;
            target.bufListDetail.AddReadyBuf(new BattleUnitBuf_Emotion_Snowqueen_Aura());
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || !this.IsAttackDice(behavior.Detail) || target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x.bufType == KeywordBuf.Binding)) == null)
                return;
            int bonus = Dmg;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                dmg = bonus,
                breakDmg =bonus
            });
            target.battleCardResultLog?.SetNewCreatureAbilityEffect("0_K/FX_IllusionCard_0_K_SnowUnATK", 2f);
        }
        public class BattleUnitBuf_Emotion_Snowqueen_Aura : BattleUnitBuf
        {
            private GameObject aura;
            public override bool Hide => true;
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (this._owner == null)
                    return;
                this.aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("0_K/FX_IllusionCard_0_K_SnowAura", 1f, this._owner.view, this._owner.view)?.gameObject;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
            public override void OnDie()
            {
                base.OnDie();
                this.Destroy();
            }
            public override void Destroy()
            {
                base.Destroy();
                this.DestroyAura();
            }
            public void DestroyAura()
            {
                if (!((UnityEngine.Object)this.aura != (UnityEngine.Object)null))
                    return;
                UnityEngine.Object.Destroy((UnityEngine.Object)this.aura.gameObject);
                this.aura = (GameObject)null;
            }
        }
    }
}
