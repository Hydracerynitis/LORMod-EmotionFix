using System;
using LOR_DiceSystem;
using UI;
using Sound;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_bloodbath : EmotionCardAbilityBase
    {
        private Battle.CreatureEffect.CreatureEffect effect;
        private static int Pow => RandomUtil.Range(1, 3);
        private static int BrDmg => RandomUtil.Range(3, 5);
        public override void OnSelectEmotion()
        {
            this.effect = this.MakeEffect("0/BloodyBath_Blood");
            if ((UnityEngine.Object)this.effect != (UnityEngine.Object)null)
                this.effect.transform.SetParent(this._owner.view.characterRotationCenter.transform.parent);
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/BloodBath_Water")?.SetGlobalPosition(this._owner.view.WorldPosition);
        }
        public override int GetBreakDamageReduction(BattleDiceBehavior behavior)
        {
            int brDmg = BrDmg;
            this._owner.battleCardResultLog?.SetEmotionAbility(true, this._emotionCard, 0, ResultOption.Sign, brDmg);
            return -brDmg;
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (!this.IsDefenseDice(behavior.Detail))
                return;
            int pow = Pow;
            this._owner.battleCardResultLog?.SetEmotionAbility(false, this._emotionCard, 1, ResultOption.Default);
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                power = pow
            });
        }
        public override void OnLayerChanged(string layerName)
        {
            if (layerName == "Character")
                layerName = "CharacterUI";
            this.effect?.SetLayer(layerName);
        }
    }
}
