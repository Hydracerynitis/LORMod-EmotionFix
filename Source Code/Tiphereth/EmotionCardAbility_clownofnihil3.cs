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
        private bool _effect;
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            SoundEffectPlayer.PlaySound("Creature/Nihil_StrongAtk");
            if (_owner.faction != Faction.Player)
                return;
            GiveBuf();
            _effect = false;
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            if (_owner.faction != Faction.Player)
                return;
            GiveBuf();
            _effect = false;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (_owner.faction != Faction.Enemy)
                return;
            if (!_effect)
            {
                SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("5_T/FX_IllusionCard_5_T_Joker_Start", 1f, _owner.view, _owner.view);
                _effect = true;
            }
            List<BattleUnitModel> player = BattleObjectManager.instance.GetAliveList(Faction.Player);
            foreach(BattleUnitModel alive in player)
            {
                alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak, 2, _owner);
                alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Disarm,2, _owner);
                alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Binding, 2, _owner);
                if (alive.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Emotion_Nihil_Debuf) == null)
                    alive.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_Nihil_Debuf());
            }
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak, player.Count, _owner);
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Disarm, player.Count, _owner);
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Binding, player.Count, _owner);       
        }
        private void GiveBuf()
        {
            if (SearchEmotion(_owner, "QueenOfHatred_Snake") == null || SearchEmotion(_owner, "KnightOfDespair_Despair") == null || SearchEmotion(_owner, "Greed_Eat") == null || SearchEmotion(_owner, "Angry_Angry") == null || _owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Emotion_Nihil) != null)
                return;
            _owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_Nihil());
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
            if (_owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Emotion_Nihil) is BattleUnitBuf_Emotion_Nihil nihil)
                nihil.Destroy();
        }
        public class BattleUnitBuf_Emotion_Nihil : BattleUnitBuf
        {
            private bool _effect=false;
            public override string keywordId => "Nihil_Nihil";
            public override string keywordIconId => "Fusion";
            public BattleUnitBuf_Emotion_Nihil() => stack = 0;
            public override void OnRoundEndTheLast()
            {
                base.OnRoundEndTheLast();
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
                {
                    if (alive != this._owner)
                    {
                        alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Weak, 5, _owner);
                        alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Disarm, 5, _owner);
                        alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Binding, 5, _owner);
                        if(alive.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Emotion_Nihil_Debuf)==null)
                            alive.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_Nihil_Debuf());
                    }
                }
            }
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (_effect)
                    return;
                SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("5_T/FX_IllusionCard_5_T_Joker_Start", 1f, _owner.view, _owner.view);
                _effect = true;
            }
        }

        public class BattleUnitBuf_Emotion_Nihil_Debuf : BattleUnitBuf
        {
            private GameObject aura;
            public override bool Hide => true;
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (aura !=null)
                    return;
                aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("5_T/FX_IllusionCard_5_T_Joker_Aura", 1f, _owner.view, _owner.view)?.gameObject;
            }
            public override void OnRoundEndTheLast()
            {
                base.OnRoundEnd();
                DestroyAura();
            }
            public override void OnDie()
            {
                base.OnDie();
                Destroy();
            }
            public override void Destroy()
            {
                base.Destroy();
                this.DestroyAura();
            }
            private void DestroyAura()
            {
                if (aura == null)
                    return;
                UnityEngine.Object.Destroy(aura);
                aura = null;
            }
        }
    }
}
