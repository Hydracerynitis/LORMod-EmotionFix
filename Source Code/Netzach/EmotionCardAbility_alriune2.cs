using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_alriune2 : EmotionCardAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null)
                return;
            if (behavior.Detail != BehaviourDetail.Penetrate && this._owner.faction == Faction.Player)
                return;
            int num = 0;
            if (_owner.faction == Faction.Enemy)
                num = 1;
            if (_owner.faction == Faction.Player)
                num = RandomUtil.Range(1, 2);
            target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Alriune_Debuf, num, this._owner);
            target.battleCardResultLog?.SetCreatureAbilityEffect("0/Alriune_Stun_Effect", 1f);
            target.battleCardResultLog?.SetCreatureEffectSound("Creature/Ali_Guard");
        }
    }
}
