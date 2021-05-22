using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_murderer2 : EmotionCardAbilityBase
    {
        public override int GetSpeedDiceAdder() => -1000;
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (this._owner.faction == Faction.Enemy)
            {
                if (!IsAttackDice(behavior.Detail))
                    return;
                int enemy = RandomUtil.Range(1, 2);
                this._owner.battleCardResultLog?.SetEmotionAbility(true, this._emotionCard, 0, ResultOption.Sign, enemy);
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = enemy
                });
            }
            if (this._owner.faction == Faction.Player)
            {
                if (behavior.Detail != BehaviourDetail.Hit)
                    return;
                int num = RandomUtil.Range(2, 3);
                this._owner.battleCardResultLog?.SetEmotionAbility(true, this._emotionCard, 0, ResultOption.Sign, num);
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = num
                });
            }
        }
        public override void OnSelectEmotion()
        {
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Abandoned_Breathe")?.SetGlobalPosition(this._owner.view.WorldPosition);
            this._owner.view.charAppearance.SetTemporaryGift("Gift_AbandonedMurder", GiftPosition.Mouth);
        }
    }
}
