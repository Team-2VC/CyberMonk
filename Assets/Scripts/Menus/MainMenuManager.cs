using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CyberMonk.Menus
{
    public struct TransitionData
    {
        public object data;
        public System.Action<object> callable;
    }

    [System.Serializable]
    public struct AnimationTransitions
    {
        [SerializeField]
        private string playGameTransition;

        public string PlayGameTransition
            => this.playGameTransition;
    }

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
        private AnimationTransitions animationTransitions;
        [SerializeField]
        private BeatPulseAnimation beatAnimation;

        private Animator _animator;
        private int _currentBeat = 0;

        private TransitionData? _currentTransition = null;

        #endregion

        #region properties

        protected bool IsInTransition
            => this._currentTransition.HasValue;

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

            if(!this.beatAnimation.Enabled || this.IsInTransition)
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
            if(this.IsInTransition)
            {
                return;
            }

            this._animator.Play(this.animationTransitions.PlayGameTransition);
            this.SetTransitionData((object a) =>
                {
                    if(a is Game.Level.LevelLoadData)
                    {
                        // TODO: Load instructions scene??
                        Game.Level.LevelLoader.LoadLevel((Game.Level.LevelLoadData)a);
                    }
                }, levelData);
        }

        /// <summary>
        /// Called when the credits button was selected.
        /// </summary>
        public void OnCreditsSelected()
        {
            if (this.IsInTransition)
            {
                return;
            }

            // TODO: Implementation
        }

        /// <summary>
        /// Called when the quit button was selected.
        /// </summary>
        public void OnQuitSelected()
        {
            if(this.IsInTransition)
            {
                return;
            }

            Application.Quit();
        }


        /// <summary>
        /// Sets the transition data.
        /// </summary>
        /// <param name="callable">The callable.</param>
        /// <param name="data">The data used by the transition.</param>
        private void SetTransitionData(System.Action<object> callable, object data = null)
        {
            TransitionData dat = new TransitionData();
            dat.data = data;
            dat.callable = callable;
            this._currentTransition = dat;
        }

        /// <summary>
        /// Called when the animation transition has finished.
        /// </summary>
        private void OnTransitionFinished()
        {
            if(!this.IsInTransition)
            {
                return;
            }

            TransitionData data = this._currentTransition.Value;
            data.callable(data.data);
            this._currentTransition = null;
        }

        #endregion
    }
}
