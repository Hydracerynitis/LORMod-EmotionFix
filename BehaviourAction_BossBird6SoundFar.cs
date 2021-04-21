using System;
using System.Collections.Generic;
using System.Linq;
using LOR_DiceSystem;
using Sound;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class BehaviourAction_BossBird6SoundFar : BehaviourActionBase
    {
        public override List<RencounterManager.MovingAction> GetMovingAction(
          ref RencounterManager.ActionAfterBehaviour self,
          ref RencounterManager.ActionAfterBehaviour opponent)
        {
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Bossbird_Longbird_On", false,1f);
            return base.GetMovingAction(ref self, ref opponent);
        }
    }
}
