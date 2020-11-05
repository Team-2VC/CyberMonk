using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Utils.Events
{
    /// <summary>
    /// The Generic Event class definition without the type.
    /// </summary>
    [CreateAssetMenu(fileName = "Game Event", menuName = "Game Event")]
    public class GameEvent : ScriptableObject
    {

        private event System.Action gameEvent
            = delegate { };

        /// <summary>
        /// Calls the generic event.
        /// </summary>
        /// <param name="input">The input type.</param>
        public virtual void Call()
        {
            this.gameEvent();
        }

        /// <summary>
        /// The addition operation overload function.
        /// </summary>
        /// <param name="e">The event that is being added.</param>
        /// <param name="func">The function that we are adding.</param>
        /// <returns>The generic event.</returns>
        public static GameEvent operator+(GameEvent e, System.Action @func)
        {
            e.gameEvent += func;
            return e;
        }

        /// <summary>
        /// The subtraction operation overload function.
        /// </summary>
        /// <param name="e">The event we are subtracting from.</param>
        /// <param name="func">The function we are subtracting.</param>
        /// <returns>The Generic Event.</returns>
        public static GameEvent operator-(GameEvent e, System.Action @func)
        {
            e.gameEvent -= func;
            return e;
        }
    }
}
