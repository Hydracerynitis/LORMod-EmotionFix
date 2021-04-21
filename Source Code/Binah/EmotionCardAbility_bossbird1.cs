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
    public class EmotionCardAbility_bossbird1 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            if (this._owner.faction == Faction.Player)
                this._owner.bufListDetail.AddBuf(new LongBird());
            if (this._owner.faction == Faction.Enemy)
                this._owner.bufListDetail.AddBuf(new Longbird_Enemy());
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            if (this._owner.faction == Faction.Player)
                this._owner.bufListDetail.AddBuf(new LongBird());
            if (this._owner.faction == Faction.Enemy)
                this._owner.bufListDetail.AddBuf(new Longbird_Enemy());
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (this._owner.faction != Faction.Player)
                return;
            foreach (BattleUnitModel enemy in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
            {
                if (!(enemy.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EggSin)) is EggSin sin))
                    enemy.bufListDetail.AddBuf(new EggSin());
            }
        }
        public void Destroy()
        {
            BattleUnitBuf Buff= this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is LongBird));
            if(Buff!=null)
                Buff.Destroy();
            Buff = this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Longbird_Enemy));
            if (Buff != null)
                Buff.Destroy();
        }
        public class LongBird : BattleUnitBuf
        {
            protected override string keywordIconId => "ApocalypseBird_LongArm";
            protected override string keywordId => "Longbird";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (!IsAttackDice(behavior.Detail))
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    max = -5
                });
            }
            public override void OnDie()
            {
                base.OnDie();
                this.Destroy();
            }
            public override void Destroy()
            {
                base.Destroy();
                foreach(BattleUnitModel enemy in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
                {
                    if (enemy.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EggSin)) is EggSin sin)
                        sin.Destroy();
                }
            }
            public override void OnRollDice(BattleDiceBehavior behavior)
            {
                base.OnRollDice(behavior);
                BattleUnitBuf sin = behavior.card.target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EggSin));
                if (!IsAttackDice(behavior.Detail) || sin==null)
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = sin.stack
                });
            }
        }
        public class EggSin: BattleUnitBuf
        {
            protected override string keywordIconId => "Sin_Abnormality_Final";
            protected override string keywordId => "EggSin";
            private bool Hit;
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
                Hit = false;
            }
            public override void OnKill(BattleUnitModel target)
            {
                base.OnKill(target);
                if (target.faction == this._owner.faction)
                    return;
                stack += 5;
            }
            public override void OnSuccessAttack(BattleDiceBehavior behavior)
            {
                base.OnSuccessAttack(behavior);
                if (behavior.card.target.faction == this._owner.faction)
                    return;
                if (!Hit)
                    Hit = true;
                stack += 1;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                if (Hit)
                    return;
                stack = (int)stack * 2 / 3;
            }
        }
        public class Longbird_Enemy: BattleUnitBuf
        {
            protected override string keywordIconId => "ApocalypseBird_LongArm";
            protected override string keywordId => "Longbird_Enemy";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
                this._owner.bufListDetail.RemoveBufAll(BufPositiveType.Negative);
            }
            public override bool IsImmune(BattleUnitBuf buf)
            {
                return buf.positiveType == BufPositiveType.Negative || base.IsImmune(buf);
            }
        }
    }
}
