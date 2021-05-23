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
                Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/8_B/FX_IllusionCard_8_B_Guardian");
                if ((UnityEngine.Object)original != (UnityEngine.Object)null)
                {
                    Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                    if ((UnityEngine.Object)creatureEffect?.gameObject.GetComponent<AutoDestruct>() == (UnityEngine.Object)null)
                    {
                        AutoDestruct autoDestruct = creatureEffect?.gameObject.AddComponent<AutoDestruct>();
                        if ((UnityEngine.Object)autoDestruct != (UnityEngine.Object)null)
                        {
                            autoDestruct.time = 5f;
                            autoDestruct.DestroyWhenDisable();
                        }
                    }
                }
            }
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
