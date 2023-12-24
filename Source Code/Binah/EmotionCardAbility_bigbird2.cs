﻿using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;

namespace EmotionalFix
{
    public class EmotionCardAbility_bigbird2 : EmotionCardAbilityBase
    {
        public override bool CanForcelyAggro()
        {
            if (_owner.faction != Faction.Player)
                return base.CanForcelyAggro();
            return true;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("8_B/FX_IllusionCard_8_B_Lamp", 1f, _owner.view, _owner.view, 3f);
            SoundEffectPlayer.PlaySound("Creature/Bigbird_Attract");
            if (_owner.faction != Faction.Enemy)
                return;
            BattleUnitModel salvation = RandomUtil.SelectOne<BattleUnitModel>(BattleObjectManager.instance.GetAliveList(Faction.Player));
            salvation.bufListDetail.AddBuf(new Charm(_owner));
        }
        public class Charm: BattleUnitBuf
        {
            public override string keywordId => "Charm";
            public override string keywordIconId => "BigBird_Charm";
            private BattleUnitModel BigBird;
            public Charm(BattleUnitModel bigbird)
            {
                BigBird = bigbird;
                stack = 0;
            }
            public override List<BattleUnitModel> GetFixedTarget()
            {
                List<BattleUnitModel> list = new List<BattleUnitModel>();
                if (BigBird != null)
                    list.Add(BigBird);
                return list;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
    }
}
