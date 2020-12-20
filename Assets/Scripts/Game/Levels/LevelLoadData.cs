using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Level
{
    /// <summary>
    /// The level load data.
    /// </summary>
    [CreateAssetMenu(fileName = "Level Load Data", menuName = "Level Load Data")]
    public class LevelLoadData : ScriptableObject
    {
        [SerializeField]
        private string levelToLoad;
        [SerializeField]
        private bool showLoadingScreen;

        [SerializeField]
        private float minLoadTime;
        [SerializeField]
        private float maxLoadTime;

        public string LevelToLoad
            => this.levelToLoad;

        public bool ShowLoadingSceen
            => this.showLoadingScreen;

        public float RandomLoadTime
            => Random.Range(this.minLoadTime, this.maxLoadTime);
    }
}


