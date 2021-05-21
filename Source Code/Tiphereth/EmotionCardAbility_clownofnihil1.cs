using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_clownofnihil1 : EmotionCardAbilityBase
    {
        private bool used1;
        private bool used2;
        private bool used3;
        private bool used4;
        private List<BattleDiceCardModel> AddedCard = new List<BattleDiceCardModel>();
        private static int Str => RandomUtil.Range(1, 2);
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            if (this._owner.faction == Faction.Player)
            {
                this.GiveCard();
            }
            if (this._owner.faction == Faction.Enemy)
            {
                used1 = true;
                used2 = true;
                used3 = true;
                used4 = true;
                this.GiveSpecialCardInDeck();
            }
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            if (this._owner.faction == Faction.Player)
            {
                this.GiveCard();
            }
            if (this._owner.faction == Faction.Enemy)
            {
                used1 = true;
                used2 = true;
                used3 = true;
                used4 = true;
                this.GiveSpecialCardInDeck();
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
        public override bool IsImmune(KeywordBuf buf)
        {
            if (this.CheckCondition())
            {
                switch (buf)
                {
                    case KeywordBuf.Weak:
                    case KeywordBuf.Disarm:
                    case KeywordBuf.Binding:
                        return true;
                }
            }
            return base.IsImmune(buf);
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (this._owner.faction != Faction.Enemy)
                return;
            this.GiveSpecialCardInDeck();
            if (!this.CheckCondition())
                return;
            this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, Str, this._owner);
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            if (curCard != null)
            {
                int? id = curCard.card?.GetID();
                int num1 = 1100007;
                int num2 = 1100008;
                int num3 = 1100009;
                int num4 = 1100010;
                if (id.GetValueOrDefault() == num1 & id.HasValue)
                {
                    this.used1 = true;
                    return;
                }
                if (id.GetValueOrDefault() == num2 & id.HasValue)
                {
                    this.used2 = true;
                    return;
                }
                if (id.GetValueOrDefault() == num3 & id.HasValue)
                {
                    this.used3 = true;
                    return;
                }
                if (id.GetValueOrDefault() == num4 & id.HasValue)
                {
                    this.used4 = true;
                    return;
                }
            }
        }
        private void GiveSpecialCardInDeck()
        {
            if (this.used1)
            {
                BattleDiceCardModel Magician =this._owner.allyCardDetail.AddNewCardToDeck(1100007);
                Magician.exhaust = true;
                AddedCard.Add(Magician);
                used1 = false;
            }
            if (this.used2)
            {
                BattleDiceCardModel Magician = this._owner.allyCardDetail.AddNewCardToDeck(1100008);
                Magician.exhaust = true;
                AddedCard.Add(Magician);
                used2 = false;
            }
            if (this.used3)
            {
                BattleDiceCardModel Magician = this._owner.allyCardDetail.AddNewCardToDeck(1100009);
                Magician.exhaust = true;
                AddedCard.Add(Magician);
                used3 = false;
            }
            if (this.used4)
            {
                BattleDiceCardModel Magician = this._owner.allyCardDetail.AddNewCardToDeck(1100010);
                Magician.exhaust = true;
                AddedCard.Add(Magician);
                used4 = false;
            }
        }
        private bool CheckCondition() => this._owner.allyCardDetail.GetHand().Find((Predicate<BattleDiceCardModel>)(x => x.GetID() == 1100007)) != null && this._owner.allyCardDetail.GetHand().Find((Predicate<BattleDiceCardModel>)(x => x.GetID() == 1100008)) != null && (this._owner.allyCardDetail.GetHand().Find((Predicate<BattleDiceCardModel>)(x => x.GetID() == 1100009)) != null && this._owner.allyCardDetail.GetHand().Find((Predicate<BattleDiceCardModel>)(x => x.GetID() == 1100010)) != null);
        public void Destroy()
        {
            foreach(BattleDiceCardModel card in AddedCard)
            {
                this._owner.allyCardDetail.ExhaustACardAnywhere(card);
            }
        }
    }
}
