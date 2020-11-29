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

    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(Renderer))]
    public class TargetComponent : MonoBehaviour
    {

        #region fields

        [SerializeField]
        private TargetComponentReferences references;
        [SerializeField, Range(0, 10)]
        private int totalActiveBeats;

        private int _beatsActive = 0;

        private bool _active = false;

        private System.DateTime? _beatTime = null;
        private System.DateTime? _pressedTime = null;

        private ZombieTargetWrapper _parentWrapper;

        // Used to make the object disappear.
        private Renderer _renderer;

        #endregion

        #region properties

        private bool Active
            => this._active;

        public bool Pressed
            => this._pressedTime.HasValue;

        public bool BeatEntered
            => this._beatsActive >= this.totalActiveBeats && this._beatTime.HasValue;

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
        /// Called to start.
        /// </summary>
        private void Start()
        {
            this._renderer = this.GetComponent<Renderer>();
        }

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
                
                this.OnPressedExact();

                if(!this.BeatEntered)
                {
                    // this is early
                    return;
                }

                System.TimeSpan span = this._pressedTime.Value.Subtract(this._beatTime.Value);
                this.OnPostPressed(span);
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
            // todo: reimplement

            if(this.Active)
            {
                this._beatsActive++;

                // Makes sure to set the beat time.
                if(this._beatsActive == this.totalActiveBeats)
                {
                    System.DateTime beatTime = System.DateTime.Now;
                    
                    if(this.Pressed)
                    {
                        this.OnPostPressed(beatTime.Subtract(this._pressedTime.Value));
                        return;
                    }

                    this._beatTime = System.DateTime.Now;
                    return;
                }

                if(this._beatsActive > this.totalActiveBeats)
                {
                    if(this.Pressed)
                    {
                        System.TimeSpan timeSpan = this._beatTime.Value.Subtract(
                            this._pressedTime.Value);
                        if(this._pressedTime.Value > this._beatTime.Value)
                        {
                            timeSpan = this._pressedTime.Value.Subtract(this._beatTime.Value);
                        }
                        this.OnPostPressed(timeSpan);
                        return;
                    }

                    DeactivationData data = new DeactivationData(DeactivationReason.REASON_RUN_OUT_OF_TIME);
                    this._parentWrapper?.OnDeactivated(data);
                    this.HandleDeactivation(data.Reason);
                }
            }
        }

        /// <summary>
        /// Called at the time when the target was exactly pressed.
        /// </summary>
        private void OnPressedExact()
        {
            if(this.Pressed)
            {
                return;
            }

            this._pressedTime = System.DateTime.Now;
            
            // Sets the renderer as disabled.
            if(this._renderer != null)
            {
                this._renderer.enabled = false;
            }
        }

        /// <summary>
        /// Called after the target is pressed & after we have discovered the time difference
        /// between when the target was pressed and the beat time.
        /// </summary>
        /// <param name="timeDifference">The Time difference between the beat pressed and the current time.</param>
        private void OnPostPressed(System.TimeSpan timeDifference)
        {
            float difference = (float)timeDifference.Milliseconds / 1000f;

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