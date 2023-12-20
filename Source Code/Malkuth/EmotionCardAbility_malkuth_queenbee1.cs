﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_malkuth_queenbee1 : EmotionCardAbilityBase
    {
        private static int Burn => RandomUtil.Range(1, 3);
        private static int Bleed => RandomUtil.Range(1, 3);
        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            if (atkDice.owner == null)
                return;
            if (dmg < _owner.MaxHp * 0.02)
                return;
            atkDice?.owner?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, Burn, this._owner);
            atkDice?.owner?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, Bleed, this._owner);
            atkDice.owner.battleCardResultLog?.SetCreatureEffectSound("Creature/QueenBee_Funga");
            _owner.battleCardResultLog?.SetCreatureAbilityEffect("1/Queenbee_Spore", 2f);
        }
    }
}
