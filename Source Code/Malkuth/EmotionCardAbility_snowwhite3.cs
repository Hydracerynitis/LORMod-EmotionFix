using System;
using UnityEngine;
using System.Reflection;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using LOR_DiceSystem;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_snowwhite3: EmotionCardAbilityBase
    {
        private Dictionary<BattleUnitModel, int> dmgData = new Dictionary<BattleUnitModel, int>();
        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            base.OnTakeDamageByAttack(atkDice, dmg);
            BattleUnitModel owner = atkDice.owner;
            if (owner == null)
                return;
            if (!this.dmgData.ContainsKey(owner))
                this.dmgData.Add(owner, dmg);
            else
                this.dmgData[owner] += dmg;
        }
        public override void OnRoundEnd()
        {
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
                if (!(battleUnitModel.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Malice_Enemy)) is Malice_Enemy malice))
                {
                    Malice_Enemy Malice = new Malice_Enemy();
                    battleUnitModel.bufListDetail.AddBuf(Malice);
                }
                else
                    malice.stack += 1;
            }
            else
            {
                List<BattleUnitModel> enemy = BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player);
                if (enemy.Count == 0)
                    return;
                BattleUnitModel victim=RandomUtil.SelectOne<BattleUnitModel>(enemy);
                if (!(victim.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Malice_Enemy)) is Malice_Enemy malice))
                {
                    Malice_Enemy Malice = new Malice_Enemy();
                    victim.bufListDetail.AddBuf(Malice);
                }
                else
                    malice.stack += 1;
            }
            this.dmgData.Clear();
            new GameObject().AddComponent<SpriteFilter_Queenbee_Spore>().Init("EmotionCardFilter/SnowWhite_Filter_Vine", false, 2f);
        }
        public override void OnStartBattle()
        {
            int num = 0;
            List<BattleUnitModel> victim = new List<BattleUnitModel>();
            foreach (BattleUnitModel enemy in BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
            {
                if (enemy.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_binding)) is BattleUnitBuf_binding bind)
                {
                    num += bind.stack;
                    victim.Add(enemy);
                }
            }
            if (victim.Count == 0)
                return;
            DiceCardXmlInfo xml = ItemXmlDataList.instance.GetCardItem(1101501).Copy(true);
            DiceBehaviour dice = xml.DiceBehaviourList[0];
            dice.Dice = dice.Dice += num;
            BattleDiceCardModel Aoe = BattleDiceCardModel.CreatePlayingCard(xml);
            BattleUnitModel target = RandomUtil.SelectOne<BattleUnitModel>(victim);
            victim.Remove(target);
            BattlePlayingCardDataInUnitModel Card = new BattlePlayingCardDataInUnitModel
            {
                owner = this._owner,
                card = Aoe,
                cardAbility = Aoe.CreateDiceCardSelfAbilityScript(),
                target = target,
                targetSlotOrder = RandomUtil.Range(0, target.cardSlotDetail.cardAry.Count - 1),
                slotOrder = RandomUtil.Range(0, this._owner.cardSlotDetail.cardAry.Count - 1)
            };
            foreach (BattleUnitModel battleUnitModel in victim)
            {
                if (battleUnitModel != target && battleUnitModel.IsTargetable(this._owner))
                {
                    BattlePlayingCardSlotDetail cardSlotDetail = battleUnitModel.cardSlotDetail;
                    int num1;
                    if (cardSlotDetail == null)
                    {
                        num1 = 0;
                    }
                    else
                    {
                        int? count = cardSlotDetail.cardAry?.Count;
                        int num2 = 0;
                        num1 = count.GetValueOrDefault() > num2 & count.HasValue ? 1 : 0;
                    }
                    if (num1 != 0)
                        Card.subTargets.Add(new BattlePlayingCardDataInUnitModel.SubTarget()
                        {
                            target = battleUnitModel,
                            targetSlotOrder = UnityEngine.Random.Range(0, battleUnitModel.speedDiceResult.Count)
                        });
                }
            }
            Card.ResetExcludedDices();
            Card.ResetCardQueueWithoutStandby();
            List<BattlePlayingCardDataInUnitModel> cardlist = typeof(StageController).GetField("_allCardList", AccessTools.all).GetValue(Singleton<StageController>.Instance) as List<BattlePlayingCardDataInUnitModel>;
            cardlist.Add(Card);
        }
        public class Malice_Enemy: BattleUnitBuf
        {
            protected override string keywordId => "Malice_Enemy";
            protected override string keywordIconId => "Snowwhite_Vine";
            public override void OnRoundStart()
            {
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Binding, this.stack);
            }
        }
    }
}
