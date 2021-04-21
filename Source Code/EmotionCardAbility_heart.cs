using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using HarmonyLib;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_heart : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            Singleton<StageController>.Instance.onChangePhase += new StageController.OnChangePhaseDelegate((this).OnChangeStagePhase);
            this._owner.bufListDetail.AddBuf(new AspirationHeart(this._owner));
            SoundEffectPlayer.PlaySound("Creature/Heartbeat");
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this._owner.bufListDetail.AddBuf(new AspirationHeart(this._owner));
        }
        public override StatBonus GetStatBonus() => new StatBonus()
        {
            hpRate = 15,       
        };
        public void Destroy()
        {
            if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is AspirationHeart)) is AspirationHeart Heart)
                Heart.Destroy();
            SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(this._owner, this._owner.faction, this._owner.hp, this._owner.breakDetail.breakGauge);
        }
        public class AspirationHeart: BattleUnitBuf
        {
            private bool Trigger;
            protected override string keywordId => "AspirationHeart";
            public AspirationHeart(BattleUnitModel owner)
            {
                this._owner = owner;
                this.stack = 0;
                typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue((object)this, (object)Resources.Load<Sprite>("Sprites/BufIcon/Ability/HeartofAspiration_Heart"));
                typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue((object)this, (object)true);
            }
            public override int GetSpeedDiceAdder(int speedDiceResult) => RandomUtil.Range(1, 2);
            public override int SpeedDiceNumAdder()
            {
                if(Trigger)
                    return 1;
                return base.SpeedDiceNumAdder();
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Trigger = false;
                if (this._owner.history.damageAtOneRound < 30)
                    return;
                Trigger = true;
            }
        }
    }
}
