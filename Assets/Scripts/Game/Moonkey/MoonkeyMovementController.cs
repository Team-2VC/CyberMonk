using CyberMonk.Game.Moonkey;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CyberMonk.Game.Moonkey
{
    public class MoonkeyMovementController
    {
        #region fields

        protected readonly Rigidbody2D _rigidbody;
        protected readonly GameObject _gameObject;
        protected readonly MoonkeySettings _settings;
        //protected readonly float _speed;
        //protected readonly float _gravityScale;
        //protected readonly float _dashSpeed;
        //protected readonly float _dashTime;

        private float _dashTime;

        #endregion


        #region constructor
        public MoonkeyMovementController(MoonkeyController controller, MoonkeySettings settings)
        {
            Debug.Log("constructing movement controller");
            this._rigidbody = controller.Component.GetComponent<Rigidbody2D>()
                ?? controller.Component.gameObject.AddComponent<Rigidbody2D>();

            this._gameObject = controller.Component.gameObject;

            this._settings = settings;
        }
        #endregion

        #region methods
        public virtual void Move()
        {
            float direction = Input.GetAxis("Horizontal");
            this._rigidbody.velocity = new Vector2(direction * this._settings.Speed, this._rigidbody.velocity.y);
        }


        public virtual void Dash()
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                this._rigidbody.velocity = (Input.mousePosition - this._gameObject.transform.position).normalized * this._settings.DashSpeed;
                this._dashTime = _settings.DashTime;
            }


            if (this._dashTime <= 0)
            {
                this._rigidbody.velocity = Vector2.zero;
            }
            else
            {
                this._dashTime -= Time.deltaTime;
            }

        }



        #endregion


    }
}
