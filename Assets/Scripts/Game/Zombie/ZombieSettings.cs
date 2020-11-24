using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Zombie
{

    [System.Serializable]
    public struct ZombieMovementData
    {
        [SerializeField]
        private float speedForce;

        public float SpeedForce
            => this.speedForce;
    }

    /// <summary>
    /// The Zombie Settings.
    /// </summary>
    [CreateAssetMenu(fileName = "Zombie Settings", menuName = "Zombie Settings")]
    public class ZombieSettings : ScriptableObject
    {
        #region fields

        [SerializeField]
        private ZombieType type;

        [SerializeField]
        private Target.ZombieTargetsData targetsData;
        [SerializeField]
        private ZombieMovementData movementData;

        #endregion

        #region properties

        public ZombieType Type => this.type;

        public Target.ZombieTargetsData TargetsData
            => this.targetsData;

        public ZombieMovementData MovementData
            => this.movementData;

        #endregion
    }
}