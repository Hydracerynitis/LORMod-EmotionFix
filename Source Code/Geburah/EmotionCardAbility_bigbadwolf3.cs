using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_bigbadwolf3 : EmotionCardAbilityBase
    {
        private int cnt;
        private BattleUnitModel target;
        private static int Vuln => RandomUtil.Range(2, 2);
        private static int Pow => RandomUtil.Range(1, 2);

        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/Wolf_Filter_Eye", false, 2f);
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (_owner.faction != Faction.Player)
                return;
            base.BeforeRollDice(behavior);
            target = null;
            if (behavior.Type != BehaviourType.Standby)
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                power = Pow
            });
            target = behavior.card?.target;
        }
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            if (_owner.faction != Faction.Player || cnt>=3)
                return;
            base.OnWinParrying(behavior);
            if (behavior.Type != BehaviourType.Standby || target == null)
                return;
            target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Vulnerable, Vuln, _owner);
            _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Wolf_Scratch");
            target = null;
            cnt++;
        }
        public override void OnStartBattle()
        {
            cnt = 0;
            if (_owner.faction != Faction.Enemy)
                return;
            DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(1106201);
            List<BattleDiceBehavior> behaviourList = new List<BattleDiceBehavior>();
            int num = 0;
            foreach (DiceBehaviour diceBehaviour2 in cardItem.DiceBehaviourList)
            {
                BattleDiceBehavior battleDiceBehavior = new BattleDiceBehavior
                {
                    behaviourInCard = diceBehaviour2
                };
                battleDiceBehavior.SetIndex(num++);
                behaviourList.Add(battleDiceBehavior);
            }
            _owner.cardSlotDetail.keepCard.AddBehaviours(BattleDiceCardModel.CreatePlayingCard(cardItem), behaviourList);
        }
    }
}
