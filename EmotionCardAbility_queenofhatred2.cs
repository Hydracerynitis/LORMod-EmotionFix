using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_queenofhatred2 : EmotionCardAbilityBase
    {
        public BattleUnitModel target;
        public Battle.CreatureEffect.CreatureEffect effect;
        private int max;
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            BattleUnitModel battleUnitModel = (BattleUnitModel)null;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
            {
                int damageToEnemyAtRound = alive.history.damageToEnemyAtRound;
                if (damageToEnemyAtRound > this.max)
                {
                    battleUnitModel = alive;
                    this.max = damageToEnemyAtRound;
                }
            }
            this.target = battleUnitModel;
            this.max = 0;
            if (!((UnityEngine.Object)this.effect != (UnityEngine.Object)null))
                return;
            UnityEngine.Object.Destroy((UnityEngine.Object)this.effect.gameObject);
            this.effect = (Battle.CreatureEffect.CreatureEffect)null;
        }

        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (this.target == null || this.target.IsDead() || this.effect!=null)
                return;
            this.effect=this.MakeEffect("5/MagicalGirl_Villain", target: this.target);
            target.bufListDetail.AddBuf(new Villain());
        }
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            base.BeforeGiveDamage(behavior);
            if (this.target != behavior.card?.target)
                return;
            int num = RandomUtil.Range(3, 5);
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                dmg = num
            });
            this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/MagicalGirl_Gun");
            this._owner.battleCardResultLog.SetAttackEffectFilter(typeof(ImageFilter_ColorBlend_Pink));
        }
        public void Destroy()
        {
            if (!((UnityEngine.Object)this.effect != (UnityEngine.Object)null))
                return;
            UnityEngine.Object.Destroy((UnityEngine.Object)this.effect.gameObject);
            this.effect = (Battle.CreatureEffect.CreatureEffect)null;
        }
        public class Villain: BattleUnitBuf
        {
            public override bool Hide => true;
            public override void OnRoundEndTheLast()
            {
                base.OnRoundEndTheLast();
                this.Destroy();
            }
            public override void OnDie()
            {
                base.OnDie();
                this.Destroy();
            }
        }
    }
}
