using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using HarmonyLib;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_whitenight2 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            this._owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_WhiteNight_Guard());
        }

        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this._owner.bufListDetail.AddBuf(new BattleUnitBuf_Emotion_WhiteNight_Guard());
        }
        public void Destroy()
        {
            BattleUnitBuf Buff = this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Emotion_WhiteNight_Guard));
            if (Buff != null)
                Buff.Destroy();
        }
        public override void OnBattleEnd_alive()
        {
            base.OnBattleEnd_alive();
            if (Singleton<StageController>.Instance.stageType != StageType.Invitation || Singleton<StageController>.Instance.GetStageModel().GetFrontAvailableWave() != null)
                return;
            if (Singleton<StageController>.Instance.GetStageModel().ClassInfo.id == 40008)
            {
                int id = 240023;
                for (int index = 0; index < 2; ++index)
                {
                    Singleton<StageController>.Instance.OnEnemyDropBookForAdded(new DropBookDataForAddedReward(id));
                    DropBookXmlInfo data = Singleton<DropBookXmlList>.Instance.GetData(id);
                    if (data == null)
                    {
                        break;
                    }
                    SingletonBehavior<BattleManagerUI>.Instance.ui_emotionInfoBar.DropBook(new List<string>()
                    {
                        TextDataModel.GetText("BattleUI_GetBook", (object) data.Name)
                    });
                }
            }
            else
            {
                foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
                {
                    int emotionLevel = battleUnitModel.emotionDetail.EmotionLevel;
                    for (int key = emotionLevel; key >= 0; --key)
                    {
                        DropTable dropTable;
                        if (battleUnitModel.UnitData.unitData.DropTable.TryGetValue(key, out dropTable))
                        {
                            using (List<DropBookDataForAddedReward>.Enumerator enumerator = dropTable.DropRemakeCompare(emotionLevel).GetEnumerator())
                            {
                                while (enumerator.MoveNext())
                                    Singleton<StageController>.Instance.OnEnemyDropBookForAdded(enumerator.Current);
                                break;
                            }
                        }
                    }
                }
                foreach (DropBookDataForAddedReward dataForAddedReward in Singleton<StageController>.Instance.GetDroppedBooksData())
                    Singleton<StageController>.Instance.OnEnemyDropBookForAdded(new DropBookDataForAddedReward(dataForAddedReward.id, dataForAddedReward.isaddedreward));
            }
        }


        public class BattleUnitBuf_Emotion_WhiteNight_Guard : BattleUnitBuf
        {
            public override bool Hide => true;
            public override bool IsTargetable(BattleUnitModel attacker)
            {
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
                {
                    if (SearchEmotion(alive, "WhiteNight_Guard_Enemy") != null)
                        continue;
                    if (alive != this._owner && alive.IsTargetable(attacker))
                        return false;
                }
                return base.IsTargetable(attacker);
            }
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
}
