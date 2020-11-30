using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CyberMonk.Game.Lights
{

    
    [RequireComponent(typeof(Animator))]
    public class LightFlash : MonoBehaviour
    {

        #region fields

        [SerializeField]
        private Utils.References.IntegerReference beatCounter;
        [SerializeField]
        private Utils.Events.GameEvent beatDownEvent;
        [SerializeField, Range(1, 100)]
        private float randomizationPercentage = 33;

        private Animator _animator;

        #endregion

        #region methods

        private void Start()
        {
            _animator = this.GetComponent<Animator>();
        }

        private void OnEnable()
        {
            if (this.beatDownEvent != null)
            {
                this.beatDownEvent += this.OnDownBeat;
            }
        }

        private void OnDisable()
        {
            if(this.beatDownEvent != null)
            {
                this.beatDownEvent -= this.OnDownBeat;
            }
        }

        private void OnDownBeat()
        {
            float randomized = Random.Range(1, 100);
            if(randomized <= this.randomizationPercentage)
            {
                this._animator.Play("Flash");
            }
        }

        #endregion
    }
}
