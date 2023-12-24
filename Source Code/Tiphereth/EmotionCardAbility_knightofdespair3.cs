using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_knightofdespair3 : EmotionCardAbilityBase
    {
        public override void OnRoundStart()
        {
            if (!(_owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Tear)) is Tear tear))
            {
                BattleUnitBuf Tear = new Tear
                {
                    stack = 1
                };
                _owner.bufListDetail.AddBuf(Tear);
            }
            else
            {
                tear.stack += 1;
            }
        }
        public class Tear : BattleUnitBuf
        {
            //private KnightOfDespairGroggyFilter _swordFilter = 
            public override string keywordIconId => "KnightOfDespair_Blessing";
            public override string keywordId => "Tear";
            public int reserve;
            public int reservePlus;
            private bool HasAttack;
            public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
            {
                if (card.isKeepedCard)
                {
                    HasAttack = true;
                    return;
                }
                foreach(BattleDiceBehavior battleDice in card.cardBehaviorQueue)
                {
                    if (IsAttackDice(battleDice.Detail))
                    {
                        HasAttack = false;
                        card.ApplyDiceAbility(DiceMatch.NextAttackDice, new DiceCardAbility_Tear());
                        return;
                    }
                }
                HasAttack = true;
            }
            public override void OnSuccessAttack(BattleDiceBehavior behavior)
            {
                HasAttack = true;
            }
            public override void OnEndParrying()
            {
                if (HasAttack)
                    return;
                _owner.TakeDamage((int)((double)_owner.MaxHp * 0.1));
                _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/KnightOfDespair_Change");
                _owner.battleCardResultLog.SetAttackEffectFilter(typeof(ImageFilter_ColorBlend_Despair));
                //new GameObject().AddComponent<SpriteFilter_Despair>().Init("EmotionCardFilter/Wolf_Filter_Eye", false, 1f);
                //_swordFilter.gameObject.SetActive(true);
                //_swordFilter.MoveSwordSprite();
                //_swordFilter.ResetSwordPosition();
                //_swordFilter.gameObject.SetActive(false);
            }
            public override bool IsInvincibleBp(BattleUnitModel attacker) => stack>=2;
            public override void OnRoundEndTheLast()
            {
                base.OnRoundEnd();
                stack = 0;
                stack += reserve;
                reserve = 0;
                stack += reservePlus;
                reserve += reservePlus;
                reservePlus = 0;
                if (stack <= 0)
                    Destroy();
            }
        }
    }
}
