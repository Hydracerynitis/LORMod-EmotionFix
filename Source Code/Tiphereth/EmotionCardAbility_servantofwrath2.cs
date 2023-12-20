using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_servantofwrath2 : EmotionCardAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || target.faction==this._owner.faction)
                return;
            if(this.GetAliveFriend() == null)
                target.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_Wrath_Friend());
            else
            {
                if (this.GetAliveFriend() != target)
                    return;
                target.battleCardResultLog?.SetNewCreatureAbilityEffect("5_T/FX_IllusionCard_5_T_ATKMarker", 1.5f);
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Angry_R_StrongAtk");
            }
        }
        public override void OnKill(BattleUnitModel target)
        {
            base.OnKill(target);
            if (target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(y => y is BattleUnitBuf_Emotion_Wrath_Friend)) == null)
                return;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Player))
            {
                alive.cardSlotDetail.RecoverPlayPoint(3);
                alive.breakDetail.RecoverBreak(10);
                alive.RecoverHP(10);
            }
            this._owner.battleCardResultLog?.SetAfterActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.KillEffect));
            this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Angry_Vert2");
        }
        public void KillEffect()
        {
            CameraFilterUtil.EarthQuake(0.08f, 0.02f, 50f, 0.6f);
            Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/5_T/FX_IllusionCard_5_T_SmokeWater");
            if (!((UnityEngine.Object)original != (UnityEngine.Object)null))
                return;
            Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
            if (!((UnityEngine.Object)creatureEffect?.gameObject.GetComponent<AutoDestruct>() == (UnityEngine.Object)null))
                return;
            AutoDestruct autoDestruct = creatureEffect?.gameObject.AddComponent<AutoDestruct>();
            if (!((UnityEngine.Object)autoDestruct != (UnityEngine.Object)null))
                return;
            autoDestruct.time = 3f;
            autoDestruct.DestroyWhenDisable();
        }
        public void Destroy()
        {
            if (GetAliveFriend().bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_Wrath_Friend)) is BattleUnitBuf_Emotion_Wrath_Friend friend)
                friend.Destroy();
        }
        private BattleUnitModel GetAliveFriend() => BattleObjectManager.instance.GetAliveList(this._owner.faction==Faction.Player?Faction.Enemy:Faction.Player).Find((Predicate<BattleUnitModel>)(x => x.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(y => y is BattleUnitBuf_Emotion_Wrath_Friend)) != null));
        public class BattleUnitBuf_Emotion_Wrath_Friend : BattleUnitBuf
        {
            public override string keywordId => "Angry_Friend";

            public override string keywordIconId => "Reclus_Head";

            public BattleUnitBuf_Emotion_Wrath_Friend() => this.stack = 0;
        }
    }
}
