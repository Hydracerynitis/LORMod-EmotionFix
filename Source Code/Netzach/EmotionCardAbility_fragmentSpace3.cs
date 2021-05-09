using System;
using LOR_DiceSystem;
using Battle.CameraFilter;
using System.Collections.Generic;
using Sound;
using UnityEngine;
using System.Collections;

namespace EmotionalFix
{
    public class EmotionCardAbility_fragmentSpace3 : EmotionCardAbilityBase
    {
        private int cnt;
        private bool Prob() => (double)RandomUtil.valueForProb <= 0.5;

        public override void OnRoundStart()
        {
            base.OnRoundStart();
            this.cnt = 0;
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || behavior == null)
                return;
            if (this.Prob() && this.cnt < 3)
            {
                ++this.cnt;
                target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Weak, 1, this._owner);
                target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Disarm, 1, this._owner);
                this._owner.battleCardResultLog.SetEndCardActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.Effect));
            }
        }
        private void Effect()
        {
            try
            {
                SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Cosmos_Sing")?.SetGlobalPosition(this._owner.view.WorldPosition);
                SingletonBehavior<BattleCamManager>.Instance?.AddCameraFilter<CameraFilterCustom_universe>(true);
            }
            catch 
            {

            }
        }
    }
}
