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
        private bool _effect;
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            if(_owner.faction==Faction.Player)
                GiveCards();
            if (_owner.faction == Faction.Enemy)
                Silence();
        }

        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            if (_owner.faction == Faction.Player)
                GiveCards();
            if (_owner.faction == Faction.Enemy)
                Silence();
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (_effect)
            {
                _effect = false;
                new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/ThePriceOfSilence_Filter", false, 2f);
                SoundEffectPlayer.PlaySound("Creature/Clock_StopCard");
                Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/9_H/FX_IllusionCard_9_H_Silence");
                if (original != null)
                {
                    Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original);
                    creatureEffect.gameObject.transform.SetParent(SingletonBehavior<BattleManagerUI>.Instance.EffectLayer);
                    creatureEffect.gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                    creatureEffect.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                }
            }
        }
        private void GiveCards()
        {
            silence=_owner.allyCardDetail.AddNewCard(1108101);
        }
        public void Destroy()
        {
            if (_owner.faction == Faction.Player)
                _owner.allyCardDetail.ExhaustACardAnywhere(silence);
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
            _effect = true;
            victim.bufListDetail.AddBuf(new SilenceStun());
            _owner.bufListDetail.AddBuf(new SilenceStun());
        }
        public class SilenceStun : BattleUnitBuf
        {
            public override bool IsActionable() => false;
            public override int SpeedDiceBreakedAdder() => 10;
            public override string keywordId => "Stun";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
                SingletonBehavior<DiceEffectManager>.Instance.CreateBufEffect("BufEffect_Disarm", owner.view);
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
    }
}
