using System;
using Sound;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_queenbee2 : EmotionCardAbilityBase
    {
        private Dictionary<BattleUnitModel, int> dmgData = new Dictionary<BattleUnitModel, int>();
        public override void OnSelectEmotion()
        {
            BattleUnitBuf attacker = this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_queenbee_attacker));
            if (attacker == null)
                return;
            attacker.Destroy();
        }
        public override void OnStartBattle()
        {
            if (this._owner.faction != Faction.Player)
                return;
            foreach (BattlePlayingCardDataInUnitModel card in this._owner.cardSlotDetail.cardAry)
            {
                if (card != null && card.target != null)
                {
                    card.target.bufListDetail.AddBuf(new BattleUnitBuf_queenbee_punish());
                    SoundEffectPlayer.PlaySound("Creature/QueenBee_AtkMode");
                    return;
                }
            }      
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (_owner.faction == Faction.Enemy)
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 1 });
        }
        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            base.OnTakeDamageByAttack(atkDice, dmg);
            BattleUnitModel owner = atkDice.owner;
            if (owner == null || owner.faction != Faction.Player)
                return;
            if (!this.dmgData.ContainsKey(owner))
                this.dmgData.Add(owner, dmg);
            else
                this.dmgData[owner] += dmg;
        }
        public override void OnRoundStart()
        {
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Player : Faction.Enemy))
            {
                if (unit.bufListDetail.GetActivatedBufList().Exists((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_queenbee_attacker)))
                    continue;
                if (unit.emotionDetail.PassiveList.Exists((Predicate<BattleEmotionCardModel>)(x => (x.XmlInfo.Name == "QueenBee_Bee"))) || unit.emotionDetail.PassiveList.Exists((Predicate<BattleEmotionCardModel>)(x => (x.XmlInfo.Name == "QueenBee_Bee_Enemy"))))
                    continue;
                unit.bufListDetail.AddBuf(new BattleUnitBuf_queenbee_attacker());
            }
            base.OnRoundStart();
            if (this._owner.faction != Faction.Enemy)
                return;
            if (this.dmgData.Count > 0)
            {
                int num = 0;
                BattleUnitModel battleUnitModel = (BattleUnitModel)null;
                foreach (KeyValuePair<BattleUnitModel, int> keyValuePair in this.dmgData)
                {
                    if (keyValuePair.Value > num && !keyValuePair.Key.IsDead())
                    {
                        num = keyValuePair.Value;
                        battleUnitModel = keyValuePair.Key;
                    }
                }
                battleUnitModel?.bufListDetail.AddBuf(new BattleUnitBuf_queenbee_punish());
                SoundEffectPlayer.PlaySound("Creature/QueenBee_AtkMode");
            }
            this.dmgData.Clear();
        }
        public void Destroy()
        {
            if (this._owner.faction == Faction.Player)
            {
                foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(Faction.Player))
                {
                    if (unit.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_queenbee_attacker)) is BattleUnitBuf_queenbee_attacker attacker)
                        attacker.Destroy();
                }
            }
            if (this._owner.faction == Faction.Enemy)
            {
                bool mulitqueen = false;
                foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
                {
                    if (SearchEmotion(unit, "QueenBee_Bee_Enemy") != null)
                        mulitqueen = true;
                }
                if (mulitqueen == true)
                    this._owner.bufListDetail.AddBuf(new BattleUnitBuf_queenbee_attacker());
                else
                {
                    foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
                    {
                        if (unit.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_queenbee_attacker)) is BattleUnitBuf_queenbee_attacker attacker)
                            attacker.Destroy();
                    }
                }
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
        public class BattleUnitBuf_queenbee_punish : BattleUnitBuf
        {
            private Battle.CreatureEffect.CreatureEffect _aura;
            protected override string keywordId => "Queenbee_Punish";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                this._aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("1_M/FX_IllusionCard_1_M_BeeMark", 1f, owner.view, owner.view);
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
                DestroyAura();
            }
            public void DestroyAura()
            {
                if (!((UnityEngine.Object)this._aura != (UnityEngine.Object)null))
                    return;
                UnityEngine.Object.Destroy((UnityEngine.Object)this._aura.gameObject);
                this._aura = (Battle.CreatureEffect.CreatureEffect)null;
            }
        }
        public class BattleUnitBuf_queenbee_attacker : BattleUnitBuf
        {
            private static int Dmg => RandomUtil.Range(2, 4);
            protected override string keywordId => "Queenbee_Punish";
            public override bool Hide => true;
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                BattleUnitModel target = behavior?.card?.target;
                if (target == null || target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_queenbee_punish)) == null)
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    dmg = Dmg
                });
            }
        }
    }
}
