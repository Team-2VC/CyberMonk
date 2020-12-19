using CyberMonk.Game.Moonkey;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Zombie.Melee
{

    public class MeleeZombieAnimationController : AZombieAnimationController
    {
        private AZombieController _controller;
        private ZombieReferences _references;

        private int _spawnBeat;

        public MeleeZombieAnimationController(AZombieController controller, RuntimeAnimatorController animController)
            : base(controller, animController)
        {
            this._controller = controller;
            this._references = this._controller.Component.References;

            this._spawnBeat = this._references.BeatCounter;
        }

        public override void HookEvents()
        {
        }

        public override void UnHookEvents()
        {
        }
    }

    /// <summary>
    /// Handles the zombie sound.
    /// </summary>
    public class MeleeZombieSoundController : AZombieSoundController
    {
        public MeleeZombieSoundController(MeleeZombieController controller, ZombieSoundData soundData)
            : base(controller, soundData) { }

        public override void HookEvents()
        {
            this._controller.LaunchBeginEvent += this.OnLaunchBegin;
        }

        public override void UnHookEvents()
        {
            this._controller.LaunchBeginEvent -= this.OnLaunchBegin;
        }

        /// <summary>
        /// Called when the zombie has begun to launch.
        /// </summary>
        private void OnLaunchBegin()
        {
            this.PlaySound(ZombieSoundType.SOUND_LAUNCH);
        }
    }


    /// <summary>
    /// Handles the melee zombie movement controller.
    /// </summary>
    public class MeleeZombieMovementController : AZombieMovementController
    {

        private Rigidbody2D _rigidbody;
        private Transform _transform;
        private ZombieMovementData _movementData;

        private MeleeZombieStateController _stateController;
        
        private MoonkeyComponent _targetMoonkey = null;

        protected bool HasTarget
            => this._targetMoonkey != null;

        // TODO: 

        public override Rigidbody2D Rigidbody
            => this._rigidbody;

        public MeleeZombieMovementController(MeleeZombieController controller, ZombieMovementData data)
            : base(controller, data) 
        {
            this._stateController = (MeleeZombieStateController)controller.StateController;
            this._rigidbody = controller.Component.GetComponent<Rigidbody2D>();
            this._transform = controller.Component.transform;
            this._movementData = data;
        }

        protected override void UpdateMovement()
        {
            if(!this.HasTarget)
            {
                this.SearchForTarget();
            }
        }

        private void SearchForTarget()
        {
            if(this._transform == null)
            {
                return;
            }

            MoonkeyComponent matchedTarget = Moonkey.MoonkeyHolder.GetClosestMoonkey(this._transform.position,
            (Moonkey.MoonkeyComponent a, Moonkey.MoonkeyComponent b) =>
            {
                // TODO: Implement lambda filter.
                return false;
            });

            if(this._targetMoonkey != matchedTarget)
            {
                this._targetMoonkey = matchedTarget;
            }
        }

        protected override void OnDownBeat()
        {
            if(this._transform == null)
            {
                return;
            }

            ZombieReferences references = this._controller.Component.References;
            int beatDown = references.BeatCounter;

            if (this.HasTarget && beatDown % 2 == 0)
            {
                AZombieStateController stateController = this._controller.StateController;
                if (stateController.State != ZombieState.STATE_DANCING)
                {
                    return;
                }

                if (this._targetMoonkey.IsAttacking)
                {
                    this.SearchForTarget();
                    return;
                }

                Vector2 movementDirection = this._targetMoonkey.transform.position - this._transform.position;
                movementDirection.y = 0f;
                this._rigidbody.AddForce(movementDirection.normalized * this._movementData.SpeedForce, ForceMode2D.Impulse);
            }
        }
    }

    /// <summary>
    /// The melee zombie state controller.
    /// </summary>
    public class MeleeZombieStateController : AZombieStateController
    {
        #region fields

        private ZombieTargetController _targetController;
        private int _attackBeat, _spawnBeat;

        private MoonkeyComponent _attacker = null;

        #endregion

        #region properties

        public ZombieReferences? References
            => this._controller?.Component.References;

        public override bool OpenForAttack
        {
            get
            {
                if(this.References.HasValue)
                {
                    return this.References.Value.BeatCounter != this._attackBeat;
                }
                
                return true;
            }
        }

        public override Moonkey.MoonkeyComponent Attacker
            => this._attacker;

        #endregion

        #region constructor

        public MeleeZombieStateController(MeleeZombieController controller)
            : base(controller) 
        {
            this._targetController = controller.TargetController;
            this._spawnBeat = this.References.Value.BeatCounter;
            this. _attackBeat = (this._spawnBeat + 3) % 4;
        }

        #endregion

        #region methods

        protected override void OnBeat()
        {
            if(this._state == ZombieState.STATE_ATTACKED
                && !this._targetController.TargetsActive)
            {
                this._targetController.TargetsActive = true;
            }
        }

        protected override void OnAttacked(MoonkeyComponent component)
        {
            if(this._attacker == null)
            {
                this._attacker = component;
            }

            this._state = ZombieState.STATE_ATTACKED;
        }

        protected override void OnAttack(AttackOutcome outcome)
        {
            if(outcome == AttackOutcome.OUTCOME_FAILED)
            {
                if(this.References.HasValue)
                {
                    this.References.Value.AttackFinishedEvent?.Call(this._attacker, this._controller.Component, outcome);
                }

                this._attacker = null;
                this._state = ZombieState.STATE_DANCING;
                return;
            }

            if(outcome == AttackOutcome.OUTCOME_NORMAL)
            {
                if (this.References.HasValue)
                {
                    this.References.Value.AttackFinishedEvent?.Call(this._attacker, this._controller.Component, outcome);
                }

                return;
            }
            
            if (this.References.HasValue)
            {
                this.References.Value.AttackFinishedEvent?.Call(this._attacker, this._controller.Component, outcome);
            }

            this.Launch();
        }


        /// <summary>
        /// Called when the zombie is launched.
        /// </summary>
        protected override void OnLaunchBegin()
        {
            this._attacker = null;
            this._state = ZombieState.STATE_LAUNCHED;
        }

        protected override void OnLaunchEnd()
        {
            // Calls the zombie death event.
            if (this.References.HasValue)
            {
                this.References.Value.ZombieDeathEvent?.Call();
            }

            // For now it just destroys the object
            GameObject.Destroy(this._controller.Component.gameObject);
        }

        #endregion
    }

    /// <summary>
    /// The main melee zombie controller implementation.
    /// </summary>
    public class MeleeZombieController : AZombieController
    {
        #region fields

        private readonly MeleeZombieStateController _stateController;
        private readonly MeleeZombieMovementController _movementController;
        private readonly MeleeZombieSoundController _soundController;
        private readonly MeleeZombieAnimationController _animationController;

        #endregion

        #region properties

        public override AZombieStateController StateController
            => this._stateController;

        public override AZombieMovementController MovementController 
            => this._movementController;

        public override AZombieSoundController SoundController 
            => this._soundController;

        public override AZombieAnimationController AnimationController 
            => this._animationController;

        #endregion

        #region constructor

        public MeleeZombieController(ZombieComponent component, ZombieSettings settings)
            : base(component, settings)
        {
            this._stateController = new MeleeZombieStateController(this);
            this._movementController = new MeleeZombieMovementController(this, settings.MovementData);
            this._soundController = new MeleeZombieSoundController(this, settings.SoundData);
            this._animationController = new MeleeZombieAnimationController(this, settings.AnimatorController);
        }

        #endregion
    }
}
