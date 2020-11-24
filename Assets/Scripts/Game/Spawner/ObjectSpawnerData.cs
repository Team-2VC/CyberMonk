using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Spawner
{
    /// <summary>
    /// The Object Spawner Data.
    /// </summary>
    [CreateAssetMenu(fileName = "Spawner Data", menuName = "Spawner Data")]
    public class ObjectSpawnerData : ScriptableObject
    {

        #region fields

        /// <summary>
        /// The Object Prefabs.
        /// </summary>
        [SerializeField]
        private List<GameObject> @objectPrefabs;

        // Determines whether the object has spawned before.
        private bool _hasSpawnedBefore = false;

        // The time an object was last spawned.
        private System.DateTime _timeLastSpawned = System.DateTime.Now;

        #endregion

        #region properties

        /// <summary>
        /// Determines the time since an object was last
        /// spawned.
        /// </summary>
        public System.TimeSpan? TimeSinceLastSpawned
        {
            get
            {
                return this._hasSpawnedBefore ?
                    System.DateTime.Now.Subtract(this._timeLastSpawned) : new System.TimeSpan?();
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Spawns a random game object based on the position and rotation.
        /// </summary>
        /// <param name="position">The position we are spawning the object at.</param>
        /// <param name="rotation">The rotation we are spawning the object at.</param>
        /// <returns>The game object that is spawned.</returns>
        public GameObject SpawnRandom(Vector2 position, Quaternion rotation)
        {
            int randomIndex = Random.Range(0, this.objectPrefabs.Count);

            GameObject prefab = this.objectPrefabs[randomIndex];

            if (prefab != null)
            {
                this._timeLastSpawned = System.DateTime.Now;
                this._hasSpawnedBefore = true;
                return Instantiate<GameObject>(prefab, position, rotation);
            }

            return null;
        }

        /// <summary>
        /// Spawns a random game object at the position with its identity rotation.
        /// </summary>
        /// <param name="position">The position we are spawning the object.</param>
        /// <returns>The game object that is spawned.</returns>
        public GameObject SpawnRandom(Vector2 position)
        {
            return this.SpawnRandom(position, Quaternion.identity);
        }

        /// <summary>
        /// Spawns a random game object at (0, 0) with its identity rotation.
        /// </summary>
        /// <returns>The game object that is spawned.</returns>
        public GameObject SpawnRandom()
        {
            return this.SpawnRandom(Vector2.zero);
        }

        #endregion
    }
}


