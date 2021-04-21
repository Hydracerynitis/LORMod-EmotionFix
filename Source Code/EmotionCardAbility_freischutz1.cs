using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_freischutz1 : EmotionCardAbilityBase
    {
        private BattleDiceCardModel request;
        public override void OnWaveStart()
        {
            request=this._owner.allyCardDetail.AddNewCard(1100005);
        }
        public override void OnSelectEmotion()
        {
            request=this._owner.allyCardDetail.AddNewCard(1100005);
        }
        public void Destroy()
        {
            this._owner.allyCardDetail.ExhaustACardAnywhere(request);
        }
    }
}
