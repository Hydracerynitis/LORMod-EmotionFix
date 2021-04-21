using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_latitia2 : EmotionCardAbilityBase
    {
        private static int Pow => RandomUtil.Range(2, 4);

        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (this._owner.allyCardDetail.GetHand().Count <= 0 || this._owner.allyCardDetail.GetHand().FindAll((Predicate<BattleDiceCardModel>)(x => x.GetBufList().Exists((Predicate<BattleDiceCardBuf>)(y => y is BattleDiceCardBuf_Emotion_Heart)))).Count!=0)
                return;
            BattleDiceCardModel randomCardInHand = RandomUtil.SelectOne<BattleDiceCardModel>(this._owner.allyCardDetail.GetHand());
            randomCardInHand.AddBuf(new BattleDiceCardBuf_Emotion_Heart());
            randomCardInHand.SetAddedIcon("Latitia_Heart");
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            BattleDiceCardBuf battleDiceCardBuf = curCard?.card?.GetBufList().Find((Predicate<BattleDiceCardBuf>)(y => y is BattleDiceCardBuf_Emotion_Heart));
            if (battleDiceCardBuf == null)
                return;
            curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
            {
                power = Pow
            });
            battleDiceCardBuf.Destroy();
        }
        public void Destroy()
        {
            foreach(BattleDiceCardModel card in this._owner.allyCardDetail.GetAllDeck())
            {
                if(card.GetBufList().Find((Predicate<BattleDiceCardBuf>)(x => x is BattleDiceCardBuf_Emotion_Heart)) is BattleDiceCardBuf_Emotion_Heart heart)
                {
                    card.RemoveAddedIcon("Latitia_Heart");
                    heart.Destroy();
                }
            }
        }
        public class BattleDiceCardBuf_Emotion_Heart : BattleDiceCardBuf
        {
            private int turn;
            private bool used;
            private static int Dmg => RandomUtil.Range(2, 7);
            public override void OnUseCard(BattleUnitModel owner)
            {
                base.OnUseCard(owner);
                this.used = true;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                if (this.used)
                {
                    this._card.RemoveAddedIcon("Latitia_Heart");
                    this.Destroy();
                }
                else
                {
                    ++this.turn;
                    if (this.turn < 2)
                        return;
                    BattleUnitModel owner = this._card?.owner;
                    owner?.TakeDamage(Dmg, DamageType.Card_Ability,owner);
                    new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/Latitia_Filter_Grey", false, 2f);
                    if (this._card.owner.faction == Faction.Player)
                        this._card.temporary = true;
                    if (this._card.owner.faction == Faction.Enemy)
                    {
                        this._card.owner.allyCardDetail.DiscardACardByAbility(this._card);
                        this._card.owner.allyCardDetail.DrawCards(1);
                    }
                    this.Destroy();
                }
            }
        }
    }
}
