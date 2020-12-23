using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CyberMonk.UI
{
    [System.Serializable]
    public struct PlayerUIProperties
    {
        [System.Serializable]
        public struct ComboMultiplier
        {

            [SerializeField]
            private Text comboText;
            [SerializeField]
            private GameObject gameObject;

            public Text ComboText
                => this.comboText;

            public GameObject GameObject
                => this.gameObject;
        }

        [SerializeField]
        private ComboMultiplier comboCounter;
        [SerializeField]
        private Slider healthBar;
        [SerializeField]
        private Text scoreText;

        public ComboMultiplier CombosMultiplier
            => this.comboCounter;

        public Slider HealthBar
            => this.healthBar;

        public Text ScoreText
            => this.scoreText;
    }

    [System.Serializable]
    public struct PlayerUIReferences
    {
        [SerializeField]
        private Utils.References.IntegerReference totalScore;
        [SerializeField]
        private Utils.References.IntegerReference comboMultiplier;
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

        public Utils.References.IntegerReference ComboMultiplier
        {
            get => this.comboMultiplier;
            set
            {
                if (this.comboMultiplier != null)
                {
                    this.comboMultiplier = value;
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
        private PlayerUIProperties properties;
        [SerializeField]
        private Game.Moonkey.MoonkeyComponent moonkey;

        private Animator _animator;

        /// <summary>
        /// Sets the health text to the health.
        /// </summary>
        private void Start()
        {
            this.HookHealthChangedEvent();

            this._animator = this.GetComponent<Animator>();

            PlayerUIProperties.ComboMultiplier counter = this.properties.CombosMultiplier;
            counter.ComboText.text = "0x";
            counter.GameObject.SetActive(false);

            this.properties.ScoreText.text = "Score: " + (int)this.references.TotalScore.Value;
            this.properties.HealthBar.value = 0f;
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
            this.references.ComboMultiplier.ChangedValueEvent += this.OnComboMultiplierChanged;
            this.references.BeatDownEvent += this.OnBeatDown;
            this.references.TotalScore.ChangedValueEvent += this.OnScoreChanged;
        }

        private void OnDisable()
        {
            if (this.moonkey.Controller != null)
            {
                this.moonkey.Controller.HealthChangedEvent -= this.OnHealthChanged;
            }

            this.references.ComboMultiplier.ChangedValueEvent -= this.OnComboMultiplierChanged;
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
            this.properties.HealthBar.value = healthBarPercentage;
        }

        private void OnComboMultiplierChanged(int combo)
        {
            PlayerUIProperties.ComboMultiplier counter = this.properties.CombosMultiplier;
            counter.GameObject.SetActive(combo > 3);
            this.properties.CombosMultiplier.ComboText.text = combo + "x";
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
            this.properties.ScoreText.text = "Score: " + scoreChanged;
        }
    }
}
