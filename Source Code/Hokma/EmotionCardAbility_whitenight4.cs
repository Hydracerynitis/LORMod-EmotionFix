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
            if (!_effect)
            {
                _effect = true;
                GameObject gameObject = Util.LoadPrefab("Battle/CreatureEffect/FinalBattle/WhiteNight_DeadApostleImageFilter");
                if (gameObject != null)
                {
                    WhiteNightApostleDeadFilter component = gameObject?.GetComponent<WhiteNightApostleDeadFilter>();
                    if (component != null)
                        component.Init(11, 12, (WhiteNightMapManager.DeadApostleFilterEndEvent)null, (BattleUnitModel)null, (List<BattleUnitModel>)null);
                    AutoDestruct autoDestruct = gameObject.AddComponent<AutoDestruct>();
                    if (autoDestruct != null)
                    {
                        autoDestruct.time = 2.5f;
                        autoDestruct.DestroyWhenDisable();
                    }
                }
            }
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(_owner.faction))
            {
                if (alive != _owner)
                {
                    BattleUnitBuf_Emotion_WhiteNight_Apostle whiteNightApostle = new BattleUnitBuf_Emotion_WhiteNight_Apostle(_owner);
                    alive.bufListDetail.AddBuf((BattleUnitBuf)whiteNightApostle);
                }
            }
        }
        public class BattleUnitBuf_Emotion_WhiteNight_Apostle : BattleUnitBuf
        {
            private BattleUnitModel _god;
            private int Dmg => RandomUtil.Range(2, 7);
            public override bool Hide => true;
            public BattleUnitBuf_Emotion_WhiteNight_Apostle(BattleUnitModel god) => _god = god;
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (_god == null || _god.IsDead())
                    return;
                if (behavior != null)
                    behavior.ApplyDiceStatBonus(new DiceStatBonus()
                    {
                        dmg = Dmg
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
                if (unit == null || unit != _god)
                    return;
                Destroy();
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
    }
}
