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
        public void OnRetryClicked()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Level");
        }

        /// <summary>
        /// Called when the exit button was clicked.
        /// </summary>
        /// <param name="mainMenu">The main menu scene.</param>
        public void OnExitClicked()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
    }
}

