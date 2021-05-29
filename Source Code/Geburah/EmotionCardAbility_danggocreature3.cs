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
        private int Healed => this._owner.UnitData.historyInWave.healed;
        private int Threshold => (int)((double)this._owner.MaxHp * 0.1);
        private int heal;
        private int absorption;
        private int count;
        public override void OnWaveStart()
        {
            heal = 0;
            absorption = 0;
            _effect = true;
            count = 0;
            this.MakeEffect("6/Dango_Emotion_Spread", target: this._owner);
        }
        public override void OnSelectEmotion()
        {
            heal = Healed;
            _effect = true;
            absorption = 0;
            height = this._owner.UnitData.unitData.customizeData.height;
        }
        public override void OnRoundEndTheLast()
        {
            int previous_count = 0;
            _effect = true;
            if(this._owner.bufListDetail.GetActivatedBufList().Exists(x => x is MoutainCorpse))
                previous_count = this._owner.bufListDetail.GetActivatedBufList().Find(x => x is MoutainCorpse).stack;
            count = 0;
            absorption += (Healed - heal);
            heal = Healed;
            absorption -= this._owner.history.takeDamageAtOneRound;
            if (absorption <= 0)
                absorption = 0;
            int Absorption = absorption;
            while (Absorption > Threshold)
            {
                count += 1;
                Absorption -= Threshold;
            }
            if (count > previous_count)
                _effect = false;
            //this._owner.UnitData.unitData.customizeData.height = height;
            //this._owner.view.CreateSkin();
        }
        public override void OnRoundStart()
        {
            this._owner.bufListDetail.AddBuf(new Indicator(absorption));
            MoutainCorpse moutain = new MoutainCorpse(count);
            this._owner.bufListDetail.AddBuf(moutain);
            this._owner.view.ChangeHeight((int)((double)height * (1 + (double)moutain.stack * 0.25)));
            if(count>5)
                PlatformManager.Instance.UnlockAchievement(AchievementEnum.ONCE_FLOOR6);
            if (!this._effect)
            {
                this._effect = true;
                CameraFilterUtil.EarthQuake(0.18f, 0.16f, 90f, 0.45f);
                Battle.CreatureEffect.CreatureEffect original1 = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/6/Dango_Emotion_Effect");
                if ((UnityEngine.Object)original1 != (UnityEngine.Object)null)
                {
                    Battle.CreatureEffect.CreatureEffect creatureEffect1 = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original1, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                    Battle.CreatureEffect.CreatureEffect creatureEffect2 = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original1, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                    Battle.CreatureEffect.CreatureEffect creatureEffect3 = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original1, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                    if ((UnityEngine.Object)creatureEffect1?.gameObject.GetComponent<AutoDestruct>() == (UnityEngine.Object)null)
                    {
                        AutoDestruct autoDestruct = creatureEffect1?.gameObject.AddComponent<AutoDestruct>();
                        if ((UnityEngine.Object)autoDestruct != (UnityEngine.Object)null)
                        {
                            autoDestruct.time = 3f;
                            autoDestruct.DestroyWhenDisable();
                        }
                    }
                    if ((UnityEngine.Object)creatureEffect2?.gameObject.GetComponent<AutoDestruct>() == (UnityEngine.Object)null)
                    {
                        AutoDestruct autoDestruct = creatureEffect2?.gameObject.AddComponent<AutoDestruct>();
                        if ((UnityEngine.Object)autoDestruct != (UnityEngine.Object)null)
                        {
                            autoDestruct.time = 3f;
                            autoDestruct.DestroyWhenDisable();
                        }
                    }
                    if ((UnityEngine.Object)creatureEffect3?.gameObject.GetComponent<AutoDestruct>() == (UnityEngine.Object)null)
                    {
                        AutoDestruct autoDestruct = creatureEffect3?.gameObject.AddComponent<AutoDestruct>();
                        if ((UnityEngine.Object)autoDestruct != (UnityEngine.Object)null)
                        {
                            autoDestruct.time = 3f;
                            autoDestruct.DestroyWhenDisable();
                        }
                    }
                }
                Battle.CreatureEffect.CreatureEffect original2 = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/7/Lumberjack_final_blood_1st");
                if ((UnityEngine.Object)original2 != (UnityEngine.Object)null)
                {
                    Battle.CreatureEffect.CreatureEffect creatureEffect1 = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original2, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                    Battle.CreatureEffect.CreatureEffect creatureEffect2 = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original2, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                    Battle.CreatureEffect.CreatureEffect creatureEffect3 = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original2, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                    if ((UnityEngine.Object)creatureEffect1?.gameObject.GetComponent<AutoDestruct>() == (UnityEngine.Object)null)
                    {
                        AutoDestruct autoDestruct = creatureEffect1?.gameObject.AddComponent<AutoDestruct>();
                        if ((UnityEngine.Object)autoDestruct != (UnityEngine.Object)null)
                        {
                            autoDestruct.time = 3f;
                            autoDestruct.DestroyWhenDisable();
                        }
                    }
                    if ((UnityEngine.Object)creatureEffect2?.gameObject.GetComponent<AutoDestruct>() == (UnityEngine.Object)null)
                    {
                        AutoDestruct autoDestruct = creatureEffect2?.gameObject.AddComponent<AutoDestruct>();
                        if ((UnityEngine.Object)autoDestruct != (UnityEngine.Object)null)
                        {
                            autoDestruct.time = 3f;
                            autoDestruct.DestroyWhenDisable();
                        }
                    }
                    if ((UnityEngine.Object)creatureEffect3?.gameObject.GetComponent<AutoDestruct>() == (UnityEngine.Object)null)
                    {
                        AutoDestruct autoDestruct = creatureEffect3?.gameObject.AddComponent<AutoDestruct>();
                        if ((UnityEngine.Object)autoDestruct != (UnityEngine.Object)null)
                        {
                            autoDestruct.time = 3f;
                            autoDestruct.DestroyWhenDisable();
                        }
                    }
                }
                this.MakeEffect("6/Dango_Emotion_Spread", target: this._owner);
                SoundEffectPlayer.PlaySound("Creature/Danggo_LvUp");
                SoundEffectPlayer.PlaySound("Creature/Danggo_Birth");
            }
        }
        public class MoutainCorpse: BattleUnitBuf
        {
            protected override string keywordId => "MountainCorpse";
            protected override string keywordIconId => "DangoCreature_Emotion_Healed";
            public MoutainCorpse(int count)
            {
                this.stack = count;
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
                this.Destroy();
            }
        }
        public class Indicator: BattleUnitBuf
        {
            protected override string keywordId => "Indicator";
            protected override string keywordIconId => "DangoCreature_Emotion_Healed";
            public Indicator(int absorption)
            {
                this.stack = absorption;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }
    }
}
