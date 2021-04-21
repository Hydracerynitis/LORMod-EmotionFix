using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_queenbee1 : EmotionCardAbilityBase
    {
        private static int Burn => RandomUtil.Range(1, 3);
        private static int Burn_Enemy => RandomUtil.Range(0, 2);
        private static int Bleed => RandomUtil.Range(1, 3);
        private static int Bleed_Enemy => RandomUtil.Range(0, 2);
        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            int burn1 = Burn;
            int burn2 = Burn_Enemy;
            int bleed1 = Bleed;
            int bleed2 = Bleed_Enemy;
            if (atkDice.owner == null)
                return;
            if (this._owner.faction == Faction.Player)
            {
                atkDice?.owner?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, burn1, this._owner);
                atkDice?.owner?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, bleed1, this._owner);
            }
            if (this._owner.faction == Faction.Enemy)
            {
                if ((double)dmg < (double)this._owner.MaxHp * 0.01)
                    return;
                atkDice?.owner?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, burn2, this._owner);
                atkDice?.owner?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, bleed2, this._owner);
            }
            atkDice.owner.battleCardResultLog?.SetCreatureEffectSound("Creature/QueenBee_Funga");
            this._owner.battleCardResultLog?.SetCreatureAbilityEffect("1/Queenbee_Spore", 2f);
        }
    }
}
