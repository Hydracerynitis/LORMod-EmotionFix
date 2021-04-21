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
    public class EmotionCardAbility_bloodytree2 : EmotionCardAbilityBase
    {
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
            {
                if (alive.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_BloodyTree)) == null)
                    alive.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_BloodyTree(this._owner));
            }
        }
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
            {
                alive.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_BloodyTree(this._owner));
            }
        }
        public void Destroy()
        {
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
            {
                if ((alive.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_BloodyTree))) is BattleUnitBuf_Emotion_BloodyTree Tree)
                    Tree.Destroy();
            }
        }
        public class BattleUnitBuf_Emotion_BloodyTree : BattleUnitBuf
        {
            private BattleUnitModel _except;
            private static int Dmg => RandomUtil.Range(1, 8);
            protected override string keywordId => "BloodyTree_Emotion_Damage";
            protected override string keywordIconId => "HokmaFirstCounter";
            public BattleUnitBuf_Emotion_BloodyTree(BattleUnitModel except)
            {
                this.hide = true;
                this._except = except;
            }
            public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
            {
                base.OnStartParrying(card);
                if (card.target == null || card.target == _except || _except.IsDead())
                    return;
                int DamageEnemy = Dmg;
                card.target.TakeDamage(DamageEnemy);
                card.target.breakDetail.TakeBreakDamage(DamageEnemy);
                int Damage = Dmg;
                this._owner.TakeDamage(Damage);
                this._owner.breakDetail.TakeBreakDamage(Damage);
            }
            public override void OnRoundEndTheLast()
            {
                base.OnRoundEndTheLast();
                if (_except.IsDead())
                    this.Destroy();
            }
        }
    }
}
