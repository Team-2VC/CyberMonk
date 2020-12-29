using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Level
{

    [System.Serializable]
    public struct LevelTransitionData
    {
        [SerializeField]
        private bool displayTransition;
        [SerializeField]
        private string transitionName;

        public bool DisplayTransition
            => this.displayTransition;

        public string TransitionName
            => this.transitionName;
    }

    /// <summary>
    /// The level load data.
    /// </summary>
    [CreateAssetMenu(fileName = "Level Load Data", menuName = "Levels/Level Load Data")]
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

        [SerializeField]
        private LevelTransitionData transitionData;

        public string LevelToLoad
            => this.levelToLoad;

        public bool ShowLoadingSceen
            => this.showLoadingScreen;

        public LevelTransitionData TransitionData
            => this.transitionData;

        public float RandomLoadTime
            => Random.Range(this.minLoadTime, this.maxLoadTime);
    }
}


