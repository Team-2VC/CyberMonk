using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CyberMonk.Game.Moonkey
{

    /// <summary>
    /// The Moonkey animation controller.
    /// </summary>
    public class MoonkeyAnimationController
    {

        #region fields

        private readonly MoonkeyController _controller;
        private readonly Animator _animator;
        private readonly SpriteRenderer _renderer;

        private MoonkeyMovementController _movementController;

        private bool _prevMoving = false;
        private bool _isDamaged = false, _isAttacking = false;

        #endregion

        #region constructor

        public MoonkeyAnimationController(MoonkeyController controller, RuntimeAnimatorController animatorController)
        {
            this._controller = controller;
            this._movementController = controller.MovementController;

            MoonkeyProperties properties = controller.Component.Properties;

            this._animator = properties.GraphicsAnimator;
            this._animator.runtimeAnimatorController = animatorController;

            this._renderer = properties.GraphicsSpriteRenderer;
        }

        #endregion

        #region methods

        public virtual void HookEvents()
        {
            this._controller.AttackFinishedEvent += this.OnAttackFinished;
            this._controller.AttackBeginEvent += this.OnAttackBegin;
            this._controller.DashEvent += this.OnDash;
            this._controller.JumpEvent += this.OnJump;
            this._controller.LandEvent += this.OnLand;
            this._controller.MoonkeyKilledEvent += this.OnKilled;
            this._controller.HealthChangedEvent += this.OnHealthChanged;
        }

        public virtual void UnhookEvents()
        {
            this._controller.AttackFinishedEvent -= this.OnAttackFinished;
            this._controller.AttackBeginEvent -= this.OnAttackBegin;
            this._controller.DashEvent -= this.OnDash;
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
            this._renderer.flipX = lookDirection == -1;
        }

        /// <summary>
        /// Updates the animations for the movement.
        /// </summary>
        private void UpdateMovingAnimator()
        {
            if(this._isDamaged || this._isAttacking)
            {
                this._prevMoving = false;
                return;
            }

            if (this._movementController.Moving)
            {
                this._animator.Play("moonkey walk persp");
                this._prevMoving = this._movementController.Moving;
                return;
            }

            if (this._prevMoving && this._movementController.OnGround)
            {
                this._animator.Play("moonkey idle");
            }

            this._prevMoving = this._movementController.Moving;
        }

        private void OnHealthChanged(float damaged)
        {
            if(damaged < 0f && this._controller.Health > 0f)
            {
                this._animator.Play("moonkey damaged");
                this._isDamaged = true;
            }
        }

        /// <summary>
        /// Called when the moonkey's damage animation ends.
        /// </summary>
        public void OnMoonkeyDamageEnd()
        {
            this._isDamaged = false;
        }

        private void OnAttackBegin(Zombie.ZombieComponent component)
        {
            this._isAttacking = true;
        }

        private void OnAttackFinished(Zombie.ZombieComponent component, Zombie.AttackOutcome outcome)
        {
            if(outcome != Zombie.AttackOutcome.OUTCOME_NORMAL)
            {
                this._isAttacking = false;
                return;
            }

            // TODO: Punch animation
        }

        private void OnDash()
        {
            this._animator.Play("moonkey fly");
        }

        private void OnJump()
        {
            this._animator.Play("moonkey jump");
        }

        private void OnLand()
        {
            // TODO: Play landed animation
            this._animator.Play("moonkey idle");
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
