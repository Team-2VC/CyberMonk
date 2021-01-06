using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace CyberMonk.Game.Moonkey
{

    

    /// <summary>
    /// The list of moonkey references.
    /// </summary>
    [System.Serializable]
    public struct MoonkeyReferences
    {
        [SerializeField]
        private Events.MoonkeyAttackEvent attackFinishedEvent;
        [SerializeField]
        private Utils.References.IntegerReference comboCounter;
        [SerializeField]
        private Utils.References.IntegerReference comboMultiplier;
        [SerializeField]
        private Utils.References.IntegerReference totalScore;
        [SerializeField]
        private Utils.References.BooleanReference paused;
        [SerializeField]
        private Utils.Events.GameEvent deathEvent;

        public Events.MoonkeyAttackEvent AttackFinishedEvent
        {
            get => this.attackFinishedEvent;
            set
            {
                if(this.attackFinishedEvent != null)
                {
                    this.attackFinishedEvent = value;
                }
            }
        }

        public Utils.Events.GameEvent DeathEvent
        {
            get => this.deathEvent;
        }

        // TODO: When null this thing makes the whole Unity unusable.
        public int ComboCounter
        {
            get => this.comboCounter != null ? this.comboCounter.Value : 0;
            set
            {
                if(this.comboCounter != null)
                {
                    this.comboCounter.Value = value;
                }
            }
        }

        public int ComboMultiplier
        {
            get => this.comboMultiplier != null ? this.comboMultiplier.Value : 0;
            set
            {
                if (this.comboMultiplier != null)
                {
                    this.comboMultiplier.Value = value;
                }
            }
        }

        public int TotalScore
        {
            get => this.totalScore != null ? this.totalScore.Value : 0;
            set
            {
                if (this.totalScore != null)
                {
                    this.totalScore.Value = value;
                }
            }
        }

        public bool Paused
            => this.paused.Value;
    }


    [System.Serializable]
    public struct MoonkeyProperties
    {
 
        [SerializeField]
        private MoonkeyGFXComponent graphicsComponent;

        [SerializeField]
        private Collider2D leftDashCollider;
        [SerializeField]
        private Collider2D rightDashCollider;

        public MoonkeyGFXComponent GraphicsComponent
            => this.graphicsComponent;

        public Collider2D LeftDashCollider
            => this.leftDashCollider;

        public Collider2D RightDashCollider
            => this.rightDashCollider;
    }

    /// <summary>
    /// Moonkey Component Monobehaviour
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class MoonkeyComponent : MonoBehaviour
    {
        #region fields
        
        [SerializeField]
        private MoonkeySettings settings;
        [SerializeField]
        private MoonkeyReferences references;
        [SerializeField]
        private MoonkeyProperties properties;

        private MoonkeyController _controller;

        #endregion

        #region properties

        public MoonkeyController Controller
            => this._controller;

        public MoonkeyReferences References
            => this.references;

        public MoonkeyProperties Properties
            => this.properties;

        public MoonkeySettings Settings
            => this.settings;

        public bool IsAttacking
            => this._controller.IsAtttacking;

        #endregion

        #region methods
        
        /// <summary>
        /// Called before start, when script is awakened
        /// </summary>
        private void Awake()
        {
            this._controller = new MoonkeyController(this, settings);
            MoonkeyHolder.Add(this);
        }

        /// <summary>
        /// Runs once per frame
        /// </summary>
        private void Update()
        {
            if(this.references.Paused)
            {
                return;
            }

            this._controller.Update();
        }

        /// <summary>
        /// Runs at a constant physics rate
        /// </summary>
        private void FixedUpdate()
        {
            if(this.references.Paused)
            {
                return;
            }

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
        /// Called when the moonkey is destroyed.
        /// </summary>
        private void OnDestroy()
        {
            // Removes this from the moonkey holder.
            MoonkeyHolder.Remove(this);
        }

        /// <summary>
        /// Called when the monkey controller collides with another collision.
        /// </summary>
        /// <param name="collision">The collision.</param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(this.references.Paused)
            {
                return;
            }

            this._controller.OnCollisionEnter2D(collision);
        }

        /// <summary>
        /// Called when the monkey controller collides with another collision.
        /// </summary>
        /// <param name="collision">The collision.</param>
        private void OnCollisionExit2D(Collision2D collision)
        {
            if(this.references.Paused)
            {
                return;
            }

            this._controller.OnCollisionExit2D(collision);
        }

        /// <summary>
        /// Called when the monkey controller enters a trigger collision.
        /// </summary>
        /// <param name="collider">The collision.</param>
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if(this.references.Paused)
            {
                return;
            }

            this._controller.OnTriggerEnter2D(collider);
        }

        /// <summary>
        /// Called when the monkey controller exits trigger collision.
        /// </summary>
        /// <param name="collider">The collision.</param>
        private void OnTriggerExit2D(Collider2D collider)
        {
            if(this.references.Paused)
            {
                return;
            }

            this._controller.OnTriggerExit2D(collider);
        }

        #endregion
    }
}