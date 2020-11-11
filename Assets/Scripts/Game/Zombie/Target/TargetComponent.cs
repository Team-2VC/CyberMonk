using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Zombie.Target
{

    /// <summary>
    /// The Target Component reference.
    /// </summary>
    [System.Serializable]
    public struct TargetComponentReferences
    {
        [SerializeField]
        private Utils.Events.GameEvent beatDownEvent;

        public Utils.Events.GameEvent BeatDownEvent
        {
            get => this.beatDownEvent;
            set
            {
                if(this.beatDownEvent != null)
                {
                    this.beatDownEvent = value;
                }
            }
        }
    }

    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class TargetComponent : MonoBehaviour
    {

        #region fields

        [SerializeField]
        private TargetComponentReferences references;

        private int _beatsActive = 0;

        private bool _active = false;

        private System.DateTime? _beatTime = null;
        private System.DateTime? _pressedTime = null;

        private ZombieTargetWrapper _parentWrapper;

        #endregion

        #region properties

        private bool Active
            => this._active;

        public bool Pressed
            => this._pressedTime.HasValue;

        public bool BeatEntered
            => this._beatsActive >= 1 && this._beatTime.HasValue;

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
            this.references.BeatDownEvent += this.OnBeatDown;
        }

        /// <summary>
        /// Called when the target is disabled.
        /// </summary>
        private void OnDisable()
        {
            this.references.BeatDownEvent -= this.OnBeatDown;
        }

        /// <summary>
        /// Called when the mouse is over the target.
        /// </summary>
        private void OnMouseOver()
        {
            if(Input.GetKeyDown(KeyCode.W) && this.Active)
            {
                // TODO: Send moonkey punch event.
                // TODO: Combo event.

                this._pressedTime = System.DateTime.Now;

                if(!this.BeatEntered)
                {
                    return;
                }

                System.TimeSpan span = this._pressedTime.Value.Subtract(this._beatTime.Value);
                this.OnPressed(span);
            }
        }

        /// <summary>
        /// Sets the target component as active.
        /// </summary>
        private void OnTargetActive()
        {
            this._active = true;
        }

        /// <summary>
        /// Calls the beat down event.
        /// </summary>
        private void OnBeatDown()
        {
            if(this.Active)
            {
                this._beatsActive++;

                // Makes sure to set the beat time.
                if(this._beatsActive == 1)
                {
                    System.DateTime beatTime = System.DateTime.Now;
                    
                    if(this.Pressed)
                    {
                        this.OnPressed(beatTime.Subtract(this._pressedTime.Value));
                        return;
                    }

                    this._beatTime = System.DateTime.Now;
                    return;
                }

                if(this._beatsActive > 1)
                {
                    if(this.Pressed)
                    {
                        System.TimeSpan timeSpan = this._beatTime.Value.Subtract(
                            this._pressedTime.Value);
                        if(this._pressedTime.Value > this._beatTime.Value)
                        {
                            timeSpan = this._pressedTime.Value.Subtract(this._beatTime.Value);
                        }
                        this.OnPressed(timeSpan);
                        return;
                    }

                    DeactivationData data = new DeactivationData(DeactivationReason.REASON_RUN_OUT_OF_TIME);
                    this._parentWrapper?.OnDeactivated(data);
                    this.HandleDeactivation(data.Reason);
                }
            }
        }

        /// <summary>
        /// Called when the target component is pressed.
        /// </summary>
        /// <param name="timeDifference">The Time difference between the beat pressed and the current time.</param>
        private void OnPressed(System.TimeSpan timeDifference)
        {
            float difference = (float)timeDifference.Milliseconds / 1000f;
            Debug.Log(difference);

            DeactivationData data = new DeactivationData(
                DeactivationReason.REASON_PLAYER_HIT, difference);

            this._parentWrapper?.OnDeactivated(data);
            this.HandleDeactivation(data.Reason);
        }

        /// <summary>
        /// Called to force the deactivation of the target component.
        /// </summary>
        public void ForceDeactivate()
        {
            this.HandleDeactivation(DeactivationReason.REASON_FORCED);
        }

        /// <summary>
        /// Called when the target is deactivated.
        /// </summary>
        /// <param name="reason">The deactivation reason.</param>
        private void HandleDeactivation(DeactivationReason reason)
        {
            // TODO: Properly implement
            Destroy(this.gameObject);
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