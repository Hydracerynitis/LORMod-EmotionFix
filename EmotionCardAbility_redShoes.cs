using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_redShoes : EmotionCardAbilityBase
    {
        public override void OnRoundStart()
        {
            if (!(this._owner.bufListDetail.GetActivatedBufList().Exists((Predicate<BattleUnitBuf>)(x => x is ShinyShoes))))
            {
                this._owner.bufListDetail.AddBuf(new ShinyShoes());
                SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/RedShoes_On")?.SetGlobalPosition(this._owner.view.WorldPosition);
            }
        }
        public override void OnSelectEmotion()
        {
            this._owner.bufListDetail.AddBuf(new ShinyShoes());
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/RedShoes_On")?.SetGlobalPosition(this._owner.view.WorldPosition);
        }
        public void Destroy()
        {
            if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is ShinyShoes)) is ShinyShoes shoe)
                shoe.Destroy();
        }
        public class ShinyShoes: BattleUnitBuf
        {
            protected override string keywordId => "ShinyShoes";
            protected override string keywordIconId => "CopiousBleeding";

            public override int GetDamageIncreaseRate() => 50;
            public override int GetBreakDamageIncreaseRate() => 50;
            public override int GetSpeedDiceAdder(int speedDiceResult)
            {
                return RandomUtil.Range(1,2);
            }
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                if (!IsAttackDice(behavior.Detail))
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = RandomUtil.Range(1, 3)
                });
            }
            public override void OnDie()
            {
                BattleUnitModel killer= this._owner.lastAttacker;
                if (killer == null)
                {
                    killer = RandomUtil.SelectOne<BattleUnitModel>(BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player));
                }
                killer.bufListDetail.AddBuf(new ShinyShoes());
            }
        }
    }
}
