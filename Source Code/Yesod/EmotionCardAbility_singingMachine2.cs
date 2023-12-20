using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_singingMachine2 : EmotionCardAbilityBase
    {
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            this.GetBuf();
        }
        private void GetBuf()
        {
            if (!(this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_SingingMachine_Rhythm)) is BattleUnitBuf_Emotion_SingingMachine_Rhythm singingMachineRhythm))
                this._owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_SingingMachine_Rhythm(1));
            else
            {
                ++singingMachineRhythm.stack;
                this._owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Strength, 1, this._owner);
            }
        }
        public void Destroy()
        {
            bool mulitrhythm = false;
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList())
            {
                if (SearchEmotion(unit, "SingingMachine_Rhythm") != null)
                    mulitrhythm = true;
                if (SearchEmotion(unit, "SingingMachine_Rhythm_Enemy") != null)
                    mulitrhythm = true;
            }
            if (mulitrhythm == true)
                return;
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList())
            {
                if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_SingingMachine_Rhythm)) is BattleUnitBuf_Emotion_SingingMachine_Rhythm rhytm)
                    rhytm.Destroy();
            }
        }
        private BattleEmotionCardModel SearchEmotion(BattleUnitModel owner, string Name)
        {
            List<BattleEmotionCardModel> emotion = owner.emotionDetail.PassiveList;
            foreach (BattleEmotionCardModel card in emotion)
            {
                if (card.XmlInfo.Name == Name)
                    return card;
            }
            return null;
        }
        public class BattleUnitBuf_Emotion_SingingMachine_Rhythm : BattleUnitBuf
        {
            private Battle.CreatureEffect.CreatureEffect _effect;
            private int reserve;
            public override string keywordId => "SingingMachine_Rhythm";
            private int _count;
            public BattleUnitBuf_Emotion_SingingMachine_Rhythm(int value = 0)
            {
                this.stack = value;
                _count = 0;
                this.reserve = Mathf.Max(0, 1 - value);
            }
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                if (this.stack <= 0)
                    return;
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 1, this._owner);
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                if (this.stack > 0)
                    this.stack = 0;
                this.stack += this.reserve;
                this.reserve = 0;
                if (this.stack <= 0)
                    this.Destroy();
                else
                    this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, this.stack, this._owner);
            }
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (!((UnityEngine.Object)this._effect == (UnityEngine.Object)null))
                    return;
                this._effect = SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("4/SingingMachine_NoteAura", 1f, this._owner.view, (BattleUnitView)null);
                this._effect?.SetLayer("Character");
                SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Singing_Rhythm")?.SetGlobalPosition(this._owner.view.WorldPosition);
            }
            public override void Destroy()
            {
                if ((UnityEngine.Object)this._effect != (UnityEngine.Object)null)
                {
                    UnityEngine.Object.Destroy((UnityEngine.Object)this._effect.gameObject);
                    this._effect = (Battle.CreatureEffect.CreatureEffect)null;
                }
                base.Destroy();
            }
            public override void OnLayerChanged(string layerName)
            {
                if (!((UnityEngine.Object)this._effect != (UnityEngine.Object)null))
                    return;
                this._effect.SetLayer(layerName);
            }
            public void Reserve(int value = 1) => this.reserve += value;
            public override void OnSuccessAttack(BattleDiceBehavior behavior)
            {
                base.OnSuccessAttack(behavior);
                if (this.stack == 0)
                    return;
                this._count += 1;
                if (_count < 3)
                    return;
                _count = 0;
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Singing_Rhythm");
                this.Ability();
            }
            private void Ability()
            {
                List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList();
                if (aliveList.Count == 0)
                    return;
                BattleUnitModel victim = RandomUtil.SelectOne<BattleUnitModel>(aliveList);
                if (!(victim.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_SingingMachine_Rhythm)) is BattleUnitBuf_Emotion_SingingMachine_Rhythm singingMachineRhythm))
                    victim.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_SingingMachine_Rhythm());
                else
                    singingMachineRhythm.Reserve();
            }
        }
    }
}
