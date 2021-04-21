using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using System.Reflection;
using Sound;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;

namespace EmotionalFix
{
    public class EmotionCardAbility_bossbird4 : EmotionCardAbilityBase
    {
        public static List<BattleDiceCardModel> Summation;
        private int round = 0;
        private ApocalypsePhase phase;
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            foreach(BattleUnitModel ally in BattleObjectManager.instance.GetAliveList(this._owner.faction))
            {
                foreach (BattleEmotionCardModel emotion in ally.emotionDetail.PassiveList)
                {
                    foreach (EmotionCardAbilityBase ablility in emotion.GetAbilityList())
                    {
                        MethodInfo destroy = ablility.GetType().GetMethod("Destroy");
                        if (destroy != null)
                            destroy.Invoke(ablility, new object[] { });
                    }
                }
                ally.emotionDetail.PassiveList.Clear();
            }
            this._owner.emotionDetail.PassiveList.Add(this._emotionCard);
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/BossBird_Birth", false, 4f);
            this._owner.bufListDetail.AddBuf(new Apocalypse(this));
        }
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            switch (round)
            {
                case 0:
                    this.phase = ApocalypsePhase.Big;
                    round += 1;
                    break;
                case 1:
                    this.phase = ApocalypsePhase.Small;
                    round += 1;
                    break;
                case 2:
                    this.phase = ApocalypsePhase.Long;
                    round = 0;
                    break;
            }
        }
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            if (phase != ApocalypsePhase.Big || this._owner.allyCardDetail.GetAllDeck().Count <= 0)
                return;
            DiceCardXmlInfo xmldata = RandomUtil.SelectOne<BattleDiceCardModel>(this._owner.allyCardDetail.GetAllDeck()).XmlData.Copy(true);
            foreach (DiceBehaviour dice in xmldata.DiceBehaviourList)
            {
                dice.ActionScript = "Final_ApcBird_LaserArea";
                if(dice.Detail==BehaviourDetail.Evasion || dice.Detail == BehaviourDetail.Guard)
                {
                    dice.Detail = RandomUtil.SelectOne<BehaviourDetail>(new List<BehaviourDetail>() { BehaviourDetail.Slash, BehaviourDetail.Hit, BehaviourDetail.Penetrate });
                }
            }
            BattleDiceCardModel EyeAtk = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(1107501));
            DiceCardXmlInfo xmlinfo =typeof(BattleDiceCardModel).GetField("_xmlData", AccessTools.all).GetValue(EyeAtk) as DiceCardXmlInfo;
            xmlinfo.DiceBehaviourList = xmldata.DiceBehaviourList;
            typeof(BattleDiceCardModel).GetField("_xmlData", AccessTools.all).SetValue(EyeAtk, xmlinfo);
            BattleUnitModel target = RandomUtil.SelectOne<BattleUnitModel>(BattleObjectManager.instance.GetAliveList(Faction.Enemy));
            BattlePlayingCardDataInUnitModel EyeAoe = new BattlePlayingCardDataInUnitModel()
            {
                owner = this._owner,
                card = EyeAtk,
                cardAbility = EyeAtk.CreateDiceCardSelfAbilityScript(),
                target=target,
                targetSlotOrder= RandomUtil.Range(0, target.cardSlotDetail.cardAry.Count - 1),
                slotOrder= RandomUtil.Range(0, this._owner.cardSlotDetail.cardAry.Count - 1)
            };
            List<BattleUnitModel> battleUnitModelList = BattleObjectManager.instance.GetAliveList(Faction.Enemy);
            battleUnitModelList.Remove(target);
            EyeAoe.subTargets = new List<BattlePlayingCardDataInUnitModel.SubTarget>();
            foreach (BattleUnitModel battleUnitModel in battleUnitModelList)
            {
                if (battleUnitModel != target && battleUnitModel.IsTargetable(this._owner))
                {
                    BattlePlayingCardSlotDetail cardSlotDetail = battleUnitModel.cardSlotDetail;
                    int num1;
                    if (cardSlotDetail == null)
                    {
                        num1 = 0;
                    }
                    else
                    {
                        int? count = cardSlotDetail.cardAry?.Count;
                        int num2 = 0;
                        num1 = count.GetValueOrDefault() > num2 & count.HasValue ? 1 : 0;
                    }
                    if (num1 != 0)
                        EyeAoe.subTargets.Add(new BattlePlayingCardDataInUnitModel.SubTarget()
                        {
                            target = battleUnitModel,
                            targetSlotOrder = UnityEngine.Random.Range(0, battleUnitModel.speedDiceResult.Count)
                        });
                }
            }
            EyeAoe.ResetExcludedDices();
            EyeAoe.ResetCardQueueWithoutStandby();
            List<BattlePlayingCardDataInUnitModel> cardlist = typeof(StageController).GetField("_allCardList", AccessTools.all).GetValue(Singleton<StageController>.Instance) as List<BattlePlayingCardDataInUnitModel>;
            cardlist.Add(EyeAoe);
        }
        public override void OnRoundStart()
        {
            if (this.phase != ApocalypsePhase.Small || this._owner.allyCardDetail.GetAllDeck().Count <= 0)
                return;
            foreach(BattleDiceCardModel card in this._owner.allyCardDetail.GetAllDeck())
            {
                DiceCardXmlInfo xmlInfo = typeof(BattleDiceCardModel).GetField("_xmlData", AccessTools.all).GetValue(card) as DiceCardXmlInfo;
                DiceCardSpec Spec = xmlInfo.Spec.Copy();
                Spec.Ranged = CardRange.FarArea;
                Spec.affection = CardAffection.Team;
                xmlInfo.Spec = Spec;
                DiceBehaviour dice = new DiceBehaviour()
                {
                    Min = 0,
                    Dice = 0,
                    Type = BehaviourType.Atk,
                    Detail = RandomUtil.SelectOne<BehaviourDetail>(new List<BehaviourDetail>() { BehaviourDetail.Slash, BehaviourDetail.Hit, BehaviourDetail.Penetrate }),
                    MotionDetail = MotionDetail.J,
                    EffectRes="",
                    Script="",
                    ActionScript = "Final_ApcBird_PutDownArea",
                    Desc=""
                };
                foreach (DiceBehaviour behaviour in xmlInfo.DiceBehaviourList)
                {
                    dice.Min += behaviour.Min;
                    dice.Dice += behaviour.Dice;
                }
                xmlInfo.DiceBehaviourList = new List<DiceBehaviour>() { dice };
                typeof(BattleDiceCardModel).GetField("_xmlData", AccessTools.all).SetValue(card, xmlInfo);
                EmotionCardAbility_bossbird4.Summation.Add(card);
            }
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            if (phase != ApocalypsePhase.Long)
                return;
            BattleUnitModel target = behavior.card.target;
            if (target == null)
                return;
            int v = Mathf.RoundToInt((float)(target.MaxHp * behavior.DiceResultValue) * 0.005f);
            target.TakeDamage(v, DamageType.Attack, this._owner);
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/LongBird_Stun", false, 1f);
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            EmotionCardAbility_bossbird4.ClearCard();
        }
        public enum ApocalypsePhase
        {
            Big,
            Small,
            Long
        }
        public static void ClearCard()
        {
            foreach (BattleDiceCardModel battleDiceCardModel1 in EmotionCardAbility_bossbird4.Summation)
            {
                FieldInfo field1 = battleDiceCardModel1.GetType().GetField("_originalXmlData", AccessTools.all);
                FieldInfo field2 = battleDiceCardModel1.GetType().GetField("_xmlData", AccessTools.all);
                DiceCardXmlInfo diceCardXmlInfo1 = ItemXmlDataList.instance.GetCardItem(battleDiceCardModel1.XmlData.id).Copy(true);
                FieldInfo field3 = diceCardXmlInfo1.GetType().GetField("Spec", AccessTools.all);
                DiceCardSpec CardSpec = diceCardXmlInfo1.Spec.Copy();
                BattleDiceCardModel battleDiceCardModel2 = battleDiceCardModel1;
                DiceCardXmlInfo diceCardXmlInfo2 = diceCardXmlInfo1;
                field3.SetValue((object)diceCardXmlInfo2, (object)CardSpec);
                field2.SetValue((object)battleDiceCardModel2, (object)diceCardXmlInfo2);
                BattleDiceCardModel battleDiceCardModel3 = battleDiceCardModel1;
                DiceCardXmlInfo diceCardXmlInfo3 = diceCardXmlInfo1;
                field3.SetValue((object)diceCardXmlInfo3, (object)CardSpec);
                field1.SetValue((object)battleDiceCardModel3, (object)diceCardXmlInfo3);
            }
            EmotionCardAbility_bossbird4.Summation.Clear();
        }
        public void Destroy()
        {
            BattleUnitBuf Buff = this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Apocalypse));
            if (Buff != null)
                Buff.Destroy();
        }
        public class Apocalypse: BattleUnitBuf
        {
            EmotionCardAbility_bossbird4 Emotion;
            public Apocalypse(EmotionCardAbility_bossbird4 emotion)
            {
                stack = 0;
                Emotion = emotion;
            }
            protected override string keywordId
            {
                get
                {
                    switch (Emotion.phase)
                    {
                        case ApocalypsePhase.Big:
                            return ("ApocalypseBird_Big");
                        case ApocalypsePhase.Small:
                            return ("ApocalypseBird_Small");
                        case ApocalypsePhase.Long:
                            return ("ApocalypseBird_Long");
                    }
                    return ("");
                }
            }
            protected override string keywordIconId => "ApocalypseBird_Apocalypse";
        }
    }
}
