using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_longbird1 : EmotionCardAbilityBase
    {
        private BattleUnitModel target;
        private bool Trigger;
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            if (behavior.card.target == null || behavior.card.target != target || !Trigger)
                return;
            behavior.card.target.TakeDamage((int)(behavior.card.target.hp * 0.15));
            Trigger = false;
            target.battleCardResultLog?.SetNewCreatureAbilityEffect("8_B/FX_IllusionCard_8_B_Scale", 2f);
            target.battleCardResultLog?.SetCreatureEffectSound("Creature/LongBird_On");
        }
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            Trigger = true;
            int num = 0;
            List<BattleUnitModel> sinner = new List<BattleUnitModel>();
            foreach(BattleUnitModel enemy in BattleObjectManager.instance.GetAliveList(this._owner.faction==Faction.Player?Faction.Enemy: Faction.Player))
            {
                if (enemy.history.damageToEnemyAtRound > num)
                {
                    num = enemy.history.damageToEnemyAtRound;
                    sinner.Clear();
                    sinner.Add(enemy);
                }
                if (enemy.history.damageToEnemyAtRound == num)
                    sinner.Add(enemy);
            }
            BattleUnitModel Sinner = RandomUtil.SelectOne<BattleUnitModel>(sinner);
            target = Sinner;
            target.bufListDetail.AddBuf(new Sinner());
        }
        public class Sinner : BattleUnitBuf
        {
            protected override string keywordId => "Sinner";
            protected override string keywordIconId => "Sin_Abnormality";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                this.stack = 0;
            }
            public override void OnRoundEnd()
            {
                this.Destroy();
            }
        }
    }
}
