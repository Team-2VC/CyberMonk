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
    public enum MonkeyState
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

        private MonkeyState _state;
        private MoonkeyController _controller;

        #endregion

        #region properties

        public MonkeyState State
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
            this._state = MonkeyState.STATE_ATTACKING;
        }

        #endregion
    }
}
