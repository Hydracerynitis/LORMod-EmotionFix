using System;
using LOR_DiceSystem;
using System.Collections;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_doki : EmotionCardAbilityBase
    {
        private int count;
        public override void OnSelectEmotion()
        {
            SingletonBehavior<BattleCamManager>.Instance?.StartCoroutine(this.Pinpong(SingletonBehavior<BattleCamManager>.Instance?.AddCameraFilter<CameraFilterPack_Blur_Radial>()));
            count = 0;
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            count = 0;
        }

        public override void OnSelectEmotionOnce()
        {
            base.OnSelectEmotionOnce();
            SoundEffectPlayer.PlaySound("Creature/Heartbeat");
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (this._owner.faction == Faction.Player)
            {
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 1);
                if(count>=2)
                    this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 1);
            }
            if (this._owner.faction == Faction.Enemy)
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 2);
        }
        private IEnumerator Pinpong(CameraFilterPack_Blur_Radial r)
        {
            float elapsedTime = 0.0f;
            while ((double)elapsedTime < 1.0)
            {
                elapsedTime += Time.deltaTime;
                r.Intensity = Mathf.PingPong(Time.time, 0.05f);
                yield return (object)new WaitForEndOfFrame();
            }
            SingletonBehavior<BattleCamManager>.Instance?.RemoveCameraFilter<CameraFilterPack_Blur_Radial>();
        }
        public override void OnBreakState()
        {
            base.OnBreakState();
            count = 0;
        }
        public override void OnRoundEnd()
        {
            if(this._owner.faction==Faction.Player&& !this._owner.IsBreakLifeZero())
                count++;
            if (this._owner.history.damageAtOneRound > 0)
                return;
            this._owner.breakDetail.TakeBreakDamage((int)((double)this._owner.breakDetail.GetDefaultBreakGauge() * 0.25));
        }
    }
}
