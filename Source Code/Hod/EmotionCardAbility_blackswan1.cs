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
        private List<KeywordBuf> ActivatedBuf;
        private KeywordBuf[] debuff => new KeywordBuf[]
        {
            KeywordBuf.Burn,
            KeywordBuf.Paralysis,
            KeywordBuf.Bleeding,
            KeywordBuf.Vulnerable,
            KeywordBuf.Weak,
            KeywordBuf.Disarm,
            KeywordBuf.Binding
        };
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if (ActivatedBuf.Count <= 0)
                return;
            KeywordBuf buff = RandomUtil.SelectOne<KeywordBuf>(ActivatedBuf);
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(this._owner.faction))
            {
                unit.bufListDetail.AddKeywordBufThisRoundByEtc(buff, 1);
            }
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
            for(int i=0; i<3;i++)
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(RandomUtil.SelectOne<KeywordBuf>(debuff),1);
        }
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            ActivatedBuf = new List<KeywordBuf>();
            foreach(KeywordBuf buftype in debuff)
            {
                if (this._owner.bufListDetail.GetActivatedBuf(buftype) != null)
                    ActivatedBuf.Add(buftype);
            }
        }
    }
}
