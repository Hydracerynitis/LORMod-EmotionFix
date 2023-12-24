using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;

namespace EmotionalFix
{
    public class EmotionCardAbility_bluestar3 : EmotionCardAbilityBase
    {
        private int round;
        private SoundEffectPlayer _loop;
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            round = 3;
            _owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_BlueStar_SoundBuf());
            _owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_BlueStar_SoundBuf_Cool(round));

        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (_owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_BlueStar_SoundBuf)) == null)
                return;
            Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/9_H/FX_IllusionCard_9_H_Voice");
            if (!(original != null))
                return;
            Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
            if (!(creatureEffect?.gameObject.GetComponent<AutoDestruct>() == null))
                return;
            AutoDestruct autoDestruct = creatureEffect?.gameObject.AddComponent<AutoDestruct>();
            if (!(autoDestruct != null))
                return;
            autoDestruct.time = 5f;
            autoDestruct.DestroyWhenDisable();
            SoundEffectPlayer.PlaySound("Creature/BlueStar_Atk");
            SingletonBehavior<BattleSoundManager>.Instance.EndBgm();
            if (!(_loop == null))
                return;
            _loop = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/BlueStar_Bgm", true, parent: SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject.transform);
        }

        public override void OnWaveStart()
        {
            base.OnWaveStart();
            if(round==3)
                _owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_BlueStar_SoundBuf());
            _owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_BlueStar_SoundBuf_Cool(round));
        }
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            round--;
            if (round <= 0)
                round = 3;
            DestroyLoopSound();
        }
        public override void OnEndBattlePhase()
        {
            base.OnEndBattlePhase();
            Destroy();
        }
        public override void OnDie(BattleUnitModel killer)
        {
            base.OnDie(killer);
            Destroy();
        }
        private void DestroyLoopSound()
        {
            if (!(_loop != null))
                return;
            SingletonBehavior<BattleSoundManager>.Instance.StartBgm();
            _loop.ManualDestroy();
            _loop = (SoundEffectPlayer)null;
        }
        public void Destroy()
        {
            DestroyLoopSound();
            BattleUnitBuf Buff = _owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_BlueStar_SoundBuf));
            if (Buff != null)
                Buff.Destroy();
            Buff = _owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_BlueStar_SoundBuf_Cool));
            if (Buff != null)
                Buff.Destroy();
        }
        public class BattleUnitBuf_Emotion_BlueStar_SoundBuf : BattleUnitBuf
        {
            public override string keywordId => "Emotion_BlueStar_SoundBuf";

            private int BDmg => RandomUtil.Range(2, 4);

            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (behavior == null)
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    breakDmg = BDmg
                });
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
        public class BattleUnitBuf_Emotion_BlueStar_SoundBuf_Cool : BattleUnitBuf
        {
            public override string keywordId => "Emotion_BlueStar_SoundBuf_Cool";
            public BattleUnitBuf_Emotion_BlueStar_SoundBuf_Cool(int cooldown)
            {
                Init(_owner);
                stack = cooldown;
                if (stack >= 3)
                    stack = 3;
            }
            public override void OnRoundEndTheLast()
            {
                base.OnRoundEndTheLast();
                --stack;
                if (stack > 0)
                    return;
                stack = 3;
                _owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_BlueStar_SoundBuf());
            }
        }
    }
}
