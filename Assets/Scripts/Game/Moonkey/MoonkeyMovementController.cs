using CyberMonk.Game.Moonkey;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        protected GameObject _gameObject;

        private Vector2 _lookDirection = Vector2.right;

        private float _dashCooldownTime = 0f;
        private int _dashCounter = 0;
        private DashingData? _dashingData = null;

        private bool _onGround = true;

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
            if (this.Dashing)
            {
                this._dashCooldownTime += Time.deltaTime;
            }
        }

        /// <summary>
        /// Updates the physics of the monkey.
        /// </summary>
        public virtual void PhysicsUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space) && this.Dash())
            {
                return;
            }

            float horizontalAxis = Input.GetAxis("Horizontal");
            if (Mathf.Abs(horizontalAxis) > 0)
            {
                this.Move(Vector2.right * horizontalAxis);
            }
            /* if(this.Dashing)
            {
                if (this._dashingData.Value.UpdatePosition(
                    this._rigidbody.position, this._lookDirection, this._settings.DashSpeed))
                {
                    this.OnDashFinished();
                }
            }
            */
        }

        /// <summary>
        /// Called to move the monkey.
        /// </summary>
        /// <param name="direction">Moves the monkey in the given direction.</param>
        public virtual void Move(Vector2 direction)
        {
            if(this.Dashing)
            {
                return;
            }

            this._lookDirection = new Vector2(direction.x, 0f);
            this._rigidbody.velocity = direction * this._settings.Speed 
                + new Vector2(0f, this._rigidbody.velocity.y);
        }

        /// <summary>
        /// Called to make the moonkey dash.
        /// </summary>
        /// <returns>True if the monkey successfully dashed, false otherwise.</returns>
        public virtual bool Dash()
        {
            /* if (this.CanDash())
            {
                this._dashingData = new DashingData(
                    this._rigidbody.position, this._lookDirection, this._settings.DashDistance);

                this._dashCooldownTime = 0f;
                this._dashCounter++;
                return true;
            } */
            if(this.CanDash())
            {
                this._rigidbody.AddForce(this._lookDirection, ForceMode2D.Impulse);
                this._dashCooldownTime = 0f;
                this._dashCounter++;
                return true;
            }

            return false;
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

        #endregion


    }
}
