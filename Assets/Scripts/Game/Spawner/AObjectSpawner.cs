using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Spawner
{

    /// <summary>
    /// Interface for spawner information.
    /// </summary>
    public interface ISpawnerInfo
    {
        float MinSeconds
        {
            get;
        }

        float MaxSeconds
        {
            get;
        }

        ObjectSpawnerData SpawnerData
        {
            get;
        }
    }

    /// <summary>
    /// The Abstract object spawner.
    /// </summary>
    public abstract class AObjectSpawner : MonoBehaviour
    {

        #region fields

        [SerializeField]
        [Tooltip("Represents the spawner cooldown until we want a spawner to start working after game starts")]
        private float delayAfterGameStarted;
        [SerializeField]
        private Utils.References.BooleanReference paused;

        private float _delayAfterGameStarted = 0f;

        #endregion

        #region properties

        /// <summary>
        /// The Spawner Information.
        /// </summary>
        protected abstract ISpawnerInfo Info
        {
            get;
        }

        /// <summary>
        /// The Time Between Spawn Seconds.
        /// </summary>
        protected virtual float BetweenSpawnTimeSeconds
        {
            get
            {
                return Random.Range(this.Info.MinSeconds, this.Info.MaxSeconds);
            }
        }

        protected virtual bool CanSpawn
        {
            get
            {
                return this._delayAfterGameStarted <= 0f && !this.paused.Value;
            }
        }

        #endregion

        #region methods

        private void Start()
        {
            this._delayAfterGameStarted = this.delayAfterGameStarted;
        }

        protected virtual void HookEvents() { }

        protected virtual void UnhookEvents() { }

        /// <summary>
        /// Called when the spawner is enabled.
        /// </summary>
        private void OnEnable()
        {
            this.HookEvents();
        }

        /// <summary>
        /// Called when the spawner is disabled.
        /// </summary>
        private void OnDisable()
        {
            this.UnhookEvents();
        }

        /// <summary>
        /// Called when each frame has been updated and the game isn't paused.
        /// </summary>
        protected virtual void OnUpdate() { }

        /// <summary>
        /// Update is called once per frame.
        /// </summary>
        private void Update()
        {
            if (!this.paused.Value)
            {
                this.OnUpdate();

                // Updates the delay after the game started.
                if (this._delayAfterGameStarted > 0)
                {
                    this._delayAfterGameStarted -= Time.deltaTime;

                    if (this._delayAfterGameStarted <= 0f)
                    {
                        this._delayAfterGameStarted = 0f;
                    }
                }
            }

            if (!this.CanSpawn)
            {
                return;
            }

            System.TimeSpan? timeSinceLastSpawned = this.Info.SpawnerData?.TimeSinceLastSpawned;

            if (timeSinceLastSpawned.HasValue)
            {
                float differenceSeconds = (float)(timeSinceLastSpawned.Value.TotalMilliseconds / 1000);
                if (differenceSeconds < this.BetweenSpawnTimeSeconds)
                {
                    return;
                }
            }

            this.TrySpawnObject();
        }

        /// <summary>
        /// Called to try & spawn an object.
        /// </summary>
        /// <returns>True if the object has been spawned, false otherwise.</returns>
        protected virtual bool TrySpawnObject()
        {
            // Spawns the object.
            GameObject spawnedObject = this.Info.SpawnerData?.SpawnRandom(this.GetSpawnPosition());

            if (spawnedObject != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// The Random Position of the spawner.
        /// </summary>
        /// <returns>A Vector2 position.</returns>
        protected virtual Vector2 GetSpawnPosition()
        {
            return this.transform.position;
        }

        #endregion
    }
}

