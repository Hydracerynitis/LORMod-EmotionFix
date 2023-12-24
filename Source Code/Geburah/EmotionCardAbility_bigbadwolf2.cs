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
            _accumulatedDmg = 0;
            if (aura == null)
                return;
            DestroyAura();
        }
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            _accumulatedDmg = 0;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (!trigger)
                return;
            trigger = false;
            _accumulatedDmg = 0;
            _owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_Wolf_Claw());
            if (!(aura == null))
                return;
            aura = MakeEffect(path, target: _owner, apply: false);
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            DestroyAura();
            _accumulatedDmg += _owner.history.takeDamageAtOneRound;
            if ((double)_accumulatedDmg >= (double)_owner.MaxHp * 0.25 && _owner.faction==Faction.Player)
                trigger = true;
            if (_owner.faction == Faction.Enemy)
            {
                if ((double)_accumulatedDmg >= (double)_owner.MaxHp * 0.5)
                    trigger = true;
                _accumulatedDmg = 0;
            }
        }
        public override void OnDie(BattleUnitModel killer)
        {
            base.OnDie(killer);
            DestroyAura();
        }
        private void DestroyAura()
        {
            if (aura != null && aura.gameObject != null)
                UnityEngine.Object.Destroy(aura.gameObject);
            aura = (Battle.CreatureEffect.CreatureEffect)null;
        }
        public class BattleUnitBuf_Emotion_Wolf_Claw : BattleUnitBuf
        {
            public override string keywordId => "Wolf_Claw";
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
                behavior?.card?.target?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, EmotionCardAbility_bigbadwolf2.BattleUnitBuf_Emotion_Wolf_Claw.Bleed, _owner);
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
            public override bool IsTargetable() => false;
            public override bool DirectAttack() => true;
        }
    }
}
