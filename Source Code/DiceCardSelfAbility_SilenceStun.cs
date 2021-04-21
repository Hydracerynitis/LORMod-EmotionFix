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
    public class DiceCardSelfAbility_SilenceStun : DiceCardSelfAbilityBase
    {
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            base.OnUseInstance(unit, self, targetUnit);
            unit.bufListDetail.AddBuf(new EmotionCardAbility_silence3.SilenceStun());
            unit.view.speedDiceSetterUI.DeselectAll();
            unit.OnRoundStartOnlyUI();
            unit.RollSpeedDice();
            targetUnit.bufListDetail.AddBuf(new EmotionCardAbility_silence3.SilenceStun());
            targetUnit.view.speedDiceSetterUI.DeselectAll();
            targetUnit.OnRoundStartOnlyUI();
            targetUnit.RollSpeedDice();
        }
    }
}
