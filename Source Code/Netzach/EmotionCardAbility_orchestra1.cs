using System;
using LOR_DiceSystem;
using UI;
using Sound;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_orchestra1 : EmotionCardAbilityBase
    {
        private int savedHp = 100;
        private int savedBp = 100;
        private bool effect;
        private bool trigger;
        private BattleDiceCardModel Dacapo;
        public override void OnWaveStart()
        {
            if(this._owner.faction==Faction.Player)
               Dacapo=this._owner.allyCardDetail.AddNewCardToDeck(1100006);
            if (this._owner.faction == Faction.Enemy)
                this.trigger = false;
        }

        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            if (this._owner.faction != Faction.Player)
                return;
            if (curCard.card.GetID() != 1100006)
                return;
            this.effect = true;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (!this.effect && !this.trigger)
                return;
            this.effect = false;
            this._owner.SetHp(this.savedHp);
            this._owner.breakDetail.breakGauge = this.savedBp;
            this._owner.cardSlotDetail.RecoverPlayPoint(this._owner.cardSlotDetail.GetMaxPlayPoint());
            SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(this._owner, this._owner.faction, this._owner.hp, this._owner.breakDetail.breakGauge);
            Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/4_N/FX_IllusionCard_4_N_Orchestra_Start");
            if (!((UnityEngine.Object)original != (UnityEngine.Object)null))
                return;
            Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
            if (!((UnityEngine.Object)creatureEffect?.gameObject.GetComponent<AutoDestruct>() == (UnityEngine.Object)null))
                return;
            AutoDestruct autoDestruct = creatureEffect?.gameObject.AddComponent<AutoDestruct>();
            if (!((UnityEngine.Object)autoDestruct != (UnityEngine.Object)null))
                return;
            autoDestruct.time = 3f;
            autoDestruct.DestroyWhenDisable();
            SoundEffectPlayer.PlaySound("Creature/Sym_movment_0_clap");
        }
        public override void OnRoundEnd()
        {
            if (this._owner.faction != Faction.Enemy || trigger)
                return;
            if (this._owner.history.takeDamageAtOneRound < (double)this._owner.MaxHp * 0.25)
                return;
            this.trigger = true;
        }
        public void Destroy()
        {
            this._owner.allyCardDetail.ExhaustACardAnywhere(Dacapo);
        }
        public override void OnSelectEmotion()
        {
            this.savedHp = (int)this._owner.hp;
            this.savedBp = this._owner.breakDetail.breakGauge;
            if (this._owner.faction == Faction.Enemy)
                this.trigger = false;
            if(this._owner.faction==Faction.Player)
                Dacapo = this._owner.allyCardDetail.AddNewCard(1100006);
        }
    }
}
