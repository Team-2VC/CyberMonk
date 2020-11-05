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

        public MeleeZombieStateController(MeleeZombieController controller)
            : base(controller) { }

        protected override void OnDownBeat()
        {
            // TODO: Implementation.
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

        public override AZombieTargetController TargetController 
            // TODO: Implementation
            => throw new System.NotImplementedException();

        #endregion

        #region constructor

        public MeleeZombieController(ZombieComponent component, ZombieSettings settings)
            : base(component, settings.Type)
        {
            this._stateController = new MeleeZombieStateController(this);
        }

        #endregion
    }
}
