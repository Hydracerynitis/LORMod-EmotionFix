using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_danggocreature1 : EmotionCardAbilityBase
    {
        private Vector3 effectPos = Vector3.zero;
        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            if (unit == this._owner)
                return;
            int Hp = (int)((double)this._owner.MaxHp * 0.2);
            if (Hp >= 30)
                Hp = 30;
            this._owner.RecoverHP(Hp);
            int Break= (int)((double)this._owner.breakDetail.GetDefaultBreakGauge() * 0.2);
            if (Break >= 30)
                Break = 30;
            this._owner.breakDetail.RecoverBreak(Break);
            if (this._owner.IsBreakLifeZero())
            {
                this._owner.breakDetail.nextTurnBreak = false;
                this._owner.RecoverBreakLife(1);
            }
            this._owner.cardSlotDetail.RecoverPlayPoint(1);
        }
        public override void OnKill(BattleUnitModel target)
        {
            base.OnKill(target);
            if (target.faction == this._owner.faction)
                return;
            target.battleCardResultLog?.SetNewCreatureAbilityEffect("6_G/FX_IllusionCard_6_G_Meet", 2f);
            this._owner.battleCardResultLog?.SetAfterActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.KillEffect));
        }
        public void KillEffect()
        {
            CameraFilterUtil.EarthQuake(0.18f, 0.16f, 90f, 0.45f);
            Battle.CreatureEffect.CreatureEffect original1 = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/6/Dango_Emotion_Effect");
            if ((UnityEngine.Object)original1 != (UnityEngine.Object)null)
            {
                Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original1, SingletonBehavior<BattleSceneRoot>.Instance.transform);
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
            Battle.CreatureEffect.CreatureEffect original2 = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/7/Lumberjack_final_blood_1st");
            if (!((UnityEngine.Object)original2 != (UnityEngine.Object)null))
                return;
            Battle.CreatureEffect.CreatureEffect creatureEffect1 = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original2, SingletonBehavior<BattleSceneRoot>.Instance.transform);
            if (!((UnityEngine.Object)creatureEffect1?.gameObject.GetComponent<AutoDestruct>() == (UnityEngine.Object)null))
                return;
            AutoDestruct autoDestruct1 = creatureEffect1?.gameObject.AddComponent<AutoDestruct>();
            if (!((UnityEngine.Object)autoDestruct1 != (UnityEngine.Object)null))
                return;
            autoDestruct1.time = 3f;
            autoDestruct1.DestroyWhenDisable();
        }
    }
}
