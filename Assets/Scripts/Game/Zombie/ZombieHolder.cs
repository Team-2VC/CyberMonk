using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Zombie
{
    /// <summary>
    /// The Zombie Holder class.
    /// </summary>
    public class ZombieHolder
    {
        private static List<ZombieComponent> zombies = new List<ZombieComponent>();

        public static void Add(ZombieComponent component)
        {
            if(!zombies.Contains(component))
            {
                zombies.Add(component);
            }
        }

        public static void Remove(ZombieComponent component)
        {
            if(zombies.Contains(component))
            {
                zombies.Add(component);
            }
        }

        public static bool Contains(ZombieComponent component)
        {
            return zombies.Contains(component);
        }
    }
}