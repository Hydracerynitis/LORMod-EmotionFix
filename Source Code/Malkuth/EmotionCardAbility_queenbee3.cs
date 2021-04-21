using System;
using UnityEngine;
using Sound;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_queenbee3 : EmotionCardAbilityBase
    {
        public override void OnRoundStart()
        {
            if (!this._owner.bufListDetail.GetActivatedBufList().Exists((Predicate<BattleUnitBuf>)(x => x is Queen)))
                this._owner.bufListDetail.AddBuf(new Queen());
        }
        public override void OnSelectEmotion()
        {
            this._owner.bufListDetail.AddBuf(new Queen());
        }
        public void Destroy()
        {
            if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Queen)) is Queen queen)
                queen.Destroy();
        }
        public class Queen : BattleUnitBuf
        {
            private bool Trigger => BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Player : Faction.Enemy).Exists((Predicate<BattleUnitModel>)(x => x != this._owner));
            public override int SpeedDiceBreakedAdder()
            {
                if (Trigger)
                    return 10;
                return base.SpeedDiceBreakedAdder();
            }
            public override void OnRoundStart()
            {
                if (!Trigger)
                    return;
                new GameObject().AddComponent<SpriteFilter_Queenbee_Spore>().Init("EmotionCardFilter/QueenBee_Filter_Spore", false, 2f);
                SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/QueenBee_Funga")?.SetGlobalPosition(this._owner.view.WorldPosition);
                foreach(BattleUnitModel worker in BattleObjectManager.instance.GetAliveList(this._owner.faction==Faction.Player? Faction.Player:Faction.Enemy).FindAll((Predicate<BattleUnitModel>)(x => x != this._owner)))
                {
                    worker.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 2);
                    worker.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, 2);
                    worker.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, 2);
                }
            }
        }
    }

}
