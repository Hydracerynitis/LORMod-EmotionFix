using System;
using LOR_DiceSystem;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class DiceCardSelfAbility_Request: DiceCardSelfAbilityBase
    {
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            if (owner.breakDetail.breakGauge < (int)((double)owner.breakDetail.GetDefaultBreakGauge() / 4))
                return false;
            return base.OnChooseCard(owner);
        }
        public override void OnStartBattle()
        {
            this.owner.breakDetail.LoseBreakGauge((int)((double)this.owner.breakDetail.GetDefaultBreakGauge() / 4));
        }
    }
}
