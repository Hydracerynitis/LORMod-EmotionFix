using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_alriune3 : EmotionCardAbilityBase
    {
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this._owner.faction==Faction.Player? Faction.Enemy:Faction.Player);
            if (aliveList.Count <= 0)
                return;
            RandomUtil.SelectOne<BattleUnitModel>(aliveList).bufListDetail.AddBuf(new BattleUnitBuf_Emotion_Alriune(this._owner));
        }

        public class BattleUnitBuf_Emotion_Alriune : BattleUnitBuf
        {
            private BattleUnitModel _target;
            private int cnt;
            private static int BDmg => RandomUtil.Range(3, 7);
            protected override string keywordId => "Alriune_Flower";
            protected override string keywordIconId => "Alriune_Petal";
            public BattleUnitBuf_Emotion_Alriune(BattleUnitModel target)
            {
                this._target = target;
                this.stack = 0;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
            public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
            {
                base.OnTakeDamageByAttack(atkDice, dmg);
                BattleUnitModel owner = atkDice?.card?.owner;
                if (owner == null || owner != this._target || this.cnt >= 4)
                    return;
                ++this.cnt;
                if (this.cnt < 4)
                    return;
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
                    alive.TakeBreakDamage(EmotionCardAbility_alriune3.BattleUnitBuf_Emotion_Alriune.BDmg, DamageType.Emotion,this._owner);
                this._target?.bufListDetail.AddBuf((BattleUnitBuf)new EmotionCardAbility_alriune3.BattleUnitBuf_Emotion_Alriune2());
            }
        }
        public class BattleUnitBuf_Emotion_Alriune2 : BattleUnitBuf
        {
            private bool added = true;
            protected override string keywordId => "NoTargeting";
            protected override string keywordIconId => "Alriune_Attacker";
            public override bool IsTargetable() => this.added;
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                if (this.added)
                    this.added = false;
                else
                    this.Destroy();
            }
        }
    }
}
