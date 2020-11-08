using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CyberMonk.Game.Moonkey
{

    /// <summary>
    /// Moonkey Component Monobehaviour
    /// </summary>
    public class MoonkeyComponent : MonoBehaviour
    {
        #region fields
        [SerializeField]
        private MoonkeySettings settings;

        private MoonkeyController _controller;

        private Vector2 _direction;

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
        void Update()
        {
            this._direction = new Vector2(Input.GetAxis("Horizontal"), 0);
            this._controller.MovementController.Move();
            //this._controller.MovementController.Dash();
            //Debug.Log(this._direction);
        }

        /// <summary>
        /// Runs at a constant physics rate
        /// </summary>
        private void FixedUpdate()
        {
            
        }


        #endregion
    }
}