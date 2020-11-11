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
        private Zombie.ZombieComponent _attackedZombieComponent = null;
        private MoonkeyReferences _references;

        #endregion

        #region properties

        public Zombie.ZombieComponent AttackedZombie
            => this._attackedZombieComponent;
        
        #endregion

        #region constructor
        
        public MoonkeyAttackController(MoonkeyController controller)
        {
            this._controller = controller;
            this._references = controller.Component.References;
            
        }

        #endregion

        #region methods

        /// <summary>
        /// Hooks the events.
        /// </summary>
        public virtual void HookEvents()
        {
            this._controller.AttackBeginEvent += this.OnBeginAttack;
            this._controller.AttackFinishedEvent += this.OnAttack;
        }

        /// <summary>
        /// Unhooks the events.
        /// </summary>
        public virtual void UnHookEvents()
        {
            this._controller.AttackBeginEvent -= this.OnBeginAttack;
            this._controller.AttackFinishedEvent -= this.OnAttack;
        }

        public virtual void Update() { }

        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
            if (this._controller.MovementController.Dashing)
            {
                Zombie.ZombieComponent component = collider.GetComponent<Zombie.ZombieComponent>();
                Zombie.TryZombieAttackOutcome? outcome = component?.TryHandleAttack(this._controller.Component);
                // TODO: Check the outcome.
            }
        }

        /// <summary>
        /// Called when the attack has begun.
        /// </summary>
        /// <param name="attackedComponent">The zombie component we are attacking.</param>
        private void OnBeginAttack(Zombie.ZombieComponent attackedComponent)
        {
            this._attackedZombieComponent = attackedComponent;
        }

        /// <summary>
        /// Called when player missed their attack or they finished.
        /// </summary>
        /// <param name="zombieComponent">The zombie failed attack event.</param>
        /// <param name="outcome">The zombie attack outcome.</param>
        private void OnAttack(Zombie.ZombieComponent component, Zombie.AttackOutcome outcome)
        {
            
            if(outcome == Zombie.AttackOutcome.OUTCOME_FAILED)
            {
                this._attackedZombieComponent = null;
                this._references.ComboCounter = 0;
                this._references.ComboMultiplier = 1;
            } else if(outcome == Zombie.AttackOutcome.OUTCOME_SUCCESS)
            {
                this._references.ComboMultiplier *= 2;
                this._attackedZombieComponent = null;
            } else if(outcome == Zombie.AttackOutcome.OUTCOME_NORMAL)
            {
                this._references.ComboCounter += 1;
                this._references.TotalScore += 100 * this._references.ComboMultiplier;
                
            }

            Debug.Log(this._references.ComboCounter);


        }

        public virtual void OnTriggerExit2D(Collider2D collider) { }



        #endregion
    }
}
   
