using System;
using LOR_DiceSystem;
using System.IO;
using UnityEngine;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class DiceCardSelfAbility_Wizard: DiceCardSelfAbilityBase
    {
        public static List<int> WizardList = new List<int>() { 1100023, 1100024, 1100025, 1100026, 1100027 };
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            GiveBuff(unit);
            foreach(BattleDiceCardModel card in unit.personalEgoDetail.GetCardAll())
            {
                if (WizardList.Exists(x => card.GetID() == x))
                    unit.personalEgoDetail.RemoveCard(card.GetID());
            }
            foreach(BattleUnitModel model in BattleObjectManager.instance.GetAliveList(unit.faction).FindAll(x=> x != unit))
                model.personalEgoDetail.RemoveCard(self.GetID());
        }
        public virtual void GiveBuff(BattleUnitModel self)
        {

        }
    }
}
