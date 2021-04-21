using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class DiceCardAbility_Tear: DiceCardAbilityBase
    {
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            if (this.card.cardBehaviorQueue.Count == 0)
            {
                this.owner.battleCardResultLog?.SetCreatureEffectSound("Creature/KnightOfDespair_Atk_Strong");
                this.owner.battleCardResultLog.SetAttackEffectFilter(typeof(ImageFilter_ColorBlend_Despair));
                this.card.target.TakeDamage((int)((double)this.card.target.MaxHp * 0.1));
            }
            else
            {
                foreach (BattleDiceBehavior battleDice in card.cardBehaviorQueue)
                {
                    if (IsAttackDice(battleDice.Detail))
                    {
                        this.card.ApplyDiceAbility(DiceMatch.NextAttackDice, new DiceCardAbility_Tear());
                        return;
                    }
                }
                this.owner.battleCardResultLog?.SetCreatureEffectSound("Creature/KnightOfDespair_Atk_Strong");
                this.owner.battleCardResultLog.SetAttackEffectFilter(typeof(ImageFilter_ColorBlend_Despair));
                this.card.target.TakeDamage((int)((double)this.card.target.MaxHp * 0.1));
            }           
        }
    }

}
