using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;
using System.IO;

namespace EmotionalFix
{
    public class EmotionCardAbility_singingMachine : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            if (this._owner.faction != Faction.Enemy)
                return;
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Singing_Atk");
            Util.LoadPrefab("Battle/CreatureCard/SingingMachineCard_play_particle", SingletonBehavior<BattleSceneRoot>.Instance.transform);
        }
        public override void OnSelectEmotionOnce()
        {
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Singing_Atk");
            Util.LoadPrefab("Battle/CreatureCard/SingingMachineCard_play_particle", SingletonBehavior<BattleSceneRoot>.Instance.transform);
        }
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            int num = RandomUtil.Range(3, 5);
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                dmg = num
            });
        }
        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            this._owner.breakDetail.TakeBreakDamage(RandomUtil.Range(3, 5));
        }
    }
}
