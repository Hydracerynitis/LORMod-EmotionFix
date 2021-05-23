using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_scarecrow3 : EmotionCardAbilityBase
    {
        private bool Trigger;
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            Trigger = false;
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if (this._owner.faction != Faction.Enemy || behavior.card.target==null || Trigger)
                return;
            Trigger = true;
            List<BattleDiceBehavior> diceinhand = new List<BattleDiceBehavior>();
            foreach (BattleDiceCardModel card in behavior.card.target.allyCardDetail.GetHand())
            {
                diceinhand.AddRange(card.CreateDiceCardBehaviorList().FindAll((Predicate<BattleDiceBehavior>)(x => x.Type==BehaviourType.Atk)));
            }
            if (diceinhand.Count == 0)
                return;
            behavior.card.AddDice(RandomUtil.SelectOne<BattleDiceBehavior>(diceinhand));
            this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Scarecrow_Dead");
        }
        public override void OnMakeBreakState(BattleUnitModel target)
        {
            base.OnMakeBreakState(target);
            if (target == null || target == this._owner || this._owner.faction!=Faction.Player)
                return;
            this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Scarecrow_Dead");
            List<BattleDiceCardModel> battleDiceCardModelList = new List<BattleDiceCardModel>();
            switch (this._owner.Book.ClassInfo.RangeType)
            {
                case EquipRangeType.Melee:
                    battleDiceCardModelList = target.allyCardDetail.GetAllDeck().FindAll((Predicate<BattleDiceCardModel>)(x => x.GetSpec().Ranged == CardRange.Near));
                    break;
                case EquipRangeType.Range:
                    battleDiceCardModelList = target.allyCardDetail.GetAllDeck().FindAll((Predicate<BattleDiceCardModel>)(x => x.GetSpec().Ranged == CardRange.Far));
                    break;
                case EquipRangeType.Hybrid:
                    battleDiceCardModelList = target.allyCardDetail.GetAllDeck();
                    break;
            }
            foreach (int index in MathUtil.Combination(2, battleDiceCardModelList.Count))
            {
                BattleDiceCardModel battleDiceCardModel = this._owner.allyCardDetail.AddNewCard(battleDiceCardModelList[index].GetID());
                battleDiceCardModel.exhaust = true;
                battleDiceCardModel.SetCurrentCost(battleDiceCardModel.GetOriginCost() - 2);
            }
        }
    }
}
