using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Threading.Tasks;

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
    public int GetDiceVanillaMax() => this.behaviourInCard.Dice;
    public int GetDiceVanillaMin() => this.behaviourInCard.Min;
    public int GetDiceMin() => Mathf.Max(1, this.behaviourInCard.Min + this._statBonus.min);
    public int GetDiceMax() => Mathf.Max(1, this.behaviourInCard.Dice + this._statBonus.face * 3 + this._statBonus.max);
    public BattleVoidBehaviour(BattleDiceBehavior OriginalDice)
    {
        this.OriginalDice = OriginalDice;
        this.owner = OriginalDice.owner;
        this.behaviourInCard =OriginalDice.behaviourInCard;
        this.abilityList = OriginalDice.abilityList;
        this.card = OriginalDice.card;
        if (OriginalDice.TargetDice != null && OriginalDice.TargetDice.Detail == BehaviourDetail.Guard)
            _damageReductionByGuard = OriginalDice.TargetDice.DiceResultValue;
        else
            _damageReductionByGuard = 0;
        _statBonus = typeof(BattleDiceBehavior).GetField("_statBonus", AccessTools.all).GetValue(this.OriginalDice) as DiceStatBonus;
        OriginalStatBonus = _statBonus.Copy();
    }
    public void RefreshStatBonus()
    {
        _statBonus = typeof(BattleDiceBehavior).GetField("_statBonus", AccessTools.all).GetValue(this.OriginalDice) as DiceStatBonus;
        typeof(BattleDiceBehavior).GetField("_statBonus", AccessTools.all).SetValue(this.OriginalDice, OriginalStatBonus);
    }
    public void BeforeRollDice()
    {
        OriginalDice.OnEventDiceAbility(DiceCardAbilityBase.DiceCardPassiveType.BeforeRollDice);
        this.RefreshStatBonus();
    }
    public void RollDice()
    {
        this._diceResultValue = DiceStatCalculator.MakeDiceResult(this.GetDiceMin(), this.GetDiceMax(), 0);
        this.owner.passiveDetail.ChangeDiceResult(OriginalDice, ref this._diceResultValue);
        this.owner.emotionDetail.ChangeDiceResult(OriginalDice, ref this._diceResultValue);
        if (this._diceResultValue < this.GetDiceMin())
            this._diceResultValue = this.GetDiceMin();
        if (this._diceResultValue < 1)
            this._diceResultValue = 1;
        OriginalDice.OnEventDiceAbility(DiceCardAbilityBase.DiceCardPassiveType.RollDice);
        this.RefreshStatBonus();
    }
    public void UpdateDiceFinalValue()
    {
        this._diceFinalResultValue = Mathf.Max(1, _diceResultValue);
        if (this.abilityList.Exists((Predicate<DiceCardAbilityBase>)(x => x.Invalidity)))
        {
            this._diceFinalResultValue = 0;
        }
        else
        {
            if (this._statBonus.ignorePower)
                return;
            if (card != null && card.ignorePower)
                return;
            if (this.owner != null && this.owner.IsNullifyPower())
                return;
            int power = this._statBonus.power;
            if (this.abilityList.Find((Predicate<DiceCardAbilityBase>)(x => x.IsDoublePower())) != null)
                power += this._statBonus.power;
            if (this.owner != null && this.owner.IsHalfPower())
                power /= 2;
            this._diceFinalResultValue = Mathf.Max(1, _diceResultValue + power);
        }
    }
    public void GiveDamage(BattleUnitModel target)
    {
        if (target == null)
            return;
        DiceStatBonus diceStatBonus = this._statBonus.Copy();
        this.abilityList.ForEach((Action<DiceCardAbilityBase>)(x => x.BeforeGiveDamage()));
        this.abilityList.ForEach((Action<DiceCardAbilityBase>)(x => x.BeforeGiveDamage(target)));
        this.owner.BeforeGiveDamage(OriginalDice);
        this.RefreshStatBonus();
        double num1 = ((double)(_diceFinalResultValue - this._damageReductionByGuard + this._statBonus.dmg + this.owner.UnitData.unitData.giftInventory.GetStatBonus_Dmg(this.behaviourInCard.Detail)) - (double)target.GetDamageReduction(OriginalDice)) * (1.0 + (double)(this._statBonus.dmgRate + target.GetDamageIncreaseRate()) / 100.0) * (1.0 + (double)target.GetDamageRate() / 100.0);
        double num2 = ((double)(_diceFinalResultValue - this._damageReductionByGuard + this._statBonus.breakDmg) - (double)target.GetBreakDamageReduction(OriginalDice)) * (1.0 + (double)(this._statBonus.breakRate + target.GetBreakDamageIncreaseRate()) / 100.0) * (1.0 + (double)target.GetBreakDamageRate() / 100.0);
        Vector3 normalized = (target.view.WorldPosition - this.owner.view.WorldPosition).normalized;
        AtkResist resistHp = target.GetResistHP(this.behaviourInCard.Detail);
        AtkResist resistBp = target.GetResistBP(this.behaviourInCard.Detail);
        bool flag = false;
        foreach (DiceCardAbilityBase ability in this.abilityList)
        {
            if (ability.IsPercentDmg)
            {
                flag = true;
                break;
            }
        }
        int dmg;
        int breakdmg;
        if (flag)
        {
            int num3 = int.MaxValue;
            foreach (DiceCardAbilityBase ability in this.abilityList)
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
            dmg =(int)(num1 * (double)BookModel.GetResistRate(resistHp));
        breakdmg = (int)(num2 * (double)BookModel.GetResistRate(resistBp));
        dmg = (int)target.ChangeDamage(this.owner, (double)dmg);
        if (this.card != null && this.card.cardAbility != null && this.card.cardAbility.IsTrueDamage())
        {
            dmg = _diceFinalResultValue;
            breakdmg = _diceFinalResultValue;
        }
        if (dmg < 1)
            dmg = 0;
        if (breakdmg < 1)
            breakdmg = 0;
        if (this.card.card.GetSpec().Ranged == CardRange.Far && target.passiveDetail.IsImmuneByFarAtk())
        {
            dmg = 0;
            breakdmg = 0;
        }
        if (this.abilityList.Exists((Predicate<DiceCardAbilityBase>)(x => x.Invalidity)))
        {
            dmg = 0;
            breakdmg = 0;
        }
        if (target.passiveDetail.IsInvincible())
        {
            dmg = 0;
            breakdmg = 0;
        }
        int damage = target.TakeDamage(dmg, attacker: this.owner);
        this.owner.emotionDetail.CheckDmg(damage, target);
        this.owner.passiveDetail.AfterGiveDamage(damage);
        target.TakeBreakDamage(breakdmg, attacker: this.owner, atkResist: resistBp);
        this.owner.battleCardResultLog.SetDamageGiven(damage);
        this.owner.battleCardResultLog.SetBreakDmgGiven(breakdmg);
        target.battleCardResultLog.SetDeathState(target.IsDead());
        this.owner.history.damageAtOneRoundByDice += damage;
        if (target.faction == Faction.Player)
        {
            if (this.owner.UnitData.unitData.EnemyUnitId == 20003 || this.owner.UnitData.unitData.EnemyUnitId == 20002)
                ++target.UnitData.historyInStage.damagedByIsadoraJulia;
            if (target.Book.GetBookClassInfoId() == 200004)
                ++target.UnitData.unitData.history.damagedFinnBook;
        }
        this.RefreshStatBonus();
    }
}