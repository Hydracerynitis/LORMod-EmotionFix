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
    public class EmotionCardAbility_bloodytree2 : EmotionCardAbilityBase
    {
        private Battle.CreatureEffect.CreatureEffect _aura;
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            _aura= SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("9_H/FX_IllusionCard_9_H_Eye", 1f, _owner.view, _owner.view);
            SoundEffectPlayer.PlaySound("Creature/MustSee_Wake_Storng");
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(_owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
            {
                if (alive.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Emotion_BloodyTree) == null)
                    alive.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_BloodyTree(_owner));
            }
        }
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            _aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("9_H/FX_IllusionCard_9_H_Eye", 1f, _owner.view, _owner.view);
            SoundEffectPlayer.PlaySound("Creature/MustSee_Wake_Storng");
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(_owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
            {
                alive.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_BloodyTree(_owner));
            }
        }
        public override void OnEndBattlePhase()
        {
            base.OnEndBattlePhase();
            DestroyAura();
        }
        public override void OnDie(BattleUnitModel killer)
        {
            base.OnDie(killer);
            Destroy();
        }
        public void DestroyAura()
        {
            if (!(_aura != null))
                return;
            UnityEngine.Object.Destroy(_aura.gameObject);
            _aura = (Battle.CreatureEffect.CreatureEffect)null;
        }
        public void Destroy()
        {
            DestroyAura();
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(_owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
            {
                if ((alive.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_BloodyTree))) is BattleUnitBuf_Emotion_BloodyTree Tree)
                    Tree.Destroy();
            }
        }
        public class BattleUnitBuf_Emotion_BloodyTree : BattleUnitBuf
        {
            private BattleUnitModel _except;
            private static int Dmg => RandomUtil.Range(1, 8);
            public override string keywordId => "BloodyTree_Emotion_Damage";
            public override string keywordIconId => "HokmaFirstCounter";
            public BattleUnitBuf_Emotion_BloodyTree(BattleUnitModel except)
            {
                hide = true;
                _except = except;
            }
            public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
            {
                base.OnStartParrying(card);
                if (card.target == null || card.target == _except || _except.IsDead())
                    return;
                int DamageEnemy = Dmg;
                card.target.TakeDamage(DamageEnemy);
                card.target.breakDetail.TakeBreakDamage(DamageEnemy);
                int Damage = Dmg;
                _owner.TakeDamage(Damage);
                _owner.breakDetail.TakeBreakDamage(Damage);
            }
            public override void OnRoundEndTheLast()
            {
                base.OnRoundEndTheLast();
                if (_except.IsDead())
                    Destroy();
            }
        }
    }
}
