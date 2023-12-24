using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_servantofwrath3 : EmotionCardAbilityBase
    {
        private bool _effect;
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            _effect = false;
            if (_owner.faction == Faction.Enemy)
            {
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Player))
                    alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Decay, 3, _owner);
            }
            if (_owner.faction == Faction.Player)
            {
                foreach(BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
                {
                    if(alive.bufListDetail.GetActivatedBufList().Exists((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_servantofwrath2.BattleUnitBuf_Emotion_Wrath_Friend)))
                        alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Decay, 3, _owner);
                    alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Decay, 3, _owner);
                }
            }
        }
        public override void OnWaveStart()
        {
            if (_owner.faction != Faction.Enemy)
                return;
            _effect = false;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Player))
                alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Decay, 3, _owner);
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (!_effect)
            {
                _effect = true;
                Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/5/Servant_Emotion_Effect");
                if (original != null)
                {
                    Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                    if (creatureEffect?.gameObject.GetComponent<AutoDestruct>() == null)
                    {
                        AutoDestruct autoDestruct = creatureEffect?.gameObject.AddComponent<AutoDestruct>();
                        if (autoDestruct != null)
                        {
                            autoDestruct.time = 3f;
                            autoDestruct.DestroyWhenDisable();
                        }
                    }
                }
                SoundEffectPlayer.PlaySound("Creature/Angry_StrongFinish");
            }
        }
    }
}
