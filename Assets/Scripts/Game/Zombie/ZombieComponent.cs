using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CyberMonk.Game.Zombie
{

    /// <summary>
    /// Structure that defines the zombie references.
    /// </summary>
    [System.Serializable]
    public struct ZombieReferences
    {
        [SerializeField]
        private Utils.References.IntegerReference beatCounter;
        [SerializeField]
        private Utils.Events.GameEvent beatDownEvent;

        public int BeatCounter
            => this.beatCounter != null ? this.beatCounter.Value : 0;

        public Utils.Events.GameEvent BeatDownEvent
        {
            set => this.beatDownEvent = value;
            get => this.beatDownEvent;
        }
    }

    /// <summary>
    /// The Zombie Component Monobehaviour.
    /// </summary>
    public class ZombieComponent : MonoBehaviour
    {
        #region fields

        [SerializeField]
        private ZombieSettings settings;
        [SerializeField]
        private ZombieReferences references;

        private AZombieController _controller;

        #endregion

        #region properties
        public ZombieReferences References
            => this.references;

        public AZombieController Controller
            => this._controller;

        #endregion

        #region methods

        /// <summary>
        /// Called when the script is first awakened.
        /// </summary>
        private void Awake() 
        {
            this._controller = AZombieController.Create(this, settings);
        }

        /// <summary>
        /// Called when the zombie component is enabled.
        /// </summary>
        private void OnEnable()
        {
            this._controller?.HookEvents();
        }

        /// <summary>
        /// Called when the zombie component is disabled.
        /// </summary>
        private void OnDisable()
        {
            this._controller?.UnHookEvents();
        }

        #endregion
    }
}
