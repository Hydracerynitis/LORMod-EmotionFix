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
    public class DiceCardSelfAbility_King: DiceCardSelfAbilityBase
    {
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            if (owner.bufListDetail.GetActivatedBufList().Exists((Predicate<BattleUnitBuf>)(x => x is Cooldown_King)))
                return false;
            return base.OnChooseCard(owner);
        }
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            unit.bufListDetail.AddBuf(new Assimilation_King());
            unit.bufListDetail.AddBuf(new Cooldown_King(4));
            SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(unit, unit.faction, unit.hp, unit.breakDetail.breakGauge, unit.bufListDetail.GetBufUIDataList());
        }
        public class Assimilation_King : BattleUnitBuf
        {
            private int light;
            public override int SpeedDiceNumAdder() => 2;
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                light = this._owner.cardSlotDetail.PlayPoint;
                this._owner.ChangeTemporaryDeck(new List<int>()
                {
                    9905631,
                    9905631,
                    9905632,
                    9905632,
                    9905633,
                    9905633,
                    9905633,
                    9905634,
                    9905634
                }, 8);
                this._owner.bufListDetail.RemoveBufAll(BufPositiveType.Negative);
                this._owner.view.ChangeCreatureSkin("Nihil_GreedLibrarian");
                this._owner.view.StartEgoSkinChangeEffect("Character");
                this._owner.cardSlotDetail.RecoverPlayPoint(this._owner.cardSlotDetail.GetMaxPlayPoint());
                this._owner.OnRoundStartOnlyUI();
                this._owner.RollSpeedDice();
            }
            public override void OnRoundEnd()
            {
                this._owner.view.ResetSkin();
                this._owner.ReturnToOriginalDeck();
                this._owner.view.StartEgoSkinChangeEffect("Character");
                this._owner.cardSlotDetail.LosePlayPoint(this._owner.cardSlotDetail.GetMaxPlayPoint());
                this._owner.cardSlotDetail.RecoverPlayPoint(light);
                this.Destroy();
            }
        }
        public class Cooldown_King : BattleUnitBuf
        {
            public Cooldown_King(int stack)
            {
                this.stack = stack;
            }
            public override void OnRoundEnd()
            {
                this.stack-=1;
                if (stack <= 0)
                    this.Destroy();
            }
        }
    }
}
