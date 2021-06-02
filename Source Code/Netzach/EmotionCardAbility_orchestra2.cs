using System;
using LOR_DiceSystem;
using UI;
using Sound;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_orchestra2 : EmotionCardAbilityBase
    {
        public CameraFilterPack_Noise_TV_2 _filter;
        private int turn;
        public void Filter()
        {
            Camera effectCam = SingletonBehavior<BattleCamManager>.Instance.EffectCam;
            if ((UnityEngine.Object)effectCam == (UnityEngine.Object)null)
                return;
            this._filter = effectCam.gameObject.AddComponent<CameraFilterPack_Noise_TV_2>();
            this._filter.Fade = 0.15f;
            this._filter.Fade_Additive = 0.0f;
            this._filter.Fade_Distortion = 0.2f;
        }
        public void DestroyFilter()
        {
            if (!((UnityEngine.Object)this._filter != (UnityEngine.Object)null))
                return;
            UnityEngine.Object.Destroy((UnityEngine.Object)this._filter);
        }
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            if (this._owner.faction == Faction.Enemy)
                turn = 3;
            if (!CheckDupFilter())
                Filter();
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (this._owner.faction == Faction.Player)
            {
                if (this.turn >= 2)
                {
                    DestroyFilter();
                    return;
                }
                ++this.turn;
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
                    alive.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_Orchestra());
                SoundEffectPlayer.PlaySound("Creature/Sym_movment_3_mov3");
            }
            if (this._owner.faction == Faction.Enemy)
            {
                ++this.turn;
                if (turn % 4 != 0)
                {
                    if (BattleObjectManager.instance.GetAliveList(Faction.Player).FindAll(x => x.bufListDetail.GetActivatedBufList().Exists(y => y is Enthusiastic)).Count == 0)
                        DestroyFilter();
                    return;
                }
                SoundEffectPlayer.PlaySound("Creature/Sym_movment_3_mov3");
                List<BattleUnitModel> Player = BattleObjectManager.instance.GetAliveList(Faction.Player).FindAll(x => !x.bufListDetail.GetActivatedBufList().Exists(y => y is Enthusiastic));
                if (Player.Count == 0)
                    return;
                BattleUnitModel unlucky = RandomUtil.SelectOne<BattleUnitModel>(Player);
                unlucky.bufListDetail.AddBuf(new Enthusiastic());
                unlucky.breakDetail.RecoverBreakLife(unlucky.MaxBreakLife);
                unlucky.breakDetail.nextTurnBreak = false;
                unlucky.turnState = BattleUnitTurnState.WAIT_CARD;
                unlucky.breakDetail.RecoverBreak(unlucky.breakDetail.GetDefaultBreakGauge());
                if(!CheckDupFilter())
                    Filter();
            }
        }
        private bool CheckDupFilter()
        {
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList())
            {
                BattleEmotionCardModel adoration = SearchEmotion(unit, "SilentOrchestra_Affect");
                if (adoration == null)
                    adoration = SearchEmotion(unit, "SilentOrchestra_Affect_Enemy");
                if (adoration == null)
                    continue;
                else
                {
                    if(adoration.AbilityList.Find(x => x is EmotionCardAbility_orchestra2) is EmotionCardAbility_orchestra2 AdorationAbility)
                    {
                        if (AdorationAbility._filter != (UnityEngine.Object)null)
                            return true;
                    }
                }
            }
            return false;
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
        public override void OnEndBattlePhase()
        {
            Destroy();
        }

        public override void OnBattleEnd()
        {
            Destroy();
        }
        public override void OnDie(BattleUnitModel killer)
        {
            base.OnDie(killer);
            Destroy();
        }
        public void Destroy()
        {
            DestroyFilter();
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
            {
                BattleEmotionCardModel adoration = SearchEmotion(unit, "SilentOrchestra_Affect_Enemy");
                if (adoration != null)
                {
                    if (adoration.AbilityList.Find(x => x is EmotionCardAbility_orchestra2) is EmotionCardAbility_orchestra2 AdorationAbility)
                    {
                        if (AdorationAbility._filter == (UnityEngine.Object)null)
                            AdorationAbility.Filter();
                    }
                }
                return;
            }
            foreach (BattleUnitModel player in BattleObjectManager.instance.GetAliveList(Faction.Player))
            {
                if (player.bufListDetail.GetActivatedBufList().Find(x => x is Enthusiastic) is Enthusiastic enthusiastic)
                    enthusiastic.Destroy();
            }
            
        }
        public class BattleUnitBuf_Emotion_Orchestra : BattleUnitBuf
        {
            private static int DmgAdd => RandomUtil.Range(2, 4);
            protected override string keywordId => "Orchestra_Affect";
            protected override string keywordIconId => "Orchestra_Enthusiastic";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                this.stack = 0;
            }
            public override bool TeamKill() => true;
            public override int GetDamageReduction(BattleDiceBehavior behavior) => -DmgAdd;
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }
        public class Enthusiastic: BattleUnitBuf
        {
            private bool _bControlable = false;
            private bool _bRecoverBreak;
            private static int DmgAdd => RandomUtil.Range(2, 4);
            protected override string keywordId => "Enthusiastic";
            protected override string keywordIconId => "Orchestra_Enthusiastic";
            public override bool IsControllable => this._bControlable;
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                this.stack = 0;
            }
            public override bool IsInvincibleBp(BattleUnitModel attacker)
            {
                if (attacker != null && attacker != this._owner)
                {
                    if (attacker.faction == Faction.Player)
                    {
                        if (attacker.bufListDetail.GetActivatedBufList().Exists((Predicate<BattleUnitBuf>)(x => x is Enthusiastic)))
                            return true;
                    }
                    else
                        return true;
                }
                return base.IsInvincibleBp(attacker);
            }
            public override int GetBreakDamageIncreaseRate() => 100;
            public override bool IsInvincibleHp(BattleUnitModel attacker)
            {
                if (attacker != null)
                {
                    if (attacker.faction == Faction.Player)
                    {
                        if (!attacker.bufListDetail.GetActivatedBufList().Exists((Predicate<BattleUnitBuf>)(x => x is Enthusiastic)))
                            return true;
                    }
                    else
                    {
                        return base.IsInvincibleHp(attacker);
                    }
                }
                return base.IsInvincibleHp(attacker);
            }
            public override bool TeamKill()
            {
                return true;
            }
            public override int GetDamageReduction(BattleDiceBehavior behavior) => -DmgAdd;
            public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
            {
                BattleUnitModel owner = atkDice.owner;
                bool flag = owner.bufListDetail.GetActivatedBufList().Exists(x => x is Enthusiastic);
                if (owner.faction != this._owner.faction || flag || (!this._owner.IsBreakLifeZero() || this._bRecoverBreak))
                    return;
                this._bRecoverBreak = true;
            }
            public override void OnRoundEndTheLast()
            {
                if (!this._bRecoverBreak)
                    return;
                this._owner.breakDetail.RecoverBreakLife(this._owner.MaxBreakLife);
                this._owner.breakDetail.nextTurnBreak = false;
                this._owner.turnState = BattleUnitTurnState.WAIT_CARD;
                this._owner.breakDetail.RecoverBreak(this._owner.breakDetail.GetDefaultBreakGauge());
                this.Destroy();
            }
            public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int currentSlotOrder)
            {
                List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList();
                aliveList.Remove(this._owner);
                if (aliveList.Count == 0)
                    return base.ChangeAttackTarget(card,currentSlotOrder);
                return RandomUtil.SelectOne<BattleUnitModel>(aliveList);
            }
        }
    }
}
