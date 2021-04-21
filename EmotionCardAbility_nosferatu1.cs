using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_nosferatu1 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            this._owner.bufListDetail.AddBuf(new FearOfWater());
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this._owner.bufListDetail.AddBuf(new FearOfWater());
        }
        private int AddDmg => RandomUtil.Range(3, 7);
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (!this.IsAttackDice(behavior.Detail))
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                dmg = AddDmg
            });
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            behavior?.card?.target?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, 2, this._owner);
        }
        public void Destroy()
        {
            if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is FearOfWater)) is FearOfWater fear)
                fear.Destroy();
        }
        public class FearOfWater : BattleUnitBuf
        {
            protected override string keywordId => "FearOfWater";
            protected override string keywordIconId => "Nosferatu_Blight";
            public override bool IsControllable => _owner.faction == Faction.Player ? false : true; 

            public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int currentSlot)
            {
                int num = 0;
                List<BattleUnitModel> battleUnitModelList = new List<BattleUnitModel>();
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
                {
                    if (alive.speedDiceResult.Count > 0 && alive.IsTargetable(this._owner))
                    {
                        int hp = (int)alive.hp;
                        if (hp > num)
                        {
                            battleUnitModelList.Clear();
                            battleUnitModelList.Add(alive);
                            num = hp;
                        }
                        else if (hp == num)
                            battleUnitModelList.Add(alive);
                    }
                }
                return battleUnitModelList.Count > 0 ? RandomUtil.SelectOne<BattleUnitModel>(battleUnitModelList) : base.ChangeAttackTarget(card,currentSlot);
            }
        }
    }
}

