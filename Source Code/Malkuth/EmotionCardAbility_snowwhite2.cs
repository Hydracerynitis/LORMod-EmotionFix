using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_snowwhite2: EmotionCardAbilityBase
    {
        private Dictionary<BattleUnitModel, int> dmgData = new Dictionary<BattleUnitModel, int>();
        private static int Damage => RandomUtil.Range(2, 8);
        public override void OnRoundStart()
        {
            dmgData.Clear();
        }
        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            base.OnTakeDamageByAttack(atkDice, dmg);
            int attack = 0;
            bool first = true;
            BattleUnitModel owner = atkDice.owner;
            if (owner == null)
                return;
            if (dmgData.ContainsKey(owner))
            {
                attack = dmgData[owner];
                first = false;
            }
            int Dmg = (int)((double)Damage*(1+0.2*attack));
            if (this._owner.faction == Faction.Player)
            {
                owner.TakeDamage(Dmg, DamageType.Emotion,this._owner);
                this._owner.RecoverHP(Dmg);
                if (first)
                    dmgData.Add(owner, 1);
                else
                    dmgData[owner] += 1;
            }
            if (this._owner.faction == Faction.Enemy)
            {
                if ((double)dmg < (double)this._owner.MaxHp * 0.01)
                    return;
                owner.TakeDamage(Dmg,DamageType.Emotion, this._owner);
                this._owner.RecoverHP(Dmg);
                if (first)
                    dmgData.Add(owner, 1);
                else
                    dmgData[owner] += 1;
            }
        }
    }
}
