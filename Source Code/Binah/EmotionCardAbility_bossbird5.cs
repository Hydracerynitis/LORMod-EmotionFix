using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using System.Reflection;
using Sound;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;

namespace EmotionalFix
{
    public class EmotionCardAbility_bossbird5 : EmotionCardAbilityBase
    {
        private Battle.CreatureEffect.CreatureEffect _aura;
        private bool init = false;
        private List<BattleDiceCardModel> Ego = new List<BattleDiceCardModel>();
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this._owner.bufListDetail.AddBuf(new EmotionCardAbility_bossbird1.Longbird_Enemy());
            this._owner.bufListDetail.AddBuf(new EmotionCardAbility_bossbird2.Bigbird_Enemy());
            this._owner.bufListDetail.AddBuf(new EmotionCardAbility_bossbird3.Smallbird_Enemy());
            this._owner.allyCardDetail.AddCardToDeck(Ego);
            this._owner.allyCardDetail.Shuffle();
        }
        public override void OnSelectEmotion()
        {
            if (init)
                return;
            base.OnSelectEmotion();
            BattleEmotionCardModel Long = SearchEmotion(_owner, "ApocalypseBird_LongArm_Enemy");
            BattleEmotionCardModel Big = SearchEmotion(_owner, "ApocalypseBird_BigEye_Enemy");
            BattleEmotionCardModel Small= SearchEmotion(_owner, "ApocalypseBird_SmallPeak_Enemy");
            if (Long != null)
            {
                foreach (EmotionCardAbilityBase ability in Long.GetAbilityList())
                {
                    MethodInfo destroy = ability.GetType().GetMethod("Destroy");
                    if (destroy != null)
                        try
                        {
                            destroy.Invoke(ability, new object[] { });
                        }
                        catch 
                        {

                        }
                }
                _owner.emotionDetail.PassiveList.Remove(Long);
                string name = RandomUtil.SelectOne<EmotionCardXmlInfo>(EmotionFixInitializer.emotion1).Name + "_Enemy";
                EmotionCardXmlInfo emotion = EmotionFixInitializer.enermy.Find(x => x.Name == name);
                _owner.emotionDetail.ApplyEmotionCard(emotion);
            }
            if (Big!= null)
            {
                foreach (EmotionCardAbilityBase ability in Big.GetAbilityList())
                {
                    MethodInfo destroy = ability.GetType().GetMethod("Destroy");
                    if (destroy != null)
                        try
                        {
                            destroy.Invoke(ability, new object[] { });
                        }
                        catch 
                        {

                        }
                }
                _owner.emotionDetail.PassiveList.Remove(Big);
                string name = RandomUtil.SelectOne<EmotionCardXmlInfo>(EmotionFixInitializer.emotion2).Name + "_Enemy";
                EmotionCardXmlInfo emotion = EmotionFixInitializer.enermy.Find(x => x.Name == name);
                _owner.emotionDetail.ApplyEmotionCard(emotion);
            }
            if (Small != null)
            {
                foreach (EmotionCardAbilityBase ability in Small.GetAbilityList())
                {
                    MethodInfo destroy = ability.GetType().GetMethod("Destroy");
                    if (destroy != null)
                        try
                        {
                            destroy.Invoke(ability, new object[] { });
                        }
                        catch 
                        {
                        }

                }
                _owner.emotionDetail.PassiveList.Remove(Small);
                string name = RandomUtil.SelectOne<EmotionCardXmlInfo>(EmotionFixInitializer.emotion2).Name + "_Enemy";
                EmotionCardXmlInfo emotion = EmotionFixInitializer.enermy.Find(x => x.Name == name);
                _owner.emotionDetail.ApplyEmotionCard(emotion);
            }
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/BossBird_Birth", false, 4f);
            _aura=SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("8_B/FX_IllusionCard_8_B_MonsterAura", 1f, _owner.view, _owner.view);
            _owner.bufListDetail.AddBuf(new EmotionCardAbility_bossbird1.Longbird_Enemy());
            _owner.bufListDetail.AddBuf(new EmotionCardAbility_bossbird2.Bigbird_Enemy());
            _owner.bufListDetail.AddBuf(new EmotionCardAbility_bossbird3.Smallbird_Enemy());
            DiceCardXmlInfo bigbirdxml = ItemXmlDataList.instance.GetCardItem(910041).Copy(true);
            bigbirdxml.optionList.Clear();
            DiceCardSpec bigbirdspec= bigbirdxml.Spec.Copy();
            bigbirdspec.Cost = 0;
            bigbirdxml.Spec = bigbirdspec;
            bigbirdxml.Priority = 100;
            bigbirdxml.Keywords.Clear();
            BattleDiceCardModel BigBirdEgo = BattleDiceCardModel.CreatePlayingCard(bigbirdxml);
            BigBirdEgo.owner = _owner;
            Ego.Add(BigBirdEgo);
            DiceCardXmlInfo smallbirdxml = ItemXmlDataList.instance.GetCardItem(910043).Copy(true);
            smallbirdxml.optionList.Clear();
            DiceCardSpec smallbirdspec = smallbirdxml.Spec.Copy();
            smallbirdspec.Cost = 0;
            smallbirdxml.Spec = smallbirdspec;
            smallbirdxml.Priority = 100;
            smallbirdxml.Keywords.Clear();
            BattleDiceCardModel SmallBirdEgo = BattleDiceCardModel.CreatePlayingCard(smallbirdxml);
            SmallBirdEgo.owner = _owner;
            Ego.Add(SmallBirdEgo);
            DiceCardXmlInfo longbirdxml = ItemXmlDataList.instance.GetCardItem(910042).Copy(true);
            longbirdxml.optionList.Clear();
            DiceCardSpec longbirdspec = longbirdxml.Spec.Copy();
            longbirdspec.Cost = 0;
            longbirdxml.Spec = longbirdspec;
            longbirdxml.Priority = 100;
            longbirdxml.Keywords.Clear();
            BattleDiceCardModel LongBirdEgo = BattleDiceCardModel.CreatePlayingCard(longbirdxml);
            LongBirdEgo.owner = _owner;
            Ego.Add(LongBirdEgo);
            _owner.allyCardDetail.AddCardToDeck(Ego);
            _owner.allyCardDetail.Shuffle();
            init = true;
        }
        public override void OnEndBattlePhase()
        {
            base.OnEndBattlePhase();
            DestroyAura();
        }
        public override void OnDie(BattleUnitModel killer)
        {
            base.OnDie(killer);
            DestroyAura();
        }
        public void Destroy()
        {
            BattleUnitBuf Buff = _owner.bufListDetail.GetActivatedBufList().Find(x => x is EmotionCardAbility_bossbird2.Bigbird_Enemy);
            if (Buff != null)
                Buff.Destroy();
            Buff = _owner.bufListDetail.GetActivatedBufList().Find(x => x is EmotionCardAbility_bossbird1.Longbird_Enemy);
            if (Buff != null)
                Buff.Destroy();
            Buff = _owner.bufListDetail.GetActivatedBufList().Find(x => x is EmotionCardAbility_bossbird3.Smallbird_Enemy);
            if (Buff != null)
                Buff.Destroy();
            foreach (BattleDiceCardModel EGO in Ego)
                _owner.allyCardDetail.ExhaustACardAnywhere(EGO);
            DestroyAura();
        }
        public void DestroyAura()
        {
            if (_aura == null)
                return;
            UnityEngine.Object.Destroy(_aura.gameObject);
            _aura = null;
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
    }
}
