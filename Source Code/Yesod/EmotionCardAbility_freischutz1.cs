using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;
using BaseMod;

namespace EmotionalFix
{
    public class EmotionCardAbility_freischutz1 : EmotionCardAbilityBase
    {
        private int count;
        private bool trigger;
        private BattleDiceCardModel request;
        public override void OnWaveStart()
        {
            request=this._owner.allyCardDetail.AddNewCard(1100005);
        }
        public override void OnSelectEmotion()
        {
            request=this._owner.allyCardDetail.AddNewCard(1100005);
        }
        public void Destroy()
        {
            this._owner.allyCardDetail.ExhaustACardAnywhere(request);
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            if (curCard.card == request)
                trigger = true;
            else
                trigger = false;
        }
        public override void OnKill(BattleUnitModel target)
        {
            base.OnKill(target);
            if (target.faction==this._owner.faction || !trigger)
                return;
            count++;
            Debug.Log("Yesod Achievement Progress " + count.ToString());
            if(count==3)
                Singleton<StageController>.Instance.UnlockAchievement(AchievementEnum.ONCE_FLOOR2);
        }
    }
}
