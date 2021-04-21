using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;

namespace EmotionalFix
{
    public class EmotionCardAbility_bossbird2 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            if (this._owner.faction == Faction.Player)
                this._owner.bufListDetail.AddBuf(new BigBird());
            if (this._owner.faction == Faction.Enemy)
                this._owner.bufListDetail.AddBuf(new Bigbird_Enemy());
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            if (this._owner.faction == Faction.Player)
                this._owner.bufListDetail.AddBuf(new BigBird());
            if (this._owner.faction == Faction.Enemy)
                this._owner.bufListDetail.AddBuf(new Bigbird_Enemy());
        }
        public override StatBonus GetStatBonus()
        {
            if (this._owner.faction == Faction.Player)
                return new StatBonus() { breakRate = -50 };
            return base.GetStatBonus();
        }
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            if (this._owner.faction != Faction.Player)
                return;
            DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(1107401);
            List<BattleDiceBehavior> behaviourList = new List<BattleDiceBehavior>();
            int num = 0;
            foreach (DiceBehaviour diceBehaviour1 in cardItem.DiceBehaviourList)
            {
                BattleDiceBehavior battleDiceBehavior = new BattleDiceBehavior
                {
                    behaviourInCard = diceBehaviour1
                };
                battleDiceBehavior.SetIndex(num++);
                behaviourList.Add(battleDiceBehavior);
            }
            this._owner.cardSlotDetail.keepCard.AddBehaviours(BattleDiceCardModel.CreatePlayingCard(cardItem), behaviourList);
        }
        public override void OnEndParrying(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnEndParrying(curCard);
            if (this._owner.faction != Faction.Player)
                return;
            if (this._owner.cardSlotDetail.keepCard.cardBehaviorQueue.Count <= 0)
            {
                DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(1107401);
                List<BattleDiceBehavior> behaviourList = new List<BattleDiceBehavior>();
                int num = 0;
                foreach (DiceBehaviour diceBehaviour1 in cardItem.DiceBehaviourList)
                {
                    BattleDiceBehavior battleDiceBehavior = new BattleDiceBehavior
                    {
                        behaviourInCard = diceBehaviour1
                    };
                    battleDiceBehavior.SetIndex(num++);
                    behaviourList.Add(battleDiceBehavior);
                }
                this._owner.cardSlotDetail.keepCard.AddBehaviours(BattleDiceCardModel.CreatePlayingCard(cardItem), behaviourList);
            }
        }
        public void Destroy()
        {
            BattleUnitBuf Buff = this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BigBird));
            if (Buff != null)
                Buff.Destroy();
            Buff = this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Bigbird_Enemy));
            if (Buff != null)
                Buff.Destroy();
            SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(this._owner, this._owner.faction, this._owner.hp, this._owner.breakDetail.breakGauge);
        }
        public class BigBird : BattleUnitBuf
        {
            protected override string keywordIconId => "ApocalypseBird_BigEye";
            protected override string keywordId => "Bigbird";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
        }
        public class Bigbird_Enemy: BattleUnitBuf
        {
            protected override string keywordIconId => "ApocalypseBird_BigEye";
            protected override string keywordId => "Bigbird_Enemy";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (behavior.TargetDice != null)
                    behavior.TargetDice.ApplyDiceStatBonus(new DiceStatBonus()
                    {
                        ignorePower = true
                    });
            }
        }
    }
}
