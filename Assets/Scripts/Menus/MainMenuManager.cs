using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CyberMonk.Menus
{

    [System.Serializable]
    public struct BeatPulseAnimation
    {
        [SerializeField]
        private string animation;
        [SerializeField]
        private Utils.Events.GameEvent beatDownEvent;
        [SerializeField]
        private bool enabled;
        [SerializeField, Range(0, 20)]
        private int pulseTime;

        public string Animation
            => this.animation;

        public Utils.Events.GameEvent BeatDownEvent
        {
            get => this.beatDownEvent;
            set
            {
                if(this.beatDownEvent != null)
                {
                    this.beatDownEvent = value;
                }
            }
        }

        public bool Enabled
        {
            get => this.enabled;
            set => this.enabled = value;
        }

        public int PulseTime
            => this.pulseTime;
    }

    /// <summary>
    /// The Main Menu implementation.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class MainMenuManager : MonoBehaviour
    {
        #region fields

        [SerializeField]
        private Game.Level.LevelLoaderReference levelLoader;
        [SerializeField]
        private BeatPulseAnimation beatAnimation;

        private Animator _animator;
        private int _currentBeat = 0;

        #endregion

        #region properties

        #endregion

        #region methods

        /// <summary>
        /// Called when the main menu has started.
        /// </summary>
        private void Start()
        {
            this._animator = this.GetComponent<Animator>();
        }

        private void OnEnable()
        {
            this.beatAnimation.BeatDownEvent += this.OnBeatDown;
        }

        private void OnDisable()
        {
            this.beatAnimation.BeatDownEvent -= this.OnBeatDown;
        }

        private void OnBeatDown()
        {
            this._currentBeat++;

            if(!this.beatAnimation.Enabled)
            {
                return;
            }

            if(this.beatAnimation.PulseTime <= 1 
                || (this._currentBeat % this.beatAnimation.PulseTime) == 0)
            {
                this._animator.Play(this.beatAnimation.Animation);
            }
        }

        /// <summary>
        /// Called when the play button is selected.
        /// </summary>
        public void OnPlaySelected(Game.Level.LevelLoadData levelData)
        {
            this.levelLoader.Loader?.LoadLevel(levelData);
        }

        /// <summary>
        /// Called when the credits button was selected.
        /// </summary>
        public void OnCreditsSelected()
        {
            // TODO: Implementation
        }

        /// <summary>
        /// Called when the quit button was selected.
        /// </summary>
        public void OnQuitSelected()
        {
            Application.Quit();
        }

        #endregion
    }
}
