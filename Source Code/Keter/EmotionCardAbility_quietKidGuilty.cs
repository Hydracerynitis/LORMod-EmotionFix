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
            if (this._owner.faction == Faction.Enemy)
            {
                if ((double)dmg < (double)this._owner.MaxHp * 0.02)
                    return;
                atkDice.owner.TakeBreakDamage(dmg, DamageType.Emotion);
            }
        }
    }
}
