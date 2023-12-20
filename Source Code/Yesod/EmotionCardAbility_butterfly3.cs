using System;
using LOR_DiceSystem;
using System.IO;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_butterfly3 : EmotionCardAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null)
                return;
            double Ratio = ((double)target.MaxHp - (double)target.hp) / (double)target.MaxHp-0.25;
            int stack = (int)(Ratio * 100);
            if (stack <= 0)
                return;
            if (stack >= 50)
                stack = 50;
            if (!behavior.card.card.XmlData.IsFloorEgo())
            {
                if (behavior.card.card.XmlData.Spec.Ranged == CardRange.Near)
                    this._owner.battleCardResultLog?.SetCreatureAbilityEffect("2/Butterfly_Emotion_Effect_Spread_Near", 1f);
                else
                    this._owner.battleCardResultLog?.SetCreatureAbilityEffect("2/Butterfly_Emotion_Effect_Spread", 1f);
            }
            if (!(target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is SealGauge)) is SealGauge sealGauge))
            {
                SealGauge SealGauge = new SealGauge();
                target.bufListDetail.AddBuf(SealGauge);
                SealGauge.stack += stack;
            }
            else
            {
                sealGauge.stack += stack;
                if (sealGauge.stack >= 100)
                {
                    if (!(target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Butterfly_Emotion_Seal)) is BattleUnitBuf_Butterfly_Emotion_Seal butterflyEmotionSeal))
                    {
                        BattleUnitBuf_Butterfly_Emotion_Seal ButterflyEmotionSeal = new BattleUnitBuf_Butterfly_Emotion_Seal();
                        target.bufListDetail.AddBuf(ButterflyEmotionSeal);
                        ButterflyEmotionSeal.Add();
                    }
                    else
                        butterflyEmotionSeal.Add();
                    sealGauge.Destroy();
                }
            }
        }
        public void Destroy()
        {
            foreach(BattleUnitModel enemy in BattleObjectManager.instance.GetAliveList(this._owner.faction==Faction.Player? Faction.Enemy: Faction.Player))
            {
                if (enemy.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is SealGauge)) is SealGauge sealGauge)
                {
                    sealGauge.Destroy();
                }
                if(enemy.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Butterfly_Emotion_Seal)) is BattleUnitBuf_Butterfly_Emotion_Seal Seal)
                {
                    Seal.Destroy();
                }
            }
        }
        public class SealGauge : BattleUnitBuf
        {
            public override string keywordIconId => "Butterfly_Seal";
            public override string keywordId => "SealGauge";
            public override void Init(BattleUnitModel owner)
            {
                this.stack = 0;
            }
        }
        public class BattleUnitBuf_Butterfly_Emotion_Seal : BattleUnitBuf
        {
            private int addedThisTurn;
            private int deleteThisTurn;
            public override string keywordId => "Butterfly_Seal";
            public BattleUnitBuf_Butterfly_Emotion_Seal() => this.stack = 0;
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.stack -= this.deleteThisTurn;
                if (this.stack > 0)
                    return;
                this.Destroy();
            }
            public override int SpeedDiceBreakedAdder() => this.stack;
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                this.deleteThisTurn = this.addedThisTurn;
                this.addedThisTurn = 0;
            }
            public void Add()
            {
                this.stack += 1;
                this.addedThisTurn += 1;
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/ButterFlyMan_Lock");
            }
        }
    }
}
