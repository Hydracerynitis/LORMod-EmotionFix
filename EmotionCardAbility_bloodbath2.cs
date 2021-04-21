using System;
using LOR_DiceSystem;
using UI;
using Sound;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_bloodbath2 : EmotionCardAbilityBase
    {
        private static bool Prob => (double)RandomUtil.valueForProb < 0.2;
        private static int Reduce => RandomUtil.Range(2, 5);
        private Dictionary<BehaviourDetail, int> dict;
        private BehaviourDetail atk;
        public override void OnSelectEmotion()
        {
            dict = new Dictionary<BehaviourDetail, int>() { { BehaviourDetail.Slash, 0 }, { BehaviourDetail.Hit, 0 },{ BehaviourDetail.Penetrate,0} };
            atk = BehaviourDetail.None;
        }
        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            atk = atkDice.Detail;
            dict[atk] += dmg;
            base.OnTakeDamageByAttack(atkDice, dmg);
        }
        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            if (this._owner.faction == Faction.Player && Prob)
            {
                this._owner.battleCardResultLog?.SetCreatureAbilityEffect("0/BloodyBath_Scar", 1f);
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/BloodBath_Barrier");
                return true;
            }
            return base.BeforeTakeDamage(attacker, dmg);
        }
        public override void OnRoundStart()
        {
            base.OnRoundEnd();
            int num = dict[BehaviourDetail.Slash];
            List<BehaviourDetail> Dmg = new List<BehaviourDetail>() { BehaviourDetail.Slash};
            if (dict[BehaviourDetail.Hit] > num)
            {
                Dmg.Clear();
                Dmg.Add(BehaviourDetail.Hit);
                num = dict[BehaviourDetail.Hit];
            }
            else if (dict[BehaviourDetail.Hit] == num)
                Dmg.Add(BehaviourDetail.Hit);
            if (dict[BehaviourDetail.Penetrate] > num)
            {
                Dmg.Clear();
                Dmg.Add(BehaviourDetail.Penetrate);
                num = dict[BehaviourDetail.Penetrate];
            }
            else if (dict[BehaviourDetail.Penetrate] == num)
                Dmg.Add(BehaviourDetail.Penetrate);
            switch (RandomUtil.SelectOne<BehaviourDetail>(Dmg))
            {
                case BehaviourDetail.Slash:
                    this._owner.bufListDetail.AddBuf(new SlashProt(this._emotionCard));
                    break;
                case BehaviourDetail.Hit:
                    this._owner.bufListDetail.AddBuf(new HitProt(this._emotionCard));
                    break;
                case BehaviourDetail.Penetrate:
                    this._owner.bufListDetail.AddBuf(new PenetrateProt(this._emotionCard));
                    break;
            }
            dict = new Dictionary<BehaviourDetail, int>() { { BehaviourDetail.Slash, 0 }, { BehaviourDetail.Hit, 0 }, { BehaviourDetail.Penetrate, 0 } };
            atk = BehaviourDetail.None;
        }
        public class SlashProt: BattleUnitBuf
        {
            private BattleEmotionCardModel emotionCard;
            protected override string keywordIconId => "StanceSlash";
            protected override string keywordId => "SlashProtect";
            public SlashProt(BattleEmotionCardModel card)
            {
                emotionCard = card;
                stack = 0;
            }
            public override int GetDamageReduction(BehaviourDetail behaviourDetail)
            {
                if (behaviourDetail == BehaviourDetail.Slash)
                {
                    int reduce = EmotionCardAbility_bloodbath2.Reduce;
                    this._owner.battleCardResultLog?.SetEmotionAbility(true, emotionCard, 0, ResultOption.Sign, -reduce);
                    this._owner.battleCardResultLog?.SetCreatureAbilityEffect("0/BloodyBath_Scar", 1f);
                    this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/BloodBath_Barrier");
                    return reduce;
                }
                return base.GetDamageReduction(behaviourDetail);
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }
        public class HitProt : BattleUnitBuf
        {
            private BattleEmotionCardModel emotionCard;
            protected override string keywordIconId => "StanceHit";
            protected override string keywordId => "HitProtect";
            public HitProt(BattleEmotionCardModel card)
            {
                emotionCard = card;
                stack = 0;
            }
            public override int GetDamageReduction(BehaviourDetail behaviourDetail)
            {
                if (behaviourDetail == BehaviourDetail.Hit)
                {
                    int reduce = EmotionCardAbility_bloodbath2.Reduce;
                    this._owner.battleCardResultLog?.SetEmotionAbility(true, emotionCard, 0, ResultOption.Sign, -reduce);
                    this._owner.battleCardResultLog?.SetCreatureAbilityEffect("0/BloodyBath_Scar", 1f);
                    this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/BloodBath_Barrier");
                    return reduce;
                }
                return base.GetDamageReduction(behaviourDetail);
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }
        public class PenetrateProt : BattleUnitBuf
        {
            private BattleEmotionCardModel emotionCard;
            protected override string keywordIconId => "StancePenetrate";
            protected override string keywordId => "PenetrateProtect";
            public PenetrateProt(BattleEmotionCardModel card)
            {
                emotionCard = card;
                stack = 0;
            }
            public override int GetDamageReduction(BehaviourDetail behaviourDetail)
            {
                if (behaviourDetail == BehaviourDetail.Penetrate)
                {
                    int reduce = EmotionCardAbility_bloodbath2.Reduce;
                    this._owner.battleCardResultLog?.SetEmotionAbility(true, emotionCard, 0, ResultOption.Sign, -reduce);
                    this._owner.battleCardResultLog?.SetCreatureAbilityEffect("0/BloodyBath_Scar", 1f);
                    this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/BloodBath_Barrier");
                    return reduce;
                }
                return base.GetDamageReduction(behaviourDetail);
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }
    }
}
