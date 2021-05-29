using System;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_teddy2 : EmotionCardAbilityBase
    {
        private int _id = -1;
        public override void OnRoundStart()
        {

            if (this._owner.faction == Faction.Player)
            {
                if (this._id < 0)
                {
                    List<BattleDiceCardModel> hand = this._owner.allyCardDetail.GetHand();
                    int highest = 0;
                    foreach (BattleDiceCardModel battleDiceCardModel in hand)
                    {
                        int cost = battleDiceCardModel.GetCost();
                        if (highest < cost)
                            highest = cost;
                    }
                    List<BattleDiceCardModel> all = hand.FindAll((Predicate<BattleDiceCardModel>)(x => x.GetCost() == highest));
                    this._id = all.Count <= 0 ? 0 : RandomUtil.SelectOne<BattleDiceCardModel>(all).GetID();
                }
                this.Ability();
            }
        }
        public override void OnSelectEmotion() => SoundEffectPlayer.PlaySound("Creature/Teddy_On");
        public override void OnParryingStart(BattlePlayingCardDataInUnitModel card)
        {
            if (card.target == null || this._owner.faction != Faction.Enemy)
                return;
            BattleUnitBuf activatedBuf = card.target.bufListDetail.GetActivatedBuf(KeywordBuf.TeddyLove);
            if (activatedBuf != null)
            {
                ++activatedBuf.stack;
            }
            else
                card.target.bufListDetail.AddBuf(new TeddyMemory());
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (behavior.card?.target == null || this._owner.faction!=Faction.Enemy || !behavior.IsParrying())
                return;
            int kewordBufStack = behavior.card.target.bufListDetail.GetKewordBufStack(KeywordBuf.TeddyLove);
            if (kewordBufStack <= 0)
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                power = kewordBufStack
            });
        }
        private void Ability()
        {
            if (this._id <= 0)
                return;
            foreach (BattleDiceCardModel battleDiceCardModel in this._owner.allyCardDetail.GetAllDeck().FindAll((Predicate<BattleDiceCardModel>)(x => x.GetID() == this._id)))
            {
                battleDiceCardModel.AddBufWithoutDuplication(new TeddyCardBuf());
                battleDiceCardModel.SetAddedIcon("TeddyIcon");
            }
        }
        public void Destroy()
        {
            if (this._owner.faction == Faction.Player)
            {
                foreach(BattleDiceCardModel card in this._owner.allyCardDetail.GetAllDeck())
                {
                    if(card.GetBufList().Find((Predicate<BattleDiceCardBuf>)(x => x is TeddyCardBuf)) is TeddyCardBuf teddy)
                    {
                        card.SetCurrentCost(card.XmlData.Spec.Cost);
                        card.RemoveAddedIcon("TeddyIcon");
                        teddy.Destroy();
                    }
                }
            }
            if (this._owner.faction == Faction.Enemy)
            {
                bool mulitteddy = false;
                foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
                {
                    if (SearchEmotion(unit, "HappyTeddyBear_Memory_Enemy") != null)
                        mulitteddy = true;
                }
                if (mulitteddy == true)
                    return;
                foreach (BattleUnitModel player in BattleObjectManager.instance.GetAliveList(Faction.Player))
                {
                    if (player.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is TeddyMemory)) is TeddyMemory memory)
                        memory.Destroy();
                }
            }
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
        public class TeddyCardBuf : BattleDiceCardBuf
        {
            public override DiceCardBufType bufType => DiceCardBufType.Teddy;

            public override void OnUseCard(BattleUnitModel owner)
            {
                if (this._card.GetOriginCost() == 4 && this._card.GetCost() <= 0)
                    PlatformManager.Instance.UnlockAchievement(AchievementEnum.ONCE_FLOOR1);
                foreach (BattleDiceCardModel battleDiceCardModel in owner.allyCardDetail.GetAllDeck().FindAll((Predicate<BattleDiceCardModel>)(x => x.GetID() == this._card.GetID())))
                    battleDiceCardModel.AddCost(-1);
            }
        }
        public class TeddyMemory : BattleUnitBuf
        {
            public override KeywordBuf bufType => KeywordBuf.TeddyLove;
            protected override string keywordIconId => "TeddyLove";
            protected override string keywordId => "Teddy_Memory";
            public override void OnRoundEnd()
            {
                this.stack -= 1;
                if (this.stack <= 0)
                {
                    this.Destroy();
                }
            }
        }
    }
}
