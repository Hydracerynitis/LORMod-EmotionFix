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
    public class DiceCardSelfAbility_Servant: DiceCardSelfAbilityBase
    {
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            if (owner.bufListDetail.GetActivatedBufList().Exists((Predicate<BattleUnitBuf>)(x => x is Cooldown_Servant)))
                return false;
            return base.OnChooseCard(owner);
        }
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            unit.bufListDetail.AddBuf(new Assimilation_Servant());
            unit.bufListDetail.AddBuf(new Cooldown_Servant(4));
            SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(unit, unit.faction, unit.hp, unit.breakDetail.breakGauge, unit.bufListDetail.GetBufUIDataList());
        }
        public class Assimilation_Servant : BattleUnitBuf
        {
            public override int SpeedDiceNumAdder() => 2;
            private int light;
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                light = this._owner.cardSlotDetail.PlayPoint;
                this._owner.ChangeTemporaryDeck(new List<LorId>()
                {
                    new LorId(9905641),
                    new LorId(9905641) ,
                    new LorId(9905641) ,
                    new LorId(9905641) ,
                    new LorId(9905642) ,
                    new LorId(9905642) ,
                    new LorId(9905642) ,
                    new LorId(9905643),
                    new LorId(9905643) 
                }, 8);
                this._owner.view.ChangeCreatureSkin("Nihil_WrathLibrarian");
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
        public class Cooldown_Servant : BattleUnitBuf
        {
            public Cooldown_Servant(int stack)
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
