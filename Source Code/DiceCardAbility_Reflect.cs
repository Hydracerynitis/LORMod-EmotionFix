using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class DiceCardAbility_Reflect: DiceCardAbilityBase
    {
        public override void OnWinParryingDefense()
        {
            if (this.behavior.TargetDice == null)
                return;
            this.card.target.battleCardResultLog?.SetCreatureAbilityEffect("3/BlackSwan_Barrier", 0.8f);
            this.card.target.TakeDamage(this.behavior.TargetDice.DiceResultValue, DamageType.Rebound,this.owner);
            this.card.target.TakeBreakDamage(this.behavior.TargetDice.DiceResultValue, DamageType.Rebound, this.owner);
        }
    }
}
