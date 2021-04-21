using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_singingMachine3 : EmotionCardAbilityBase
    {
        private static int Add => RandomUtil.Range(1, 3);
        private static int Minus => RandomUtil.Range(1, 3);
        public override void OnSelectEmotionOnce()
        {
            base.OnSelectEmotionOnce();
            Util.LoadPrefab("Battle/CreatureCard/SingingMachineCard_note_filter", SingletonBehavior<BattleSceneRoot>.Instance.transform);
            SoundEffectPlayer.PlaySound("Creature/SingingMachine_Open");
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (this.IsAttackDice(behavior.Detail))
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = Add
                });
            }
            else
            {
                if (!this.IsDefenseDice(behavior.Detail))
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = -Minus
                });
            }
        }
    }
}
