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
        private Battle.CreatureEffect.CreatureEffect _aura;
        private bool ExtraHit;
        private bool activated;
        private int DamageReductionByGuard;
        private List<BehaviourDetail> All => new List<BehaviourDetail>()
        {
            BehaviourDetail.Hit,BehaviourDetail.Penetrate,BehaviourDetail.Slash
        };
        private List<BehaviourDetail> Remain;
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            _aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("8_B/FX_IllusionCard_8_B_MonsterAura", 1f, _owner.view, _owner.view);
        }
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            activated = false;
            if (SearchEmotion(_owner, "ApocalypseBird_LongArm") == null || SearchEmotion(_owner, "ApocalypseBird_SmallPeak") == null || SearchEmotion(_owner, "ApocalypseBird_BigEye") == null)
                return;
            GameObject gameObject = Util.LoadPrefab("Battle/CreatureEffect/FinalBattle/BinahFinalBattle_ImageFilter");
            if (!(gameObject != null))
                return;
            Creature_Final_Binah_ImageFilter component = gameObject?.GetComponent<Creature_Final_Binah_ImageFilter>();
            if (component != null)
                component.Init(4);
            gameObject.AddComponent<AutoDestruct>().time = 10f;
            _aura=SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("8_B/FX_IllusionCard_8_B_MonsterAura", 1f, _owner.view, _owner.view);
            activated=true;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            activated = false;
            if (SearchEmotion(_owner, "ApocalypseBird_LongArm") == null || SearchEmotion(_owner, "ApocalypseBird_SmallPeak") == null || SearchEmotion(_owner, "ApocalypseBird_BigEye") == null)
                return;
            activated = true;
        }
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            if (!activated || ExtraHit)
                return;
            Remain = new List<BehaviourDetail>();
            Remain.AddRange(All);
            Remain.Remove(behavior.Detail);
            DamageReductionByGuard = (int)typeof(BattleDiceBehavior).GetField("_damageReductionByGuard", AccessTools.all).GetValue(behavior);
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
        public override void OnEndBattlePhase()
        {
            base.OnEndBattlePhase();
            DestroyAura();
        }
        public override void OnDie(BattleUnitModel killer)
        {
            base.OnDie(killer);
            DestroyAura();
        }
        public void Destroy()
        {
            DestroyAura();
        }
        public void DestroyAura()
        {
            if (!(_aura != null))
                return;
            UnityEngine.Object.Destroy(_aura.gameObject);
            _aura = (Battle.CreatureEffect.CreatureEffect)null;
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
