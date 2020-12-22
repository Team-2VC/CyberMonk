using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CyberMonk.Game.Moonkey
{

    /// <summary>
    /// Controls the dash effects.
    /// </summary>
    public class MoonkeyDashFXController
    {
        private List<MoonkeyDashEffect> _effects;

        private MoonkeyComponent _component;
        private MoonkeyProperties _properties;

        protected bool IsDashing
            => this._component.Controller.MovementController.Dashing;

        public MoonkeyDashFXController(MoonkeyComponent component)
        {
            this._effects = new List<MoonkeyDashEffect>();
            this._component = component;
            this._properties = component.Properties;
        }

        public void Update()
        {
            for (int i = this._effects.Count - 1; i >= 0; i--)
            {
                MoonkeyDashEffect effect = this._effects[i];
                if(effect.IsFinished)
                {
                    this._effects.RemoveAt(i);
                }
                effect?.Update();
            }

            if (!this.IsDashing)
            {
                return;
            }

            this.SpawnDashEffect();
        }

        private void SpawnDashEffect()
        {
            MoonkeyDashEffect effect = new MoonkeyDashEffect(
                this._properties.DashFXProperties.DisplayTime, this._component);
            this._effects.Add(effect);
        }
    }

    /// <summary>
    /// The Moonkey animation controller.
    /// </summary>
    public class MoonkeyAnimationController
    {

        #region fields

        private const int MAX_PUNCH_NUMBER = 3;

        private readonly MoonkeyController _controller;
        private MoonkeyProperties _properties;

        private MoonkeyMovementController _movementController;
        private MoonkeyDashFXController _dashEffectsController;

        private RuntimeAnimatorController _animatorController;

        private bool _prevMoving = false;
        private bool _isDamaged = false, _isAttacking = false;
        private bool _isJumping = false;

        private int _punchNumber = 0;

        protected Animator Animator
        {
            get
            {
                Animator animator = this._properties.GraphicsComponent.Animator;
                if(animator == null)
                {
                    return animator;
                }

                if(animator.runtimeAnimatorController == null)
                {
                    animator.runtimeAnimatorController = this._animatorController;
                }
                return animator;
            }
        }

        protected SpriteRenderer Renderer
            => this._properties.GraphicsComponent.SpriteRenderer;

        #endregion

        #region constructor

        public MoonkeyAnimationController(MoonkeyController controller, RuntimeAnimatorController animatorController)
        {
            this._controller = controller;
            this._movementController = controller.MovementController;
            this._properties = controller.Component.Properties;
            this._animatorController = animatorController;
            this._dashEffectsController = new MoonkeyDashFXController(controller.Component);
        }

        #endregion

        #region methods

        public virtual void HookEvents()
        {
            this._controller.AttackFinishedEvent += this.OnAttackFinished;
            this._controller.AttackBeginEvent += this.OnAttackBegin;
            this._controller.DashBeginEvent += this.OnDashBegin;
            this._controller.DashEndEvent -= this.OnDashEnd;
            this._controller.JumpBeginEvent += this.OnJumpBegin;
            this._controller.JumpEvent += this.OnJump;
            this._controller.LandEvent += this.OnLand;
            this._controller.MoonkeyKilledEvent += this.OnKilled;
            this._controller.HealthChangedEvent += this.OnHealthChanged;
        }

        public virtual void UnhookEvents()
        {
            this._controller.AttackFinishedEvent -= this.OnAttackFinished;
            this._controller.AttackBeginEvent -= this.OnAttackBegin;
            this._controller.DashBeginEvent -= this.OnDashBegin;
            this._controller.DashEndEvent -= this.OnDashEnd;
            this._controller.JumpBeginEvent -= this.OnJumpBegin;
            this._controller.JumpEvent -= this.OnJump;
            this._controller.LandEvent -= this.OnLand;
            this._controller.MoonkeyKilledEvent -= this.OnKilled;
            this._controller.HealthChangedEvent -= this.OnHealthChanged;
        }

        public virtual void Update()
        {
            this.UpdateLookDirection();
            this.UpdateMovingAnimator();
        }

        /// <summary>
        /// Updates the look direction.
        /// </summary>
        private void UpdateLookDirection()
        {
            int lookDirection = (int)Mathf.RoundToInt(this._movementController.LookDirection);
            this.Renderer.flipX = lookDirection == -1;
        }

        /// <summary>
        /// Updates the animations for the movement.
        /// </summary>
        private void UpdateMovingAnimator()
        {
            this._dashEffectsController.Update();

            if(this._isJumping)
            {
                this._prevMoving = this._movementController.Moving;
                return;
            }

            if(this._isDamaged || this._isAttacking)
            {
                this._prevMoving = false;
                return;
            }

            if(this._movementController.OnGround && !this._movementController.Dashing)
            {
                if (this._movementController.Moving)
                {
                    this.Animator.Play("moonkey walk persp");
                    this._prevMoving = this._movementController.Moving;
                    return;
                }

                if (this._prevMoving)
                {
                    this.Animator.Play("moonkey idle");
                }
            }

            this._prevMoving = this._movementController.Moving;
        }

        private void OnHealthChanged(float damaged)
        {
            if(damaged < 0f && this._controller.Health > 0f)
            {
                this.Animator.Play("moonkey damaged");
                this._isDamaged = true;
            }
        }

        /// <summary>
        /// Called when the moonkey's damage animation ends.
        /// </summary>
        public void OnMoonkeyDamageEnd()
        {
            this.PlayNormalState();
            this._isDamaged = false;
        }

        private void OnAttackBegin(Zombie.ZombieComponent component)
        {
            // TODO: fighting stance idle?
            this.Animator.Play("moonkey idle");
            this._isAttacking = true;
        }

        private void OnAttackFinished(Zombie.ZombieComponent component, Zombie.AttackOutcome outcome)
        {
            if(outcome != Zombie.AttackOutcome.OUTCOME_FAILED)
            {
                this.Animator.Play("moonkey punch " + this._punchNumber);
                this._punchNumber++;
                this._punchNumber %= MAX_PUNCH_NUMBER;

                if(outcome == Zombie.AttackOutcome.OUTCOME_SUCCESS)
                {
                    this._punchNumber = 0;
                    this._isAttacking = false;
                }
                return;
            }

            this._punchNumber = 0;
            this._isAttacking = false;
        }

        private void OnDashBegin()
        {
            this.Animator.Play("moonkey fly");
        }

        private void OnDashEnd()
        {
            this.PlayNormalState();
        }

        private void OnJumpBegin()
        {
            this._isJumping = true;
            this.Animator.Play("moonkey jump");
        }

        private void OnJump()
        {
            this._isJumping = false;
        }

        private void OnLand()
        {
            // TODO: Play landed animation
            Debug.Log("land pls");
            if(this._isJumping)
            {
                this._isJumping = false;
            }

            this.PlayNormalState();
        }

        private void PlayNormalState()
        {
            if (this._movementController.Moving)
            {
                this.Animator.Play("moonkey walk persp");
                return;
            }

            this._prevMoving = false;
            this.Animator.Play("moonkey idle");
        }

        /// <summary>
        /// Called when the moonkey was killed.
        /// </summary>
        private void OnKilled()
        {
            // TODO: start death animation, once death animation is finished, call on death event
        }

        #endregion
    }
}
