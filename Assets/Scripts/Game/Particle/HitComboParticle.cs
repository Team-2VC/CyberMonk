using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CyberMonk.Game.Particle
{
    /// <summary>
    /// The hit combo particle.
    /// </summary>
    // TODO: Text is subject to change.
    [RequireComponent(typeof(Text), typeof(Animator))]
    public class HitComboParticle : MonoBehaviour
    {
        [SerializeField]
        private Utils.References.IntegerReference comboCounter;
        [SerializeField, Range(0, 1000)]
        private int counterOffset = 0;

        private Text _text;

        private void Start()
        {
            this._text = this.GetComponent<Text>();
            this._text.text = (this.comboCounter.Value + this.counterOffset) + "x Combo";
        }

        private void OnAnimationFinished()
        {
            Destroy(this.gameObject);
        }
    }
}

