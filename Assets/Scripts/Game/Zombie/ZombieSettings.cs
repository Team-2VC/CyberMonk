using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Zombie
{

    [System.Serializable]
    public struct ZombieMovementData
    {

        [System.Serializable]
        public struct ZombieLaunchData
        {
            [SerializeField]
            private float launchHeight;
            [SerializeField]
            private float launchDuration;

            public float LaunchHeight
                => this.launchHeight;

            public float LaunchDuration
                => this.launchDuration;
        }

        [SerializeField]
        private float speedForce;
        [SerializeField]
        private ZombieLaunchData launchData;

        public float SpeedForce
            => this.speedForce;

        public ZombieLaunchData LaunchData
            => this.launchData;
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