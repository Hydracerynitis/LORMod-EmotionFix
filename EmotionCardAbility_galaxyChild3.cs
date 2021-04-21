using System;
using LOR_DiceSystem;
using Battle.CameraFilter;
using Battle.CreatureEffect;
using System.Collections.Generic;
using Sound;
using UnityEngine;
using System.Collections;

namespace EmotionalFix
{
    public class EmotionCardAbility_galaxyChild3 : EmotionCardAbilityBase
    {
        private int _roundCount;
        private List<Battle.CreatureEffect.CreatureEffect> _recoverEffects = new List<Battle.CreatureEffect.CreatureEffect>();
        public override void OnRoundStart()
        {
            if (this._roundCount >= 3)
                return;
            ++this._roundCount;
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(this._owner.faction==Faction.Player? Faction.Player : Faction.Enemy))
            {
                if (this._owner.faction == Faction.Player)
                {
                    int num = (int)((double)unit.MaxHp * 0.15);
                    if (num >= 18)
                        num = 18;
                    unit.bufListDetail.AddBuf(new BattleUnitBuf_galaxyChild_Friend());
                    unit.RecoverHP(num);
                    unit.ShowTypoTemporary(this._emotionCard, 0, ResultOption.Default, num);
                    Battle.CreatureEffect.CreatureEffect creatureEffect = this.MakeEffect("4/GalaxyBoy_Recover", target: unit);
                    this._recoverEffects.Add(creatureEffect);
                    (creatureEffect as CreatureEffect_Hit).SetPerm();
                }
                if (this._owner.faction == Faction.Enemy)
                {
                    int num = (int)((double)unit.MaxHp * 0.1);
                    if (num >= 12)
                        num = 12;
                    unit.bufListDetail.AddBuf(new BattleUnitBuf_galaxyChild_Friend());
                    unit.RecoverHP(num);
                    unit.ShowTypoTemporary(this._emotionCard, 0, ResultOption.Default, num);
                    Battle.CreatureEffect.CreatureEffect creatureEffect = this.MakeEffect("4/GalaxyBoy_Recover", target: unit);
                    this._recoverEffects.Add(creatureEffect);
                    (creatureEffect as CreatureEffect_Hit).SetPerm();
                }
            }
        }
        public override void OnSelectEmotion() => this._owner.view.unitBottomStatUI.SetBufs();
        public override void OnDrawCard()
        {
            foreach (Battle.CreatureEffect.CreatureEffect recoverEffect in this._recoverEffects)
            {
                if ((bool)(UnityEngine.Object)recoverEffect)
                    recoverEffect.ManualDestroy();
            }
            this._recoverEffects.Clear();
        }
        public override void OnLayerChanged(string layerName)
        {
        }
        public override Sprite GetAbilityBufIcon() => Resources.Load<Sprite>("Sprites/BufIcon/Ability/GalaxyBoy_Stone");
        public class BattleUnitBuf_galaxyChild_Friend : BattleUnitBuf
        {
            protected override string keywordId => "GalaxyBoy_Stone";
            protected override string keywordIconId => "Ability/GalaxyBoy_Stone";
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }
    }
}
