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

        private float? _beatTime = null;
        private float? _pressedTime = null;

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
            => this._beatTime.HasValue;

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
            if(Input.GetKeyDown(KeyCode.W) && this.Active && this.BeatEntered)
            {
                this.OnPressed();
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
                if (!this._beatTime.HasValue)
                {
                    this._beatTime = Time.time;
                }

                this._beatsActive++;

                if(this._beatsActive > this.totalActiveBeats)
                {
                    this.Deactivate(DeactivationReason.REASON_RUN_OUT_OF_TIME);
                }
            }
        }

        /// <summary>
        /// Called at the time when the target was exactly pressed.
        /// </summary>
        private void OnPressed()
        {
            if(this.Pressed)
            {
                return;
            }

            this._pressedTime = Time.time;
            
            // Sets the renderer as disabled.
            if(this._renderer != null)
            {
                this._renderer.enabled = false;
            }

            this.Deactivate(DeactivationReason.REASON_PLAYER_HIT, this._pressedTime.Value - this._beatTime.Value);
        }

        /// <summary>
        /// Deactivates the target.
        /// </summary>
        /// <param name="reason">The deactivation reason.</param>
        /// <param name="dat">The data of the deactivation.</param>
        private void Deactivate(DeactivationReason reason, object dat = null)
        {
            DeactivationData data;
            
            if(dat is float)
            {
                data = new DeactivationData(reason, (float)dat);
            }
            else
            {
                data = new DeactivationData(reason);
            }

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