using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CyberMonk.Game.Moonkey
{
    /// <summary>
    /// The Monkey State.
    /// </summary>
    public enum MoonkeyState
    {
        STATE_MOVING,
        STATE_ATTACKING,
        STATE_DYING,
        STATE_DEAD
    }

    /// <summary>
    /// Handles the moonkey state controller.
    /// </summary>
    public class MoonkeyStateController
    {

        #region fields

        private MoonkeyState _state;
        private MoonkeyController _controller;

        #endregion

        #region properties

        public event System.Action<Zombie.ZombieComponent, Zombie.AttackOutcome> AttackFinishedEvent
            = delegate { };

        public event System.Action MoonkeyKilledEvent
            = delegate { };

        public MoonkeyState State
            => this._state;

        #endregion

        #region constuctor

        public MoonkeyStateController(MoonkeyController controller)
        {
            this._controller = controller;
        }

        #endregion

        #region methods

        /// <summary>
        /// Hooks the events to the moonkey state controller.
        /// </summary>
        public void HookEvents()
        {
            this._controller.AttackBeginEvent += this.OnBeginAttack;
        }

        /// <summary>
        /// Unhooks the events from the moonkey state controller.
        /// </summary>
        public void UnHookEvents()
        {
            this._controller.AttackBeginEvent -= this.OnBeginAttack;
        }

        /// <summary>
        /// Called when the attack has begun.
        /// </summary>
        /// <param name="component">The zombie component.</param>
        private void OnBeginAttack(Zombie.ZombieComponent component)
        {
            // TODO: 
            this._state = MoonkeyState.STATE_ATTACKING;
        }

        /// <summary>
        /// Kills the moonkey.
        /// </summary>
        public void Kill()
        {
            if(this._state >= MoonkeyState.STATE_DYING)
            {
                return;
            }

            this._state = MoonkeyState.STATE_DYING;
            this.MoonkeyKilledEvent();
            
            // TODO: Remove this;
            GameObject.Destroy(this._controller.Component.gameObject);
        }

        /// <summary>
        /// Called when the attack has failed.
        /// </summary>
        /// <param name="component">The zombie component.</param>
        /// <param name="outcome">The attack outcome.</param>
        public void OnAttackFinished(Zombie.ZombieComponent component, Zombie.AttackOutcome outcome)
        {
            if(outcome == Zombie.AttackOutcome.OUTCOME_FAILED)
            {
                if(this._controller.Damage(component.References.DamageAmount))
                {
                    return;
                }
            }

            // TODO: Change
            if(outcome != Zombie.AttackOutcome.OUTCOME_NORMAL)
            {
                this._state = MoonkeyState.STATE_MOVING;
            }

            this.AttackFinishedEvent(component, outcome);
        }

        #endregion
    }
}
