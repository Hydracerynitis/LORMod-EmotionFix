using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_freischutz3 : EmotionCardAbilityBase
    {
        private string _PREFAB_PATH = "Battle/DiceAttackEffects/CreatureBattle/EGO_Freischutz_6thBullet";
        private bool _effect;
        private bool trigger;

        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            trigger = false;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
                if(alive.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Emotion_Fruischutz_Flame)==null)
                    alive.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_Fruischutz_Flame());
        }

        public override void OnWaveStart()
        {
            base.OnWaveStart();
            if (trigger && this._owner.faction == Faction.Enemy)
                return;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
                if (alive.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Emotion_Fruischutz_Flame) == null)
                    alive.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_Fruischutz_Flame());
        }
        public override void OnEndBattlePhase()
        {
            base.OnEndBattlePhase();
            trigger = true;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (trigger && this._owner.faction == Faction.Enemy)
                return;
            if (!this._effect)
            {
                this._effect = true;
                Util.LoadPrefab(this._PREFAB_PATH).GetComponent<FarAreaEffect_EGO_Freischutz_6thBullet>().Init(this._owner);
                List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList_opponent(_owner.faction);
                if (aliveList.Count > 0)
                    Util.LoadPrefab(this._PREFAB_PATH).GetComponent<FarAreaEffect_EGO_Freischutz_6thBullet>().Init(aliveList[0]);
            }
            if (this._owner.faction == Faction.Player)
            {
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, RandomUtil.Range(1, 2), this._owner);
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, RandomUtil.Range(2, 4), this._owner);
            }
            else
            {
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, RandomUtil.Range(2, 4), this._owner);
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, RandomUtil.Range(1, 2), this._owner);
            }
        }

        public class BattleUnitBuf_Emotion_Fruischutz_Flame : BattleUnitBuf
        {
            protected override string keywordId => "Matan_Flame";

            public override AtkResist GetResistHP(AtkResist origin, BehaviourDetail detail)
            {
                if (base.GetResistHP(origin, detail) == AtkResist.Endure)
                    return AtkResist.Vulnerable;
                return base.GetResistHP(origin, detail) == AtkResist.Resist ? AtkResist.Weak : base.GetResistHP(origin, detail);
            }

            public override AtkResist GetResistBP(AtkResist origin, BehaviourDetail detail)
            {
                if (base.GetResistBP(origin, detail) == AtkResist.Endure)
                    return AtkResist.Vulnerable;
                return base.GetResistBP(origin, detail) == AtkResist.Resist ? AtkResist.Weak : base.GetResistBP(origin, detail);
            }
        }
    }
}
