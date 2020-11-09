using System.Collections;
using System.Collections.Generic;
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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            this._controller.OnCollisionEnter2D(collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            this._controller.OnCollisionExit2D(collision);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            
        }


        #endregion
    }
}