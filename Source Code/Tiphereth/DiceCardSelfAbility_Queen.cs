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
    public class DiceCardSelfAbility_Queen: DiceCardSelfAbilityBase
    {
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            if (owner.bufListDetail.GetActivatedBufList().Exists((Predicate<BattleUnitBuf>)(x => x is Cooldown_Queen)))
                return false;
            return base.OnChooseCard(owner);
        }
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            unit.bufListDetail.AddBuf(new Assimilation_Queen());
            unit.bufListDetail.AddBuf(new Cooldown_Queen(4));
            SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(unit, unit.faction, unit.hp, unit.breakDetail.breakGauge, unit.bufListDetail.GetBufUIDataList());
        }
        public class Assimilation_Queen : BattleUnitBuf
        {
            public override int SpeedDiceNumAdder() => 2;
            private int light;
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                light = _owner.cardSlotDetail.PlayPoint;
                _owner.ChangeTemporaryDeck(new List<LorId>()
                {
                    new LorId(9905611) ,
                    new LorId(9905612) ,
                    new LorId(9905612) ,
                    new LorId(9905613) ,
                    new LorId(9905613) ,
                    new LorId(9905613),
                    new LorId(9905614) ,
                    new LorId(9905614),
                    new LorId(9905614) 
                }, 8);
                _owner.view.ChangeCreatureSkin("Nihil_HatredLibrarian");
                _owner.view.StartEgoSkinChangeEffect("Character");
                _owner.cardSlotDetail.RecoverPlayPoint(_owner.cardSlotDetail.GetMaxPlayPoint());
                _owner.OnRoundStartOnlyUI();
                _owner.RollSpeedDice();
            }
            public override void OnRoundEnd()
            {
                _owner.view.ResetSkin();
                _owner.ReturnToOriginalDeck();
                _owner.view.StartEgoSkinChangeEffect("Character");
                _owner.cardSlotDetail.LosePlayPoint(_owner.cardSlotDetail.GetMaxPlayPoint());
                _owner.cardSlotDetail.RecoverPlayPoint(light);
                Destroy();
            }
        }
        public class Cooldown_Queen: BattleUnitBuf
        {
            public Cooldown_Queen(int stack)
            {
                this.stack = stack;
            }
            public override void OnRoundEnd()
            {
                stack-=1;
                if (stack <= 0)
                    Destroy();
            }
        }
    }
}
