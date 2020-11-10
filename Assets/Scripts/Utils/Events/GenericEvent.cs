using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Utils.Events
{
    /// <summary>
    /// The basic generic event class.
    /// </summary>
    /// <typeparam name="T">The type of input.</typeparam>
    public class GenericEvent<T> : ScriptableObject
    {

        protected event System.Action<T> gameEvent
            = delegate { };

        /// <summary>
        /// Calls the generic event.
        /// </summary>
        /// <param name="input">The input type.</param>
        public virtual void Call(T input)
        {
            this.gameEvent(input);
        }

        /// <summary>
        /// The addition operation overload function.
        /// </summary>
        /// <param name="e">The event that is being added.</param>
        /// <param name="func">The function that we are adding.</param>
        /// <returns>The generic event.</returns>
        public static GenericEvent<T> operator+(GenericEvent<T> e, System.Action<T> @func)
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
        public static GenericEvent<T> operator-(GenericEvent<T> e, System.Action<T> @func)
        {
            e.gameEvent -= func;
            return e;
        }
    }
}