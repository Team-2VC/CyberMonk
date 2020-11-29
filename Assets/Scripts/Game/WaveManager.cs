using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game
{

    [System.Serializable]
    public struct WaveManagerValues
    {
        [SerializeField, Range(0, 100)]
        [Tooltip("Represents the number of zombies to spawn for the first wave.")]
        private int startNumberZombies;
        [SerializeField, Range(0.1f, 90f)]
        private float delayBetweenWaves;

        public int StartNumberOfZombies
        {
            get => this.startNumberZombies;
        }

        public float DelayBetweenWaves
        {
            get => this.delayBetweenWaves;
        }
    }

    [System.Serializable]
    public struct WaveManagerEvents
    {
        [SerializeField]
        private Utils.Events.GameEvent waveStartEvent;
        [SerializeField]
        private Utils.Events.GameEvent waveEndEvent;
        [SerializeField]
        private Utils.Events.GameEvent zombieDeathEvent;

        public Utils.Events.GameEvent WaveStartEvent
            => this.waveStartEvent;

        public Utils.Events.GameEvent WaveEndEvent
            => this.waveEndEvent;

        public Utils.Events.GameEvent ZombieDeathEvent
        {
            get => this.zombieDeathEvent;
            set
            {
                if(this.zombieDeathEvent != null)
                {
                    this.zombieDeathEvent = value;
                }
            }
        }
    }

    [System.Serializable]
    public struct WaveManagerReferences
    {
        [SerializeField]
        private Utils.References.IntegerReference zombiesSpawnedPerWave;
        [SerializeField]
        private Utils.References.IntegerReference currentWave;

        public int ZombiesSpawnedPerWave
        {
            get => this.zombiesSpawnedPerWave.Value;
            set => this.zombiesSpawnedPerWave.Value = value;
        }

        public int CurrentWave
        {
            get => this.currentWave.Value;
            set => this.currentWave.Value = value;
        }
    }

    /// <summary>
    /// Handles each particular wave.
    /// </summary>
    public class WaveManager : MonoBehaviour
    {
        #region fields

        [SerializeField]
        private WaveManagerValues values;
        [SerializeField]
        private WaveManagerEvents events;
        [SerializeField]
        private WaveManagerReferences references;

        private float _delayBetweenWaves = 0f;

        #endregion

        #region properties

        private bool IsInWaveCooldown
            => this._delayBetweenWaves > 0f;

        #endregion

        #region methods

        /// <summary>
        /// Called when the wave manager has started.
        /// </summary>
        private void Start()
        {
            this._delayBetweenWaves = this.values.DelayBetweenWaves;
        }

        /// <summary>
        /// Updates the wave manager.
        /// </summary>
        private void Update()
        {
            if(this.IsInWaveCooldown)
            {
                this._delayBetweenWaves -= Time.deltaTime;

                if(this._delayBetweenWaves <= 0f)
                {
                    this._delayBetweenWaves = 0f;
                    this.OnBeginWave();
                }
                return;
            }
        }

        /// <summary>
        /// Called when the wave manager was enabled.
        /// </summary>
        private void OnEnable()
        {
            this.events.ZombieDeathEvent += this.OnZombieDeath;
        }

        /// <summary>
        /// Called when the wave manager was disabled.
        /// </summary>
        private void OnDisable()
        {
            this.events.ZombieDeathEvent -= this.OnZombieDeath;
        }

        /// <summary>
        /// Called when the zombie was killed.
        /// </summary>
        private void OnZombieDeath()
        {
            if(this.references.ZombiesSpawnedPerWave <= 0)
            {
                this.OnWaveEnd();
                return;
            }
        }

        /// <summary>
        /// Called when the wave should begin.
        /// </summary>
        private void OnBeginWave()
        {
            this.references.CurrentWave++;
            // TODO: Calculate number of zombie spawns.
            this.references.ZombiesSpawnedPerWave = this.values.StartNumberOfZombies;

            this.events.WaveStartEvent?.Call();
        }

        /// <summary>
        /// Called when the wave has ended.
        /// </summary>
        private void OnWaveEnd()
        {
            this._delayBetweenWaves = this.values.DelayBetweenWaves;
            this.events.WaveEndEvent?.Call();
        }

        #endregion
    }
}

