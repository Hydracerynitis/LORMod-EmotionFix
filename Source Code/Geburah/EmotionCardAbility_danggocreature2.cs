using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_danggocreature2 : EmotionCardAbilityBase
    {
        private bool _effect;
        List<BattleDiceCardModel> Dead;
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            Dead = new List<BattleDiceCardModel>();
            _effect = false;
        }
        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            if (unit == this._owner || this._owner.faction!=unit.faction)
                return;
            _effect = true;
            List<BattleDiceCardModel> deadman = unit.allyCardDetail.GetAllDeck();
            List<BattleDiceCardModel> remain = new List<BattleDiceCardModel>();
            for(int i=0; i < 3; i++)
            {
                BattleDiceCardModel deadcard = RandomUtil.SelectOne<BattleDiceCardModel>(deadman);
                deadman.Remove(deadcard);
                remain.Add(deadcard);
            }
            this._owner.allyCardDetail.AddCardToDeck(remain);
            Dead.AddRange(remain);
            this._owner.allyCardDetail.Shuffle();
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (_effect)
            {
                _effect = false;
                SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("6_G/FX_IllusionCard_6_G_Shout", 1f, this._owner.view, this._owner.view, 3f);
                CameraFilterUtil.EarthQuake(0.08f, 0.02f, 50f, 0.3f);
                SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Battle/Tomary_Phase2");
            }
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this._owner.allyCardDetail.AddCardToDeck(Dead);
            this._owner.allyCardDetail.Shuffle();
        }
        public void Destroy()
        {
            if (Dead.Count <= 0)
                return;
            foreach (BattleDiceCardModel card in Dead)
                this._owner.allyCardDetail.ExhaustACardAnywhere(card);
        }
    }
}
