using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_freischutz3 : EmotionCardAbilityBase
    {
        private int Flames;
        private Battle.CreatureEffect.CreatureEffect aura;
        private string path = "2/Freischutz_Emotion_Aura";
        public override void OnSelectEmotion()
        {
            Flames = 0;
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            if (Flames != 0)
            {
                Flame flame = new Flame();
                this._owner.bufListDetail.AddBuf(flame);
                flame.stack += Flames;
                this._owner.breakDetail.UpdateBreakMax();
            }
            if (!((UnityEngine.Object)this.aura != (UnityEngine.Object)null))
                return;
            this.DestroyAura();
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (!((UnityEngine.Object)this.aura == (UnityEngine.Object)null) || Flames <= 0)
                return;
            this.aura = this.MakeEffect(this.path, target: this._owner, apply: false);
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Matan_Flame")?.SetGlobalPosition(this._owner.view.WorldPosition);
        }

        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            if (this._owner.IsBreakLifeZero())
                Flames = 0;
            if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Flame)) is Flame Flame)
                Flame.Destroy();
            if (Flames == 0)
                return;
            Flame flame = new Flame();
            this._owner.bufListDetail.AddBuf(flame);
            flame.stack += Flames;
            this._owner.breakDetail.UpdateBreakMax();
        }
        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            Flames += 1;
        }
        public override void OnDie(BattleUnitModel killer)
        {
            base.OnDie(killer);
            this.DestroyAura();
        }
        public void Destroy()
        {
            if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Flame)) is Flame Flame)
                Flame.Destroy();
        }
        private void DestroyAura()
        {
            if ((UnityEngine.Object)this.aura != (UnityEngine.Object)null && (UnityEngine.Object)this.aura.gameObject != (UnityEngine.Object)null)
                UnityEngine.Object.Destroy((UnityEngine.Object)this.aura.gameObject);
            this.aura = (Battle.CreatureEffect.CreatureEffect)null;
        }
        public class Flame: BattleUnitBuf
        {
            protected override string keywordId => "Flame";
            protected override string keywordIconId => "BurnSpread";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                this.stack = 0;
            }
            public override StatBonus GetStatBonus()
            {
                return new StatBonus()
                {
                    breakRate = -Mathf.Min(99, this.stack * 12)
                };
            }
            public override void OnRoundStart()
            {
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, stack);
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, stack);
            }
        }
    }
}
