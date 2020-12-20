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
        /// <summary>
        /// Called when the retry button was clicked.
        /// </summary>
        /// <param name="levelData">The level data.</param>
        public void OnRetryClicked(Game.Level.LevelLoadData levelData)
        {
            Time.timeScale = 1f;
            Game.Level.LevelLoader.LoadLevel(levelData);
        }

        /// <summary>
        /// Called when the exit button was clicked.
        /// </summary>
        /// <param name="levelData">The level data.</param>
        public void OnExitClicked(Game.Level.LevelLoadData levelData)
        {
            Time.timeScale = 1f;
            Game.Level.LevelLoader.LoadLevel(levelData);
        }
    }
}

