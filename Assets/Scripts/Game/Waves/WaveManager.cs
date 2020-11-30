using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Waves
{

    [System.Serializable]
    public struct WaveManagerValues
    {
        [SerializeField, Range(0, 100)]
        [Tooltip("Represents the number of zombies to spawn for the first wave.")]
        private int startNumberZombies;
        [SerializeField, Range(0.1f, 90f)]
        private float delayBetweenWaves;
        [SerializeField, Range(1, 100)]
        private int spawnMultiplier;

        public int StartNumberOfZombies
        {
            get => this.startNumberZombies;
        }

        public float DelayBetweenWaves
        {
            get => this.delayBetweenWaves;
        }

        public int SpawnMultiplier
            => this.spawnMultiplier;
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
        [SerializeField]
        private Utils.References.BooleanReference paused;

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

        public bool Paused
        {
            get => this.paused.Value;
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
            if(this.references.Paused)
            {
                return;
            }

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

            this.references.ZombiesSpawnedPerWave = this.CalculateNumberOfZombies();
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

        private int CalculateNumberOfZombies()
        {
            // Logs the number of zombies per each round.
            /* return Mathf.RoundToInt(
                this.logMultiplier * Mathf.Log((int)this.references.CurrentWave) + this.values.StartNumberOfZombies); */
            return Mathf.RoundToInt(
                this.values.SpawnMultiplier * this.references.CurrentWave * this.references.CurrentWave + this.values.StartNumberOfZombies);
        }

        #endregion
    }
}

