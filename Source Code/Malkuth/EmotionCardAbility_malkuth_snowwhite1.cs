﻿using System;
using System.Collections.Generic;
using LOR_DiceSystem;
using Sound;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_malkuth_snowwhite1: EmotionCardAbilityBase
    {
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (!_owner.bufListDetail.GetActivatedBufList().Exists(x => x is VineSeeker))
                _owner.bufListDetail.AddBuf(new VineSeeker());
            List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList_opponent(_owner.faction);
            foreach(BattleUnitModel unit in aliveList)
            {
                if (unit.bufListDetail.GetActivatedBufList().Exists(x => x is BattleUnitBuf_snowwhite_vine))
                    aliveList.Remove(unit);
            }
            if (aliveList.Count <= 0)
                return;
            BattleUnitModel victim = RandomUtil.SelectOne(aliveList);
            victim.bufListDetail.AddBuf(new BattleUnitBuf_snowwhite_vine());
            victim.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Binding, 6, _owner);
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            BattleUnitModel target = behavior.card.target;
            if (target == null || target.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_snowwhite_vine) == null || !IsAttackDice(behavior.Detail))
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                power = 1
            });
        }
        public class VineSeeker: BattleUnitBuf
        {
            public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int currentSlot)
            {
                List<BattleUnitModel> vine = BattleObjectManager.instance.GetAliveList(Faction.Player).FindAll(x => x.bufListDetail.GetActivatedBufList().Exists(y => y is BattleUnitBuf_snowwhite_vine));
                if (vine.Count <= 0 && currentSlot>_owner.speedDiceResult.Count/2)
                    return base.ChangeAttackTarget(card,currentSlot);
                return RandomUtil.SelectOne(vine);
            }
        }
        public class BattleUnitBuf_snowwhite_vine : BattleUnitBuf
        {
            private Battle.CreatureEffect.CreatureEffect _aura;
            public override string keywordId => "Snowwhite_Vine";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                this._aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("1_M/FX_IllusionCard_1_M_Vine", 1f, owner.view, owner.view);
                SoundEffectPlayer.PlaySound("Creature/SnowWhite_StongAtk_Ready");
            }
            public override void OnDie()
            {
                base.OnDie();
                this.Destroy();
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
            public override void Destroy()
            {
                base.Destroy();
                this.DestroyAura();
            }
            public void DestroyAura()
            {
                if (!((UnityEngine.Object)this._aura != (UnityEngine.Object)null))
                    return;
                UnityEngine.Object.Destroy((UnityEngine.Object)this._aura.gameObject);
                this._aura = (Battle.CreatureEffect.CreatureEffect)null;
            }
        }
    }
}