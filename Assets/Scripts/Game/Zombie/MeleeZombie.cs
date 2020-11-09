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
        private int _spawnBeat;

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
                    return this.References.Value.BeatCounter == this._spawnBeat;
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
            this._spawnBeat = controller.Component.References.BeatCounter;
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
            this._state = ZombieState.STATE_ATTACKED;
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
