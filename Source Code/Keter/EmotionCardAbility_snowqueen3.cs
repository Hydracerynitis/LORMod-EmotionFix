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
            if (this._effect)
                return;
            this._effect = true;
            Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/0_K/FX_IllusionCard_0_K_Blizzard");
            if (!((UnityEngine.Object)original != (UnityEngine.Object)null))
                return;
            Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
            if (!((UnityEngine.Object)creatureEffect?.gameObject.GetComponent<AutoDestruct>() == (UnityEngine.Object)null))
                return;
            AutoDestruct autoDestruct = creatureEffect?.gameObject.AddComponent<AutoDestruct>();
            if (!((UnityEngine.Object)autoDestruct != (UnityEngine.Object)null))
                return;
            autoDestruct.time = 3f;
            autoDestruct.DestroyWhenDisable();
            SoundEffectPlayer.PlaySound("Creature/SnowQueen_StrongAtk2");
        }
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            this._effect = false;
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
            private Battle.CreatureEffect.CreatureEffect _aura;
            private static int Bind => RandomUtil.Range(6, 6);
            protected override string keywordId => "SnowQueen_Emotion_Stun";
            protected override string keywordIconId => "SnowQueen_Stun";
            public BattleUnitBuf_Emotion_SnowQueen_Stun(BattleUnitModel attacker) => this._attacker = attacker;
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Stun, 1);
            }
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (this._owner.bufListDetail.GetActivatedBuf(KeywordBuf.Stun) == null || this._owner.IsImmune(KeywordBuf.Stun) || this._owner.bufListDetail.IsImmune(BufPositiveType.Negative))
                    return;
                this._owner.view.charAppearance.ChangeMotion(ActionDetail.Damaged);
                this._aura = SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("0/SnowQueen_Emotion_Frozen", 1f, this._owner.view, this._owner.view);
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                if (this._owner.faction != this._attacker.faction)
                    this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Binding, Bind, this._attacker);
                if ((UnityEngine.Object)this._aura != (UnityEngine.Object)null)
                {
                    UnityEngine.Object.Destroy((UnityEngine.Object)this._aura.gameObject);
                    this._aura = (Battle.CreatureEffect.CreatureEffect)null;
                    this._owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
                }
                this.Destroy();
            }
        }
    }
}
