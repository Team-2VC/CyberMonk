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
        private GameObject waveDisplayPrefab;
        [SerializeField]
        private WaveManagerValues values;
        [SerializeField]
        private WaveManagerEvents events;
        [SerializeField]
        private WaveManagerReferences references;

        private float _delayBetweenWaves = 0f;
        private GameObject _currentDisplay = null;

        private int _currentZombiesAlive = 0;

        #endregion

        #region properties

        private bool IsInWaveCooldown
            => this._delayBetweenWaves > 0f;

        private float WaveDelayPercentage
            => (this.values.DelayBetweenWaves - this._delayBetweenWaves) / this.values.DelayBetweenWaves;

        public int ZombiesAlive
            => this._currentZombiesAlive;

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

                if(this.WaveDelayPercentage <= 0.5f && this.WaveDelayPercentage > 0.48f)
                {
                    this.OnDisplayWaveUI();
                    return;
                }

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
            this._currentZombiesAlive--;

            if(this.references.ZombiesSpawnedPerWave <= 0 && this._currentZombiesAlive <= 0)
            {
                this.OnWaveEnd();
            }
        }

        private void OnDisplayWaveUI()
        {
            if(this._currentDisplay != null)
            {
                return;
            }

            this.references.CurrentWave++;
            this._currentDisplay = Instantiate(this.waveDisplayPrefab);
        }

        /// <summary>
        /// Called when the wave should begin.
        /// </summary>
        private void OnBeginWave()
        {
            this.references.ZombiesSpawnedPerWave = this.CalculateNumberOfZombies();
            this._currentZombiesAlive = this.references.ZombiesSpawnedPerWave;

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
            return Mathf.RoundToInt(
                this.values.SpawnMultiplier * this.references.CurrentWave * this.references.CurrentWave + this.values.StartNumberOfZombies);
        }

        #endregion
    }
}

