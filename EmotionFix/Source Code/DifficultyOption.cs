using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UI;
using UnityEngine;

namespace EmotionalFix
{
    public class DifficultyOption
    {
        public TMP_Dropdown DifficultyDropDown;
        /*public UITextDataLoader OptionText; */
        public void Init(bool interable)
        {
            DifficultyDropDown.ClearOptions();
            DifficultyDropDown.AddOptions(new List<TMP_Dropdown.OptionData>()
              {
                new TMP_Dropdown.OptionData(TextDataModel.GetText("EF_difficulty_None")),
                new TMP_Dropdown.OptionData(TextDataModel.GetText("EF_difficulty_Easy")),
                new TMP_Dropdown.OptionData(TextDataModel.GetText("EF_difficulty_Normal")),
                new TMP_Dropdown.OptionData(TextDataModel.GetText("EF_difficulty_Hard")),
              });
            switch (EmotionFixInitializer.diff)
            {
                case Difficulty.None:
                    DifficultyDropDown.value = 0;
                    break;
                case Difficulty.Easy:
                    DifficultyDropDown.value = 1;
                    break;
                case Difficulty.Normal:
                    DifficultyDropDown.value = 2;
                    break;
                case Difficulty.Hard:
                    DifficultyDropDown.value = 3;
                    break;
            }
            DifficultyDropDown.interactable = interable;
            UIOptionDropdown buttonEffect = DifficultyDropDown.gameObject.GetComponent<UIOptionDropdown>();
            buttonEffect.SetDisabled(!interable);
        }
        public Difficulty ApplySetting()
        {
            if (DifficultyDropDown.value == 0)
                return Difficulty.None;
            else if (DifficultyDropDown.value == 1)
                return Difficulty.Easy;
            else if (DifficultyDropDown.value == 2)
                return Difficulty.Normal;
            else if (DifficultyDropDown.value == 3)
                return Difficulty.Hard;
            else
            {
                Debug.Log("EmotionFix Setting Invalid Value: " + DifficultyDropDown.value);
                return Difficulty.Normal;
            }
        }
    }
}
