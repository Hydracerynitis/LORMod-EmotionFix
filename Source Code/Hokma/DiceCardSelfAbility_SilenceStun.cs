using System;
using LOR_DiceSystem;
using System.IO;
using Sound;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class DiceCardSelfAbility_SilenceStun : DiceCardSelfAbilityBase
    {
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/ThePriceOfSilence_Filter", false, 2f);
            SoundEffectPlayer.PlaySound("Creature/Clock_StopCard");
            Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/9_H/FX_IllusionCard_9_H_Silence");
            if (original != null)
            {
                Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original);
                creatureEffect.gameObject.transform.SetParent(SingletonBehavior<BattleManagerUI>.Instance.EffectLayer);
                creatureEffect.gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                creatureEffect.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            base.OnUseInstance(unit, self, targetUnit);
            unit.bufListDetail.AddBuf(new EmotionCardAbility_silence3.SilenceStun());
            unit.view.speedDiceSetterUI.DeselectAll();
            unit.OnRoundStartOnlyUI();
            unit.RollSpeedDice();
            targetUnit.bufListDetail.AddBuf(new EmotionCardAbility_silence3.SilenceStun());
            targetUnit.view.speedDiceSetterUI.DeselectAll();
            targetUnit.OnRoundStartOnlyUI();
            targetUnit.RollSpeedDice();
        }
    }
}
