using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_latitia1 : EmotionCardAbilityBase
    {
        private BattleUnitModel _target;
        public override void OnParryingStart(BattlePlayingCardDataInUnitModel card)
        {
            base.OnParryingStart(card);
            BattleUnitModel target = card?.target;
            if (target == null || this._target != null)
                return;
            BattleUnitBuf_Emotion_Latitia_Gift emotionLatitiaGift = new BattleUnitBuf_Emotion_Latitia_Gift(this._owner);
            target.bufListDetail.AddBuf(emotionLatitiaGift);
            this._target = target;
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this._target = null;
        }
        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            base.OnDieOtherUnit(unit);
            if (unit != this._target)
                return;
            this._target = null;
        }
        public override void OnDie(BattleUnitModel killer)
        {
            base.OnDie(killer);
            Destroy();
        }
        public void Destroy()
        {
            if (_target == null)
                return;
            if (_target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_Latitia_Gift)) is BattleUnitBuf_Emotion_Latitia_Gift Gift)
                Gift.Destroy();
        }
        public class BattleUnitBuf_Emotion_Latitia_Gift : BattleUnitBuf
        {
            private BattleUnitModel _giver;
            private static bool Prob => (double)RandomUtil.valueForProb <= 0.5;

            private static int Dmg => RandomUtil.Range(3, 8);

            private static int Bleed => RandomUtil.Range(2, 2);

            public override string keywordId => "Latitia_Gift";
            public override string keywordIconId => "Latitia_Heart";
            public BattleUnitBuf_Emotion_Latitia_Gift(BattleUnitModel giver) => this._giver = giver;
            public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
            {
                base.OnStartParrying(card);
                BattleUnitModel target = card?.target;
                if (target == null || this._giver == null || (this._giver == target || !Prob))
                    return;
                this._owner.battleCardResultLog?.SetCreatureAbilityEffect("3/Latitia_Boom", 1.5f);
                this._owner.TakeDamage(Dmg, DamageType.Buf,this._giver);
                this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, Bleed, this._giver);
            }
        }
    }
}
