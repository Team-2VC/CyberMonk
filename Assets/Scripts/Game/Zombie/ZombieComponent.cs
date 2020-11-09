using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CyberMonk.Game.Zombie
{

    /// <summary>
    /// The types of attack outcomes.
    /// </summary>
    public enum ZombieAttackedOutcome
    {
        OUTCOME_FAILED_UNKNOWN,
        OUTCOME_FAILED_PLAYER_ATTACKING,
        OUTCOME_FAILED_ZOMBIE_ATTACKING,
        OUTCOME_SUCCESS
    }

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
        [SerializeField]
        private Utils.References.FloatReference timeBetweenBeats;

        public int BeatCounter
            => this.beatCounter != null ? this.beatCounter.Value : 0;

        public Utils.Events.GameEvent BeatDownEvent
        {
            set => this.beatDownEvent = value;
            get => this.beatDownEvent;
        }

        public float TimeBetweenBeats
        {
            get => this.timeBetweenBeats != null ? this.timeBetweenBeats.Value : 0f;
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
        /// Called to find out whether or not the monkey component
        /// can attack the zombie.
        /// </summary>
        /// <param name="component">The monkey component trying to attack the zombie.</param>
        /// <returns>True if the zombie component is attacked, false otherwise.</returns>
        public ZombieAttackedOutcome TryHandleAttack(Moonkey.MoonkeyComponent component)
        {
            if(component == null)
            {
                return ZombieAttackedOutcome.OUTCOME_FAILED_UNKNOWN;
            }

            return this._controller.OnMoonkeyAttack(component);
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
