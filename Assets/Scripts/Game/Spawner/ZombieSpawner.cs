using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Spawner
{

    [System.Serializable]
    public class ZombieSpawnerInfo : ISpawnerInfo
    {
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
            => this.zombiesToSpawn.Value;
    }

    /// <summary>
    /// The zombie spawner component.
    /// </summary>
    public class ZombieSpawner : AObjectSpawner
    {

        [SerializeField]
        private ZombieSpawnerInfo spawnerInfo;

        private int _zombiesLeftForWave = -1;

        protected override ISpawnerInfo Info 
            => this.spawnerInfo;

        protected override bool TrySpawnObject()
        {
            if(this._zombiesLeftForWave < 0)
            {
                this._zombiesLeftForWave = this.spawnerInfo.ZombiesToSpawn;
            }

            if(this._zombiesLeftForWave <= 0)
            {
                return false;
            }

            bool output = base.TrySpawnObject();
            
            if(output)
            {
                this._zombiesLeftForWave--;

                if(this._zombiesLeftForWave <= 0)
                {
                    this._zombiesLeftForWave = 0;
                }
            }

            return output;
        }
    }
}

