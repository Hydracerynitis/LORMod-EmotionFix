using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_blackswan3 : EmotionCardAbilityBase
    {
        private List<BattleDiceCardModel> Nettle = new List<BattleDiceCardModel>();
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            Nettle.Add(this._owner.allyCardDetail.AddNewCardToDeck(1103502));
            Nettle.Add(this._owner.allyCardDetail.AddNewCardToDeck(1103502));
            Nettle.Add(this._owner.allyCardDetail.AddNewCardToDeck(1103502));
            new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/BlackSwan_Filter_Nettle", false, 2f);
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this._owner.allyCardDetail.AddCardToDeck(Nettle);
            this._owner.allyCardDetail.Shuffle();
            new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/BlackSwan_Filter_Nettle", false, 2f);
        }
        public void Destroy()
        {
            foreach (BattleDiceCardModel card in Nettle)
                this._owner.allyCardDetail.ExhaustACardAnywhere(card);
            foreach(BattleUnitModel ally in BattleObjectManager.instance.GetAliveList(this._owner.faction))
            {
                if (ally.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is DiceCardSelfAbility_Clothes.Nettle)) is DiceCardSelfAbility_Clothes.Nettle cloths)
                    cloths.Destroy();
            }
        }
    }
}
