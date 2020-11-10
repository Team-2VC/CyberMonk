using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Moonkey.Events
{
    /// <summary>
    /// Handles when the monkey failed an attack.
    /// </summary>
    [CreateAssetMenu(fileName = "Moonkey Attack Event", menuName = "Moonkey Attack Event")]
    public class MoonkeyAttackEvent : ScriptableObject
    {

        private event System.Action<Moonkey.MoonkeyComponent, Zombie.ZombieComponent, Zombie.AttackOutcome> gameEvent
            = delegate { };

        /// <summary>
        /// Calls the event.
        /// </summary>
        /// <param name="m">The monkey component.</param>
        /// <param name="z">The zombie component.</param>
        /// <param name="e">The attack outcome.</param>
        public void Call(Moonkey.MoonkeyComponent m, Zombie.ZombieComponent z, Zombie.AttackOutcome e)
        {
            this.gameEvent(m, z, e);
        }

        /// <summary>
        /// The addition operation overload function.
        /// </summary>
        /// <param name="e">The event that is being added.</param>
        /// <param name="func">The function that we are adding.</param>
        /// <returns>The generic event.</returns>
        public static MoonkeyAttackEvent operator +(MoonkeyAttackEvent e, System.Action<Moonkey.MoonkeyComponent, Zombie.ZombieComponent, Zombie.AttackOutcome> @func)
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
        public static MoonkeyAttackEvent operator -(MoonkeyAttackEvent e, System.Action<Moonkey.MoonkeyComponent, Zombie.ZombieComponent, Zombie.AttackOutcome> @func)
        {
            e.gameEvent -= func;
            return e;
        }
    }
}

