using System;
using LOR_DiceSystem;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class DiceCardSelfAbility_BigBirdParalysis : DiceCardSelfAbilityBase
    {
        public override void OnSucceedAreaAttack(BattleUnitModel target)
        {
            base.OnSucceedAreaAttack(target);
            if (target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BigBirdParalysis)) == null)
                target.bufListDetail.AddBuf(new BigBirdParalysis());
        }
        public class BigBirdParalysis: BattleUnitBuf
        {
            protected override string keywordIconId => "BigBird_Charm";
            protected override string keywordId => "BigBirdParalysis";
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    ignorePower = true,
                    max = -5
                }) ;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }
    }
}
