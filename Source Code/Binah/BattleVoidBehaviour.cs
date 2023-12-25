using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class BattleVoidBehaviour
    {
        public DiceBehaviour behaviourInCard;
        public BattleDiceBehavior OriginalDice;
        public BattleUnitModel owner;
        public BattlePlayingCardDataInUnitModel card;
        public int _damageReductionByGuard;
        public List<DiceCardAbilityBase> abilityList;
        public DiceStatBonus _statBonus;
        public DiceStatBonus OriginalStatBonus;
        public int _diceResultValue;
        public int _diceFinalResultValue;
        public int GetDiceVanillaMax() => behaviourInCard.Dice;
        public int GetDiceVanillaMin() => behaviourInCard.Min;
        public int GetDiceMin() => Mathf.Max(1, behaviourInCard.Min + _statBonus.min);
        public int GetDiceMax() => Mathf.Max(1, behaviourInCard.Dice + _statBonus.face * 3 + _statBonus.max);
        public BattleVoidBehaviour(BattleDiceBehavior OriginalDice)
        {
            OriginalDice = OriginalDice;
            owner = OriginalDice.owner;
            behaviourInCard = OriginalDice.behaviourInCard;
            abilityList = OriginalDice.abilityList;
            card = OriginalDice.card;
            if (OriginalDice.TargetDice != null && OriginalDice.TargetDice.Detail == BehaviourDetail.Guard)
                _damageReductionByGuard = OriginalDice.TargetDice.DiceResultValue;
            else
                _damageReductionByGuard = 0;
            _statBonus = typeof(BattleDiceBehavior).GetField("_statBonus", AccessTools.all).GetValue(OriginalDice) as DiceStatBonus;
            OriginalStatBonus = _statBonus.Copy();
        }
        public void RefreshStatBonus()
        {
            _statBonus = typeof(BattleDiceBehavior).GetField("_statBonus", AccessTools.all).GetValue(OriginalDice) as DiceStatBonus;
            typeof(BattleDiceBehavior).GetField("_statBonus", AccessTools.all).SetValue(OriginalDice, OriginalStatBonus);
        }
        public void BeforeRollDice()
        {
            OriginalDice.OnEventDiceAbility(DiceCardAbilityBase.DiceCardPassiveType.BeforeRollDice);
            RefreshStatBonus();
        }
        public void RollDice()
        {
            _diceResultValue = DiceStatCalculator.MakeDiceResult(GetDiceMin(), GetDiceMax(), 0);
            owner.passiveDetail.ChangeDiceResult(OriginalDice, ref _diceResultValue);
            owner.emotionDetail.ChangeDiceResult(OriginalDice, ref _diceResultValue);
            owner.bufListDetail.ChangeDiceResult(OriginalDice, ref _diceResultValue);
            if (_diceResultValue < GetDiceMin())
                _diceResultValue = GetDiceMin();
            if (_diceResultValue < 1)
                _diceResultValue = 1;
            OriginalDice.OnEventDiceAbility(DiceCardAbilityBase.DiceCardPassiveType.RollDice);
            RefreshStatBonus();
        }
        public void UpdateDiceFinalValue()
        {
            _diceFinalResultValue = Mathf.Max(1, _diceResultValue);
            if (abilityList.Exists((Predicate<DiceCardAbilityBase>)(x => x.Invalidity)))
            {
                _diceFinalResultValue = 0;
            }
            else
            {
                if (_statBonus.ignorePower)
                    return;
                if (card != null && card.ignorePower)
                    return;
                if (owner != null && owner.IsNullifyPower())
                    return;
                int power = _statBonus.power;
                if (abilityList.Find((Predicate<DiceCardAbilityBase>)(x => x.IsDoublePower())) != null)
                    power += _statBonus.power;
                if (owner != null && owner.IsHalfPower())
                    power /= 2;
                _diceFinalResultValue = Mathf.Max(1, _diceResultValue + power);
            }
        }
        public void GiveDamage(BattleUnitModel target)
        {
            if (target == null)
                return;
            abilityList.ForEach((Action<DiceCardAbilityBase>)(x => x.BeforeGiveDamage()));
            abilityList.ForEach((Action<DiceCardAbilityBase>)(x => x.BeforeGiveDamage(target)));
            owner.BeforeGiveDamage(OriginalDice);
            if (OriginalDice.IsBlocked)
            {
                RefreshStatBonus();
                owner.battleCardResultLog.SetIsBlocked(true);
                return;
            }
            double num1 = ((double)(_diceFinalResultValue - _damageReductionByGuard + _statBonus.dmg + owner.UnitData.unitData.giftInventory.GetStatBonus_Dmg(behaviourInCard.Detail)) - (double)target.GetDamageReduction(OriginalDice)) * (double)Mathf.Max((1.0f + (float)(_statBonus.dmgRate + target.GetDamageIncreaseRate()) / 100.0f), 0.0f) * (double)target.GetDamageRate();
            double num2 = ((double)(_diceFinalResultValue - _damageReductionByGuard + _statBonus.breakDmg) - (double)target.GetBreakDamageReduction(OriginalDice)) * (double)Mathf.Max((1.0f + (float)(_statBonus.breakRate + target.GetBreakDamageIncreaseRate()) / 100.0f), 0f) * (double)target.GetBreakDamageRate();
            if (target.emotionDetail.IsTakeDamageDouble())
                num1 *= 2.0;
            if (owner.emotionDetail.IsGiveDamageDouble())
                num1 *= 2.0;
            AtkResist resistHp = target.GetResistHP(behaviourInCard.Detail);
            AtkResist resistBp = target.GetResistBP(behaviourInCard.Detail);
            bool flag = false;
            foreach (DiceCardAbilityBase ability in abilityList)
            {
                if (ability.IsPercentDmg)
                {
                    flag = true;
                    break;
                }
            }
            double dmg;
            double breakdmg;
            if (flag)
            {
                int num3 = int.MaxValue;
                foreach (DiceCardAbilityBase ability in abilityList)
                {
                    int maximumPercentDmg = ability.GetMaximumPercentDmg();
                    if (maximumPercentDmg < num3)
                        num3 = maximumPercentDmg;
                }
                dmg = (int)((double)target.MaxHp * (num1 / 100.0));
                if (dmg >= target.MaxHp)
                    dmg = target.MaxHp;
                dmg = dmg * 2;
                if (dmg > num3)
                    dmg = num3;
            }
            else
                dmg = (int)(num1 * (double)BookModel.GetResistRate(resistHp));
            breakdmg = (int)(num2 * (double)BookModel.GetResistRate(resistBp));
            dmg = (int)target.ChangeDamage(owner, (double)dmg);
            if (card != null && card.cardAbility != null && card.cardAbility.IsTrueDamage())
            {
                dmg = _diceFinalResultValue;
                breakdmg = _diceFinalResultValue;
            }
            if (dmg < 1)
                dmg = 0;
            if (breakdmg < 1)
                breakdmg = 0;
            if (card.card.GetSpec().Ranged == CardRange.Far && target.passiveDetail.IsImmuneByFarAtk())
            {
                dmg = 0;
                breakdmg = 0;
            }
            if (abilityList.Exists((Predicate<DiceCardAbilityBase>)(x => x.Invalidity)))
            {
                dmg = 0;
                breakdmg = 0;
            }
            if (target.passiveDetail.IsInvincible())
            {
                dmg = 0;
                breakdmg = 0;
            }
            int damage = target.TakeDamage((int)dmg, attacker: owner);
            owner.emotionDetail.CheckDmg(damage, target);
            owner.passiveDetail.AfterGiveDamage(damage);
            target.TakeBreakDamage((int)breakdmg, attacker: owner, atkResist: resistBp);
            owner.battleCardResultLog.SetDamageGiven(damage);
            owner.battleCardResultLog.SetBreakDmgGiven((int)breakdmg);
            target.battleCardResultLog.SetDeathState(target.IsDead());
            owner.history.damageAtOneRoundByDice += damage;
            if (target.faction == Faction.Player)
            {
                if (owner.UnitData.unitData.EnemyUnitId == 20003 || owner.UnitData.unitData.EnemyUnitId == 20002)
                    ++target.UnitData.historyInStage.damagedByIsadoraJulia;
                if (target.Book.GetBookClassInfoId() == 200004)
                    ++target.UnitData.unitData.history.damagedFinnBook;
            }
            RefreshStatBonus();
        }
    }
}
