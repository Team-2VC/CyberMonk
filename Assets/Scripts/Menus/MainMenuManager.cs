using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CyberMonk.Menus
{
    /// <summary>
    /// The Main Menu implementation.
    /// </summary>
    public class MainMenuManager : MonoBehaviour
    {
        /// <summary>
        /// Called when the play button is selected.
        /// </summary>
        public void OnPlaySelected(string level)
        {
            SceneManager.LoadScene(level);
        }

        /// <summary>
        /// Called when the quit button was selected.
        /// </summary>
        public void OnQuitSelected()
        {
            Application.Quit();
        }
    }
}
