using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_knightofdespair1 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            if (this._owner.faction != Faction.Player)
                return;
            BattleUnitBuf Gaho = new BattleUnitBuf_Gaho
            {
                stack = 2
            };
            Weakest(BattleObjectManager.instance.GetAliveList(this._owner.faction)).bufListDetail.AddBuf(Gaho);
            if (!(this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_knightofdespair3.Tear)) is EmotionCardAbility_knightofdespair3.Tear tear))
            {
                EmotionCardAbility_knightofdespair3.Tear Tear = new EmotionCardAbility_knightofdespair3.Tear
                {
                    stack=0,
                    reservePlus = 1
                };
                this._owner.bufListDetail.AddBuf(Tear);
            }
            else
            {
                tear.reservePlus += 1;
            }
            new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/KnightOfDespair_Gaho", false, 2f);
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/KnightOfDespair_Gaho")?.SetGlobalPosition(this._owner.view.WorldPosition);
        }
        public override void OnRoundStart()
        {
            if (this._owner.faction != Faction.Enemy)
                return;
            BattleUnitBuf Gaho = new BattleUnitBuf_Gaho
            {
                stack = 1
            };
            Weakest(BattleObjectManager.instance.GetAliveList(this._owner.faction)).bufListDetail.AddBuf(Gaho);
            new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/KnightOfDespair_Gaho", false, 2f);
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/KnightOfDespair_Gaho")?.SetGlobalPosition(this._owner.view.WorldPosition);
        }
        private BattleUnitModel Weakest(List<BattleUnitModel> list)
        {
            if (list.Count == 0)
                return null;
            int num = 10000;
            List<BattleUnitModel> battleUnitModelList = new List<BattleUnitModel>();
            foreach (BattleUnitModel alive in list)
            {
                int hp = (int)alive.hp;
                if (hp < num)
                {
                    battleUnitModelList.Clear();
                    battleUnitModelList.Add(alive);
                    num = hp;
                }
                else if (hp == num)
                    battleUnitModelList.Add(alive);
            }
            if (battleUnitModelList.Count == 0)
                return null;
            return RandomUtil.SelectOne<BattleUnitModel>(battleUnitModelList);
        }
        public void Destroy()
        {
            if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is EmotionCardAbility_knightofdespair3.Tear)) is EmotionCardAbility_knightofdespair3.Tear Tear)
                Tear.Destroy();
            foreach(BattleUnitModel ally in BattleObjectManager.instance.GetAliveList(this._owner.faction))
            {
                if (ally.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Gaho)) is BattleUnitBuf_Gaho Gaho)
                    Gaho.Destroy();
            }
        }
        public class BattleUnitBuf_Gaho : BattleUnitBuf
        {
            protected override string keywordId => "Gaho";
            public override int GetDamageIncreaseRate() => -30;
            public override int GetBreakDamageIncreaseRate() => -30;
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.stack -= 1;
                if(stack<=0)
                    this.Destroy();
            }
        }
    }
}
