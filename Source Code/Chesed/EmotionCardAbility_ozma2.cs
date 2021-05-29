using System;
using LOR_DiceSystem;
using UI;
using System.IO;
using Sound;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_ozma2 : EmotionCardAbilityBase
    {
        private bool trigger;
        public override void OnSelectEmotion()
        {
            trigger= false;
            if (this._owner.faction == Faction.Enemy)
                this._owner.bufListDetail.AddBuf(new DiceCardSelfAbility_ozma_forgotten.BattleUnitBuf_ozma_forgotten());
            else
                this._owner.bufListDetail.AddBuf(new Oblivion());
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (!trigger)
            {
                Filter();
                trigger = true;
            }
            if (this._owner.faction != Faction.Enemy && !this._owner.bufListDetail.GetActivatedBufList().Exists(x => x is Oblivion))
                this._owner.allyCardDetail.DrawCards(4);
        }
        public void Filter()
        {
            Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/7_C/FX_IllusionCard_7_C_Mist");
            if (!((UnityEngine.Object)original != (UnityEngine.Object)null))
                return;
            Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original);
            creatureEffect.gameObject.transform.SetParent(SingletonBehavior<BattleManagerUI>.Instance.EffectLayer);
            creatureEffect.gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            creatureEffect.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            SoundEffectPlayer.PlaySound("Creature/Ozma_StrongAtk_Start");
        }
        public void Destory()
        {
            BattleUnitBuf buff = this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is Oblivion));
            if (buff != null)
                buff.Destroy();
            buff = this._owner.bufListDetail.GetActivatedBufList().Find(x => x is DiceCardSelfAbility_ozma_forgotten.BattleUnitBuf_ozma_forgotten);
            if (buff != null)
                buff.Destroy();
        }
        public class Oblivion: BattleUnitBuf
        {
            private int count;
            public override bool IsActionable() => false;
            public override bool IsTargetable() => false;
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                count = 0;
            }
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                _owner.view.charAppearance.ChangeMotion(ActionDetail.Damaged);
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                count++;
                if (count >= 3)
                    this.Destroy();
                if (BattleObjectManager.instance.GetAliveList(_owner.faction).FindAll(x => x != this._owner).Count <= 0)
                    Singleton<StageController>.Instance.KillAllLibrarian();
            }
        }
    }
}
