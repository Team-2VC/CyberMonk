using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CyberMonk.Game.Moonkey
{
    public class MoonkeyAttackController
    {
        #region fields
        protected readonly MoonkeyMovementController _movementController;

        private bool _isDashing;
        #endregion

        #region properties
        
        #endregion

        #region constructor
        public MoonkeyAttackController(MoonkeyController controller)
        {
            this._movementController = controller.MovementController;
            
        }
        #endregion

        #region methods
        public virtual void Update()
        {
            if (this._movementController.IsDashing)
            {
                Debug.Log("Is Dashing! from Attack Controller");
            }   
        }


        #endregion


    }
}
   
