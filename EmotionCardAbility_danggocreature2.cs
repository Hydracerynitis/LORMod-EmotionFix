using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_danggocreature2 : EmotionCardAbilityBase
    {
        List<BattleDiceCardModel> Dead;
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            Dead = new List<BattleDiceCardModel>();
        }
        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            if (unit == this._owner || this._owner.faction!=unit.faction)
                return;
            List<BattleDiceCardModel> deadman = unit.allyCardDetail.GetAllDeck();
            List<BattleDiceCardModel> remain = new List<BattleDiceCardModel>();
            for(int i=0; i < 3; i++)
            {
                BattleDiceCardModel deadcard = RandomUtil.SelectOne<BattleDiceCardModel>(deadman);
                deadman.Remove(deadcard);
                remain.Add(deadcard);
            }
            this._owner.allyCardDetail.AddCardToDeck(remain);
            Dead.AddRange(remain);
            this._owner.allyCardDetail.Shuffle();
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this._owner.allyCardDetail.AddCardToDeck(Dead);
            this._owner.allyCardDetail.Shuffle();
        }
        public void Destroy()
        {
            if (Dead.Count <= 0)
                return;
            foreach (BattleDiceCardModel card in Dead)
                this._owner.allyCardDetail.ExhaustACardAnywhere(card);
        }
    }
}
