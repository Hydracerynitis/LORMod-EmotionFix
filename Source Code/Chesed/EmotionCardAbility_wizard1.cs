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
            if(_owner.faction==Faction.Player)
                jade=_owner.allyCardDetail.AddNewCardToDeck(_targetID);
        }
        public override void OnSelectEmotion()
        {
            if(_owner.faction==Faction.Player)
                jade=_owner.allyCardDetail.AddNewCard(_targetID);
            if (_owner.faction == Faction.Enemy)
            {
                round = 4;
                Trigger = false;
            }
        }
        public override void OnRoundStart()
        {
            base.OnRoundStartOnce();
            if (_owner.faction != Faction.Enemy)
                return;
            if (round!=5)
                round += 1;
            if (round == 5)
            {
                if (_owner.cardSlotDetail.PlayPoint >= 3)
                {
                    _owner.cardSlotDetail.SpendCost(3);
                    SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Magic", 1f, _owner.view, _owner.view, 3f);
                    SoundEffectPlayer.PlaySound("Creature/Oz_Atk_Up");
                    Trigger = true;
                }
            }
        }
        public override void OnStartBattle()
        {
            if (!Trigger || _owner.faction != Faction.Enemy)
                return;
            BattleUnitModel victim = RandomUtil.SelectOne<BattleUnitModel>(BattleObjectManager.instance.GetAliveList(Faction.Player));
            victim.cardSlotDetail.DestroyCard(2);
            Trigger = false;
            round = 0;
        }
        public void Destroy()
        {
            if (_owner.faction == Faction.Player)
                _owner.allyCardDetail.ExhaustACardAnywhere(jade);
        }
    }
}
