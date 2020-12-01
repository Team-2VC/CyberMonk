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

    [System.Serializable]
    public class ZombieSoundData
    {
        [SerializeField]
        private string launchSound;

        public Dictionary<ZombieSoundType, FMOD.Studio.EventInstance> GetSoundsList(ZombieType zombieType)
        {
            Dictionary<ZombieSoundType, FMOD.Studio.EventInstance> sounds =
                new Dictionary<ZombieSoundType, FMOD.Studio.EventInstance>();
            this.AddSound(ZombieSoundType.SOUND_LAUNCH, this.launchSound, ref sounds);
            return sounds;
        }

        /// <summary>
        /// Adds a sound to the sounds dictionary.
        /// </summary>
        /// <param name="soundType">The sound type.</param>
        /// <param name="soundPath">The sound path.</param>
        /// <param name="sounds">The references to the list of sounds</param>
        private void AddSound(ZombieSoundType soundType, string soundPath, ref Dictionary<ZombieSoundType, FMOD.Studio.EventInstance> sounds)
        {
            if (sounds.ContainsKey(soundType))
            {
                return;
            }

            try
            {
                FMOD.Studio.EventInstance outputSound = FMODUnity.RuntimeManager.CreateInstance(soundPath);
                sounds.Add(soundType, outputSound);
            }
            catch (System.Exception e)
            {
                Debug.Log("Failed to Add Sound \"" + soundPath + "\":" + e.StackTrace);
            }
        }
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
        [SerializeField]
        private ZombieSoundData soundData;

        #endregion

        #region properties

        public ZombieType Type => this.type;

        public Target.ZombieTargetsData TargetsData
            => this.targetsData;

        public ZombieMovementData MovementData
            => this.movementData;

        public ZombieSoundData SoundData
            => this.soundData;

        #endregion
    }
}