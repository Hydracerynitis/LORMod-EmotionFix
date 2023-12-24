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
    public class EmotionCardAbility_bigbadwolf1 : EmotionCardAbilityBase
    {
        private BattleDiceBehavior last;
        private int win;
        private static int Pow => RandomUtil.Range(1, 2);
        private static int Heal => RandomUtil.Range(3, 7);
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/Wolf_Filter_Sheep", false, 2f);
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            win = 0;
            if (curCard == null)
                return;
            BattleDiceBehavior[] array = curCard.cardBehaviorQueue?.ToArray();
            if (array == null || array.Length == 0)
                return;
            last = array[array.Length - 1];
        }
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            BattlePlayingCardDataInUnitModel card = behavior?.card;
            if (card == null || behavior == last)
                return;
            win++;
            card.ApplyDiceStatBonus(DiceMatch.LastDice, new DiceStatBonus()
            {
                power = Pow
            });
        }

        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            if (behavior != last)
                return;
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Wolf_Bite");
            for(int i = 0; i < win; i++)
            {
                _owner.RecoverHP(Heal);
            }
        }
    }
}
