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

        private readonly MoonkeyController _controller;
        private readonly Animator _animator;
        private readonly SpriteRenderer _renderer;

        private MoonkeyMovementController _movementController;

        private bool _prevMoving = false;

        public MoonkeyAnimationController(MoonkeyController controller, RuntimeAnimatorController animatorController)
        {
            this._controller = controller;
            this._movementController = controller.MovementController;

            MoonkeyProperties properties = controller.Component.Properties;

            this._animator = properties.GraphicsAnimator;
            this._animator.runtimeAnimatorController = animatorController;

            this._renderer = properties.GraphicsSpriteRenderer;
        }

        public virtual void HookEvents()
        {
            this._controller.AttackBeginEvent += this.OnAttackBegin;
            this._controller.AttackFinishedEvent += this.OnAttackFinished;
            this._controller.DashEvent += this.OnDash;
            this._controller.JumpEvent += this.OnJump;
            this._controller.LandEvent += this.OnLand;
        }

        public virtual void UnhookEvents()
        {
            this._controller.AttackBeginEvent -= this.OnAttackBegin;
            this._controller.AttackFinishedEvent -= this.OnAttackFinished;
            this._controller.DashEvent -= this.OnDash;
            this._controller.JumpEvent -= this.OnJump;
            this._controller.LandEvent -= this.OnLand;
        }

        public virtual void Update()
        {
            this.UpdateMovingAnimator();
        }

        /// <summary>
        /// Updates the animations for the movement.
        /// </summary>
        private void UpdateMovingAnimator()
        {
            if (this._movementController.Moving)
            {
                this._animator.Play("moonkey walk persp");
                int lookDirection = (int)Mathf.RoundToInt(this._movementController.LookDirection);
                this._renderer.flipX = lookDirection == -1;
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
    }
}
