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
            if (_owner.faction == Faction.Player)
            {
                _owner.bufListDetail.AddBuf(new BigBird());
                GameObject gameObject = Util.LoadPrefab("Battle/CreatureEffect/FinalBattle/BinahFinalBattle_ImageFilter");
                if (!(gameObject != null))
                    return;
                Creature_Final_Binah_ImageFilter component = gameObject.GetComponent<Creature_Final_Binah_ImageFilter>();
                if (component != null)
                    component.Init(1);
                gameObject.AddComponent<AutoDestruct>().time = 10f;
            }
            if (_owner.faction == Faction.Enemy)
                _owner.bufListDetail.AddBuf(new Bigbird_Enemy());
        }
        public void Effect()
        {
            if (Singleton<StageController>.Instance.IsLogState())
                _owner.battleCardResultLog?.SetNewCreatureAbilityEffect("8_B/FX_IllusionCard_8_B_See", 2f);
            else
                SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("8_B/FX_IllusionCard_8_B_See", 1f, _owner.view, _owner.view, 2f);
        }

        public override void OnWaveStart()
        {
            base.OnWaveStart();
            if (_owner.faction == Faction.Player)
                _owner.bufListDetail.AddBuf(new BigBird());
            if (_owner.faction == Faction.Enemy)
                _owner.bufListDetail.AddBuf(new Bigbird_Enemy());
        }
        public override StatBonus GetStatBonus()
        {
            if (_owner.faction == Faction.Player)
                return new StatBonus() { breakRate = -50 };
            return base.GetStatBonus();
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            Effect();
        }
        public override void OnStartTargetedOneSide(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnStartTargetedOneSide(curCard);
            Effect();
        }
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            if (_owner.faction != Faction.Player)
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
            _owner.cardSlotDetail.keepCard.AddBehaviours(BattleDiceCardModel.CreatePlayingCard(cardItem), behaviourList);
        }
        public override void OnEndParrying(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnEndParrying(curCard);
            if (_owner.faction != Faction.Player)
                return;
            if (_owner.cardSlotDetail.keepCard.cardBehaviorQueue.Count <= 0)
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
                _owner.cardSlotDetail.keepCard.AddBehaviours(BattleDiceCardModel.CreatePlayingCard(cardItem), behaviourList);
            }
        }
        public void Destroy()
        {
            BattleUnitBuf Buff = _owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BigBird));
            if (Buff != null)
                Buff.Destroy();
            Buff = _owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Bigbird_Enemy));
            if (Buff != null)
                Buff.Destroy();
            SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(_owner, _owner.faction, _owner.hp, _owner.breakDetail.breakGauge);
        }
        public class BigBird : BattleUnitBuf
        {
            public override string keywordIconId => "ApocalypseBird_BigEye";
            public override string keywordId => "Bigbird";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
        }
        public class Bigbird_Enemy: BattleUnitBuf
        {
            public override string keywordIconId => "ApocalypseBird_BigEye";
            public override string keywordId => "Bigbird_Enemy";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                RandomUtil.SelectOne<BattleUnitModel>(BattleObjectManager.instance.GetAliveList(Faction.Player)).bufListDetail.AddBuf(new BattleUnitBuf_halfPower());
            }
        }
    }
}
