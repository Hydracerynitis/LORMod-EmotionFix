using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_orchestra3 : EmotionCardAbilityBase
    {
        private bool trigger;
        private static int Reduce => RandomUtil.Range(3, 4);
        public override void OnMakeBreakState(BattleUnitModel target)
        {
            base.OnMakeBreakState(target);
            this.trigger = true;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (!this.trigger)
                return;
            this.trigger = false;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
                alive.cardSlotDetail.LosePlayPoint(Reduce);
        }
    }
}
