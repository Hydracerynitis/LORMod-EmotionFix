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
            BattleUnitBuf buf = target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Alriune_Debuf));
            if (buf == null)
            {
                buf = new BattleUnitBuf_Alriune_Debuf();
                target.bufListDetail.AddBuf(buf);
            }
            if (!(buf is BattleUnitBuf_Alriune_Debuf unitBufAlriuneDebuf))
                return;
            unitBufAlriuneDebuf.Reserve();
            if(this._owner.faction==Faction.Player)
                unitBufAlriuneDebuf.Reserve();
        }
    }
}
