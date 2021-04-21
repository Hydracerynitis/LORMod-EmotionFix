using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_redhood3 : EmotionCardAbilityBase
    {
        private int Reduce;
        private int Threshold => (int)((double)this._owner.MaxHp * 0.15);
        private int strcount;
        private Battle.CreatureEffect.CreatureEffect aura;
        private string path = "6/RedHood_Emotion_Aura";
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            Reduce = this._owner.MaxHp - (int)this._owner.hp;
            int reduce = Reduce;
            this._owner.bufListDetail.AddBuf(new Scar(Reduce));
            for (strcount=0 ; reduce > Threshold; reduce -= Threshold)
                strcount += 1;
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/RedHood_Change");
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this._owner.bufListDetail.AddBuf(new Scar(Reduce));
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            this.DestroyAura();
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, strcount, this._owner);
            if (!((UnityEngine.Object)this.aura == (UnityEngine.Object)null))
                return;
            this.aura = this.MakeEffect(this.path, target: this._owner, apply: false);
        }
        public override void OnDie(BattleUnitModel killer)
        {
            base.OnDie(killer);
            this.DestroyAura();
        }
        public void Destroy()
        {
            this.DestroyAura();
            BattleUnitBuf buff = this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Scar));
            if (buff != null)
                buff.Destroy();
        }
        private void DestroyAura()
        {
            if ((UnityEngine.Object)this.aura != (UnityEngine.Object)null && (UnityEngine.Object)this.aura.gameObject != (UnityEngine.Object)null)
                UnityEngine.Object.Destroy((UnityEngine.Object)this.aura.gameObject);
            this.aura = (Battle.CreatureEffect.CreatureEffect)null;
        }
        public class Scar : BattleUnitBuf
        {
            private int Reduce;
            public Scar(int Reduce)
            {
                this.Reduce = Reduce;
            }
            public override StatBonus GetStatBonus()
            {
                return new StatBonus() 
                { 
                    hpAdder=-Reduce
                };
            }
        }
    }
}
