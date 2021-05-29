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
    public class EmotionCardAbility_blackswan1 : EmotionCardAbilityBase
    {
        private GameObject aura;
        private List<KeywordBuf> ActivatedBuf;
        private bool _sound;
        private KeywordBuf[] debuff => new KeywordBuf[]
        {
            KeywordBuf.Burn,
            KeywordBuf.Paralysis,
            KeywordBuf.Bleeding,
            KeywordBuf.Vulnerable,
            KeywordBuf.Weak,
            KeywordBuf.Disarm,
            KeywordBuf.Binding
        };
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            _sound = false;
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if (ActivatedBuf.Count <= 0)
                return;
            KeywordBuf buff = RandomUtil.SelectOne<KeywordBuf>(ActivatedBuf);
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(this._owner.faction))
            {
                unit.bufListDetail.AddKeywordBufByEtc(buff, 1);
            }
            behavior?.card?.target?.battleCardResultLog?.SetNewCreatureAbilityEffect("3_H/FX_IllusionCard_3_H_Dertyfeather", 2f);
        }
        public override void OnRoundStart()
        {
            if (this._sound)
                SoundEffectPlayer.PlaySound("Creature/Shark_Ocean");
            this._sound = false;
            for (int i=0; i<3;i++)
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(RandomUtil.SelectOne<KeywordBuf>(debuff),1);
            this.aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("3_H/FX_IllusionCard_3_H_Dertyfeather_Loop", 1f, this._owner.view, this._owner.view)?.gameObject;
        }
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            ActivatedBuf = new List<KeywordBuf>();
            foreach(KeywordBuf buftype in debuff)
            {
                if (this._owner.bufListDetail.GetActivatedBuf(buftype) != null)
                    ActivatedBuf.Add(buftype);
            }
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            DestroyAura();
        }
        public override void OnDie(BattleUnitModel killer)
        {
            base.OnDie(killer);
            DestroyAura();
        }
        public override void OnEndBattlePhase()
        {
            base.OnEndBattlePhase();
            DestroyAura();
        }
        public void Destroy()
        {
            DestroyAura();
        }
        public void DestroyAura()
        {
            if (!((UnityEngine.Object)this.aura != (UnityEngine.Object)null))
                return;
            UnityEngine.Object.Destroy((UnityEngine.Object)this.aura.gameObject);
            this.aura = (GameObject)null;
        }
    }
}
