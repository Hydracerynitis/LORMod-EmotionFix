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
            if (atkDice.owner == null)
                return;
            if (this._owner.faction == Faction.Player)
            {
                atkDice.owner.TakeBreakDamage(dmg, DamageType.Emotion);
            }
            atkDice.owner.battleCardResultLog?.SetNewCreatureAbilityEffect("0_K/FX_IllusionCard_0_K_RedEye", 1f);
            this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Slientgirl_Guilty");
        }
        public override double ChangeDamage(BattleUnitModel attacker, double dmg)
        {
            return dmg-attacker.history.damageToEnemyAtRound/5;
        }
    }
}
