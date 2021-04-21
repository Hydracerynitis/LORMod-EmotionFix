using System;
using LOR_DiceSystem;
using UI;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_ozma2 : EmotionCardAbilityBase
    {
        private List<int> Memory;
        private List<BattleDiceCardModel> LastRoundInUse;
        private List<BattleDiceCardModel> deck;
        public override StatBonus GetStatBonus() => new StatBonus() 
        {
            breakRate=-50
        };
        public override void OnSelectEmotion()
        {
            Memory=new List<int>();
            LastRoundInUse = new List<BattleDiceCardModel>();
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            deck = this._owner.allyCardDetail.GetAllDeck();
            if (deck.Count > 0) 
            {
                if(LastRoundInUse.Count>0 && Memory.Count > 0)
                {                 
                    foreach (BattleDiceCardModel originalCard in LastRoundInUse)
                    {
                        if (!deck.Contains(originalCard))
                            Memory.Remove(originalCard.GetID());
                     }
                }
                foreach (BattleDiceCardModel Card in deck)
                {
                    if (LastRoundInUse.Count == 0 || !LastRoundInUse.Contains(Card))
                        Memory.Add(Card.GetID());
                }
            }
            this._owner.allyCardDetail.ExhaustAllCards();
            List<SpeedDice> speedDiceList = this._owner.Book.GetSpeedDiceRule(this._owner).Roll(this._owner);
            List<int> IDs = new List<int>();
            IDs.AddRange(Memory);
            for (int i = 0; i < speedDiceList.Count(); i++)
            {
                int card = RandomUtil.SelectOne<int>(IDs);
                BattleDiceCardModel Card=this._owner.allyCardDetail.AddNewCard(card);
                IDs.Remove(card);
                Card.AddCost(-10);
            }          
            LastRoundInUse = this._owner.allyCardDetail.GetHand();
        }
        public void Destory()
        {
            foreach(int id in Memory)
            {
                this._owner.allyCardDetail.AddNewCardToDeck(id);
            }
            this._owner.allyCardDetail.DrawCards(this._owner.Book.AddedStartDraw + 3);
            SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(this._owner, this._owner.faction, this._owner.hp, this._owner.breakDetail.breakGauge);
        }
    }
}
