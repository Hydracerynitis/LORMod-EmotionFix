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
    public class DiceCardSelfAbility_Knight: DiceCardSelfAbilityBase
    {
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            if (owner.bufListDetail.GetActivatedBufList().Exists((Predicate<BattleUnitBuf>)(x => x is Cooldown_Knight)))
                return false;
            return base.OnChooseCard(owner);
        }
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            unit.bufListDetail.AddBuf(new Assimilation_Knight());
            unit.bufListDetail.AddBuf(new Cooldown_Knight(4));
            SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(unit, unit.faction, unit.hp, unit.breakDetail.breakGauge, unit.bufListDetail.GetBufUIDataList());
        }
        public class Assimilation_Knight : BattleUnitBuf
        {
            public override int SpeedDiceNumAdder() => 2;
            private int light;
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                light = this._owner.cardSlotDetail.PlayPoint;
                this._owner.ChangeTemporaryDeck(new List<int>()
                {
                    9905621,
                    9905621,
                    9905621,
                    9905622,
                    9905622,
                    9905622,
                    9905623,
                    9905623,
                    9905623
                }, 8);
                this._owner.bufListDetail.RemoveBufAll(BufPositiveType.Negative);
                this._owner.view.ChangeCreatureSkin("Nihil_DespairLibrarian");
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
        public class Cooldown_Knight : BattleUnitBuf
        {
            public Cooldown_Knight(int stack)
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
