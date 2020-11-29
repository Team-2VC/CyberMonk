using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Spawner
{

    [System.Serializable]
    public class WaveZombieSpawnerInfo : ISpawnerInfo
    {
        [SerializeField]
        private Utils.Events.GameEvent waveStartEvent;
        [SerializeField]
        private Utils.Events.GameEvent waveEndEvent;
        [SerializeField]
        private Utils.References.IntegerReference zombiesToSpawn;
        [SerializeField]
        private float minSpawnSeconds;
        [SerializeField]
        private float spawnSecondsDifference;
        [SerializeField]
        private ObjectSpawnerData spawnerData;

        public float MinSeconds 
            => this.minSpawnSeconds;

        public float MaxSeconds 
            => this.minSpawnSeconds + this.spawnSecondsDifference;

        public ObjectSpawnerData SpawnerData 
            => spawnerData;

        public int ZombiesToSpawn
        {
            get => this.zombiesToSpawn.Value;
            set => this.zombiesToSpawn.Value = value;
        }

        public Utils.Events.GameEvent WaveStartEvent
        { 
            get => this.waveStartEvent;
            set
            {
                if(this.waveStartEvent != null)
                {
                    this.waveStartEvent = value;
                }
            }
        }

        public Utils.Events.GameEvent WaveEndEvent
        {
            get => this.waveEndEvent;
            set
            {
                if(this.waveEndEvent != null)
                {
                    this.waveEndEvent = value;
                }
            }
        }
    }

    /// <summary>
    /// The zombie spawner component.
    /// </summary>
    public class WaveZombieSpawner : AObjectSpawner
    {

        [SerializeField]
        private WaveZombieSpawnerInfo spawnerInfo;
        private bool _currentlyInWave = false;

        protected override ISpawnerInfo Info 
            => this.spawnerInfo;

        protected override void HookEvents()
        {
            this.spawnerInfo.WaveStartEvent += this.OnWaveStart;
            this.spawnerInfo.WaveEndEvent += this.OnWaveEnd;
        }

        protected override void UnhookEvents()
        {
            this.spawnerInfo.WaveStartEvent -= this.OnWaveStart;
            this.spawnerInfo.WaveEndEvent -= this.OnWaveEnd;
        }

        /// <summary>
        /// Called when the wave has started.
        /// </summary>
        private void OnWaveStart()
        {
            this._currentlyInWave = true;
        }

        /// <summary>
        /// Called when the wave has ended.
        /// </summary>
        private void OnWaveEnd()
        {
            this._currentlyInWave = false;
        }

        protected override bool TrySpawnObject()
        {
            if(!this._currentlyInWave || this.spawnerInfo.ZombiesToSpawn <= 0)
            {
                return false;
            }

            bool output = base.TrySpawnObject();
            
            if(output)
            {
                this.spawnerInfo.ZombiesToSpawn--;

                if(this.spawnerInfo.ZombiesToSpawn <= 0)
                {
                    this.spawnerInfo.ZombiesToSpawn = 0;
                }
            }

            return output;
        }
    }
}

