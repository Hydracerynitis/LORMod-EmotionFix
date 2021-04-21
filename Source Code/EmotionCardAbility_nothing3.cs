using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_nothing3 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            this._owner.bufListDetail.AddBuf(new Shell());
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this._owner.bufListDetail.AddBuf(new Shell());
        }
        public void Destroy()
        {
            if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Shell)) is Shell Shell)
                Shell.Destroy();
        }
        public class Shell: BattleUnitBuf
        {
            protected override string keywordId => "Shell";
            protected override string keywordIconId => "Nothing_Skin";
            public override bool IsInvincibleBp(BattleUnitModel attacker) => true;
        }
    }
}

