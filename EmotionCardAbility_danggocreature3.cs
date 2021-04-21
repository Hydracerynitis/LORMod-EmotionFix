using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_danggocreature3 : EmotionCardAbilityBase
    {
        private int height;
        private int Healed => this._owner.UnitData.historyInWave.healed;
        private int Threshold => (int)((double)this._owner.MaxHp * 0.1);
        private int heal;
        private int absorption;
        private int count;
        public override void OnWaveStart()
        {
            heal = 0;
            absorption = 0;
            count = 0;
        }
        public override void OnSelectEmotion()
        {
            heal = Healed;
            absorption = 0;
            height = this._owner.UnitData.unitData.customizeData.height;
        }
        public override void OnRoundEndTheLast()
        {
            count = 0;
            absorption += (Healed - heal);
            heal = Healed;
            absorption -= this._owner.history.takeDamageAtOneRound;
            if (absorption <= 0)
                absorption = 0;
            int Absorption = absorption;
            while (Absorption > Threshold)
            {
                count += 1;
                Absorption -= Threshold;
            }
            this._owner.UnitData.unitData.customizeData.height = height;
            this._owner.view.CreateSkin();
        }
        public override void OnRoundStart()
        {
            this._owner.bufListDetail.AddBuf(new Indicator(absorption));
            MoutainCorpse moutain = new MoutainCorpse(count);
            this._owner.bufListDetail.AddBuf(moutain );
            this._owner.UnitData.unitData.customizeData.height =(int)((double)height *(1+(double)moutain.stack*0.25));
            this._owner.view.CreateSkin();
            
        }
        public class MoutainCorpse: BattleUnitBuf
        {
            protected override string keywordId => "MountainCorpse";
            protected override string keywordIconId => "DangoCreature_Emotion_Healed";
            public MoutainCorpse(int count)
            {
                this.stack = count;
            }
            public override int GetDamageIncreaseRate() => 25*stack;
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = stack
                });
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }
        public class Indicator: BattleUnitBuf
        {
            protected override string keywordId => "Indicator";
            protected override string keywordIconId => "DangoCreature_Emotion_Healed";
            public Indicator(int absorption)
            {
                this.stack = absorption;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }
    }
}
