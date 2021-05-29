using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using HarmonyLib;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_wizard3: EmotionCardAbilityBase
    {
        private BattleDiceCardModel trick;
        private int round;
        private int _targetID = 1100016;
        public override void OnWaveStart()
        {
            if(this._owner.faction==Faction.Player)
                trick=this._owner.allyCardDetail.AddNewCardToDeck(this._targetID);
        }
        public override void OnSelectEmotion()
        {
            if(this._owner.faction==Faction.Player)
                trick=this._owner.allyCardDetail.AddNewCard(this._targetID);
            if (this._owner.faction == Faction.Enemy)
            {
                round = 3;
            }
        }
        public override void OnRoundStart()
        {
            base.OnRoundStartOnce();
            if (this._owner.faction != Faction.Enemy)
                return;
            if (round!=4)
                round += 1;
            if (round == 4)
            {
                if (this._owner.cardSlotDetail.PlayPoint >= 4)
                {
                    this._owner.cardSlotDetail.SpendCost(4);
                    foreach (BattleDiceCardModel battleDiceCardModel in this._owner.allyCardDetail.GetHand())
                        battleDiceCardModel.AddBuf(new DiceCardSelfAbility_oz_control.BattleDiceCardBuf_costZero1Round());
                    round = 0;
                    SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Magic", 1f, _owner.view,_owner.view, 3f);
                    SoundEffectPlayer.PlaySound("Creature/Oz_CardMagic");
                }
            }
        }
        public void Destroy()
        {
            if (this._owner.faction == Faction.Player)
                this._owner.allyCardDetail.ExhaustACardAnywhere(trick);
        }
    }
}
