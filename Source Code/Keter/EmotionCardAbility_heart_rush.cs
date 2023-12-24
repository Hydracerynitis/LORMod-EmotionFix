﻿using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using HarmonyLib;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_heart_rush : EmotionCardAbilityBase
    {
        private int count;

        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            count = 0;
            SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("0_K/FX_IllusionCard_0_K_FastBeat", 1f, _owner.view, _owner.view);
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Heart_Fast")?.SetGlobalPosition(_owner.view.WorldPosition);
        }

        public override void OnWaveStart()
        {
            base.OnWaveStart();
            SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("0_K/FX_IllusionCard_0_K_FastBeat", 1f, _owner.view, _owner.view);
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Heart_Fast")?.SetGlobalPosition(_owner.view.WorldPosition);
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (_owner.faction == Faction.Enemy && count > 0)
                return;
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 4, _owner);
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, 4, _owner);
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, 4, _owner);
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, 4, _owner);
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            if (_owner.faction == Faction.Player)
            {
                ++count;
                if (count < 3)
                    return;
                _owner.Die();
            }
            if (_owner.faction == Faction.Enemy)
            {
                if (count > 0)
                    return;
                _owner.bufListDetail.AddBuf(new Exhaust());
                count += 1;
            }
        }
        public override void OnKill(BattleUnitModel target)
        {
            if (target.faction != Faction.Enemy)
                return;
            Singleton<StageController>.Instance.GetStageModel().AddHeartKillCount();
        }
        public override AtkResist GetResistBP(AtkResist origin, BehaviourDetail detail)
        {
            if (_owner.faction == Faction.Player && count < 3)
                return AtkResist.Immune;
            return base.GetResistBP(origin, detail);
        }
        public class Exhaust: BattleUnitBuf
        {
            public override string keywordIconId => "Stun";
            public override string keywordId => "HeartExhaust";
            private int count;
            public override int SpeedDiceBreakedAdder() => 100;
            public override int GetDamageReduction(BattleDiceBehavior behavior) => -2;
            public override int GetBreakDamageReduction(BehaviourDetail behaviourDetail) => -2;
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                count = 0;
                stack = 0;
            }
            public override void OnRoundEndTheLast()
            {
                base.OnRoundEndTheLast();
                count++;
                if (count < 2)
                    return;
                Destroy();
            }
        }
    }
}
