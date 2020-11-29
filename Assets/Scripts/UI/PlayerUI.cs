using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CyberMonk.UI
{

    /// <summary>
    /// The Player UI.
    /// </summary>
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField]
        private Game.Moonkey.MoonkeyComponent moonkey;
        [SerializeField]
        private Text healthText;

        /// <summary>
        /// Sets the health text to the health.
        /// </summary>
        private void Start()
        {
            if (this.moonkey.Controller != null)
            {
                this.moonkey.Controller.HealthChangedEvent += this.OnHealthChanged;
            }

            this.healthText.text = "Health: " + (int)this.moonkey.Controller.Health;
        }

        private void OnEnable()
        {
            if(this.moonkey.Controller != null)
            {
                this.moonkey.Controller.HealthChangedEvent += this.OnHealthChanged;
            }
        }

        private void OnDisable()
        {
            if (this.moonkey.Controller != null)
            {
                this.moonkey.Controller.HealthChangedEvent -= this.OnHealthChanged;
            }
        }

        /// <summary>
        /// Called when the health has been changed.
        /// </summary>
        /// <param name="healthChanged">The amount of health that has been changed.</param>
        private void OnHealthChanged(float healthChanged)
        {
            this.healthText.text = "Health: " + (int)this.moonkey.Controller.Health;
        }
    }
}
