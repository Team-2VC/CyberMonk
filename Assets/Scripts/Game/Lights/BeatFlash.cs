using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CyberMonk.Game.Lights
{
    public class BeatFlash : MonoBehaviour
    {

        [SerializeField]
        private Utils.References.IntegerReference beatCounter;

        [SerializeField]
        private Utils.Events.GameEvent beatDownEvent;

        [SerializeField]
        private bool upBeat = true;

        private Animator animator;

        private void Start()
        {
            animator = this.GetComponent<Animator>();
        }

        private void OnEnable()
        {
            HookEvent();
        }

        private void OnDisable()
        {
            UnhookEvent();
        }

        private void HookEvent()
        {
            this.beatDownEvent += this.OnDownBeat;
        }

        private void UnhookEvent()
        {
            this.beatDownEvent -= this.OnDownBeat;
        }

        private void OnDownBeat()
        {
            int x = upBeat ? 1 : 0;
            if(beatCounter.Value % 2 == x)
                this.animator.Play("Flash");
        }


    }
}

