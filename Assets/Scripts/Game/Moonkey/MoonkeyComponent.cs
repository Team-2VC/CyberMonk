using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace CyberMonk.Game.Moonkey
{

    /// <summary>
    /// Moonkey Component Monobehaviour
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class MoonkeyComponent : MonoBehaviour
    {
        #region fields
        
        [SerializeField]
        private MoonkeySettings settings;

        private MoonkeyController _controller;

        #endregion

        #region properties

        public MoonkeyController Controller
            => this._controller;

        #endregion

        #region methods
        
        /// <summary>
        /// Called before start, when script is awakened
        /// </summary>
        private void Awake()
        {
            this._controller = new MoonkeyController(this, settings);
        }

        /// <summary>
        /// Runs once per frame
        /// </summary>
        private void Update()
        {
            this._controller.Update();
        }

        /// <summary>
        /// Runs at a constant physics rate
        /// </summary>
        private void FixedUpdate()
        {
            this._controller.PhysicsUpdate();
        }

        /// <summary>
        /// Called when the moonkey component is enabled.
        /// </summary>
        private void OnEnable()
        {
            this._controller?.HookEvents();
        }

        /// <summary>
        /// Called when moonkey component is disabled.
        /// </summary>
        private void OnDisable()
        {
            this._controller?.UnhookEvents();
        }

        /// <summary>
        /// Called when the moonkey begins an attack.
        /// </summary>
        /// <param name="component">The zombie component we are attacking.</param>
        public void OnBeginAttack(Zombie.ZombieComponent component)
        {
            this._controller.OnBeginAttack(component);
        }

        /// <summary>
        /// Called when the monkey controler collides with another collision.
        /// </summary>
        /// <param name="collision">The collision.</param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            this._controller.OnCollisionEnter2D(collision);
        }

        /// <summary>
        /// Called when the monkey controller collides with another collision.
        /// </summary>
        /// <param name="collision">The collision.</param>
        private void OnCollisionExit2D(Collision2D collision)
        {
            this._controller.OnCollisionExit2D(collision);
        }

        /// <summary>
        /// Called when the monkey controller enters a trigger collision.
        /// </summary>
        /// <param name="collider">The collision.</param>
        private void OnTriggerEnter2D(Collider2D collider)
        {
            this._controller.OnTriggerEnter2D(collider);
        }

        /// <summary>
        /// Called when the monkey controller exits trigger collision.
        /// </summary>
        /// <param name="collider">The collision.</param>
        private void OnTriggerExit2D(Collider2D collider)
        {
            this._controller.OnTriggerExit2D(collider);
        }


        #endregion
    }
}