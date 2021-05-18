using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix.Source_Code.Keter
{
    public class EmotionCardAbility_quietKidGuilty: EmotionCardAbilityBase
    {
        public override int GetBreakDamageReduction(BattleDiceBehavior behavior) => RandomUtil.Range(1, 3);

        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            if (this._owner.faction == Faction.Player)
            {
                atkDice.owner.TakeBreakDamage(dmg, DamageType.Emotion);
            }
        }
        public override double ChangeDamage(BattleUnitModel attacker, double dmg)
        {
            return dmg-attacker.history.damageToEnemyAtRound/5;
        }
    }
}
