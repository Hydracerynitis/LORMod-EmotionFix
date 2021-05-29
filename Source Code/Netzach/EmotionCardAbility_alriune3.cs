using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_alriune3 : EmotionCardAbilityBase
    {
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this._owner.faction==Faction.Player? Faction.Enemy:Faction.Player);
            if (aliveList.Count <= 0)
                return;
            RandomUtil.SelectOne<BattleUnitModel>(aliveList).bufListDetail.AddBuf(new BattleUnitBuf_Emotion_Alriune(this._owner));
        }

        public class BattleUnitBuf_Emotion_Alriune : BattleUnitBuf
        {
            private Battle.CreatureEffect.CreatureEffect _aura;
            private BattleUnitModel _target;
            private int cnt;
            private static int BDmg => RandomUtil.Range(3, 7);
            protected override string keywordId => "Alriune_Flower";
            protected override string keywordIconId => "Alriune_Petal";
            public BattleUnitBuf_Emotion_Alriune(BattleUnitModel target)
            {
                this._target = target;
                this.stack = 0;
            }
            public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
            {
                base.OnTakeDamageByAttack(atkDice, dmg);
                BattleUnitModel owner = atkDice?.card?.owner;
                if (owner == null || owner != this._target || this.cnt >= 4)
                    return;
                ++this.cnt;
                if (this.cnt < 4)
                    return;
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
                    alive.TakeBreakDamage(EmotionCardAbility_alriune3.BattleUnitBuf_Emotion_Alriune.BDmg, DamageType.Emotion,this._owner);
                this._target?.bufListDetail.AddBuf((BattleUnitBuf)new EmotionCardAbility_alriune3.BattleUnitBuf_Emotion_Alriune2());
                this._target?.battleCardResultLog?.SetAfterActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.Curtain));
            }
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                this._aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("4_N/FX_IllusionCard_4_N_Spring", 1f, this._owner.view, this._owner.view);
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

            public void DestroyAura()
            {
                if (!((UnityEngine.Object)this._aura != (UnityEngine.Object)null))
                    return;
                UnityEngine.Object.Destroy((UnityEngine.Object)this._aura.gameObject);
                this._aura = (Battle.CreatureEffect.CreatureEffect)null;
            }
            public void Curtain()
            {
                Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/4_N/FX_IllusionCard_4_N_SpringAct");
                if ((UnityEngine.Object)original != (UnityEngine.Object)null)
                {
                    Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleManagerUI>.Instance.EffectLayer);
                    if ((UnityEngine.Object)creatureEffect?.gameObject.GetComponent<AutoDestruct>() == (UnityEngine.Object)null)
                    {
                        AutoDestruct autoDestruct = creatureEffect?.gameObject.AddComponent<AutoDestruct>();
                        if ((UnityEngine.Object)autoDestruct != (UnityEngine.Object)null)
                            autoDestruct.time = 3f;
                    }
                }
                SoundEffectPlayer.PlaySound("Creature/Ali_curtain");
            }
        }
        public class BattleUnitBuf_Emotion_Alriune2 : BattleUnitBuf
        {
            private bool added = true;
            protected override string keywordId => "NoTargeting";
            protected override string keywordIconId => "Alriune_Attacker";
            public override bool IsTargetable() => this.added;
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                if (this.added)
                    this.added = false;
                else
                    this.Destroy();
            }
        }
    }
}
