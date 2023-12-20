using System;
using LOR_DiceSystem;
using System.IO;
using UnityEngine;
using Sound;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class DiceCardSelfAbility_WizardHeart: DiceCardSelfAbility_Wizard
    {
        public override void GiveBuff(BattleUnitModel self)
        {
            base.GiveBuff(self);
            self.bufListDetail.AddBuf(new EmotionCardAbility_wizard2.BattleUnitBuf_heart());
            SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(self, self.faction, self.hp, self.breakDetail.breakGauge, self.bufListDetail.GetBufUIDataList());
            SoundEffectPlayer.PlaySound("Creature/Oz_StongAtk_Start");
        }
    }
}
