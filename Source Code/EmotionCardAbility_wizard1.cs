using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using HarmonyLib;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_wizard1: EmotionCardAbilityBase
    {
        private BattleDiceCardModel jade;
        private int round;
        private bool Trigger;
        private int _targetID = 1100015;
        public override void OnWaveStart()
        {
            if(this._owner.faction==Faction.Player)
                jade=this._owner.allyCardDetail.AddNewCardToDeck(this._targetID);
        }
        public override void OnSelectEmotion()
        {
            if(this._owner.faction==Faction.Player)
                jade=this._owner.allyCardDetail.AddNewCard(this._targetID);
            if (this._owner.faction == Faction.Enemy)
            {
                round = 4;
                Trigger = false;
            }
        }
        public override void OnRoundStart()
        {
            base.OnRoundStartOnce();
            if (this._owner.faction != Faction.Enemy)
                return;
            if (round!=5)
                round += 1;
            if (round == 5)
            {
                if (this._owner.cardSlotDetail.PlayPoint >= 3)
                {
                    this._owner.cardSlotDetail.SpendCost(3);
                    Trigger = true;
                }
            }
        }
        public override void OnStartBattle()
        {
            if (!Trigger || this._owner.faction != Faction.Enemy)
                return;
            BattleUnitModel victim = RandomUtil.SelectOne<BattleUnitModel>(BattleObjectManager.instance.GetAliveList(Faction.Player));
            victim.cardSlotDetail.DestroyCard(2);
            Trigger = false;
            round = 0;
        }
        public void Destroy()
        {
            if (this._owner.faction == Faction.Player)
                this._owner.allyCardDetail.ExhaustACardAnywhere(jade);
        }
    }
}
