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
        private bool _effect;
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (_effect)
                return;
            _effect = true;
            Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/0_K/FX_IllusionCard_0_K_Blizzard");
            if (!(original != null))
                return;
            Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
            if (!(creatureEffect?.gameObject.GetComponent<AutoDestruct>() == null))
                return;
            AutoDestruct autoDestruct = creatureEffect?.gameObject.AddComponent<AutoDestruct>();
            if (!(autoDestruct != null))
                return;
            autoDestruct.time = 3f;
            autoDestruct.DestroyWhenDisable();
            SoundEffectPlayer.PlaySound("Creature/SnowQueen_StrongAtk2");
        }
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            _effect = false;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
            {
                if (alive != _owner)
                    alive.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_SnowQueen_Stun(_owner));
            }
        }

        public class BattleUnitBuf_Emotion_SnowQueen_Stun : BattleUnitBuf
        {
            private BattleUnitModel _attacker;
            private Battle.CreatureEffect.CreatureEffect _aura;
            private static int Bind => RandomUtil.Range(6, 6);
            public override string keywordId => "SnowQueen_Emotion_Stun";
            public override string keywordIconId => "SnowQueen_Stun";
            public BattleUnitBuf_Emotion_SnowQueen_Stun(BattleUnitModel attacker) => _attacker = attacker;
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Stun, 1);
            }
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (_owner.bufListDetail.GetActivatedBuf(KeywordBuf.Stun) == null || _owner.IsImmune(KeywordBuf.Stun) || _owner.bufListDetail.IsImmune(BufPositiveType.Negative))
                    return;
                _owner.view.charAppearance.ChangeMotion(ActionDetail.Damaged);
                _aura = SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("0/SnowQueen_Emotion_Frozen", 1f, _owner.view, _owner.view);
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                if (_owner.faction ==Faction.Enemy)
                    _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Binding, Bind, _attacker);
                if (_aura != null)
                {
                    UnityEngine.Object.Destroy(_aura.gameObject);
                    _aura = (Battle.CreatureEffect.CreatureEffect)null;
                    _owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
                }
                Destroy();
            }
        }
    }
}
