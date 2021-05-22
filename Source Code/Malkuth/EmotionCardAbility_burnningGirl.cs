using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_burnningGirl: EmotionCardAbilityBase
    {
        private bool trigger;
        private static bool Prob => (double)RandomUtil.valueForProb < 0.4;
        private static int Burn => RandomUtil.Range(1, 3);
        private static int Burn_Enemy => RandomUtil.Range(0, 2);
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (!this.trigger)
                return;
            this.trigger = false;
            this._owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_BurningGirl_Burn());
        }
        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            int burn1 = Burn;
            int burn2 = Burn_Enemy;
            if (atkDice.owner == null)
                return;
            if (this._owner.faction == Faction.Player)
            {
                atkDice?.owner?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, burn1, this._owner);
                if (!Prob)
                    return;
                this.trigger = true;
            }
            if (this._owner.faction == Faction.Enemy)
            {
                if ((double)dmg < (double)this._owner.MaxHp * 0.01)
                    return;
                atkDice?.owner?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, burn2, this._owner);
            }
            this._owner.battleCardResultLog?.SetCreatureAbilityEffect("1/MatchGirl_Ash", 1f);
            this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/MatchGirl_Barrier");

        }
        public class BattleUnitBuf_Emotion_BurningGirl_Burn : BattleUnitBuf
        {
            protected override string keywordId => "BurningGirl_Burn";
            protected override string keywordIconId => "Burning_Match";
            private static int Burn => RandomUtil.Range(1, 1);
            public override void OnSuccessAttack(BattleDiceBehavior behavior)
            {
                base.OnSuccessAttack(behavior);
                behavior?.card?.target?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, Burn, this._owner);
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }
    }
}
