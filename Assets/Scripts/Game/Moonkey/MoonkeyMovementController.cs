using CyberMonk.Game.Moonkey;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace CyberMonk.Game.Moonkey
{

    /// <summary>
    /// Handles the moonkey movement.
    /// </summary>
    public class MoonkeyMovementController
    {
        #region fields

        protected readonly Rigidbody2D _rigidbody;
        protected readonly Transform _transform;

        private MoonkeyController _controller;
        private readonly MoonkeyMovementSettings _movementSettings;
        
        private float _dashTime = 0f;
        private float _dashCooldownTime = 0f;
        private int _dashCounter = 0;

        private Vector2 _lookDirection = Vector2.right;

        private int _jumpBuffer = 0;
        private float _jumpBoostTimeCounter = 0f;
        
        private bool _onGround = true;
        private float _offGroundTime = 0f;

        private bool _currentlyDamaged = false;

        public event System.Action JumpEvent
            = delegate { };

        public event System.Action JumpBeginEvent
            = delegate { };

        public event System.Action LandEvent
            = delegate { };

        public event System.Action DashBeginEvent
            = delegate { };

        public event System.Action DashEndEvent
            = delegate { };

        #endregion

        #region properties

        // TODO: Reference user input.
        protected bool JumpPressed
            => Input.GetKeyDown(KeyCode.Space);

        // TODO: Reference user input.
        protected bool JumpBoostPressed
            => Input.GetKey(KeyCode.Space);

        // TODO: Reference user input.
        protected bool DashPressed
            => Input.GetMouseButtonDown(0);

        /// <summary>
        /// Determines whether the moonkey's jump can be boosted.
        /// </summary>
        protected bool BoostJump
            => this._jumpBoostTimeCounter > 0f && this.JumpBoostPressed;

        public bool Dashing 
            => this._dashTime > 0f;

        /// <summary>
        /// Determines whether the monkey is on the ground.
        /// </summary>
        public bool OnGround
            => this._onGround;

        public bool Idle
            => this._onGround && !this.Moving;

        public bool Moving
            => Mathf.Abs(this._rigidbody.velocity.x) > 0f;

        public float LookDirection
            => this._lookDirection.x;

        #endregion

        #region constructor
        
        /// <summary>
        /// The Moonkey movement controller.
        /// </summary>
        /// <param name="controller">The moonkey controller reference.</param>
        /// <param name="settings">The moonkey settings.</param>
        public MoonkeyMovementController(MoonkeyController controller, MoonkeySettings settings)
        {
            this._controller = controller;
            this._rigidbody = controller.Component.GetComponent<Rigidbody2D>();
            this._transform = controller.Component.transform;

            this._movementSettings = settings.MovementSettings;

            this._dashCounter = this._movementSettings.DashMaxCounter;
            this._dashCooldownTime = this._movementSettings.DashCooldownTime;
        }

        #endregion

        #region methods

        public void HookEvents()
        {
            this._controller.AttackBeginEvent += this.OnAttackBegin;
            this._controller.HealthChangedEvent += this.OnHealthChanged;
        }

        public void UnHookEvents()
        {
            this._controller.AttackBeginEvent -= this.OnAttackBegin;
            this._controller.HealthChangedEvent -= this.OnHealthChanged;
        }

        private void OnHealthChanged(float healthChanged)
        {
            if(healthChanged < 0f && this._controller.Health > 0f)
            {
                if(this.Dashing)
                {
                    this.ForceStopDashing(false);
                }
                this._currentlyDamaged = true;
            }
        }

        public void OnMoonkeyDamageEnd()
        {
            this._currentlyDamaged = false;
        }

        private void OnAttackBegin(Zombie.ZombieComponent component)
        {
            if(this.Dashing)
            {
                Vector2 difference = component.transform.position - this._transform.position;
                float direction = difference.x < 0 ? -1 : 1;
                if(direction != this.LookDirection)
                {
                    this._lookDirection.x = direction;
                }

                this.ForceStopDashing();
            }
        }

        /// <summary>
        /// Called to update the moonkey movement controller.
        /// </summary>
        public virtual void Update()
        {
            this.HandleJumpingInput();
            this.HandleDashing();

            if(!this.OnGround)
            {
                this._offGroundTime += Time.deltaTime;
            }
        }

        /// <summary>
        /// Handles the jumping input.
        /// </summary>
        private void HandleJumpingInput()
        {
            if(this.JumpPressed)
            {
                this._jumpBuffer = this._movementSettings.InputBufferForFrames;
            }

            if(this._onGround)
            {
                this.DecrementJumpBuffer();

                if (this.CanJump())
                {
                    this.OnJumpBegin();
                }
                return;
            }

            if(this._jumpBoostTimeCounter > 0f)
            {
                this._jumpBoostTimeCounter -= Time.deltaTime;

                if(this._jumpBoostTimeCounter <= 0f)
                {
                    this._jumpBoostTimeCounter = 0f;
                }
            }

            this.DecrementJumpBuffer();
        }

        /// <summary>
        /// Decrements the jump buffer.
        /// </summary>
        private void DecrementJumpBuffer()
        {
            if(this._jumpBuffer > 0)
            {
                this._jumpBuffer--;

                if(this._jumpBuffer <= 0)
                {
                    this._jumpBuffer = 0;
                }
            }
        }

        /// <summary>
        /// Handles the dashing of the moonkey.
        /// </summary>
        private void HandleDashing()
        {
            if (this._dashCounter < this._movementSettings.DashMaxCounter)
            {
                this._dashCooldownTime -= Time.deltaTime;
                if (this._dashCooldownTime <= 0)
                {
                    this._dashCounter++;
                    this._dashCooldownTime = this._movementSettings.DashCooldownTime;
                }
            }

            if (this.DashPressed && this.CanDash())
            {
                this.Dash();
            }
        }

        /// <summary>
        /// Updates the physics of the monkey.
        /// </summary>
        public virtual void PhysicsUpdate()
        {
            float horizontalAxis = Input.GetAxis("Horizontal");
            if (Mathf.Abs(horizontalAxis) > 0)
            {
                this.Move(Vector2.right * horizontalAxis);
            }

            // Applies a jump boost if the moonkey isn't on ground.
            if (this.BoostJump && !this._onGround)
            {
                // Applies a jump boost.
                this.ApplyJumpBoost();
            }

            if (this.Dashing)
            {
                this._dashTime -= Time.fixedDeltaTime;
                
                if (this._dashTime <= 0)
                {
                    // Stops Dashing here
                    this.ForceStopDashing();
                }
            }
        }

        /// <summary>
        /// Forces the moonkey to stop dashing.
        /// </summary>
        /// <param name="callEvent">Call the dash end event.</param>
        public void ForceStopDashing(bool callEvent = true)
        {
            if(callEvent)
            {
                this.DashEndEvent();
            }

            this._dashTime = 0f;
            this._rigidbody.velocity = Vector2.zero;
        }

        /// <summary>
        /// Called to move the monkey.
        /// </summary>
        /// <param name="direction">Moves the monkey in the given direction.</param>
        public virtual void Move(Vector2 direction)
        {
            if(this.Dashing || this._currentlyDamaged)
            {
                return;
            }

            Vector2 newDirection = direction.normalized;
            this._lookDirection = newDirection;
            this._rigidbody.velocity = new Vector2((direction.x * this._movementSettings.MovementSpeed), this._rigidbody.velocity.y);
        }

        /// <summary>
        /// Determines whether the Moonkey can jump.
        /// </summary>
        /// <returns>True if the monkey can jump, false otherwise.</returns>
        protected bool CanJump()
        {
            return (this._jumpBuffer > 0 || this.JumpPressed) && this._onGround && !this._currentlyDamaged;
        }

        /// <summary>
        /// Called to start the jumping sequence.
        /// </summary>
        private void OnJumpBegin()
        {
            this.JumpBeginEvent();
        }

        /// <summary>
        /// Forces the player to jump.
        /// </summary>
        public virtual void Jump()
        {
            // Ensures that the jump event is called once.
            if (this._jumpBoostTimeCounter <= 0f)
            {
                this.JumpEvent();
            }

            this._jumpBoostTimeCounter = this._movementSettings.MaxJumpBoostTime;
            Vector2 currentVelocity = this._rigidbody.velocity;
            this._rigidbody.velocity = new Vector2(currentVelocity.x, this._movementSettings.JumpForce);
        }

        /// <summary>
        /// Applies a jump boost to the player.
        /// </summary>
        protected virtual void ApplyJumpBoost()
        {
            // TODO: Possibly add a jump boost force variable?
            Vector2 currentVelocity = this._rigidbody.velocity;
            this._rigidbody.velocity = new Vector2(currentVelocity.x, this._movementSettings.JumpForce);
        }

        /// <summary>
        /// Determines whether or not the moonkey can dash.
        /// </summary>
        /// <returns>True if the monkey can dash, false otherwise.</returns>
        protected bool CanDash()
        {
            return this._dashCounter > 0 && !this._currentlyDamaged;
        }

        /// <summary>
        /// Called to make the moonkey dash.
        /// </summary>
        /// <returns>True if the monkey successfully dashed, false otherwise.</returns>
        public virtual void Dash()
        {
            this._dashTime = this._movementSettings.DashTime;
            this._rigidbody.velocity = this._lookDirection * this._movementSettings.DashSpeed;
            this._dashCounter--;

            this.DashBeginEvent();
        }

        /// <summary>
        /// Called when the moonkey has entered a collision.
        /// </summary>
        /// <param name="collision">The collision entered.</param>
        public void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.tag == "ground")
            {
                if(!this.OnGround && this._offGroundTime >= 0.1f)
                {
                    this.LandEvent();
                }

                this._offGroundTime = 0f;
                this._onGround = true;
            }
        }

        /// <summary>
        /// Called when the moonkey has exited a collision.
        /// </summary>
        /// <param name="collision">The collision exited.</param>
        public void OnCollisionExit2D(Collision2D collision)
        {
            if(collision.gameObject.tag == "ground")
            {
                this._onGround = false;
            }
        }

        #endregion
    }
}
