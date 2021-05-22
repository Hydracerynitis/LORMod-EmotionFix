using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_kingofgreed1 : EmotionCardAbilityBase
    {
        private bool Trigger;
        private static bool Prob => (double)RandomUtil.valueForProb < 0.5;
        public override void OnParryingStart(BattlePlayingCardDataInUnitModel card)
        {
            base.OnParryingStart(card);
            Trigger = false;
        }
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (Trigger)
                return;
            Trigger = true;
            if (target == null || behavior == null || (!Prob && SearchEmotion(this._owner, "Greed_Eat")==null))
                return;
            target.currentDiceAction.ApplyDiceStatBonus(DiceMatch.AllDice,new DiceStatBonus() 
            { 
                power =-2
            });
            this._owner.battleCardResultLog?.SetAfterActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.Filter));
        }
        public override void OnDrawParrying(BattleDiceBehavior behavior)
        {
            base.OnDrawParrying(behavior);
            if (Trigger)
                return;
            Trigger = true;
        }
        public override void OnLoseParrying(BattleDiceBehavior behavior)
        {
            base.OnLoseParrying(behavior);
            if (Trigger)
                return;
            Trigger = true;
            if (behavior == null || (!Prob && SearchEmotion(this._owner, "Greed_Eat") == null))
                return;
            behavior.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
            {
                power = -2
            });
            this._owner.battleCardResultLog?.SetAfterActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.Filter));

        }
        private BattleEmotionCardModel SearchEmotion(BattleUnitModel owner, string Name)
        {
            List<BattleEmotionCardModel> emotion = owner.emotionDetail.PassiveList;
            foreach (BattleEmotionCardModel card in emotion)
            {
                if (card.XmlInfo.Name == Name)
                    return card;
            }
            return null;
        }
        public void Filter() => new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/KingOfGreed_Yellow", false);
    }
}
