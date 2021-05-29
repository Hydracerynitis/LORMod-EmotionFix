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
            this.GiveBuf();
        }

        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this.GiveBuf();
        }
        public void GiveBuf()
        {
            if (this._owner.faction == Faction.Player)
                this._owner.bufListDetail.AddBuf(new Lion());
            if (this._owner.faction == Faction.Enemy)
                this._owner.bufListDetail.AddBuf(new Lion_Enemy());
        }
        public void Destroy()
        {
            if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Lion)) is Lion lion)
                lion.Destroy();
            if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Lion_Enemy)) is Lion_Enemy lionenemy)
                lionenemy.Destroy();
        }
        public class Lion : BattleUnitBuf
        {
            protected override string keywordId => "Lion_Player";
            protected override string keywordIconId => "Wizard_Lion";
            private int Pow => RandomUtil.Range(1, 2);
            public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
            {
                base.OnStartParrying(card);
                if(card.target.currentDiceAction.earlyTarget!=this._owner)
                    card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
                    {
                         power = this.Pow
                    });
                this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("7_C/FX_IllusionCard_7_C_Together", 2f);
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/House_MakeRoad");
            }
        }
        public class Lion_Enemy : BattleUnitBuf
        {
            protected override string keywordId => "Lion_Enemy";
            protected override string keywordIconId => "Wizard_Lion";
            private int Pow => RandomUtil.Range(1, 2);
            public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
            {
                base.OnStartParrying(card);
                if (card.earlyTarget==card.target)
                    card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
                    {
                        power = this.Pow
                    });
                this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("7_C/FX_IllusionCard_7_C_Together", 2f);
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/House_MakeRoad");
            }
        }
    }
}
