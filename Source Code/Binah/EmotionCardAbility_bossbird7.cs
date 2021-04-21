using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using HarmonyLib;
using System.Reflection;
using System.Collections.Generic;
using System.IO;

namespace EmotionalFix
{
    public class EmotionCardAbility_bossbird7 : EmotionCardAbilityBase
    {
        public static List<BattleDiceCardModel> Change = new List<BattleDiceCardModel>();
        private BattleUnitView.SkinInfo OriginalSkin;
        private bool ExtraHit;
        private bool activated;
        private int DamageReductionByGuard;
        private List<BehaviourDetail> All => new List<BehaviourDetail>()
        {
            BehaviourDetail.Hit,BehaviourDetail.Penetrate,BehaviourDetail.Slash
        };
        private List<BehaviourDetail> Remain;
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            activated = false;
            if (SearchEmotion(this._owner, "ApocalypseBird_LongArm") == null || SearchEmotion(this._owner, "ApocalypseBird_SmallPeak") == null || SearchEmotion(this._owner, "ApocalypseBird_BigEye") == null)
                return;
            Activate();
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            activated = false;
            if (SearchEmotion(this._owner, "ApocalypseBird_LongArm") == null || SearchEmotion(this._owner, "ApocalypseBird_SmallPeak") == null || SearchEmotion(this._owner, "ApocalypseBird_BigEye") == null)
                return;
            Activate();
        }
        public override void OnRoundStart()
        {
            if (!activated)
                return;
            foreach(BattleDiceCardModel card in this._owner.allyCardDetail.GetAllDeck())
                ChangeCardForEgo(card);
        }
        public override void OnStartBattle()
        {
            if (!activated)
                return;
            Activate();
        }
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            if (!activated || ExtraHit)
                return;
            base.BeforeRollDice(behavior);
            Remain = new List<BehaviourDetail>();
            Remain.AddRange(All);
            Remain.Remove(behavior.Detail);
            DamageReductionByGuard = (int)typeof(BattleDiceBehavior).GetField("_damageReductionByGuard", AccessTools.all).GetValue(behavior);
        }
        public override void OnDie(BattleUnitModel killer)
        {
            base.OnDie(killer);
            this._owner.view.ChangeSkinBySkinInfo(OriginalSkin);
            this._owner.view.StartEgoSkinChangeEffect("Character");
        }
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            EmotionCardAbility_bossbird7.ClearCard();
            if (BattleObjectManager.instance.GetAliveList(Faction.Enemy).Count <= 0 && activated)
            {
                this._owner.view.ChangeSkinBySkinInfo(OriginalSkin);
                this._owner.view.StartEgoSkinChangeEffect("Character");
            }

        }
        
        public override void AfterDiceAction(BattleDiceBehavior behavior)
        {
            base.AfterDiceAction(behavior);
            ExtraHit = false;
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            if (!activated || ExtraHit)
                return;
            BattleUnitModel target = behavior.card.target;
            ExtraHit = true;
            foreach (BehaviourDetail detail in Remain)
            {
                switch (detail)
                {
                    case BehaviourDetail.Hit:
                        if (target == null)
                            return;
                        GiveHitDamage(target, behavior);
                        break;
                    case BehaviourDetail.Penetrate:
                        if (target == null)
                            return;
                        GivePenetrateDamage(target, behavior);
                        break;
                    case BehaviourDetail.Slash:
                        if (target == null)
                            return;
                        GiveSlashDamage(target, behavior);
                        break;
                }
            }
        }
        public void Activate()
        {
            activated = true;
            if(this._owner.view.GetCurrentSkinInfo().skinName!= "EGO_LongBird")
            {
                OriginalSkin = this._owner.view.GetCurrentSkinInfo();
                this._owner.view.ChangeEgoSkin("EGO_LongBird");
                this._owner.view.StartEgoSkinChangeEffect("Character");
                SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Bossbird_StoryFilter_Dead");
            }
        }
        public void ChangeCardForEgo(BattleDiceCardModel card)
        {
            DiceCardXmlInfo xmlData = typeof(BattleDiceCardModel).GetField("_xmlData", AccessTools.all).GetValue(card) as DiceCardXmlInfo;
            List<DiceBehaviour> diceBehaviourList = new List<DiceBehaviour>();
            foreach(DiceBehaviour diceBehaviour in xmlData.DiceBehaviourList)
            {
                DiceBehaviour diceBehaviour1 = diceBehaviour.Copy();
                diceBehaviour1.EffectRes = "";
                if(diceBehaviour1.MotionDetail!=MotionDetail.E && diceBehaviour.MotionDetail != MotionDetail.G)
                {
                    if (xmlData.Spec.Ranged == CardRange.Far)
                    {
                        diceBehaviour1.ActionScript = "BossBird6SoundFar";
                        diceBehaviour1.MotionDetail = MotionDetail.S1;
                    }
                    if (xmlData.Spec.Ranged == CardRange.Near)
                    {
                        diceBehaviour1.ActionScript = "BossBird6SoundNear";
                        diceBehaviour1.MotionDetail = MotionDetail.S2;
                    }
                }
                diceBehaviourList.Add(diceBehaviour1);
            }
            xmlData.DiceBehaviourList = diceBehaviourList;
            typeof(BattleDiceCardModel).GetField("_xmlData", AccessTools.all).SetValue(card, xmlData);
            EmotionCardAbility_bossbird7.Change.Add(card);
        }
        public static void ClearCard()
        {
            foreach (BattleDiceCardModel battleDiceCardModel1 in EmotionCardAbility_bossbird7.Change)
            {
                FieldInfo field1 = battleDiceCardModel1.GetType().GetField("_originalXmlData", AccessTools.all);
                FieldInfo field2 = battleDiceCardModel1.GetType().GetField("_xmlData", AccessTools.all);
                DiceCardXmlInfo diceCardXmlInfo1 = ItemXmlDataList.instance.GetCardItem(battleDiceCardModel1.XmlData.id).Copy(true);
                BattleDiceCardModel battleDiceCardModel2 = battleDiceCardModel1;
                DiceCardXmlInfo diceCardXmlInfo2 = diceCardXmlInfo1;
                field2.SetValue((object)battleDiceCardModel2, (object)diceCardXmlInfo2);
                BattleDiceCardModel battleDiceCardModel3 = battleDiceCardModel1;
                DiceCardXmlInfo diceCardXmlInfo3 = diceCardXmlInfo1;
                field1.SetValue((object)battleDiceCardModel3, (object)diceCardXmlInfo3);
            }
            EmotionCardAbility_bossbird7.Change.Clear();
        }
        public void Destroy()
        {
            EmotionCardAbility_bossbird7.ClearCard();
            this._owner.view.ChangeSkinBySkinInfo(OriginalSkin);
            this._owner.view.StartEgoSkinChangeEffect("Character");
        }
        public void GiveHitDamage(BattleUnitModel enemy,BattleDiceBehavior dice)
        {
            DiceBehaviour xml = dice.behaviourInCard.Copy();
            xml.Detail = BehaviourDetail.Hit;
            BattleVoidBehaviour Newdice = new BattleVoidBehaviour(dice);
            Newdice.behaviourInCard = xml;
            Newdice._damageReductionByGuard = DamageReductionByGuard;
            Newdice.RollDice();
            Newdice.UpdateDiceFinalValue();
            Newdice.GiveDamage(enemy);
        }
        public void GivePenetrateDamage(BattleUnitModel enemy, BattleDiceBehavior dice)
        {
            DiceBehaviour xml = dice.behaviourInCard.Copy();
            xml.Detail = BehaviourDetail.Penetrate;
            BattleVoidBehaviour Newdice = new BattleVoidBehaviour(dice);
            Newdice.behaviourInCard = xml;
            Newdice._damageReductionByGuard = DamageReductionByGuard;
            Newdice.RollDice();
            Newdice.UpdateDiceFinalValue();
            Newdice.GiveDamage(enemy);
        }
        public void GiveSlashDamage(BattleUnitModel enemy, BattleDiceBehavior dice)
        {
            DiceBehaviour xml = dice.behaviourInCard.Copy();
            xml.Detail = BehaviourDetail.Slash;
            BattleVoidBehaviour Newdice = new BattleVoidBehaviour(dice);
            Newdice.behaviourInCard = xml;
            Newdice._damageReductionByGuard = DamageReductionByGuard;
            Newdice.RollDice();
            Newdice.UpdateDiceFinalValue();
            Newdice.GiveDamage(enemy);
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
    }
}
