using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using HarmonyLib;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_pinocchio3 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Pino_Success")?.SetGlobalPosition(_owner.view.WorldPosition);
        }
        public override void OnDrawCard()
        {
            base.OnDrawCard();
            _owner.allyCardDetail.ReturnAllToDeck();
            _owner.allyCardDetail.DrawCards(4);
            MakeEffect("0/Pinocchio_Curiosity", destroyTime: 3f);
        }
    }
}
