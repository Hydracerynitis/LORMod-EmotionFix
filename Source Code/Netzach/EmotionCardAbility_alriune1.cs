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
    public class EmotionCardAbility_alriune1 : EmotionCardAbilityBase
    {
        private double Ratio => (double)this._owner.breakDetail.breakGauge / (double)this._owner.breakDetail.GetDefaultBreakGauge();
        public override void OnRoundStart()
        {
            if (Ratio<0.5)
                return;
            int num = (int)((double)this._owner.breakDetail.breakGauge * 0.25);
            this._owner.TakeBreakDamage(num);
            this._owner.view.BreakDamaged(num, BehaviourDetail.Penetrate, this._owner,AtkResist.Normal);
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(this._owner.faction==Faction.Player? Faction.Player : Faction.Enemy).FindAll((Predicate<BattleUnitModel>)(x=>x !=this._owner)))
            {
                unit.breakDetail.RecoverBreak(num);
            }
            if (Singleton<StageController>.Instance.IsLogState())
                this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("4_N/FX_IllusionCard_4_N_FlowerPiece", 2f);
            else
                SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("4_N/FX_IllusionCard_4_N_FlowerPiece", 1f, this._owner.view, this._owner.view, 2f);
        }
    }
}
