using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Tests{
public class ProtoMoonkeyMovement : MonoBehaviour
    {
        
        Animator animator;
        bool runOnBeat;
        
        
        // Start is called before the first frame update
        private void Start()
        {
            this.HookEvents(); 
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
                if(Input.GetKey(KeyCode.Space) && runOnBeat){
                    animator.SetBool("isRunning", true);
                } else {
                    animator.SetBool("isRunning", false);
                    runOnBeat = false;
                }
            
        }

        private void OnEnable()
        {
            this.HookEvents();
        }

        private void HookEvents()
        {
            if (Game.Conductor.Instance != null)
            {
                Game.Conductor.Instance.DownBeatEvent += OnDownBeat;
            }
        }

        
        private void OnDisable()
        {
            if(Game.Conductor.Instance != null)
            {
                Game.Conductor.Instance.DownBeatEvent -= OnDownBeat;
            }
        }

        private void OnDownBeat()
        {
            runOnBeat = true;
            Debug.Log("beat");
            
        }
    }
}
