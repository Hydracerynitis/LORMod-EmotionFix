using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix.Geburah
{
    public class EmotionCardAbility_geburah_nosferatu1 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            _owner.bufListDetail.AddBuf(new FearOfWater());
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            _owner.bufListDetail.AddBuf(new FearOfWater());
        }
        private int AddDmg => RandomUtil.Range(3, 7);
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (!IsAttackDice(behavior.Detail))
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                dmg = AddDmg
            });
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            behavior?.card?.target?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, 2, _owner);
            behavior?.card?.target?.battleCardResultLog?.SetCreatureAbilityEffect("6/Nosferatu_Emotion_BloodDrain");
            behavior?.card?.target?.battleCardResultLog?.SetCreatureEffectSound("Nosferatu_Changed_BloodEat");
        }
    }
}

