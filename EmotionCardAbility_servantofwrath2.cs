using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_servantofwrath2 : EmotionCardAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || target.faction==this._owner.faction|| this.GetAliveFriend() != null)
                return;
            target.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_Wrath_Friend());
        }
        public override void OnKill(BattleUnitModel target)
        {
            base.OnKill(target);
            if (target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(y => y is BattleUnitBuf_Emotion_Wrath_Friend)) == null)
                return;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Player))
            {
                alive.cardSlotDetail.RecoverPlayPoint(3);
                alive.breakDetail.RecoverBreak(10);
                alive.RecoverHP(10);
            }
        }
        public void Destroy()
        {
            if (GetAliveFriend().bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_Wrath_Friend)) is BattleUnitBuf_Emotion_Wrath_Friend friend)
                friend.Destroy();
        }
        private BattleUnitModel GetAliveFriend() => BattleObjectManager.instance.GetAliveList(this._owner.faction==Faction.Player?Faction.Enemy:Faction.Player).Find((Predicate<BattleUnitModel>)(x => x.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(y => y is BattleUnitBuf_Emotion_Wrath_Friend)) != null));
        public class BattleUnitBuf_Emotion_Wrath_Friend : BattleUnitBuf
        {
            protected override string keywordId => "Angry_Friend";

            protected override string keywordIconId => "Reclus_Head";

            public BattleUnitBuf_Emotion_Wrath_Friend() => this.stack = 0;
        }
    }
}
