using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CyberMonk.Menus
{
    /// <summary>
    /// The pause menu manager.
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class PauseMenuManager : MonoBehaviour
    {
        [SerializeField]
        private Utils.References.BooleanReference paused;
        [SerializeField]
        private bool closeOnContinue;

        /// <summary>
        /// Called when continue button was pressed.
        /// </summary>
        public void OnContinuePressed()
        {
            this.paused.Value = false;
            if(this.closeOnContinue)
            {
                Destroy(this.gameObject);
            }
        }

        /// <summary>
        /// Called when the exit menu was pressed.
        /// </summary>
        /// <param name="level">The level to load.</param>
        public void OnExitPressed(Game.Level.LevelLoadData levelLoadData)
        {
            this.paused.Value = false;
            Game.Level.LevelLoader.LoadLevel(levelLoadData);
        }
    }
}

