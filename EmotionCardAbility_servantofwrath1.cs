using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_servantofwrath1 : EmotionCardAbilityBase
    {
        public override void OnWaveStart()
        {
            if (this._owner.faction == Faction.Player)
            {
                if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_Wrath_Berserk)) != null)
                    return;
                this._owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_Wrath_Berserk());
            }
            if (this._owner.faction == Faction.Enemy)
            {
                this._owner.bufListDetail.AddBuf(new Berserk_Enemy());
            }
        }
        public override void OnSelectEmotion()
        {
            if (this._owner.faction == Faction.Player)
            {
                this._owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_Wrath_Berserk());
            }
            if (this._owner.faction == Faction.Enemy)
            {
                this._owner.bufListDetail.AddBuf(new Berserk_Enemy());
            }
        }
        public void Destroy()
        {
            if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_Wrath_Berserk)) is BattleUnitBuf_Emotion_Wrath_Berserk Berserk)
                Berserk.Destroy();
            if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Berserk_Enemy)) is Berserk_Enemy BerserkEnemy)
                BerserkEnemy.Destroy();
        }
        public class BattleUnitBuf_Emotion_Wrath_Berserk : BattleUnitBuf
        {
            protected override string keywordId => "Angry_Angry";
            protected override string keywordIconId => "Wrath_Head";
            public override bool IsControllable => this.Controlable();
            private bool Controlable() => this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_clownofnihil3.BattleUnitBuf_Emotion_Nihil)) != null;
            public override bool TeamKill() => true;
            public override void OnRoundEndTheLast()
            {
                base.OnRoundEndTheLast();
                this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 2, this._owner);
            }
            public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int currentSlot)
            {
                foreach(BattleUnitModel enemy in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
                {
                    if (enemy.bufListDetail.GetActivatedBufList().Exists((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_servantofwrath2.BattleUnitBuf_Emotion_Wrath_Friend)))
                        return enemy;
                }
                return null;
            }
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                this._owner.cardSlotDetail.RecoverPlayPoint(2);
                this._owner.allyCardDetail.DrawCards(2);
            }
        }
        public class Berserk_Enemy: BattleUnitBuf
        {
            protected override string keywordId => "Berserk_Enemy";
            protected override string keywordIconId => "Wrath_Head";
            public override int SpeedDiceNumAdder() => 1;
            public override void OnRoundEndTheLast()
            {
                this._owner.allyCardDetail.AddNewCard(1104401).temporary = true;
            }
        }
    }
}
