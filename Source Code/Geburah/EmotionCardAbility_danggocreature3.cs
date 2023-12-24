using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_danggocreature3 : EmotionCardAbilityBase
    {
        private bool _effect;
        private int height;
        private int previousStack;
        private int Healed => _owner.UnitData.historyInWave.healed;
        private int Threshold => (int)((double)_owner.MaxHp * 0.1);
        private int heal;
        private int absorption;
        private int count;
        public override void OnWaveStart()
        {
            heal = 0;
            absorption = 0;
            _effect = true;
            count = 0;
            previousStack = 0;
            MakeEffect("6/Dango_Emotion_Spread", target: _owner);
        }
        public override void OnSelectEmotion()
        {
            heal = Healed;
            _effect = true;
            absorption = 0;
            previousStack = 0;
            height = _owner.UnitData.unitData.customizeData.height;
        }
        public override void OnRoundEndTheLast()
        {
            _effect = true;
            count = 0;
            absorption += (Healed - heal);
            heal = Healed;
            absorption -= _owner.history.takeDamageAtOneRound;
            if (absorption <= 0)
                absorption = 0;
            int Absorption = absorption;
            while (Absorption > Threshold)
            {
                count += 1;
                Absorption -= Threshold;
            }
            if (count > previousStack)
                _effect = false;
            previousStack = count;
            //_owner.UnitData.unitData.customizeData.height = height;
            //_owner.view.CreateSkin();
        }
        public override void OnRoundStart()
        {
            _owner.bufListDetail.AddBuf(new Indicator(absorption));
            MoutainCorpse moutain = new MoutainCorpse(count);
            _owner.bufListDetail.AddBuf(moutain);
            _owner.view.ChangeHeight((int)((double)height * (1 + (double)moutain.stack * 0.25)));
            if (!_effect)
            {
                _effect = true;
                CameraFilterUtil.EarthQuake(0.18f, 0.16f, 90f, 0.45f);
                Battle.CreatureEffect.CreatureEffect original1 = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/6/Dango_Emotion_Effect");
                if (original1 != null)
                {
                    Battle.CreatureEffect.CreatureEffect creatureEffect1 = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original1, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                    Battle.CreatureEffect.CreatureEffect creatureEffect2 = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original1, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                    Battle.CreatureEffect.CreatureEffect creatureEffect3 = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original1, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                    if (creatureEffect1?.gameObject.GetComponent<AutoDestruct>() == null)
                    {
                        AutoDestruct autoDestruct = creatureEffect1?.gameObject.AddComponent<AutoDestruct>();
                        if (autoDestruct != null)
                        {
                            autoDestruct.time = 3f;
                            autoDestruct.DestroyWhenDisable();
                        }
                    }
                    if (creatureEffect2?.gameObject.GetComponent<AutoDestruct>() == null)
                    {
                        AutoDestruct autoDestruct = creatureEffect2?.gameObject.AddComponent<AutoDestruct>();
                        if (autoDestruct != null)
                        {
                            autoDestruct.time = 3f;
                            autoDestruct.DestroyWhenDisable();
                        }
                    }
                    if (creatureEffect3?.gameObject.GetComponent<AutoDestruct>() == null)
                    {
                        AutoDestruct autoDestruct = creatureEffect3?.gameObject.AddComponent<AutoDestruct>();
                        if (autoDestruct != null)
                        {
                            autoDestruct.time = 3f;
                            autoDestruct.DestroyWhenDisable();
                        }
                    }
                }
                Battle.CreatureEffect.CreatureEffect original2 = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/7/Lumberjack_final_blood_1st");
                if (original2 != null)
                {
                    Battle.CreatureEffect.CreatureEffect creatureEffect1 = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original2, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                    Battle.CreatureEffect.CreatureEffect creatureEffect2 = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original2, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                    Battle.CreatureEffect.CreatureEffect creatureEffect3 = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original2, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                    if (creatureEffect1?.gameObject.GetComponent<AutoDestruct>() == null)
                    {
                        AutoDestruct autoDestruct = creatureEffect1?.gameObject.AddComponent<AutoDestruct>();
                        if (autoDestruct != null)
                        {
                            autoDestruct.time = 3f;
                            autoDestruct.DestroyWhenDisable();
                        }
                    }
                    if (creatureEffect2?.gameObject.GetComponent<AutoDestruct>() == null)
                    {
                        AutoDestruct autoDestruct = creatureEffect2?.gameObject.AddComponent<AutoDestruct>();
                        if (autoDestruct != null)
                        {
                            autoDestruct.time = 3f;
                            autoDestruct.DestroyWhenDisable();
                        }
                    }
                    if (creatureEffect3?.gameObject.GetComponent<AutoDestruct>() == null)
                    {
                        AutoDestruct autoDestruct = creatureEffect3?.gameObject.AddComponent<AutoDestruct>();
                        if (autoDestruct != null)
                        {
                            autoDestruct.time = 3f;
                            autoDestruct.DestroyWhenDisable();
                        }
                    }
                }
                MakeEffect("6/Dango_Emotion_Spread", target: _owner);
                SoundEffectPlayer.PlaySound("Creature/Danggo_LvUp");
                SoundEffectPlayer.PlaySound("Creature/Danggo_Birth");
            }
        }
        public class MoutainCorpse: BattleUnitBuf
        {
            public override string keywordId => "MountainCorpse";
            public override string keywordIconId => "DangoCreature_Emotion_Healed";
            public MoutainCorpse(int count)
            {
                stack = count;
            }
            public override int GetDamageIncreaseRate() => 25*stack;
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = stack
                });
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
        public class Indicator: BattleUnitBuf
        {
            public override string keywordId => "Indicator";
            public override string keywordIconId => "DangoCreature_Emotion_Healed";
            public Indicator(int absorption)
            {
                stack = absorption;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
    }
}
