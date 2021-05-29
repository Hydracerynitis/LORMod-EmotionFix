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
    public class EmotionCardAbility_orchestra3 : EmotionCardAbilityBase
    {
        private bool trigger;
        private static int Reduce => RandomUtil.Range(3, 4);
        public override void OnMakeBreakState(BattleUnitModel target)
        {
            base.OnMakeBreakState(target);
            this.trigger = true;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (!this.trigger)
                return;
            this.trigger = false;
            Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/4_N/FX_IllusionCard_4_N_Orchestra_Light");
            if ((UnityEngine.Object)original != (UnityEngine.Object)null)
            {
                Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                if ((UnityEngine.Object)creatureEffect?.gameObject.GetComponent<AutoDestruct>() == (UnityEngine.Object)null)
                {
                    AutoDestruct autoDestruct = creatureEffect?.gameObject.AddComponent<AutoDestruct>();
                    if ((UnityEngine.Object)autoDestruct != (UnityEngine.Object)null)
                    {
                        autoDestruct.time = 3f;
                        autoDestruct.DestroyWhenDisable();
                    }
                }
            }
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
                alive.cardSlotDetail.LosePlayPoint(Reduce);
            SoundEffectPlayer.PlaySound("Creature/Sym_movment_5_finale");
        }
    }
}
