using System;
using LOR_DiceSystem;
using UI;
using Battle.CameraFilter;
using Sound;
using UnityEngine;
using System.Collections;

namespace EmotionalFix
{
    public class EmotionCardAbility_fragmentSpace : EmotionCardAbilityBase
    {
        private static bool Trigger => RandomUtil.valueForProb <= 0.5;
        private static int BrkDmg => RandomUtil.Range(5, 10);
        private static int RecoverBP => RandomUtil.Range(5, 10);
        public override void OnSelectEmotion()
        {
            if (this._owner.faction != Faction.Player)
                return;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
            {
                if (alive.faction != this._owner.faction)
                    alive.TakeBreakDamage(BrkDmg, DamageType.Emotion,this._owner);
                else
                    alive.breakDetail.RecoverBreak(RecoverBP);
            }
            Camera effectCam = SingletonBehavior<BattleCamManager>.Instance.EffectCam;
            CameraFilterPack_Distortion_Dream2 r = effectCam.GetComponent<CameraFilterPack_Distortion_Dream2>();
            if ((UnityEngine.Object)r == (UnityEngine.Object)null)
                r = effectCam.gameObject.AddComponent<CameraFilterPack_Distortion_Dream2>();
            SingletonBehavior<BattleCamManager>.Instance?.StartCoroutine(this.DistortionRoutine(r));
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Cosmos_Sing")?.SetGlobalPosition(this._owner.view.WorldPosition);
        }
        public override void OnStartTargetedOneSide(BattlePlayingCardDataInUnitModel curCard)
        {
            if(this._owner.faction==Faction.Enemy && Trigger)
            {
                curCard.owner.TakeBreakDamage(BrkDmg);
                this._owner.battleCardResultLog.SetEndCardActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.Effect));
            }
        }
        private void Effect()
        {
            try
            {
                SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Cosmos_Sing")?.SetGlobalPosition(this._owner.view.WorldPosition);
                SingletonBehavior<BattleCamManager>.Instance?.AddCameraFilter<CameraFilterCustom_universe>(true);
            }
            catch
            {

            }
        }
        private IEnumerator DistortionRoutine(CameraFilterPack_Distortion_Dream2 r)
        {
            float e = 0.0f;
            float amount = UnityEngine.Random.Range(20f, 30f);
            int speed = 15;
            while ((double)e < 1.0)
            {
                e += Time.deltaTime * 2f;
                r.Distortion = Mathf.Lerp(amount, 0.0f, e);
                r.Speed = Mathf.Lerp((float)speed, 0.0f, e);
                yield return (object)null;
            }
            UnityEngine.Object.Destroy(r);
        }
    }
}
