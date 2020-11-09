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

        public event System.Action<ZombieComponent> AttackBeginEvent
            = delegate { };
        
        private readonly MoonkeyComponent _component;
        private readonly MoonkeySettings _settings;
        private readonly MoonkeyMovementController _movementController;
        private readonly MoonkeyAttackController _attackController;
        private readonly MoonkeyStateController _stateController;

        #endregion

        #region properties
        
        public MoonkeyComponent Component
            => this._component;
        
        public MoonkeySettings Settings
            => this._settings;

        public virtual MoonkeyMovementController MovementController
            => this._movementController;

        public virtual MoonkeyAttackController AttackController
            => this._attackController;

        public virtual MoonkeyStateController StateController
            => this._stateController;

        #endregion

        #region constructor

        public MoonkeyController(MoonkeyComponent component, MoonkeySettings settings )
        {
            Debug.Log("constructing controller class");
            this._component = component;
            this._settings = settings;
            this._movementController = new MoonkeyMovementController(this, settings);
            this._attackController = new MoonkeyAttackController(this);

        }

        #endregion


        #region methods

        /// <summary>
        /// Called to hook the events.
        /// </summary>
        public void HookEvents()
        {
            this._stateController.HookEvents();
        }

        /// <summary>
        /// Called to unhook the events.
        /// </summary>
        public void UnhookEvents()
        {
            this._stateController.UnHookEvents();
        }

        /// <summary>
        /// Called when the zombie component has begun an attack.
        /// </summary>
        /// <param name="component">The zombie component.</param>
        public void OnBeginAttack(Zombie.ZombieComponent component)
        {
            this.AttackBeginEvent(component);
        }

        /// <summary>
        /// Updates the monkey.
        /// </summary>
        public void Update()
        {
            this._movementController?.Update();
            this._attackController?.Update();
        }

        /// <summary>
        /// Updates the physics of the monkey.
        /// </summary>
        public void PhysicsUpdate()
        {
            this._movementController?.PhysicsUpdate();
        }
        
        /// <summary>
        /// Called when the monkey controler collides with another collision.
        /// </summary>
        /// <param name="collision">The collision.</param>
        public void OnCollisionEnter2D(Collision2D collision)
        {
            this._movementController?.OnCollisionEnter2D(collision);
        }

        /// <summary>
        /// Called when the monkey controller collides with another collision.
        /// </summary>
        /// <param name="collision">The collision.</param>
        public void OnCollisionExit2D(Collision2D collision)
        {
            this._movementController?.OnCollisionExit2D(collision);
        }

        /// <summary>
        /// Called when the monkey controller enters a trigger collision.
        /// </summary>
        /// <param name="collider">The collision.</param>
        public void OnTriggerEnter2D(Collider2D collider)
        {
            this._attackController?.OnTriggerEnter2D(collider);
        }

        /// <summary>
        /// Called when the monkey exits a trigger collision.
        /// </summary>
        /// <param name="collider">The collision.</param>
        public void OnTriggerExit2D(Collider2D collider)
        {
            this._attackController?.OnTriggerExit2D(collider);
        }

        #endregion

        #region static_methods



        #endregion
    }
}
