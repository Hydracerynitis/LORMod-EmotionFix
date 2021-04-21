using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class DiceCardAbility_Execute : DiceCardAbilityBase
    {
        public override void BeforRollDice() => this.behavior.ApplyDiceStatBonus(new DiceStatBonus()
        {
            dmg = -10000,
            breakDmg = -10000
        });

        public override void AfterAction()
        {
            this.behavior.card.target.TakeDamage(this.behavior.DiceResultValue);
            this.behavior.card.target.TakeBreakDamage(this.behavior.DiceResultValue);
        }
    }
}
