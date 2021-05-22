using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_clownofnihil3 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            if (this._owner.faction != Faction.Player)
                return;
            this.GiveBuf();
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            if (this._owner.faction != Faction.Player)
                return;
            this.GiveBuf();
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (this._owner.faction != Faction.Enemy)
                return;
            SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("5_T/FX_IllusionCard_5_T_Joker_Start", 1f, this._owner.view, this._owner.view);
            List<BattleUnitModel> player = BattleObjectManager.instance.GetAliveList(Faction.Player);
            foreach(BattleUnitModel alive in player)
            {
                alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak, 2, this._owner);
                alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Disarm,2, this._owner);
                alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Binding, 2, this._owner);
                alive.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_Nihil_Debuf());
            }
            this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak, player.Count, this._owner);
            this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Disarm, player.Count, this._owner);
            this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Binding, player.Count, this._owner);
            this._owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_Nihil_Debuf());         
        }
        private void GiveBuf()
        {
            if (SearchEmotion(this._owner, "QueenOfHatred_Snake") == null || SearchEmotion(this._owner, "KnightOfDespair_Despair") == null || SearchEmotion(this._owner, "Greed_Eat") == null || SearchEmotion(this._owner, "Angry_Angry") == null || this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_Nihil)) != null)
                return;
            this._owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_Nihil());
        }
        private BattleEmotionCardModel SearchEmotion(BattleUnitModel owner, string Name)
        {
            List<BattleEmotionCardModel> emotion = owner.emotionDetail.PassiveList;
            foreach (BattleEmotionCardModel card in emotion)
            {
                if (card.XmlInfo.Name == Name)
                    return card;
            }
            return null;
        }
        public void Destroy()
        {
            if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_Nihil)) is BattleUnitBuf_Emotion_Nihil nihil)
                nihil.Destroy();
        }
        public class BattleUnitBuf_Emotion_Nihil : BattleUnitBuf
        {
            protected override string keywordId => "Nihil_Nihil";
            protected override string keywordIconId => "Fusion";
            public BattleUnitBuf_Emotion_Nihil() => this.stack = 0;
            public override void OnRoundEndTheLast()
            {
                base.OnRoundEndTheLast();
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
                {
                    if (alive != this._owner)
                    {
                        alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Weak, 5, this._owner);
                        alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Disarm, 5, this._owner);
                        alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Binding, 5, this._owner);
                        alive.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_Nihil_Debuf());
                    }
                }
            }
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("5_T/FX_IllusionCard_5_T_Joker_Start", 1f, this._owner.view, this._owner.view);
            }
        }

        public class BattleUnitBuf_Emotion_Nihil_Debuf : BattleUnitBuf
        {
            private GameObject aura;
            public override bool Hide => true;
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (!((UnityEngine.Object)this.aura == (UnityEngine.Object)null))
                    return;
                this.aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("5_T/FX_IllusionCard_5_T_Joker_Aura", 1f, this._owner.view, this._owner.view)?.gameObject;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.DestroyAura();
            }
            public override void OnDie()
            {
                base.OnDie();
                this.Destroy();
            }
            public override void Destroy()
            {
                base.Destroy();
                this.DestroyAura();
            }
            private void DestroyAura()
            {
                if (!((UnityEngine.Object)this.aura != (UnityEngine.Object)null))
                    return;
                UnityEngine.Object.Destroy((UnityEngine.Object)this.aura);
                this.aura = (GameObject)null;
            }
        }
    }
}
