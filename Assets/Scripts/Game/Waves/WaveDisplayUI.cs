using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CyberMonk.Game.Waves
{
    [System.Serializable]
    public struct WaveDisplayAnimations
    {
        [SerializeField]
        private string idleAnimation;
        [SerializeField, Range(1, 10)]
        private int idleAnimationLoops;
        [SerializeField]
        private string exitAnimation;

        public string IdleAnimation
            => this.idleAnimation;

        public int IdleAnimationLoops
            => this.idleAnimationLoops;

        public string ExitAnimation
            => this.exitAnimation;


    }

    /// <summary>
    /// The wave display UI.
    /// </summary>
    [RequireComponent(typeof(Canvas), typeof(Animator))]
    public class WaveDisplayUI : MonoBehaviour
    {
        #region fields
        
        [SerializeField]
        private WaveDisplayAnimations animationSettings;
        [SerializeField]
        private Utils.References.IntegerReference currentWave;
        [SerializeField]
        private Text uiText;

        private Animator _animator;
        private int _idleAnimationLoops = 0;

        #endregion

        #region methods

        private void Start()
        {
            this.uiText.text = "Wave " + currentWave.Value;
            this._animator = this.GetComponent<Animator>();
        }

        private void OnEnterCompleted()
        {
            this._animator.Play(this.animationSettings.IdleAnimation);
        }

        private void OnIdleCompleted()
        {
            this._idleAnimationLoops++;

            if(this._idleAnimationLoops < this.animationSettings.IdleAnimationLoops)
            {
                this._animator.Play(this.animationSettings.IdleAnimation);
                return;
            }
            this._animator.Play(this.animationSettings.ExitAnimation);
        }

        private void OnExitCompleted()
        {
            Destroy(this.gameObject);
        }

        #endregion
    }
}

