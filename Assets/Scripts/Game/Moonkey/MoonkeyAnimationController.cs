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
            this._controller.AttackBeginEvent += this.OnAttackBegin;
            this._controller.AttackFinishedEvent += this.OnAttackFinished;
            this._controller.DashEvent += this.OnDash;
            this._controller.JumpEvent += this.OnJump;
            this._controller.LandEvent += this.OnLand;
            this._controller.MoonkeyKilledEvent += this.OnKilled;
        }

        public virtual void UnhookEvents()
        {
            this._controller.AttackBeginEvent -= this.OnAttackBegin;
            this._controller.AttackFinishedEvent -= this.OnAttackFinished;
            this._controller.DashEvent -= this.OnDash;
            this._controller.JumpEvent -= this.OnJump;
            this._controller.LandEvent -= this.OnLand;
            this._controller.MoonkeyKilledEvent -= this.OnKilled;
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
            if (this._movementController.Moving)
            {
                this._animator.Play("moonkey walk persp");
                this._prevMoving = this._movementController.Moving;
                return;
            }

            if (this._prevMoving)
            {
                this._animator.Play("moonkey idle");
            }

            this._prevMoving = this._movementController.Moving;
        }

        private void OnAttackBegin(Zombie.ZombieComponent component)
        {
            // TODO: Implementation.
        }

        private void OnAttackFinished(Zombie.ZombieComponent component, Zombie.AttackOutcome outcome)
        {
            // TODO: Implementation.
        }

        private void OnDash()
        {
            // TODO: Implementation.
        }

        private void OnJump()
        {
            // TODO: Implementation.
            Debug.Log("Moonkey jumped.");
        }

        private void OnLand()
        {
            // TODO: Implementation.
            Debug.Log("Moonkey landed.");
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
