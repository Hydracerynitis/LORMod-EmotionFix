using System;
using System.Collections.Generic;
using LOR_DiceSystem;
using Sound;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_snowwhite1: EmotionCardAbilityBase
    {
        private static int Bind => RandomUtil.Range(6, 6);
        private static int Pow => RandomUtil.Range(1, 2);
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (this._owner.faction == Faction.Enemy)
            {
                if (!this._owner.bufListDetail.GetActivatedBufList().Exists((Predicate<BattleUnitBuf>)(x => x is VineSeeker)))
                    this._owner.bufListDetail.AddBuf(new VineSeeker());
            }
            List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this._owner.faction==Faction.Player? Faction.Enemy: Faction.Player);
            foreach(BattleUnitModel unit in aliveList)
            {
                if (unit.bufListDetail.GetActivatedBufList().Exists((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_snowwhite_vine)))
                    aliveList.Remove(unit);
            }
            if (aliveList.Count <= 0)
                return;
            BattleUnitModel victim = RandomUtil.SelectOne<BattleUnitModel>(aliveList);
            victim.bufListDetail.AddBuf(new BattleUnitBuf_snowwhite_vine());
            victim.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Binding, Bind, this._owner);
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            BattleUnitModel target = behavior.card.target;
            if (target == null || target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_snowwhite_vine)) == null)
                return;
            if (this._owner.faction == Faction.Player && behavior.Detail == BehaviourDetail.Penetrate)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = Pow
                });
            }
            if (this._owner.faction == Faction.Enemy && IsAttackDice(behavior.Detail))
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = Pow
                });
            }
        }
        public class VineSeeker: BattleUnitBuf
        {
            public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int currentSlot)
            {
                List<BattleUnitModel> vine = BattleObjectManager.instance.GetAliveList(Faction.Player).FindAll((Predicate<BattleUnitModel>)(x => x.bufListDetail.GetActivatedBufList().Exists((Predicate<BattleUnitBuf>)(y => y is BattleUnitBuf_snowwhite_vine))));
                if (vine.Count <= 0 && currentSlot>this._owner.speedDiceResult.Count/2)
                    return base.ChangeAttackTarget(card,currentSlot);
                return RandomUtil.SelectOne<BattleUnitModel>(vine);
            }
        }
        public class BattleUnitBuf_snowwhite_vine : BattleUnitBuf
        {
            private Battle.CreatureEffect.CreatureEffect _aura;
            protected override string keywordId => "Snowwhite_Vine";
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
