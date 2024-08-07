﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public static class Helper
    {
        public static BattleEmotionCardModel SearchEmotion(BattleUnitModel owner, string Name)
        {
            List<BattleEmotionCardModel> emotion = owner.emotionDetail.PassiveList;
            foreach (BattleEmotionCardModel card in emotion)
            {
                if (card.XmlInfo.Name == Name)
                    return card;
            }
            return null;
        }
        public static bool CheckOtherMod(string DLLname)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if(assembly.GetName().Name == DLLname) return true;
            }
            return false;
        }
    }
    public enum EmotionBundle
    {
        None,
        Clown,
        Whitenight,
        Gift
    }
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }
}
