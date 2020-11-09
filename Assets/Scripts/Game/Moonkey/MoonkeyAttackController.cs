using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


namespace CyberMonk.Game.Moonkey
{
    public class MoonkeyAttackController
    {
        #region fields

        private MoonkeyController _controller;
        private bool _isDashing;
        
        #endregion

        #region properties
        
        #endregion

        #region constructor
        
        public MoonkeyAttackController(MoonkeyController controller)
        {
            this._controller = controller;
        }

        #endregion

        #region methods

        public virtual void Update()
        {
            /* if (this._controller.MovementController.Dashing)
            {
                Debug.Log("Is Dashing! from Attack Controller");
            } */  
        }


        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
            if (this._controller.MovementController.Dashing)
            {
                Zombie.ZombieComponent component = collider.GetComponent<Zombie.ZombieComponent>();
                Zombie.TryZombieAttackOutcome? outcome = component?.TryHandleAttack(this._controller.Component);
                Debug.Log(outcome);
            }
        }

        public virtual void OnTriggerExit2D(Collider2D collider)
        {
            // TODO: Implementation.
        }

        #endregion
    }
}
   
