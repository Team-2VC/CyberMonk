using CyberMonk.Game.Moonkey;
using CyberMonk.Game.Zombie;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Moonkey 
{
    /// <summary>
    /// The Monkey Controller class definition.
    /// </summary>
    public class MoonkeyController
    {
        #region fields
        
        private readonly MoonkeyComponent _component;
        private readonly MoonkeySettings _settings;
        private readonly MoonkeyMovementController _movementController;

        #endregion

        #region properties
        
        public MoonkeyComponent Component
            => this._component;
        
        public MoonkeySettings Settings
            => this._settings;

        public virtual MoonkeyMovementController MovementController
            => this._movementController;

        #endregion

        #region constructor

        public MoonkeyController(MoonkeyComponent component, MoonkeySettings settings )
        {
            Debug.Log("constructing controller class");
            this._component = component;
            this._settings = settings;
            this._movementController = new MoonkeyMovementController(this, settings);

        }

        #endregion


        #region methods

        /// <summary>
        /// Called to hook the events.
        /// </summary>
        public void HookEvents()
        {
            // TODO: hooks the events
        }

        /// <summary>
        /// Called to unhook the events.
        /// </summary>
        public void UnhookEvents()
        {
            // TODO: Unhooks the events.
        }

        /// <summary>
        /// Updates the monkey.
        /// </summary>
        public void Update()
        {
            this._movementController?.Update();
        }

        /// <summary>
        /// Updates the physics of the monkey.
        /// </summary>
        public void PhysicsUpdate()
        {
            this._movementController?.PhysicsUpdate();
        }

        #endregion

        #region static_methods



        #endregion
    }
}
