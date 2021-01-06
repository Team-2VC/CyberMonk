using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Utils.Events
{
    /// <summary>
    /// The string game event.
    /// </summary>
    [CreateAssetMenu(fileName = "Game Event (String)", menuName = "Events/Game Event (String)")]
    public class StringEvent : GenericEvent<string> 
    {

        /// <summary>
        /// The addition operation overload function.
        /// </summary>
        /// <param name="e">The event that is being added.</param>
        /// <param name="func">The function that we are adding.</param>
        /// <returns>The generic event.</returns>
        public static StringEvent operator +(StringEvent e, System.Action<string> @func)
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
        public static StringEvent operator -(StringEvent e, System.Action<string> @func)
        {
            e.gameEvent -= func;
            return e;
        }
    }
}