using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using HarmonyLib;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_waybackhome2 : EmotionCardAbilityBase
    {
        private int stack = 1;
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            if (this._owner.faction != Faction.Enemy || this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Home)) !=null)
                return;
            this._owner.bufListDetail.AddBuf(new Home());
            
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            if (this._owner.faction != Faction.Enemy || this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Home)) != null)
                return;
            this._owner.bufListDetail.AddBuf(new Home());
        }
        public void Destroy()
        {
            BattleUnitBuf buff = this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Home));
            if (buff != null)
                buff.Destroy();
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            BattleUnitModel target = curCard?.target;
            if (target == null)
                return;
            if (this.CheckAbility(target))
            {
                curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
                {
                    dmg = 2 * this.stack
                });
                if(this._owner.faction==Faction.Player)
                    curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
                    {
                        power = 1 * this.stack
                    });
                ++this.stack;
                this._owner.battleCardResultLog?.SetCreatureAbilityEffect("7/WayBeckHome_Emotion_Atk", 1f);
                this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/House_NormalAtk");
            }
            else
                this.stack = 0;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            this.stack = 1;
            List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player);
            double num1 = Math.Ceiling((double)aliveList.Count * 0.5);
            int num2 = 0;
            while (aliveList.Count > 0 && num1 > 0.0)
            {
                BattleUnitModel battleUnitModel = RandomUtil.SelectOne<BattleUnitModel>(aliveList);
                if (battleUnitModel != null)
                {
                    ++num2;
                    BattleUnitBuf_Emotion_WayBackHome_Target wayBackHomeTarget = new BattleUnitBuf_Emotion_WayBackHome_Target(num2);
                    battleUnitModel.bufListDetail.AddBuf((BattleUnitBuf)wayBackHomeTarget);
                    --num1;
                }
                aliveList.Remove(battleUnitModel);
            }
        }
        private bool CheckAbility(BattleUnitModel target)
        {
            BattleUnitBuf battleUnitBuf = target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_WayBackHome_Target));
            return battleUnitBuf != null && battleUnitBuf.stack == this.stack;
        }
        public class Home : BattleUnitBuf
        {
            public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int idx)
            {
                if (idx >= 3)
                    return base.ChangeAttackTarget(card, idx);
                foreach (BattleUnitModel enemy in BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
                {
                    if (enemy.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_WayBackHome_Target)) is BattleUnitBuf_Emotion_WayBackHome_Target goldbrick)
                    {
                        if (goldbrick.stack - 1 == idx)
                            return enemy;
                    }
                }
                return base.ChangeAttackTarget(card, idx);
            }
        }
        public class BattleUnitBuf_Emotion_WayBackHome_Target : BattleUnitBuf
        {
            private GameObject aura;
            protected override string keywordId => this._owner.faction==Faction.Enemy? "WayBackHome_Emotion_Target": "WayBackHome_Emotion_Target_Enemy";
            protected override string keywordIconId => "WayBackHome_Target";
            public BattleUnitBuf_Emotion_WayBackHome_Target(int value) => this.stack = value;
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                this.aura = SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("7/WayBeckHome_Emotion_Way", 1f, owner.view, owner.view)?.gameObject;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
            public override void OnDie()
            {
                base.OnDie();
                this.Destroy();
            }
            public override void Destroy()
            {
                base.Destroy();
                this.DestroyAura();
            }
            private void DestroyAura()
            {
                if (!((UnityEngine.Object)this.aura != (UnityEngine.Object)null))
                    return;
                UnityEngine.Object.Destroy((UnityEngine.Object)this.aura);
                this.aura = (GameObject)null;
            }
        }
    }
}
