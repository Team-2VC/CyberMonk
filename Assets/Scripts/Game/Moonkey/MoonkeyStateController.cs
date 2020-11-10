using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this._controller.AttackFinishedEvent += this.OnFinishedAttack;
        }

        /// <summary>
        /// Unhooks the events from the moonkey state controller.
        /// </summary>
        public void UnHookEvents()
        {
            this._controller.AttackBeginEvent -= this.OnBeginAttack;
            this._controller.AttackFinishedEvent -= this.OnFinishedAttack;
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
        /// Called when the attack has failed.
        /// </summary>
        /// <param name="component">The zombie component.</param>
        /// <param name="outcome">The attack outcome.</param>
        private void OnFinishedAttack(Zombie.ZombieComponent component, Zombie.AttackOutcome outcome)
        {
            // TODO: damage the health & state based on outcome.

            this._state = MoonkeyState.STATE_MOVING;
        }

        #endregion
    }
}
