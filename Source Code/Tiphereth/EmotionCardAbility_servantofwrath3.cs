using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_servantofwrath3 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            if (this._owner.faction == Faction.Enemy)
            {
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Player))
                    alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Decay, 3, this._owner);
            }
            if (this._owner.faction == Faction.Player)
            {
                foreach(BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
                {
                    if(alive.bufListDetail.GetActivatedBufList().Exists((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_servantofwrath2.BattleUnitBuf_Emotion_Wrath_Friend)))
                        alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Decay, 3, this._owner);
                    alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Decay, 3, this._owner);
                }
            }

        }
        public override void OnWaveStart()
        {
            if (this._owner.faction != Faction.Enemy)
                return;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Player))
                alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Decay, 3, this._owner);
        }
    }
}
