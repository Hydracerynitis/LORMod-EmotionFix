using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_nosferatu2 : EmotionCardAbilityBase
    {
        public override void OnRoundStart()
        {
            int wine = (int)(this._owner.hp * 0.1);
            this._owner.TakeDamage(wine);
            foreach (BattleUnitModel ally in BattleObjectManager.instance.GetAliveList(this._owner.faction).FindAll((Predicate<BattleUnitModel>)(x => x != this._owner)))
                ally.RecoverHP(wine);
        }
    }
}

