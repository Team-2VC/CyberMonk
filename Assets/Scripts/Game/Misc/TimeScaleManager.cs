using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Misc
{
    /// <summary>
    /// Used to keep track of the time scale all in one place.
    /// </summary>
    public class TimeScaleManager : MonoBehaviour
    {
        [SerializeField]
        private Utils.Events.GameEvent exitGameEvent;
        [SerializeField]
        private Utils.Events.GameEvent retryGameEvent;
        [SerializeField]
        private Utils.Events.GameEvent gameOverEvent;
        [SerializeField]
        private Utils.References.BooleanReference paused;

        private static bool _generated = false;

        private void Start()
        {
            if (_generated)
            {
                Destroy(this.gameObject);
                return;
            }

            _generated = true;
            DontDestroyOnLoad(this.gameObject);
        }

        private void OnEnable()
        {
            this.exitGameEvent += this.OnExitGame;
            this.retryGameEvent += this.OnRetryGame;
            this.gameOverEvent += this.OnGameOver;
            this.paused.ChangedValueEvent += this.OnPaused;
        }

        private void OnDisable()
        {
            this.exitGameEvent -= this.OnExitGame;
            this.retryGameEvent -= this.OnRetryGame;
            this.gameOverEvent -= this.OnGameOver;
            this.paused.ChangedValueEvent -= this.OnPaused;
        }

        private void OnExitGame()
        {
            Time.timeScale = 1f;
        }

        private void OnPaused(bool paused)
        {
            Time.timeScale = paused ? 0f : 1f;
        }

        private void OnRetryGame()
        {
            Time.timeScale = 1f;
        }

        private void OnGameOver()
        {
            Time.timeScale = 1f;
        }

        /// <summary>
        /// Helper function that waits to update the time scale.
        /// </summary>
        /// <param name="delay">The time delay.</param>
        /// <param name="timeScale">The result time scale value.</param>
        private IEnumerator WaitToUpdateScale(float delay, float timeScale)
        {
            yield return new WaitForSeconds(delay);
            Time.timeScale = timeScale;
        }
    }
}

