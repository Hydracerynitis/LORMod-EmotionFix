using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_spiderBud2 : EmotionCardAbilityBase
    {
        private bool breaked;
        private static int Heal => RandomUtil.Range(3, 7);
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            this.breaked = false;
            if (behavior == null)
                return;
            bool? nullable = behavior.card?.target?.IsBreakLifeZero();
            bool flag = false;
            if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
                return;
            this.breaked = true;
        }

        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if (behavior?.card?.target == null || !behavior.card.target.IsBreakLifeZero() || this.breaked)
                return;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction==Faction.Player? Faction.Player:Faction.Enemy))
                alive.RecoverHP(Heal);
            string resPath = "";
            switch (behavior.behaviourInCard.MotionDetail)
            {
                case MotionDetail.H:
                    resPath = "3/SpiderBud_Spiders_H";
                    break;
                case MotionDetail.J:
                    resPath = "3/SpiderBud_Spiders_J";
                    break;
                case MotionDetail.Z:
                    resPath = "3/SpiderBud_Spiders_Z";
                    break;
            }
            if (!string.IsNullOrEmpty(resPath))
                this._owner.battleCardResultLog.SetCreatureAbilityEffect(resPath, 1f);
            this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Spider_gochiDown");
        }
    }
}
