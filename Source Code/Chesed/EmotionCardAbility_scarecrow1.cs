using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_scarecrow1 : EmotionCardAbilityBase
    {
        private List<BattleDiceCardModel> Lost = new List<BattleDiceCardModel>();
        private List<BattleDiceCardModel> Gain = new List<BattleDiceCardModel>();
        private static bool Prop => RandomUtil.valueForProb <= 0.25;
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            List<BattleDiceCardModel> Deck = _owner.allyCardDetail.GetAllDeck();
            List<BattleDiceCardModel> Exhaust = new List<BattleDiceCardModel>();
            for (int i = 0; i < 3; i++)
            {
                BattleDiceCardModel victim = RandomUtil.SelectOne<BattleDiceCardModel>(Deck);
                Exhaust.Add(victim);
                Deck.Remove(victim);
            }
            Lost.AddRange(Exhaust);
            foreach (BattleDiceCardModel card in Exhaust)
                _owner.allyCardDetail.ExhaustACard(card);
        }
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            SetFilter();
            List<BattleDiceCardModel> Deck = _owner.allyCardDetail.GetAllDeck();
            List<BattleDiceCardModel> Exhaust = new List<BattleDiceCardModel>();
            for(int i=0; i < 3; i++)
            {
                BattleDiceCardModel victim = RandomUtil.SelectOne<BattleDiceCardModel>(Deck);
                Exhaust.Add(victim);
                Deck.Remove(victim);
            }
            Lost.AddRange(Exhaust);
            foreach (BattleDiceCardModel card in Exhaust)
                _owner.allyCardDetail.ExhaustACardAnywhere(card);
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            if (!Prop || behavior.card.target==null)
                return;
            List<BattleDiceCardModel> Wisdom = new List<BattleDiceCardModel>
            {
                RandomUtil.SelectOne<BattleDiceCardModel>(behavior.card.target.allyCardDetail.GetAllDeck())
            };
            Gain.AddRange(Wisdom);
            _owner.allyCardDetail.AddCardToDeck(Wisdom);
            _owner.allyCardDetail.Shuffle();
            _owner.battleCardResultLog?.SetEndCardActionEvent(new BattleCardBehaviourResult.BehaviourEvent(PrintSound));
        }
        public void Destroy()
        {
            foreach (BattleDiceCardModel card in Gain)
                _owner.allyCardDetail.ExhaustACardAnywhere(card);
            _owner.allyCardDetail.AddCardToDeck(Lost);
            _owner.allyCardDetail.Shuffle();
        }
        private void PrintSound() => SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Scarecrow_Special");
        private void SetFilter()
        {
            GameObject gameObject = SingletonBehavior<BattleCamManager>.Instance.EffectCam.gameObject;
            if (!(gameObject != null))
                return;
            CameraFilterPack_Distortion_ShockWave distortionShockWave = gameObject.AddComponent<CameraFilterPack_Distortion_ShockWave>();
            distortionShockWave.PosX = 0.5f;
            distortionShockWave.PosY = 0.5f;
            distortionShockWave.Speed = 1.2f;
            AutoScriptDestruct autoScriptDestruct = SingletonBehavior<BattleCamManager>.Instance?.EffectCam.gameObject.AddComponent<AutoScriptDestruct>() ?? (AutoScriptDestruct)null;
            if (!(autoScriptDestruct != null))
                return;
            autoScriptDestruct.targetScript = (MonoBehaviour)distortionShockWave;
            autoScriptDestruct.time = 1.5f;
        }
    }
}
