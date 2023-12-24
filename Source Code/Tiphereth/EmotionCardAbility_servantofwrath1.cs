using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;
using BaseMod;

namespace EmotionalFix
{
    public class EmotionCardAbility_servantofwrath1 : EmotionCardAbilityBase
    {
        public override void OnWaveStart()
        {
            if (_owner.faction == Faction.Player)
            {
                if (_owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_Wrath_Berserk)) != null)
                    return;
                _owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_Wrath_Berserk());
            }
            if (_owner.faction == Faction.Enemy)
            {
                _owner.bufListDetail.AddBuf(new Berserk_Enemy());
            }
        }
        public override void OnSelectEmotion()
        {
            if (_owner.faction == Faction.Player)
            {
                _owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_Wrath_Berserk());
            }
            if (_owner.faction == Faction.Enemy)
            {
                _owner.bufListDetail.AddBuf(new Berserk_Enemy());
            }
        }
        public void Destroy()
        {
            if (_owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_Wrath_Berserk)) is BattleUnitBuf_Emotion_Wrath_Berserk Berserk)
                Berserk.Destroy();
            if (_owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Berserk_Enemy)) is Berserk_Enemy BerserkEnemy)
                BerserkEnemy.Destroy();
        }
        public class BattleUnitBuf_Emotion_Wrath_Berserk : BattleUnitBuf
        {
            private GameObject aura;
            public override string keywordId => "Angry_Angry";
            public override string keywordIconId => "Wrath_Head";
            public override bool IsControllable => Controlable();
            private bool Controlable() => _owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_clownofnihil3.BattleUnitBuf_Emotion_Nihil)) != null;
            public override bool TeamKill() => true;
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("5_T/FX_IllusionCard_5_T_Rage", 1f, owner.view, owner.view)?.gameObject;
                SoundEffectPlayer.PlaySound("Creature/Angry_Meet");
            }
            public override void OnRoundEndTheLast()
            {
                base.OnRoundEndTheLast();
                _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 2, _owner);
            }
            public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int currentSlot)
            {
                foreach(BattleUnitModel enemy in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
                {
                    if (enemy.bufListDetail.GetActivatedBufList().Exists(x => x is EmotionCardAbility_servantofwrath2.BattleUnitBuf_Emotion_Wrath_Friend))
                        return enemy;
                }
                return null;
            }
            public override void OnDie()
            {
                base.OnDie();
                Destroy();
            }
            public override void Destroy()
            {
                base.Destroy();
                DestroyAura();
            }
            public void DestroyAura()
            {
                if (!(aura != null))
                    return;
                UnityEngine.Object.Destroy(aura);
                aura = (GameObject)null;
            }
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                _owner.cardSlotDetail.RecoverPlayPoint(2);
                _owner.allyCardDetail.DrawCards(2);
                if (_owner.bufListDetail.GetActivatedBufList().Find(x => x is EmotionCardAbility_clownofnihil3.BattleUnitBuf_Emotion_Nihil) != null)
                    DestroyAura();
            }
        }
        public class Berserk_Enemy: BattleUnitBuf
        {
            private GameObject aura;
            public override string keywordId => "Berserk_Enemy";
            public override string keywordIconId => "Wrath_Head";
            public override int SpeedDiceNumAdder() => 1;
            public override void OnRoundEndTheLast()
            {
                _owner.allyCardDetail.AddNewCard(1104401).temporary = true;
            }
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("5_T/FX_IllusionCard_5_T_Rage", 1f, owner.view, owner.view)?.gameObject;
            }
            public override void OnDie()
            {
                base.OnDie();
                Destroy();
            }
            public override void Destroy()
            {
                base.Destroy();
                DestroyAura();
            }
            public void DestroyAura()
            {
                if (!(aura != null))
                    return;
                UnityEngine.Object.Destroy(aura);
                aura = (GameObject)null;
            }
        }
    }
}
