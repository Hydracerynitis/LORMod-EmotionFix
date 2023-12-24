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
            if (!_effect)
            {
                _effect = true;
                Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/8_B/FX_IllusionCard_8_B_Guardian");
                if (original != null)
                {
                    Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                    if (creatureEffect?.gameObject.GetComponent<AutoDestruct>() == null)
                    {
                        AutoDestruct autoDestruct = creatureEffect?.gameObject.AddComponent<AutoDestruct>();
                        if (autoDestruct != null)
                        {
                            autoDestruct.time = 5f;
                            autoDestruct.DestroyWhenDisable();
                        }
                    }
                }
                SoundEffectPlayer.PlaySound("Creature/Bossbird_ForestKeeper");
            }
            List<BattleUnitModel> ally = BattleObjectManager.instance.GetList(_owner.faction);
            int num = ally.Count;
            int ready = 0;
            if (_owner.faction == Faction.Player)
            {
                foreach (BattleUnitModel battleUnitModel in ally)
                {
                    if (battleUnitModel.emotionDetail.PassiveList.Count > 0)
                        ++ready;
                }
                foreach (BattleUnitModel alive in ally)
                {
                    alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, 2, _owner);
                    if (ready >= num)
                    {
                        alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 2, _owner);
                        alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, 2, _owner);
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
                _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, 2, _owner);
                if (ready >= num)
                {
                    _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 2, _owner);
                    _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, 2, _owner);
                }
            }
        }
    }
}
