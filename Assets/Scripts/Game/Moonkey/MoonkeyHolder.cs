using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Moonkey
{

    /// <summary>
    /// Contains all of the active moonkeys.
    /// </summary>
    public class MoonkeyHolder
    {

        private static List<MoonkeyComponent> moonkeys = new List<MoonkeyComponent>();

        public static void Add(MoonkeyComponent component)
        {
            if(!moonkeys.Contains(component))
            {
                moonkeys.Add(component);
            }
        }

        public static void Remove(MoonkeyComponent component)
        {
            if(moonkeys.Contains(component))
            {
                moonkeys.Remove(component);
            }
        }

        /// <summary>
        /// Searches through the entire list to find the moonkey closest to the start position based on certain filters, etc...
        /// </summary>
        /// <param name="startPosition">The position.</param>
        /// <param name="filter">The filter callable.</param>
        /// <returns>Null or a Moonkey.</returns>
        public static MoonkeyComponent GetClosestMoonkey(Vector2 startPosition, System.Func<MoonkeyComponent, MoonkeyComponent, bool> filter = null)
        {
            if(moonkeys.Count <= 0)
            {
                return null;
            }

            if(moonkeys.Count == 1)
            {
                return moonkeys[0];
            }

            MoonkeyComponent @out = moonkeys[0];
            float shortestDist = Vector2.Distance(startPosition, @out.transform.position);

            for(int i = 1; i < moonkeys.Count; i++)
            {
                MoonkeyComponent compared = moonkeys[1];
                float compShortest = Vector2.Distance(startPosition, compared.transform.position);

                if(compShortest >= shortestDist)
                {
                    continue;
                }

                // Checks the output of the filter.
                if(filter != null && !filter(@out, compared))
                {
                    continue;
                }

                @out = compared;
                shortestDist = compShortest;
            }

            return @out;
        }
    }
}