using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CyberMonk.UI
{

    [System.Serializable]
    public struct PlayerUIReferences
    {
        [SerializeField]
        private Utils.References.IntegerReference totalScore;
        [SerializeField]
        private Utils.Events.GameEvent beatDownEvent;

        public Utils.References.IntegerReference TotalScore
        {
            get => this.totalScore;
            set
            {
                if(this.totalScore != null)
                {
                    this.totalScore = value;
                }
            }
        }

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
    }

    /// <summary>
    /// The Player UI.
    /// </summary>
    [RequireComponent(typeof(Animator), typeof(Canvas))]
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField]
        private PlayerUIReferences references;
        [SerializeField]
        private Game.Moonkey.MoonkeyComponent moonkey;

        [SerializeField]
        private Slider healthBar;
        [SerializeField]
        private Text scoreText;

        private Animator _animator;

        /// <summary>
        /// Sets the health text to the health.
        /// </summary>
        private void Start()
        {
            this.HookHealthChangedEvent();

            this._animator = this.GetComponent<Animator>();
            this.scoreText.text = "Score: " + (int)this.references.TotalScore.Value;
            this.healthBar.value = 0f;
        }

        private void HookHealthChangedEvent()
        {
            if (this.moonkey.Controller != null)
            {
                this.moonkey.Controller.HealthChangedEvent += this.OnHealthChanged;
            }
        }
        
        private void OnEnable()
        {
            this.HookHealthChangedEvent();
            this.references.BeatDownEvent += this.OnBeatDown;
            this.references.TotalScore.ChangedValueEvent += this.OnScoreChanged;
        }

        private void OnDisable()
        {
            if (this.moonkey.Controller != null)
            {
                this.moonkey.Controller.HealthChangedEvent -= this.OnHealthChanged;
            }

            this.references.BeatDownEvent -= this.OnBeatDown;
            this.references.TotalScore.ChangedValueEvent -= this.OnScoreChanged;
        }

        /// <summary>
        /// Called when the health has been changed.
        /// </summary>
        /// <param name="healthChanged">The amount of health that has been changed.</param>
        private void OnHealthChanged(float healthChanged)
        {
            float maxHealth = this.moonkey.Controller.MaxHealth;
            float healthBarPercentage = (maxHealth - this.moonkey.Controller.Health) / maxHealth;
            Debug.Log(healthBarPercentage);
            this.healthBar.value = healthBarPercentage;
        }

        /// <summary>
        /// Called when the beat is down.
        /// </summary>
        private void OnBeatDown()
        {
            this._animator.Play("BeatPulse");
        }

        /// <summary>
        /// Called when the score has changed.
        /// </summary>
        /// <param name="scoreChanged">The amount changed.</param>
        private void OnScoreChanged(int scoreChanged)
        {
            this.scoreText.text = "Score: " + scoreChanged;
        }
    }
}
