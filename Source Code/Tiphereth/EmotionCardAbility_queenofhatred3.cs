using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_queenofhatred3 : EmotionCardAbilityBase
    {
        private bool active;
        private int Pow => RandomUtil.Range(1, 3);
        private int Dmg => RandomUtil.Range(3, 7);
        private int BreakDmg => RandomUtil.Range(2, 6);
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            int power = Pow;
            this._owner.battleCardResultLog?.SetEmotionAbility(true, this._emotionCard, 0, ResultOption.Sign, power);
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                power = power
            });
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (SearchEmotion(this._owner, "QueenOfHatred_Gun") == null || this._owner.faction != Faction.Player)
            {
                if (SearchEmotion(this._owner, "QueenOfHatred_Gun").GetAbilityList().Find((x => x is EmotionCardAbility_queenofhatred2)) is EmotionCardAbility_queenofhatred2 villain)
                {
                    villain.target = RandomUtil.SelectOne<BattleUnitModel>(BattleObjectManager.instance.GetAliveList(Faction.Enemy));
                    if (!((UnityEngine.Object)villain.effect != (UnityEngine.Object)null))
                        return;
                    UnityEngine.Object.Destroy((UnityEngine.Object)villain.effect.gameObject);
                    villain.effect = this.MakeEffect("5/MagicalGirl_Villain", target: villain.target);
                }
            }
            if (active)
            {
                foreach (BattleUnitModel victim in BattleObjectManager.instance.GetAliveList(this._owner.faction))
                {
                    victim.TakeBreakDamage(BreakDmg);
                    victim.TakeDamage(Dmg);
                    active = false;
                }
                this.LaserEffect();
            }
        }
        public override void OnRoundEnd()
        {
            if (Chaos() || ifNilil(this._owner))
                return;
            active = true;
        }
        public bool ifNilil(BattleUnitModel owner)
        {
            return this._owner.bufListDetail.GetActivatedBufList().Find((x => x is EmotionCardAbility_clownofnihil3.BattleUnitBuf_Emotion_Nihil)) != null;
        }
        public bool Chaos()
        {
            List<BattleUnitModel> People = BattleObjectManager.instance.GetAliveList(this._owner.faction).FindAll((x => x != this._owner));
            if (People.Count <= 0)
                return true;
            foreach (BattleUnitModel people in People)
            {
                if (people.history.takeDamageAtOneRound >= (int)((double)people.MaxHp * 0.2))
                    return true;
            }
            return false;
        }
        private void LaserEffect()
        {
            Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/5/QueenOfHatred_Laser");
            if (!((UnityEngine.Object)original != (UnityEngine.Object)null))
                return;
            Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
            creatureEffect.transform.localScale = Vector3.one;
            creatureEffect.transform.localPosition = new Vector3(50f, 2f, 0.0f);
            creatureEffect.transform.localRotation = Quaternion.identity;
        }
        private BattleEmotionCardModel SearchEmotion(BattleUnitModel owner, string Name)
        {
            List<BattleEmotionCardModel> emotion = owner.emotionDetail.PassiveList;
            foreach(BattleEmotionCardModel card in emotion)
            {
                if (card.XmlInfo.Name == Name)
                    return card;
            }
            return null;
        }
    }
}
