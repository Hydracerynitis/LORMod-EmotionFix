using System;
using LOR_DiceSystem;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class DiceCardSelfAbility_BlessingPlagueUpGraded : DiceCardSelfAbilityBase
    {
        public override void OnStartBattle()
        {
            SingletonBehavior<DiceEffectManager>.Instance.CreateBehaviourEffect("WhiteNight_Blessing", 1f, card.target.view, card.target.view);
            this.card.target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Endurance, 2);
            this.card.target.RecoverHP(12);
            EmotionCardAbility_plaguedoctor1.WhiteNightClock[this.owner.UnitData] += 1;
        }
        public override bool IsTargetChangable(BattleUnitModel attacker)
        {
            return false;
        }
    }
}
