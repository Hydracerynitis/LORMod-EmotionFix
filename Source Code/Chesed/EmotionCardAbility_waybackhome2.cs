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
            if (_owner.faction != Faction.Enemy || _owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Home)) !=null)
                return;
            _owner.bufListDetail.AddBuf(new Home());
            
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            if (_owner.faction != Faction.Enemy || _owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Home)) != null)
                return;
            _owner.bufListDetail.AddBuf(new Home());
        }
        public void Destroy()
        {
            BattleUnitBuf buff = _owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Home));
            if (buff != null)
                buff.Destroy();
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            BattleUnitModel target = curCard?.target;
            if (target == null)
                return;
            if (CheckAbility(target))
            {
                curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
                {
                    dmg = 2 * stack
                });
                if(_owner.faction==Faction.Player)
                    curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
                    {
                        power = 1 * stack
                    });
                ++stack;
                _owner.battleCardResultLog?.SetCreatureAbilityEffect("7/WayBeckHome_Emotion_Atk", 1f);
                _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/House_NormalAtk");
            }
            else
                stack = 0;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            stack = 1;
            List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(_owner.faction == Faction.Player ? Faction.Enemy : Faction.Player);
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
            return battleUnitBuf != null && battleUnitBuf.stack == stack;
        }
        public class Home : BattleUnitBuf
        {
            public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int idx)
            {
                if (idx >= 3)
                    return base.ChangeAttackTarget(card, idx);
                foreach (BattleUnitModel enemy in BattleObjectManager.instance.GetAliveList(_owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
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
            public override string keywordId => _owner.faction==Faction.Enemy? "WayBackHome_Emotion_Target": "WayBackHome_Emotion_Target_Enemy";
            public override string keywordIconId => "WayBackHome_Target";
            public BattleUnitBuf_Emotion_WayBackHome_Target(int value) => stack = value;
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                aura = SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("7/WayBeckHome_Emotion_Way", 1f, owner.view, owner.view)?.gameObject;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
            public override void OnDie()
            {
                base.OnDie();
                Destroy();
            }
            public override void Destroy()
            {
                base.Destroy();
                DestroyAura();
            }
            private void DestroyAura()
            {
                if (!(aura != null))
                    return;
                UnityEngine.Object.Destroy(aura);
                aura = (GameObject)null;
            }
        }
    }
}
