using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using HarmonyLib;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_whitenight4 : EmotionCardAbilityBase
    {
        private bool _effect;
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            _effect = false;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (!this._effect)
            {
                this._effect = true;
                GameObject gameObject = Util.LoadPrefab("Battle/CreatureEffect/FinalBattle/WhiteNight_DeadApostleImageFilter");
                if ((UnityEngine.Object)gameObject != (UnityEngine.Object)null)
                {
                    WhiteNightApostleDeadFilter component = gameObject?.GetComponent<WhiteNightApostleDeadFilter>();
                    if ((UnityEngine.Object)component != (UnityEngine.Object)null)
                        component.Init(11, 12, (WhiteNightMapManager.DeadApostleFilterEndEvent)null, (BattleUnitModel)null, (List<BattleUnitModel>)null);
                    AutoDestruct autoDestruct = gameObject.AddComponent<AutoDestruct>();
                    if ((UnityEngine.Object)autoDestruct != (UnityEngine.Object)null)
                    {
                        autoDestruct.time = 2.5f;
                        autoDestruct.DestroyWhenDisable();
                    }
                }
            }
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
            {
                if (alive != this._owner)
                {
                    BattleUnitBuf_Emotion_WhiteNight_Apostle whiteNightApostle = new BattleUnitBuf_Emotion_WhiteNight_Apostle(this._owner);
                    alive.bufListDetail.AddBuf((BattleUnitBuf)whiteNightApostle);
                }
            }
        }
        public class BattleUnitBuf_Emotion_WhiteNight_Apostle : BattleUnitBuf
        {
            private BattleUnitModel _god;
            private int Dmg => RandomUtil.Range(2, 7);
            public override bool Hide => true;
            public BattleUnitBuf_Emotion_WhiteNight_Apostle(BattleUnitModel god) => this._god = god;
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (this._god == null || this._god.IsDead())
                    return;
                if (behavior != null)
                    behavior.ApplyDiceStatBonus(new DiceStatBonus()
                    {
                        dmg = this.Dmg
                    });
                if (EmotionCardAbility_plaguedoctor1.WhiteNightClock.ContainsKey(_god.UnitData) || EmotionCardAbility_plaguedoctor1.WhiteNightClock[_god.UnitData]<12)
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = 1
                });
            }
            public override void OnDieOtherUnit(BattleUnitModel unit)
            {
                base.OnDieOtherUnit(unit);
                if (unit == null || unit != this._god)
                    return;
                this.Destroy();
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }
    }
}
