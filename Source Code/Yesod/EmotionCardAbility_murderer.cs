using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_murderer : EmotionCardAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            BattleUnitModel target = behavior.card?.target;
            if (target == null)
                return;
            if (this._owner.faction == Faction.Player)
            {
                if (behavior.Detail != BehaviourDetail.Hit)
                    return;
                BattleUnitBuf Debuff = target.bufListDetail.GetActivatedBufList().Find((x => x is HitDebuff));
                if (Debuff == null)
                    target.bufListDetail.AddBuf(new HitDebuff());
                else
                    Debuff.stack += 1;
            }
            if (this._owner.faction == Faction.Enemy)
            {
                BattleUnitBuf Debuff = target.bufListDetail.GetActivatedBufList().Find((x => x is HitDebuffEnemy));
                if (Debuff == null)
                    target.bufListDetail.AddBuf(new HitDebuffEnemy());
                else
                    Debuff.stack += 1;
            }
            target.battleCardResultLog?.SetCreatureAbilityEffect("2/AbandonedMurder_Hit", 1f);
            target.battleCardResultLog?.SetCreatureEffectSound("Creature/Abandoned_Atk");
        }
        public void Destroy()
        {
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList())
            {
                if (unit.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is HitDebuff)) is HitDebuff Hit)
                    Hit.Destroy();
                if (unit.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is HitDebuffEnemy)) is HitDebuffEnemy HitEnemy)
                    HitEnemy.Destroy();
            }
        }
        public class HitDebuff: BattleUnitBuf
        {
            protected override string keywordIconId => "Weak";
            protected override string keywordId => "HitDebuff";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                this.stack = 1;
            }
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                if (!IsAttackDice(behavior.Detail))
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    face = -1,
                    dmgRate = -50,
                    breakRate = -50
                });
                this.stack -= 1;
                if (stack <= 0)
                    this.Destroy();
            }
        }
        public class HitDebuffEnemy : BattleUnitBuf
        {
            protected override string keywordIconId => "Weak";
            protected override string keywordId => "HitDebuff_Enemy";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                this.stack = 1;
            }
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                if (!IsAttackDice(behavior.Detail))
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    face = -1,
                    dmgRate = -25,
                    breakRate = -25
                });
                this.stack -= 1;
                if (stack <= 0)
                    this.Destroy();
            }
        }
    }
}
