using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using HarmonyLib;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_pinocchio1 : EmotionCardAbilityBase
    {
        private BattleDiceCardModel learn;
        private List<LorId> copyiedList;
        public override void OnWaveStart()
        {
            if (_owner.faction == Faction.Player)
                learn=_owner.allyCardDetail.AddNewCard(GetTargetCardId());
            if (_owner.faction == Faction.Enemy)
                copyiedList = new List<LorId>();
        }
        public override void OnSelectEmotion()
        {
            SoundEffectPlayer.PlaySound("Creature/Pino_On");
            if (_owner.faction == Faction.Player)
                learn=_owner.allyCardDetail.AddNewCard(GetTargetCardId());
            if (_owner.faction == Faction.Enemy)
                copyiedList = new List<LorId>();        
        }
        public override void OnDrawCard()
        {
            base.OnDrawCard();
            if(_owner.faction==Faction.Enemy)
                SoundEffectPlayer.PlaySound("Creature/Pino_On");
            BattleDiceCardModel copy = _owner.allyCardDetail.AddNewCard(RandomUtil.SelectOne<LorId>(copyiedList));
            copy.temporary = true;
            copy.SetPriorityAdder(100);
            copyiedList.Clear();
        }
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            if (_owner.faction != Faction.Enemy)
                return;
            foreach (BattleUnitModel player in BattleObjectManager.instance.GetAliveList(Faction.Player))
            {
                List<BattlePlayingCardDataInUnitModel> cardAry = player.cardSlotDetail.cardAry;
                if (cardAry == null)
                    continue;
                foreach (BattlePlayingCardDataInUnitModel cardDataInUnitModel in cardAry)
                {
                    if (cardDataInUnitModel?.card != null)
                    {
                        if (cardDataInUnitModel.card.XmlData.optionList.Contains(CardOption.Personal))
                            continue;
                        LorId num = cardDataInUnitModel.card.GetID();
                        copyiedList.Add(num);
                    }
                }
            }
        }
        public void Destroy()
        {
            _owner.allyCardDetail.ExhaustACardAnywhere(learn);
        }
        private int GetTargetCardId()
        {
            int num = 1100001;
            if (_owner.Book.ClassInfo.RangeType == EquipRangeType.Range)
                num = 1100002;
            return num;
        }
    }
}
