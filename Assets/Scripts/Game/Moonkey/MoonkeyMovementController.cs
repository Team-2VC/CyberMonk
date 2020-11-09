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
    /// Contains the data for dashing.
    /// </summary>
    public struct DashingData
    {
        
        private Vector2 finalPosition;
        
        private float lowestDistance;

        public Vector2 FinalPosition
            => this.finalPosition;

        public DashingData(Vector2 initalPosition, Vector2 lookDirection, float distance)
        {
            this.finalPosition = new Vector2(
                initalPosition.x + (lookDirection.x * distance), initalPosition.y);
            
            this.lowestDistance = float.MaxValue;
        }

        /// <summary>
        /// Updates the position of the data.
        /// </summary>
        /// <param name="currentPosition">The current position.</param>
        /// <param name="direction">The jump direction.</param>
        /// <param name="speed">The speed of the monkey.</param>
        /// <returns>True if it has reached the final position, false otherwise.</returns>
        public bool UpdatePosition(Vector2 currentPosition, Vector2 direction, float speed)
        {
            Vector2 nextTranslation = direction * speed * Time.deltaTime;
            Vector2 nextPosition = (Vector2)currentPosition + nextTranslation;

            float distance = Vector2.Distance(nextPosition, this.finalPosition);

            if(distance < this.lowestDistance)
            {
                this.lowestDistance = distance;
            }

            if(distance > this.lowestDistance)
            {
                return true;
            }

            return false;
        }
    }

    public class MoonkeyMovementController
    {
        #region fields

        // TODO: Add Cooldown

        protected readonly Rigidbody2D _rigidbody;
        protected readonly MoonkeySettings _settings;
        protected readonly GameObject _gameObject;
        
     
        private float _dashTime = 0f;
        private bool _isDashing = false;

        private float _dashCooldownTime = 0f;
        private int _dashCounter = 0;

        private float _floatTime = 0;
        private bool _isFloating = false;

        
        private DashingData? _dashingData = null;

        private bool _onGround = true;
        private int _jumpBuffer = 0;
        private bool _jumpPressed = false;
        private bool _isJumping = false;
        private float _jumpTimeCounter = 0;


        #endregion

        #region properties

        public bool Dashing
            => this._dashingData.HasValue;

        #endregion


        #region constructor

        public MoonkeyMovementController(MoonkeyController controller, MoonkeySettings settings)
        {
            Debug.Log("constructing movement controller");
            this._rigidbody = controller.Component.GetComponent<Rigidbody2D>();

            this._gameObject = controller.Component.gameObject;
            this._settings = settings;
            
            
        }

        #endregion

        #region methods

        /// <summary>
        /// Called to update the moonkey movement controller.
        /// </summary>
        public virtual void Update()
        {
            
            #region jump inputs

            if (Input.GetKeyDown(KeyCode.Space))
            {
                this._jumpPressed = true;
                this._jumpBuffer = this._settings.InputBufferForFrames;
            }

            // Buffer jump input so that it feels better to jump if they push early.
            if (this._jumpBuffer >= 0)
            {
                
                if (this._onGround && this._jumpPressed)
                {
                    this.Jump();
                    this._isJumping = true;
                    this._jumpTimeCounter = this._settings.JumpTime;
                    this._jumpPressed = false;
                }
                this._jumpBuffer -= 1;
            }

            if(this._isJumping && Input.GetKey(KeyCode.Space))
            {
                if(this._jumpTimeCounter > 0)
                {
                    this.Jump();
                    this._jumpTimeCounter -= Time.deltaTime;
                } else
                {
                    this._isJumping = false;
                }
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                this._isJumping = false;
            }

            #endregion 



            if (Input.GetMouseButtonDown(0))
            {
                this._isDashing = this.Dash();
                this._isFloating = this.Float();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                this._rigidbody.gravityScale = 1;
                this._isFloating = false;
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

            if (this._isDashing)
            {
                this._dashTime -= Time.fixedDeltaTime;
                if (this._dashTime <= 0)
                {
                    // Stops Dashing here
                    // TODO: C# Event to stop dashing.
                    
                    this._rigidbody.velocity = Vector2.zero;
                    this._isFloating = this.Float();
                    this._isDashing = false;
                }
            }

            if (this._isFloating)
            {
                this._floatTime -= Time.fixedDeltaTime;
                if(this._floatTime <= 0)
                {
                    this._rigidbody.gravityScale = 1;
                    this._isFloating = false;
                }
            }




        }

        /// <summary>
        /// Called to move the monkey.
        /// </summary>
        /// <param name="direction">Moves the monkey in the given direction.</param>
        public virtual void Move(Vector2 direction)
        {
            if(this._isDashing && this._isFloating)
            {
                return;
            }
            this._rigidbody.gravityScale = 1;
            this._rigidbody.velocity = new Vector2((direction.x * this._settings.Speed), this._rigidbody.velocity.y);
        }

        /// <summary>
        /// Called to make the moonkey dash.
        /// </summary>
        /// <returns>True if the monkey successfully dashed, false otherwise.</returns>
        public virtual bool Dash()
        {
            Debug.Log("Dashing");
            Vector2 mousePos = Vector2.zero;
            if(Camera.main != null)
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            //v = d/t --> t = d/v time to dash 
            this._dashTime = Vector2.Distance(mousePos, (Vector2)this._gameObject.transform.position)/this._settings.DashSpeed;

            this._rigidbody.velocity = (mousePos - (Vector2)this._gameObject.transform.position).normalized * this._settings.DashSpeed;
            
            
            return true;           
  
        }

        public virtual bool Float()
        {
            this._rigidbody.gravityScale = 0;
            this._floatTime = this._settings.FloatTime;
            return true;
        }

        // TODO: Hold jump to jump higher.
        // TODO: isGrounded Implementation

        public virtual void Jump()
        {
            this._rigidbody.velocity = Vector2.up * this._settings.JumpForce;
        }


        /// <summary>
        /// Determines whether or not the moonkey can dash.
        /// </summary>
        /// <returns>True if the monkey can dash, false otherwise.</returns>
        protected bool CanDash()
        {
            if(this._dashCounter > this._settings.DashMaxCounter)
            {
                return false;
            }

            if(this._dashCooldownTime <= this._settings.DashCooldownTime && this.Dashing)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Called when the dash is finished.
        /// </summary>
        protected void OnDashFinished()
        {
            this._dashingData = null;
            this._dashCooldownTime = 0f;
            this._dashCounter = 0;

            Debug.Log("Dashing has finished");

            if(this._onGround)
            {
                this._rigidbody.velocity = Vector2.zero;
            }
        }



        public void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("Collision Enter");
            if(collision.gameObject.tag == "ground")
            {
                this._onGround = true;
            }
                
        }

        public void OnCollisionExit2D(Collision2D collision)
        {

            Debug.Log("Collision Exit");
            if(collision.gameObject.tag == "ground")
            {
                this._onGround = false;
            }
            
        }

        #endregion


        

    }
}
