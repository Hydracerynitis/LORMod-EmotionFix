using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_snowqueen3 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
            {
                if (alive != this._owner)
                {
                    alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Stun, 1);
                    if (this._owner.faction == Faction.Player)
                        alive.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_SnowQueen_Stun(this._owner));
                }
            }
        }

        public class BattleUnitBuf_Emotion_SnowQueen_Stun : BattleUnitBuf
        {
            private BattleUnitModel _attacker;
            private static int Bind => RandomUtil.Range(6, 6);
            protected override string keywordId => "SnowQueen_Emotion_Stun";
            protected override string keywordIconId => "SnowQueen_Stun";
            public BattleUnitBuf_Emotion_SnowQueen_Stun(BattleUnitModel attacker) => this._attacker = attacker;
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                if (this._owner.faction != this._attacker.faction)
                    this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Binding, Bind, this._attacker);
                this.Destroy();
            }
        }
    }
}
