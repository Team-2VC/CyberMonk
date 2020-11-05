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

            #region test_code

            // TODO: Remove

            if (this._controller.Component != null)
            {
                int beatCounter = this._controller.Component.References.BeatCounter;

                if(beatCounter == 3 && !this._targetController.TargetsActive)
                {
                    this._targetController.TargetsActive = true;
                }
            }

            #endregion
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
