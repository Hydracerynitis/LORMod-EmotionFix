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
            if (!(this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Tear)) is Tear tear))
            {
                BattleUnitBuf Tear = new Tear
                {
                    stack = 1
                };
                this._owner.bufListDetail.AddBuf(Tear);
            }
            else
            {
                tear.stack += 1;
            }
        }
        public class Tear : BattleUnitBuf
        {
            //private KnightOfDespairGroggyFilter _swordFilter = 
            protected override string keywordIconId => "KnightOfDespair_Blessing";
            protected override string keywordId => "Tear";
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
                this._owner.TakeDamage((int)((double)this._owner.MaxHp * 0.1));
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/KnightOfDespair_Change");
                this._owner.battleCardResultLog.SetAttackEffectFilter(typeof(ImageFilter_ColorBlend_Despair));
                //new GameObject().AddComponent<SpriteFilter_Despair>().Init("EmotionCardFilter/Wolf_Filter_Eye", false, 1f);
                //this._swordFilter.gameObject.SetActive(true);
                //this._swordFilter.MoveSwordSprite();
                //this._swordFilter.ResetSwordPosition();
                //this._swordFilter.gameObject.SetActive(false);
            }
            public override bool IsInvincibleBp(BattleUnitModel attacker) => this.stack>=2;
            public override void OnRoundEndTheLast()
            {
                base.OnRoundEnd();
                this.stack = 0;
                this.stack += reserve;
                reserve = 0;
                this.stack += reservePlus;
                reserve += reservePlus;
                reservePlus = 0;
                if (stack <= 0)
                    this.Destroy();
            }
        }
    }
}
