using System;
using LOR_XML;
using System.Collections.Generic;

namespace EmotionalFix
{
    public class PassiveAbility_1106301: PassiveAbilityBase
    {
        public PassiveAbility_1106301(BattleUnitModel model)
        {
            this.Init(model);
            this.name = Singleton<PassiveDescXmlList>.Instance.GetName(1106301);
            this.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(1106301);
            this.rare = Rarity.Rare;
        }

    }
}
