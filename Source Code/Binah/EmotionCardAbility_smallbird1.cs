using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_smallbird1 : EmotionCardAbilityBase
    {
        private bool dmged;
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            dmged = false;
        }
        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            base.BeforeTakeDamage(attacker, dmg);
            if (_owner.IsImmuneDmg() || _owner.IsInvincibleHp(null))
                return false;
            if (_owner.faction == Faction.Enemy && dmg < (int)((double)_owner.MaxHp * 0.02))
                return false;
            dmged = true;
            return false;
        }
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            if (dmged)
                _owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_SmallBird_Punish());
            dmged = false;
        }
        public class BattleUnitBuf_Emotion_SmallBird_Punish : BattleUnitBuf
        {
            private GameObject _aura;
            private static int Pow => RandomUtil.Range(1, 3);
            public override string keywordId => "SmallBird_Punishment";
            public override string keywordIconId => "SmallBird_Emotion_Punish";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                _aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("8_B/FX_IllusionCard_8_B_Punising", 1f, _owner.view, _owner.view)?.gameObject;
                SoundEffectPlayer.PlaySound("Creature/SmallBird_StrongAtk");
            }
            public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
            {
                base.OnUseCard(curCard);
                curCard.ApplyDiceStatBonus(DiceMatch.NextDice, new DiceStatBonus()
                {
                    power = Pow
                });
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
            public override void OnDie()
            {
                base.OnDie();
                Destroy();
            }

            public override void Destroy()
            {
                base.Destroy();
                DestroyAura();
            }

            private void DestroyAura()
            {
                if (!(_aura != null))
                    return;
                UnityEngine.Object.Destroy(_aura);
                _aura = (GameObject)null;
            }
        }
    }
}
