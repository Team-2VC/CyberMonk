using CyberMonk.Game.Moonkey;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Zombie.Melee
{
    /// <summary>
    /// The melee zombie state controller.
    /// </summary>
    public class MeleeZombieStateController : AZombieStateController
    {
        #region fields

        private ZombieTargetController _targetController;
        private int _attackBeat, _spawnBeat;

        private MoonkeyComponent _attacker = null;

        #endregion

        #region properties

        public ZombieReferences? References
            => this._controller?.Component.References;

        public override bool OpenForAttack
        {
            get
            {
                if(this.References.HasValue)
                {
                    return this.References.Value.BeatCounter != this._attackBeat;
                }
                
                return true;
            }
        }

        #endregion

        #region constructor

        public MeleeZombieStateController(MeleeZombieController controller)
            : base(controller) 
        {
            this._targetController = controller.TargetController;
            this._spawnBeat = this.References.Value.BeatCounter;
            this. _attackBeat = (this._spawnBeat + 3) % 4;
        }

        #endregion

        #region methods

        protected override void OnDownBeat()
        {
            if(this._state == ZombieState.STATE_ATTACKED
                && !this._targetController.TargetsActive)
            {
                this._targetController.TargetsActive = true;
            }
        }

        protected override void OnAttacked(MoonkeyComponent component)
        {
            if(this._attacker == null)
            {
                this._attacker = component;
            }

            this._state = ZombieState.STATE_ATTACKED;
        }

        protected override void OnAttackEnd(AttackOutcome outcome)
        {
            if(outcome == AttackOutcome.OUTCOME_FAILED)
            {
                if(this.References.HasValue)
                {
                    this.References.Value.AttackFinishedEvent?.Call(this._attacker, this._controller.Component, outcome);
                }

                this._attacker = null;
                this._state = ZombieState.STATE_DANCING;
                return;
            }
            
            if (this.References.HasValue)
            {
                this.References.Value.AttackFinishedEvent?.Call(this._attacker, this._controller.Component, outcome);
            }

            this._attacker = null;
            this._state = ZombieState.STATE_LAUNCHED;
 
            // TODO: event where the zombie is launched
            // For now it just destroys the object
            GameObject.Destroy(this._controller.Component.gameObject);
        }

        #endregion
    }

    /// <summary>
    /// The main melee zombie controller implementation.
    /// </summary>
    public class MeleeZombieController : AZombieController
    {
        #region fields

        private readonly MeleeZombieStateController _stateController;

        #endregion

        #region properties

        public override AZombieStateController StateController
            => this._stateController;

        #endregion

        #region constructor

        public MeleeZombieController(ZombieComponent component, ZombieSettings settings)
            : base(component, settings)
        {
            this._stateController = new MeleeZombieStateController(this);
        }

        #endregion
    }
}
