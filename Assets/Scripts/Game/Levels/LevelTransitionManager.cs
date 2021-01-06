using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Level
{
    /// <summary>
    /// Holds data for the scene transition.
    /// </summary>
    [System.Serializable]
    public struct SceneTransition
    {
        [SerializeField]
        private string transitionName;
        [SerializeField]
        private GameObject transition;

        public string TransitionName
            => this.transitionName;

        public GameObject Transition
            => this.transition;
    }

    /// <summary>
    /// Used for handling scene transitions.
    /// </summary>
    [RequireComponent(typeof(Canvas), typeof(Animator))]
    public class LevelTransitionManager : MonoBehaviour
    {
        [SerializeField]
        private List<SceneTransition> transitions;

        [SerializeField]
        private Utils.Events.StringEvent beginTransitionEvent;
        [SerializeField]
        private Utils.Events.GameEvent midTransitionEvent;
        [SerializeField]
        private Utils.Events.GameEvent switchLevelEvent;

        private Animator _animator;
        private SceneTransition? _currentTransition = null;

        private static bool _generated = false;

        protected bool IsInTransition
            => this._currentTransition.HasValue;


        private void Start()
        {
            if (_generated)
            {
                Destroy(this.gameObject);
                return;
            }

            _generated = true;
            this._animator = this.GetComponent<Animator>();
            DontDestroyOnLoad(this.gameObject);
            foreach (SceneTransition transition in this.transitions)
            {
                transition.Transition?.SetActive(false);
            }
        }

        private void OnEnable()
        {
            this.switchLevelEvent += this.ContinueTransition;
            this.beginTransitionEvent += this.BeginTransition;
        }

        private void OnDisable()
        {
            this.switchLevelEvent -= this.ContinueTransition;
            this.beginTransitionEvent -= this.BeginTransition;
        }

        /// <summary>
        /// Used to begin the transition.
        /// </summary>
        /// <param name="transitionName">The transition name.</param>
        private void BeginTransition(string transitionName)
        {
            if (this.IsInTransition)
            {
                return;
            }

            foreach (SceneTransition transition in this.transitions)
            {
                if (transition.TransitionName == transitionName)
                {
                    transition.Transition?.SetActive(true);
                    this._currentTransition = transition;
                    this._animator.Play("InTransition");
                    return;
                }
            }

            // Only is called if the transition doesn't exist.
            this.midTransitionEvent?.Call();
        }

        /// <summary>
        /// Called when the beginning transition has ended.
        /// </summary>
        private void MidTransitionEvent()
        {
            this.midTransitionEvent?.Call();
        }

        /// <summary>
        /// Used to continue the transition.
        /// </summary>
        private void ContinueTransition()
        {
            try
            {
                if (!this.IsInTransition)
                {
                    return;
                }

                this._animator.Play("OutTransition");
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
                this.OnEndTransition();
            }
        }

        /// <summary>
        /// Used to end the transition.
        /// </summary>
        private void OnEndTransition()
        {
            if (this._currentTransition.HasValue)
            {
                this._currentTransition.Value.Transition?.SetActive(false);
            }

            this._currentTransition = null;
        }
    }
}

