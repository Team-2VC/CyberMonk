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
            this._controller.AttackFinishedEvent += this.OnAttack;
        }

        /// <summary>
        /// Unhooks the events.
        /// </summary>
        public virtual void UnHookEvents()
        {
            this._controller.AttackFinishedEvent -= this.OnAttack;
        }

        public virtual void Update() { }

        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
            if (this._controller.MovementController.Dashing)
            {
                Zombie.ZombieComponent component = collider.GetComponent<Zombie.ZombieComponent>();
                
                if(component != null)
                {
                    Zombie.TryZombieAttackOutcome outcome = component.TryHandleAttack(this._controller.Component);
                    this.OnBeginAttack(component, outcome);
                }
            }
        }

        /// <summary>
        /// Called when the player has attempted to begin an attack.
        /// </summary>
        /// <param name="attackedComponent">The zombie component we are attacking.</param>
        /// <param name="outcome">The outcome of the attemped zombie attack.</param>
        private void OnBeginAttack(Zombie.ZombieComponent attackedComponent, Zombie.TryZombieAttackOutcome outcome)
        {
            switch (outcome)
            {
                case Zombie.TryZombieAttackOutcome.OUTCOME_SUCCESS:
                    this._controller.OnBeginAttack(attackedComponent);
                    this._attackedZombieComponent = attackedComponent;
                    break;
                case Zombie.TryZombieAttackOutcome.OUTCOME_FAILED_ZOMBIE_ATTACKING:
                    // TODO: Damage the player.
                    this._controller.MovementController.ForceStopDashing();
                    this._controller.Damage(attackedComponent.References.DamageAmount);
                    break;
            }
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
            } 
            else if(outcome == Zombie.AttackOutcome.OUTCOME_SUCCESS)
            {
                this._references.ComboMultiplier *= 2;
                this._references.ComboCounter++;
                this._attackedZombieComponent = null;
            } 
            else if(outcome == Zombie.AttackOutcome.OUTCOME_NORMAL)
            {
                this._references.ComboCounter += 1;
                this._references.TotalScore += 100 * this._references.ComboMultiplier;
            }
        }

        public virtual void OnTriggerExit2D(Collider2D collider) { }



        #endregion
    }
}
   
