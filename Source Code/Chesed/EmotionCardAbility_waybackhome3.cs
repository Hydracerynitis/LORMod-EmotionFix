using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using HarmonyLib;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_waybackhome3 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            GiveBuf();
        }

        public override void OnWaveStart()
        {
            base.OnWaveStart();
            GiveBuf();
        }
        public void GiveBuf()
        {
            if (_owner.faction == Faction.Player)
                _owner.bufListDetail.AddBuf(new Lion());
            if (_owner.faction == Faction.Enemy)
                _owner.bufListDetail.AddBuf(new Lion_Enemy());
        }
        public void Destroy()
        {
            if (_owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Lion)) is Lion lion)
                lion.Destroy();
            if (_owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Lion_Enemy)) is Lion_Enemy lionenemy)
                lionenemy.Destroy();
        }
        public class Lion : BattleUnitBuf
        {
            public override string keywordId => "Lion_Player";
            public override string keywordIconId => "Wizard_Lion";
            private int Pow => RandomUtil.Range(1, 2);
            public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
            {
                base.OnStartParrying(card);
                if(card.target.currentDiceAction.earlyTarget!=_owner)
                    card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
                    {
                         power = Pow
                    });
                _owner.battleCardResultLog?.SetNewCreatureAbilityEffect("7_C/FX_IllusionCard_7_C_Together", 2f);
                _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/House_MakeRoad");
            }
        }
        public class Lion_Enemy : BattleUnitBuf
        {
            public override string keywordId => "Lion_Enemy";
            public override string keywordIconId => "Wizard_Lion";
            private int Pow => RandomUtil.Range(1, 2);
            public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
            {
                base.OnStartParrying(card);
                if (card.earlyTarget==card.target)
                    card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
                    {
                        power = Pow
                    });
                _owner.battleCardResultLog?.SetNewCreatureAbilityEffect("7_C/FX_IllusionCard_7_C_Together", 2f);
                _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/House_MakeRoad");
            }
        }
    }
}
