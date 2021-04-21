﻿using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using HarmonyLib;
using System.Reflection;
using System.Collections.Generic;
using System.IO;

namespace EmotionalFix
{
    public class EmotionCardAbility_bossbird8 : EmotionCardAbilityBase
    {
        private BattleUnitModel _target;
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            _target = null;
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || this.GetAliveCreature() != null)
                return;
            this._target = target;
            target.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_BossBird_Creature());
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (this._target != null && !this._target.IsDead())
            {
                this._target.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_BossBird_Creature());
                return;
            }
            this._target =null;
        }
        private BattleUnitModel GetAliveCreature() => BattleObjectManager.instance.GetAliveList(Faction.Player).Find((Predicate<BattleUnitModel>)(x => x.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(y => y is BattleUnitBuf_Emotion_BossBird_Creature)) != null)) ?? this._target;
        public class BattleUnitBuf_Emotion_BossBird_Creature : BattleUnitBuf
        {
            private int ReduceDmg => RandomUtil.Range(3, 5);
            private int ReduceBreakDmg => RandomUtil.Range(3, 5);
            private int AddDmg => RandomUtil.Range(3, 5);
            protected override string keywordIconId => "ApocalypseBird_Apocalypse";
            protected override string keywordId => "ApocalypseBird_Peace";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                this.stack = 0;
            }
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    dmg = -this.ReduceDmg,
                    breakDmg = -this.ReduceBreakDmg
                });
            }
            public override int GetBreakDamageReduction(BehaviourDetail behaviourDetail)
            {
                return -AddDmg;
            }
            public override int GetDamageReduction(BehaviourDetail behaviourDetail)
            {
                return -AddDmg;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }
    }
}
