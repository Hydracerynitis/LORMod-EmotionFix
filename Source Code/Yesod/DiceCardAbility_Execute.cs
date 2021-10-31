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
        public override void BeforeRollDice() => this.behavior.ApplyDiceStatBonus(new DiceStatBonus()
        {
            dmg = -10000,
            breakDmg = -10000
        });

        public override void AfterAction()
        {
            this.behavior.card.target.TakeDamage(this.behavior.DiceResultValue);
            this.behavior.card.target.TakeBreakDamage(this.behavior.DiceResultValue);
            behavior?.card?.target?.battleCardResultLog?.SetNewCreatureAbilityEffect("2_Y/FX_IllusionCard_2_Y_Seven", 3f);
            this.behavior.card.target.battleCardResultLog?.SetCreatureEffectSound("Creature/Matan_FinalShot");
        }
    }
}
