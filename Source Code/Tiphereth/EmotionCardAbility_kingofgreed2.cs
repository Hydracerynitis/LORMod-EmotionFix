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
            if (this._owner.faction != Faction.Player)
                return;
            AddedCard.Add(this._owner.allyCardDetail.AddNewCard(902523));
            AddedCard.Add(this._owner.allyCardDetail.AddNewCard(902523));
        }
        public override void OnKill(BattleUnitModel target)
        {
            if (this._owner.faction != Faction.Player)
                return;
            base.OnKill(target);
            if (SearchEmotion(this._owner, "Greed_Break") == null)
                return;
            new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/KingOfGreed_Yellow", false, 2f);
            AddedCard.Add(this._owner.allyCardDetail.AddNewCard(902523));
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (this.count > 0)
            {
                this.aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("5_T/FX_IllusionCard_5_T_Happiness", 1f, this._owner.view, this._owner.view);
                SoundEffectPlayer.PlaySound("Creature/Greed_MakeDiamond");
            }       
            this.count = 0;
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
            this.DestroyAura();
            if (this.count <= 0 || this._owner.faction!=Faction.Enemy)
                return;
            for(int i=this.GetStack(count);i>0 ;i--)
            {
                int j = RandomUtil.Range(1, 3);
                switch (j)
                {
                    case 1:
                        this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 1);
                        break;
                    case 2:
                        this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Endurance, 1);
                        break;
                    case 3:
                        this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Quickness, 1);
                        break;
                }
            }
        }
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            ++this.count;
        }
        public void DestroyAura()
        {
            if (!((UnityEngine.Object)this.aura != (UnityEngine.Object)null))
                return;
            UnityEngine.Object.Destroy((UnityEngine.Object)this.aura.gameObject);
            this.aura = (Battle.CreatureEffect.CreatureEffect)null;
        }

        public void Destroy()
        {
            foreach (BattleDiceCardModel card in AddedCard)
                this._owner.allyCardDetail.ExhaustACardAnywhere(card);
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
