using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Zombie
{
    /// <summary>
    /// The Zombie Settings.
    /// </summary>
    [CreateAssetMenu(fileName = "Zombie Settings", menuName = "Zombie Settings")]
    public class ZombieSettings : ScriptableObject
    {
        #region fields

        [SerializeField]
        private ZombieType type;

        #endregion

        #region properties

        public ZombieType Type => this.type;

        #endregion
    }
}