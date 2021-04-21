using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_blackswan1 : EmotionCardAbilityBase
    {
        private bool givedmg;
        private BattleUnitModel tempTarget;
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            this.tempTarget = behavior?.card?.target;
            this.givedmg = true;
            this.tempTarget.Book.SetResistHP(BehaviourDetail.Slash, AtkResist.Weak);
            this.tempTarget.Book.SetResistHP(BehaviourDetail.Penetrate, AtkResist.Weak);
            this.tempTarget.Book.SetResistHP(BehaviourDetail.Hit, AtkResist.Weak);
            this.tempTarget.Book.SetResistBP(BehaviourDetail.Slash, AtkResist.Weak);
            this.tempTarget.Book.SetResistBP(BehaviourDetail.Penetrate, AtkResist.Weak);
            this.tempTarget.Book.SetResistBP(BehaviourDetail.Hit, AtkResist.Weak);
        }
        public override void CheckDmg(int dmg, BattleUnitModel target)
        {
            if (this.tempTarget == null || !this.givedmg)
                return;
            this.tempTarget.Book.SetOriginalResists();
            this.tempTarget = null;
            this.givedmg = false;
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            UnityEngine.Object original = Resources.Load("Prefabs/Battle/CreatureEffect/FinalBattle/EGO_BlackSwan_Feather");
            if (!(original != (UnityEngine.Object)null))
                return;
            BattleUnitModel target = behavior?.card?.target;
            if (target == null)
                return;
            GameObject gameObject = UnityEngine.Object.Instantiate(original, target.view.atkEffectRoot) as GameObject;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localScale = Vector3.one;
        }
        public override void OnRoundStart()
        {
            this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak, 2);
        }
    }
}
