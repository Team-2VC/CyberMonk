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
    }

    /// <summary>
    /// The Player UI.
    /// </summary>
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField]
        private PlayerUIReferences references;
        [SerializeField]
        private Game.Moonkey.MoonkeyComponent moonkey;
        
        [SerializeField]
        private Text healthText;
        [SerializeField]
        private Text scoreText;

        /// <summary>
        /// Sets the health text to the health.
        /// </summary>
        private void Start()
        {
            this.HookHealthChangedEvent();

            this.healthText.text = "Health: " + (int)this.moonkey.Controller.Health;
            this.scoreText.text = "Score: " + (int)this.references.TotalScore.Value;
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
            this.references.TotalScore.ChangedValueEvent += this.OnScoreChanged;
        }

        private void OnDisable()
        {
            if (this.moonkey.Controller != null)
            {
                this.moonkey.Controller.HealthChangedEvent -= this.OnHealthChanged;
            }

            this.references.TotalScore.ChangedValueEvent -= this.OnScoreChanged;
        }

        /// <summary>
        /// Called when the health has been changed.
        /// </summary>
        /// <param name="healthChanged">The amount of health that has been changed.</param>
        private void OnHealthChanged(float healthChanged)
        {
            this.healthText.text = "Health: " + (int)this.moonkey.Controller.Health;
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
