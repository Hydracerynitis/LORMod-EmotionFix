using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;

namespace EmotionalFix
{
    public class EmotionCardAbility_bossbird6 : EmotionCardAbilityBase
    {
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            List<BattleUnitModel> ally = BattleObjectManager.instance.GetList(this._owner.faction);
            int num = ally.Count;
            int ready = 0;
            if (this._owner.faction == Faction.Player)
            {
                foreach (BattleUnitModel battleUnitModel in ally)
                {
                    if (battleUnitModel.emotionDetail.PassiveList.Count > 0)
                        ++ready;
                }
                foreach (BattleUnitModel alive in ally)
                {
                    alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, 2, this._owner);
                    if (ready >= num)
                    {
                        alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 2, this._owner);
                        alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, 2, this._owner);
                    }
                }
            }
            else
            {
                foreach (BattleUnitModel battleUnitModel in ally)
                {
                    if (battleUnitModel.emotionDetail.EmotionLevel >= 5)
                        ++ready;
                }
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, 2, this._owner);
                if (ready >= num)
                {
                    this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 2, this._owner);
                    this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, 2, this._owner);
                }
            }
        }
    }
}
