using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_teddy: EmotionCardAbilityBase
    {
        private BattleUnitModel _lastTarget;
        private int _parryingCount;
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            if (RandomUtil.valueForProb >= 0.2 + (double)this._parryingCount * 0.1 && this._owner.faction==Faction.Player)
                return;
            if (RandomUtil.valueForProb >= 0.4 && this._owner.faction == Faction.Enemy)
                return;
            BattleUnitModel target = behavior?.card?.target;
            if (target == null)
                return;
            int diceResultValue = behavior.DiceResultValue;
            this._owner.battleCardResultLog?.SetEmotionAbility(true, this._emotionCard, 0, ResultOption.Default, diceResultValue);
            target.TakeBreakDamage(diceResultValue,DamageType.Emotion ,this._owner);
            target.battleCardResultLog?.SetCreatureEffectSound("Creature/Teddy_Atk");
            target.battleCardResultLog?.SetCreatureAbilityEffect("1/HappyTeddy_Hug");
        }
        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            if (!behavior.IsParrying())
                return;
            if (behavior.card.target == this._lastTarget)
            {
                ++this._parryingCount;
            }
            else
            {
                this._parryingCount = 0;
                this._lastTarget = behavior.card?.target;
            }
        }
        public override void OnSelectEmotion()
        {
            this._owner.bufListDetail.AddBuf(new BattleUnitBuf_teddy_hug());
        }
        public class BattleUnitBuf_teddy_hug : BattleUnitBuf
        {
            protected override string keywordId => "Teddy_Head";
            public override BufPositiveType positiveType => BufPositiveType.Positive;
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                this.stack = 0;
            }
            public override void OnDie() => this.Destroy();
        }
    }
}
