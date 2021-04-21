using System;
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
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (this._owner.faction == Faction.Enemy && count > 0)
                return;
            int n = this._owner.faction==Faction.Player? 4:2;
            this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, n, this._owner);
            this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, n, this._owner);
            this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, n, this._owner);
            this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, n, this._owner);
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            if (this._owner.faction == Faction.Player)
            {
                ++this.count;
                if (this.count < 3)
                    return;
                this._owner.Die();
            }
            if (this._owner.faction == Faction.Enemy)
            {
                if (this.count > 0)
                    return;
                this._owner.bufListDetail.AddBuf(new Exhaust());
                count += 1;
            }
        }
        public override AtkResist GetResistBP(AtkResist origin, BehaviourDetail detail)
        {
            if (this._owner.faction == Faction.Player && this.count < 3)
                return AtkResist.Immune;
            return base.GetResistBP(origin, detail);
        }
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            this.count = 0;
            this.MakeEffect("0/HeartofAspiration_Rush", destroyTime: 2f);
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Heart_Fast")?.SetGlobalPosition(this._owner.view.WorldPosition);
        }
        public class Exhaust: BattleUnitBuf
        {
            protected override string keywordIconId => "Stun";
            protected override string keywordId => "HeartExhaust";
            private int count;
            public override int SpeedDiceBreakedAdder() => 100;
            public override int GetDamageReduction(BehaviourDetail behaviourDetail) => -2;
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
                this.Destroy();
            }
        }
    }
}
