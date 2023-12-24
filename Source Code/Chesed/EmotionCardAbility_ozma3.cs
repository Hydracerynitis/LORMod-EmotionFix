using System;
using Sound;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_ozma3 : EmotionCardAbilityBase
    {
        private bool _effect;
        private bool _activated;
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            _effect = false;
        }
        public override void OnRoundStart()
        {
            if (!_effect)
            {
                _effect = true;
                SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Particle", 1f, _owner.view, _owner.view, 3f);
            }
            if (_activated)
            {
                _owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_ozma3.BattleUnitBuf_ozmaReviveCheck))?.Destroy();
            }
            else
            {
                if (_owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_ozma3.BattleUnitBuf_ozmaReviveCheck)) != null)
                    return;
                _owner.bufListDetail.AddBuf((BattleUnitBuf)new EmotionCardAbility_ozma3.BattleUnitBuf_ozmaReviveCheck());
            }
        }
        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            if (_activated || (double)_owner.hp > (double)dmg)
                return false;
            _activated = true;
            _owner.RecoverHP((int)(double)_owner.MaxHp);
            _owner.breakDetail.RecoverBreakLife(_owner.MaxBreakLife);
            _owner.breakDetail.nextTurnBreak = false;
            _owner.breakDetail.RecoverBreak(_owner.breakDetail.GetDefaultBreakGauge());
            _owner.cardSlotDetail.LosePlayPoint(_owner.cardSlotDetail.GetMaxPlayPoint());
            if (Singleton<StageController>.Instance.IsLogState())
            {
                _owner.battleCardResultLog?.SetNewCreatureAbilityEffect("7_C/FX_IllusionCard_7_C_Particle", 3f);
                _owner.battleCardResultLog?.SetCreatureEffectSound("CreatureOzma_FarAtk");
            }
            else
            {
                SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Particle", 1f, _owner.view, _owner.view, 3f);
                SoundEffectPlayer.PlaySound("CreatureOzma_FarAtk");
            }
            return true;
        }
        public void Destroy()
        {
            if (_owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_ozmaReviveCheck)) is BattleUnitBuf_ozmaReviveCheck revive)
                revive.Destroy();
        }
        public class BattleUnitBuf_ozmaReviveCheck : BattleUnitBuf
        {
            public override string keywordId => "Ozma_revive";
            public override string keywordIconId => "Ozma_AwakenPumpkin";
            public BattleUnitBuf_ozmaReviveCheck() => stack = 0;
        }
    }
}
