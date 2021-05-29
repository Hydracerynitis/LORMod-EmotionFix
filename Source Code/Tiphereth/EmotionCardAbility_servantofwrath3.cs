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
            _effect = false;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Player))
                alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Decay, 3, this._owner);
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (!this._effect)
            {
                this._effect = true;
                Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/5/Servant_Emotion_Effect");
                if ((UnityEngine.Object)original != (UnityEngine.Object)null)
                {
                    Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                    if ((UnityEngine.Object)creatureEffect?.gameObject.GetComponent<AutoDestruct>() == (UnityEngine.Object)null)
                    {
                        AutoDestruct autoDestruct = creatureEffect?.gameObject.AddComponent<AutoDestruct>();
                        if ((UnityEngine.Object)autoDestruct != (UnityEngine.Object)null)
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
