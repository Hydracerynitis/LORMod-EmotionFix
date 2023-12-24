using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;
using BaseMod;

namespace EmotionalFix
{
    public class EmotionCardAbility_clownofnihil1 : EmotionCardAbilityBase
    {
        private List<BattleDiceCardModel> AddedCard = new List<BattleDiceCardModel>();
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            if (_owner.faction == Faction.Player)
            {
                GiveCard();
            }
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            if (_owner.faction == Faction.Player)
            {
                GiveCard();
            }
        }
        private void GiveCard()
        {
            if (SearchEmotion(_owner, "QueenOfHatred_Laser") == null || SearchEmotion(_owner, "KnightOfDespair_Gaho") == null || SearchEmotion(_owner, "Greed_Protect") == null || SearchEmotion(_owner, "Angry_Poison") == null)
                return;
            AddedCard.Add(_owner.allyCardDetail.AddNewCardToDeck(1104501));
            AddedCard.Add(_owner.allyCardDetail.AddNewCardToDeck(1104502));
            AddedCard.Add(_owner.allyCardDetail.AddNewCardToDeck(1104503));
            AddedCard.Add(_owner.allyCardDetail.AddNewCardToDeck(1104504));
            _owner.allyCardDetail.Shuffle();
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
        public override void OnLoseParrying(BattleDiceBehavior behavior)
        {
            base.OnLoseParrying(behavior);
            if (_owner.faction != Faction.Enemy)
                return;
            if (_owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Emotion_Void) != null)
                return;
            if (!(_owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Emotion_Void_Ready) is BattleUnitBuf_Emotion_Void_Ready emotionVoidReady))
            {
                emotionVoidReady = new BattleUnitBuf_Emotion_Void_Ready();
                _owner.bufListDetail.AddBuf(emotionVoidReady);
            }
            emotionVoidReady.Add();
        }
        public class BattleUnitBuf_Emotion_Void_Ready : BattleUnitBuf
        {
            public static int StackMax = 20;

            public override string keywordIconId => "CardBuf_NihilClown_Card";

            public override string keywordId => "Buf_NihilClown_Card";

            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
            public override void OnRoundEndTheLast()
            {
                base.OnRoundEndTheLast();
                if (stack < StackMax)
                    return;
                _owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_Void());
                Destroy();
            }
            public void Add()
            {
                ++stack;
                if (stack <= StackMax)
                    return;
                stack = StackMax;
            }
        }
        public class BattleUnitBuf_Emotion_Void : BattleUnitBuf
        {
            private GameObject aura;
            private int cnt;

            public override string keywordIconId => "CardBuf_NihilClown_Card";

            public override string keywordId => "NihilClown_Card";

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                cnt = 0;
                _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, 5, _owner);
            }
            public override bool IsImmune(BattleUnitBuf buf) => buf.positiveType == BufPositiveType.Negative || base.IsImmune(buf);
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
                owner.bufListDetail.RemoveBufAll(BufPositiveType.Negative);
                aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("5_T/FX_IllusionCard_5_T_MagicGirl", 1f, owner.view, owner.view)?.gameObject;
                SoundEffectPlayer.PlaySound("Creature/Nihil_Filter");
            }
            public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
            {
                base.OnTakeDamageByAttack(atkDice, dmg);
                BattleUnitModel owner = atkDice?.owner;
                if (owner == null || cnt >= 2)
                    return;
                ++cnt;
                owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Decay, 1, _owner);
                owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Vulnerable, 1, _owner);
            }
            public override void OnDie()
            {
                base.OnDie();
                Destroy();
            }
            public override void Destroy()
            {
                base.Destroy();
                DestroyAura();
            }
            private void DestroyAura()
            {
                if (!(aura != null))
                    return;
                UnityEngine.Object.Destroy(aura);
                aura = (GameObject)null;
            }
        }
    }
}
