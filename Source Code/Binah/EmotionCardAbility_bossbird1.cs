using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;

namespace EmotionalFix
{
    public class EmotionCardAbility_bossbird1 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            if (_owner.faction == Faction.Player)
            {
                _owner.bufListDetail.AddBuf(new LongBird());
                GameObject gameObject = Util.LoadPrefab("Battle/CreatureEffect/FinalBattle/BinahFinalBattle_ImageFilter");
                if (!(gameObject != null))
                    return;
                Creature_Final_Binah_ImageFilter component = gameObject?.GetComponent<Creature_Final_Binah_ImageFilter>();
                if (component != null)
                    component.Init(3);
                gameObject.AddComponent<AutoDestruct>().time = 10f;
            }
            if (_owner.faction == Faction.Enemy)
                _owner.bufListDetail.AddBuf(new Longbird_Enemy());
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            if (_owner.faction == Faction.Player)
                _owner.bufListDetail.AddBuf(new LongBird());
            if (_owner.faction == Faction.Enemy)
                _owner.bufListDetail.AddBuf(new Longbird_Enemy());
        }
        public void Destroy()
        {
            BattleUnitBuf Buff= _owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is LongBird));
            if(Buff!=null)
                Buff.Destroy();
            Buff = _owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Longbird_Enemy));
            if (Buff != null)
                Buff.Destroy();
        }
        public class LongBird : BattleUnitBuf
        {
            public override string keywordIconId => "ApocalypseBird_LongArm";
            public override string keywordId => "Longbird";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (!IsAttackDice(behavior.Detail))
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    dmgRate = -30,
                    breakRate=-30
                });
            }
            public override void OnSuccessAttack(BattleDiceBehavior behavior)
            {
                base.OnSuccessAttack(behavior);
                int dmg = (int)(behavior.card.target.MaxHp * 0.1);
                behavior.card.target.TakeDamage(Math.Min(dmg, 10), DamageType.Emotion);
            }
        }
        public class Longbird_Enemy: BattleUnitBuf
        {
            public override string keywordIconId => "ApocalypseBird_LongArm";
            public override string keywordId => "Longbird_Enemy";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
                _owner.bufListDetail.RemoveBufAll(BufPositiveType.Negative);
            }
            public override bool IsImmune(BattleUnitBuf buf)
            {
                return buf.positiveType == BufPositiveType.Negative || base.IsImmune(buf);
            }
        }
    }
}
