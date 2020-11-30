using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Lights
{
    /// <summary>
    /// Flashes a beat.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class BeatFlash : MonoBehaviour
    {
        #region fields

        [SerializeField]
        private Utils.References.IntegerReference beatCounter;
        [SerializeField]
        private Utils.Events.GameEvent beatDownEvent;

        [SerializeField]
        private bool upBeat = true;

        private Animator _animator;

        #endregion

        #region methods

        private void Start()
        {
            this._animator = this.GetComponent<Animator>();
        }

        private void OnEnable()
        {
            if(this.beatDownEvent != null)
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
            if(this.beatCounter.Value % 2 == System.Convert.ToInt32(this.upBeat))
            {
                this._animator.Play("Flash");
            }
        }

        #endregion
    }
}

