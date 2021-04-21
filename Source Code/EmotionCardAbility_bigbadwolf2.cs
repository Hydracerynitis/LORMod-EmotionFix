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
    public class EmotionCardAbility_bigbadwolf2 : EmotionCardAbilityBase
    {
        private int _accumulatedDmg;
        private bool trigger;
        private Battle.CreatureEffect.CreatureEffect aura;
        private string path = "6/BigBadWolf_Emotion_Aura";
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this._accumulatedDmg = 0;
            if ((UnityEngine.Object)this.aura == (UnityEngine.Object)null)
                return;
            this.DestroyAura();
        }
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            this._accumulatedDmg = 0;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (!this.trigger)
                return;
            this.trigger = false;
            this._accumulatedDmg = 0;
            this._owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_Wolf_Claw());
            if (!((UnityEngine.Object)this.aura == (UnityEngine.Object)null))
                return;
            this.aura = this.MakeEffect(this.path, target: this._owner, apply: false);
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            this.DestroyAura();
            _accumulatedDmg += this._owner.history.takeDamageAtOneRound;
            if ((double)this._accumulatedDmg >= (double)this._owner.MaxHp * 0.25 && this._owner.faction==Faction.Player)
                this.trigger = true;
            if (this._owner.faction == Faction.Enemy)
            {
                if ((double)this._accumulatedDmg >= (double)this._owner.MaxHp * 0.5)
                    this.trigger = true;
                this._accumulatedDmg = 0;
            }
        }
        public override void OnDie(BattleUnitModel killer)
        {
            base.OnDie(killer);
            this.DestroyAura();
        }
        private void DestroyAura()
        {
            if ((UnityEngine.Object)this.aura != (UnityEngine.Object)null && (UnityEngine.Object)this.aura.gameObject != (UnityEngine.Object)null)
                UnityEngine.Object.Destroy((UnityEngine.Object)this.aura.gameObject);
            this.aura = (Battle.CreatureEffect.CreatureEffect)null;
        }
        public class BattleUnitBuf_Emotion_Wolf_Claw : BattleUnitBuf
        {
            protected override string keywordId => "Wolf_Claw";
            private static int Str => RandomUtil.Range(2, 2);
            private static int Bleed => RandomUtil.Range(1, 1);
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, EmotionCardAbility_bigbadwolf2.BattleUnitBuf_Emotion_Wolf_Claw.Str, owner);
                SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Wolf_FogChange");
            }
            public override void OnSuccessAttack(BattleDiceBehavior behavior)
            {
                base.OnSuccessAttack(behavior);
                behavior?.card?.target?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, EmotionCardAbility_bigbadwolf2.BattleUnitBuf_Emotion_Wolf_Claw.Bleed, this._owner);
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
            public override bool IsTargetable() => false;
            public override bool DirectAttack() => true;
        }
    }
}
