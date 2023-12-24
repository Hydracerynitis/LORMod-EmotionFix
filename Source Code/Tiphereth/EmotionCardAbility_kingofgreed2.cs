using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_kingofgreed2 : EmotionCardAbilityBase
    {
        private Battle.CreatureEffect.CreatureEffect aura;
        private List<BattleDiceCardModel> AddedCard = new List<BattleDiceCardModel>();
        private int count;
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/KingOfGreed_Yellow", false, 2f);
            if (_owner.faction != Faction.Player)
                return;
            AddedCard.Add(_owner.allyCardDetail.AddNewCard(902523));
            AddedCard.Add(_owner.allyCardDetail.AddNewCard(902523));
        }
        public override void OnKill(BattleUnitModel target)
        {
            if (_owner.faction != Faction.Player)
                return;
            base.OnKill(target);
            if (SearchEmotion(_owner, "Greed_Break") == null)
                return;
            new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/KingOfGreed_Yellow", false, 2f);
            AddedCard.Add(_owner.allyCardDetail.AddNewCard(902523));
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (count > 0)
            {
                aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("5_T/FX_IllusionCard_5_T_Happiness", 1f, _owner.view, _owner.view);
                SoundEffectPlayer.PlaySound("Creature/Greed_MakeDiamond");
            }       
            count = 0;
        }
        public override void OnEndBattlePhase()
        {
            base.OnEndBattlePhase();
            DestroyAura();
        }
        public override void OnDie(BattleUnitModel killer)
        {
            base.OnDie(killer);
            DestroyAura();
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            DestroyAura();
            if (count <= 0 || _owner.faction!=Faction.Enemy)
                return;
            for(int i=GetStack(count);i>0 ;i--)
            {
                int j = RandomUtil.Range(1, 3);
                switch (j)
                {
                    case 1:
                        _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 1);
                        break;
                    case 2:
                        _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Endurance, 1);
                        break;
                    case 3:
                        _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Quickness, 1);
                        break;
                }
            }
        }
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            ++count;
        }
        public void DestroyAura()
        {
            if (!(aura != null))
                return;
            UnityEngine.Object.Destroy(aura.gameObject);
            aura = (Battle.CreatureEffect.CreatureEffect)null;
        }

        public void Destroy()
        {
            foreach (BattleDiceCardModel card in AddedCard)
                _owner.allyCardDetail.ExhaustACardAnywhere(card);
            DestroyAura();
        }
        private int GetStack(int cnt) => Mathf.Min(3, cnt);
        private BattleEmotionCardModel SearchEmotion(BattleUnitModel owner, string Name)
        {
            List<BattleEmotionCardModel> emotion = owner.emotionDetail.PassiveList;
            foreach (BattleEmotionCardModel card in emotion)
            {
                if (card.XmlInfo.Name == Name)
                    return card;
            }
            return null;
        }
    }
}
