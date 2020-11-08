using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Zombie.Target
{

    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class TargetComponent : MonoBehaviour
    {

        #region fields

        [SerializeField, Range(1, 5)]
        private int beatsActiveCounter;
        [SerializeField]
        private Utils.Events.GameEvent beatDownEvent;

        private bool _didGetHit = false;
        private int _beatsActive = 0;

        private System.DateTime? _startActiveTime = null;
        private ZombieTargetWrapper _parentWrapper;

        #endregion

        #region properties

        private bool Active
            => this._startActiveTime.HasValue;

        /// <summary>
        /// Gets/Sets the wrapper class of the target.
        /// </summary>
        public ZombieTargetWrapper Wrapper
        {
            get => this._parentWrapper;
        }

        #endregion

        #region methods

        /// <summary>
        /// Called when the target is enabled.
        /// </summary>
        private void OnEnable()
        {
            this.beatDownEvent += this.OnBeatDown;
        }

        /// <summary>
        /// Called when the target is disabled.
        /// </summary>
        private void OnDisable()
        {
            this.beatDownEvent -= this.OnBeatDown;
        }

        /// <summary>
        /// Called when the mouse is over the target.
        /// </summary>
        private void OnMouseOver()
        {
            if(Input.GetKeyDown(KeyCode.Space) && this.Active)
            {
                this._didGetHit = true;
                DeactivationData data = new DeactivationData(
                    DeactivationReason.REASON_PLAYER_HIT, System.DateTime.Now.Subtract(this._startActiveTime.Value));
                this._parentWrapper?.OnDeactivated(data);
                this.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Sets the target component as active.
        /// </summary>
        private void OnTargetActive()
        {
            this._startActiveTime = System.DateTime.Now;
        }

        /// <summary>
        /// Calls the beat down event.
        /// </summary>
        private void OnBeatDown()
        {
            if(this.Active)
            {
                this._beatsActive++;

                if(this._beatsActive > this.beatsActiveCounter)
                {
                    DeactivationData data = new DeactivationData(DeactivationReason.REASON_RUN_OUT_OF_TIME);
                    this._parentWrapper?.OnDeactivated(data);
                    this.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Sets the target wrapper class.
        /// </summary>
        /// <param name="wrapper">The zombie target wrapper.</param>
        public void SetWrapper(ZombieTargetWrapper wrapper)
        {
            if(this._parentWrapper == null)
            {
                this._parentWrapper = wrapper;
            }
        }

        #endregion
    }
}