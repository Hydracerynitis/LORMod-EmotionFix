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
            if (this._owner.faction == Faction.Player)
            {
                this.GiveCard();
            }
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            if (this._owner.faction == Faction.Player)
            {
                this.GiveCard();
            }
        }
        private void GiveCard()
        {
            if (SearchEmotion(this._owner, "QueenOfHatred_Laser") == null || SearchEmotion(this._owner, "KnightOfDespair_Gaho") == null || SearchEmotion(this._owner, "Greed_Protect") == null || SearchEmotion(this._owner, "Angry_Poison") == null)
                return;
            AddedCard.Add(this._owner.allyCardDetail.AddNewCardToDeck(1104501));
            AddedCard.Add(this._owner.allyCardDetail.AddNewCardToDeck(1104502));
            AddedCard.Add(this._owner.allyCardDetail.AddNewCardToDeck(1104503));
            AddedCard.Add(this._owner.allyCardDetail.AddNewCardToDeck(1104504));
            this._owner.allyCardDetail.Shuffle();
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
            if (this._owner.faction != Faction.Enemy)
                return;
            if (this._owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Emotion_Void) != null)
                return;
            if (!(this._owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Emotion_Void_Ready) is BattleUnitBuf_Emotion_Void_Ready emotionVoidReady))
            {
                emotionVoidReady = new BattleUnitBuf_Emotion_Void_Ready();
                this._owner.bufListDetail.AddBuf(emotionVoidReady);
            }
            emotionVoidReady.Add();
        }
        public class BattleUnitBuf_Emotion_Void_Ready : BattleUnitBuf
        {
            public static int StackMax = 20;

            protected override string keywordIconId => "CardBuf_NihilClown_Card";

            protected override string keywordId => "Buf_NihilClown_Card";

            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                this.stack = 0;
            }
            public override void OnRoundEndTheLast()
            {
                base.OnRoundEndTheLast();
                if (this.stack < StackMax)
                    return;
                this._owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_Void());
                this.Destroy();
            }
            public void Add()
            {
                ++this.stack;
                if (this.stack <= StackMax)
                    return;
                this.stack = StackMax;
            }
        }
        public class BattleUnitBuf_Emotion_Void : BattleUnitBuf
        {
            private GameObject aura;
            private int cnt;

            protected override string keywordIconId => "CardBuf_NihilClown_Card";

            protected override string keywordId => "NihilClown_Card";

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                this.cnt = 0;
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, 5, this._owner);
            }
            public override bool IsImmune(BattleUnitBuf buf) => buf.positiveType == BufPositiveType.Negative || base.IsImmune(buf);
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                this.stack = 0;
                owner.bufListDetail.RemoveBufAll(BufPositiveType.Negative);
                this.aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("5_T/FX_IllusionCard_5_T_MagicGirl", 1f, owner.view, owner.view)?.gameObject;
                SoundEffectPlayer.PlaySound("Creature/Nihil_Filter");
            }
            public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
            {
                base.OnTakeDamageByAttack(atkDice, dmg);
                BattleUnitModel owner = atkDice?.owner;
                if (owner == null || this.cnt >= 2)
                    return;
                ++this.cnt;
                owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Decay, 1, this._owner);
                owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Vulnerable, 1, this._owner);
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
