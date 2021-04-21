using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;
using System.IO;

namespace EmotionalFix
{
    public class EmotionCardAbility_silence3 : EmotionCardAbilityBase
    {
        private BattleDiceCardModel silence;
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            if(this._owner.faction==Faction.Player)
                this.GiveCards();
            if (this._owner.faction == Faction.Enemy)
                this.Silence();
        }

        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            if (this._owner.faction == Faction.Player)
                this.GiveCards();
            if (this._owner.faction == Faction.Enemy)
                this.Silence();
        }

        private void GiveCards()
        {
            silence=this._owner.allyCardDetail.AddNewCard(1108101);
        }
        public void Destroy()
        {
            if (this._owner.faction == Faction.Player)
                this._owner.allyCardDetail.ExhaustACardAnywhere(silence);
        }
        public void Silence()
        {
            List<BattleUnitModel> nonstun = new List<BattleUnitModel>();
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(Faction.Player))
            {
                if (unit.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is SilenceStun)) == null)
                    nonstun.Add(unit);
            }
            if (nonstun.Count <= 0)
                return;
            BattleUnitModel victim = RandomUtil.SelectOne<BattleUnitModel>(nonstun);
            victim.bufListDetail.AddBuf(new SilenceStun());
            this._owner.bufListDetail.AddBuf(new SilenceStun());
        }
        public class SilenceStun : BattleUnitBuf
        {
            public override bool IsActionable() => false;
            public override int SpeedDiceBreakedAdder() => 10;
            protected override string keywordId => "Stun";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                this.stack = 0;
                SingletonBehavior<DiceEffectManager>.Instance.CreateBufEffect("BufEffect_Disarm", owner.view);
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }
    }
}
