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

        private ZombieTargetController _targetController;

        public MeleeZombieStateController(MeleeZombieController controller)
            : base(controller) 
        {
            this._targetController = controller.TargetController;
        }

        protected override void OnDownBeat()
        {
            // TODO: implement the code for beats
        }
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
