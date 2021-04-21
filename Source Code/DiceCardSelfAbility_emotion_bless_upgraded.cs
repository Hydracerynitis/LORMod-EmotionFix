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
    public class DiceCardSelfAbility_emotion_bless_upgraded : DiceCardSelfAbilityBase
    {
        public override bool IsValidTarget(
          BattleUnitModel unit,
          BattleDiceCardModel self,
          BattleUnitModel targetUnit)
        {
            return targetUnit != null && targetUnit.faction == unit.faction && targetUnit.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_PlagueDoctor_Blessed)) == null;
        }
        public override bool IsOnlyAllyUnit() => true;

        public override void OnUseInstance(
          BattleUnitModel unit,
          BattleDiceCardModel self,
          BattleUnitModel targetUnit)
        {
            self.exhaust = true;
            if (targetUnit != null)
            {
                targetUnit.RecoverHP(12);
                targetUnit.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Endurance, 2, unit);
                targetUnit.bufListDetail.AddBuf((BattleUnitBuf)new BattleUnitBuf_PlagueDoctor_Blessed());
            }
            unit.allyCardDetail.AddNewCard(self.GetID());
            if (!(this.owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_PlagueDoctor_BlessStack)) is BattleUnitBuf_PlagueDoctor_BlessStack doctorBlessStack))
            {
                doctorBlessStack = new BattleUnitBuf_PlagueDoctor_BlessStack();
                this.owner.bufListDetail.AddBuf((BattleUnitBuf)doctorBlessStack);
            }
            doctorBlessStack.Add();
            SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(unit, unit.faction, unit.hp, unit.breakDetail.breakGauge, unit.bufListDetail.GetBufUIDataList());
            SingletonBehavior<BattleManagerUI>.Instance.ui_unitCardsInHand.SetCardsObject(unit);
        }
    }
}
