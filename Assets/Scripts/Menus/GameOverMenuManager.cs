using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CyberMonk.Menus
{
    /// <summary>
    /// The Game Over menu manager.
    /// </summary>
    public class GameOverMenuManager : MonoBehaviour
    {
        [SerializeField]
        private Game.Level.LevelLoaderReference levelLoader;
        [SerializeField]
        private Utils.Events.GameEvent exitGameEvent;
        [SerializeField]
        private Utils.Events.GameEvent retryGameEvent;

        /// <summary>
        /// Called when the retry button was clicked.
        /// </summary>
        /// <param name="levelData">The level data.</param>
        public void OnRetryClicked(Game.Level.LevelLoadData levelData)
        {
            this.levelLoader.Loader?.LoadLevel(levelData);
        }

        /// <summary>
        /// Called when the exit button was clicked.
        /// </summary>
        /// <param name="levelData">The level data.</param>
        public void OnExitClicked(Game.Level.LevelLoadData levelData)
        {
            this.exitGameEvent?.Call();
            this.levelLoader.Loader?.LoadLevel(levelData);
        }
    }
}

