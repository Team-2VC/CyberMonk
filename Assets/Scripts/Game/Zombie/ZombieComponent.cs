using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CyberMonk.Game.Zombie
{

    /// <summary>
    /// The types of attack outcomes.
    /// </summary>
    public enum TryZombieAttackOutcome
    {
        OUTCOME_FAILED_MISC,
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
        private Utils.Events.GameEvent zombieDeathEvent;
        [SerializeField]
        private Moonkey.Events.MoonkeyAttackEvent attackFinishedEvent;
        [SerializeField]
        private Utils.References.BooleanReference paused;
        [SerializeField]
        private Utils.References.Vector2Reference moonPosition;

        [SerializeField, Range(0.1f, 100f)]
        private float _damageAmount;

        public int BeatCounter
            => this.beatCounter != null ? this.beatCounter.Value : 0;

        public float DamageAmount
            => this._damageAmount;

        public Vector2 MoonPosition
            => this.moonPosition.Value;

        public Utils.Events.GameEvent BeatDownEvent
        {
            set => this.beatDownEvent = value;
            get => this.beatDownEvent;
        }

        public Moonkey.Events.MoonkeyAttackEvent AttackFinishedEvent
        {
            set => this.attackFinishedEvent = value;
            get => this.attackFinishedEvent;
        }

        public Utils.Events.GameEvent ZombieDeathEvent
        {
            get => this.zombieDeathEvent;
        }

        public bool Paused
        {
            get => this.paused.Value;
        }
    }

    [System.Serializable]
    public class ZombieGraphics
    {
        [SerializeField]
        private Animator graphicsAnimator;
        [SerializeField]
        private SpriteRenderer graphicsSpriteRenderer;

        public Animator GraphicsAnimator
            => this.graphicsAnimator;

        public SpriteRenderer GraphicsSpriteRenderer
            => this.graphicsSpriteRenderer;
    }

    /// <summary>
    /// The Zombie Component Monobehaviour.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class ZombieComponent : MonoBehaviour
    {
        #region fields

        [SerializeField]
        private ZombieSettings settings;
        [SerializeField]
        private ZombieReferences references;
        [SerializeField]
        private ZombieGraphics graphics;

        private AZombieController _controller;

        #endregion

        #region properties
        
        public ZombieReferences References
            => this.references;

        public AZombieController Controller
            => this._controller;

        public ZombieGraphics Graphics
            => this.graphics;

        #endregion

        #region methods

        /// <summary>
        /// Called when the script is first awakened.
        /// </summary>
        private void Awake() 
        {
            this._controller = AZombieController.Create(this, settings);
            ZombieHolder.Add(this);
        }

        /// <summary>
        /// Called to find out whether or not the monkey component
        /// can attack the zombie.
        /// </summary>
        /// <param name="component">The monkey component trying to attack the zombie.</param>
        /// <returns>True if the zombie component is attacked, false otherwise.</returns>
        public TryZombieAttackOutcome TryHandleAttack(Moonkey.MoonkeyComponent component)
        {
            if(component == null)
            {
                return TryZombieAttackOutcome.OUTCOME_FAILED_MISC;
            }

            return this._controller.OnMoonkeyAttack(component);
        }

        private void FixedUpdate()
        {
            if(this.references.Paused)
            {
                return;
            }

            this._controller?.PhysicsUpdate();
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

        /// <summary>
        /// Called when the zombie component is destroyed.
        /// </summary>
        private void OnDestroy()
        {
            ZombieHolder.Remove(this);
        }

        #endregion
    }
}
