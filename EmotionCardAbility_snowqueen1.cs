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
        private static int Dmg => RandomUtil.Range(2, 5);
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || !Prob)
                return;
            target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Binding, Bind, this._owner);
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
        }
    }
}
