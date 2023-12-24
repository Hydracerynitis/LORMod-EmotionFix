using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_queenofhatred1 : EmotionCardAbilityBase
    {
        private int count;
        private static int RecoverHP => RandomUtil.Range(3, 5);
        private static int RecoverBreak => RandomUtil.Range(2, 4);
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            if (_owner.faction != Faction.Player || count == 0)
                return;
            count -= 1;
        }
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            if (_owner.faction != Faction.Player)
                return;
            count = 1;
            foreach(BattleUnitModel enemy in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
            {
                if(enemy.bufListDetail.GetActivatedBufList().Exists((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_queenofhatred2.Villain)))
                {
                    count += 1;
                    return;
                }
            }
        }
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            if(_owner.faction == Faction.Player && count == 0)
                return;
            _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/MagicalGirl_kiss");
            _owner.battleCardResultLog?.SetEmotionAbilityEffect("5/MagicalGirl_Heart");
            if (IsAttackDice(behavior.Detail))
            {
                if (_owner.faction == Faction.Enemy)
                {
                    foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList_random(_owner.faction, 1))
                        battleUnitModel.RecoverHP(RecoverHP);
                }
                if (_owner.faction == Faction.Player && count!=0)
                {
                    foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(_owner.faction))
                        battleUnitModel.RecoverHP(RecoverHP);
                }
            }
            else
            {
                if (!IsDefenseDice(behavior.Detail))
                    return;
                List<BattleUnitModel> list = new List<BattleUnitModel>();
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(_owner.faction))
                {
                    if (!alive.IsBreakLifeZero())
                        list.Add(alive);
                }
                if (_owner.faction == Faction.Enemy)
                {
                    RandomUtil.SelectOne<BattleUnitModel>(list).breakDetail.RecoverBreak(RecoverBreak);
                }
                if(_owner.faction==Faction.Player && count != 0)
                {
                    foreach (BattleUnitModel battleUnitModel in list)
                        battleUnitModel.breakDetail.RecoverBreak(RecoverBreak);
                }
            }
        }
        public override void OnPrintEffect(BattleDiceBehavior behavior) => base.OnPrintEffect(behavior);
    }
}
