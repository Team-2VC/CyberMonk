using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CyberMonk.Game.Zombie
{
    public class ZombieComponent : MonoBehaviour
    {

        private AZombieController _controller;

        [SerializeField]
        private ZombieSettings settings;

        [SerializeField]
        private Utils.References.IntegerReference beatCounter;

        public int BeatCounter{
            get{
                if(beatCounter != null){
                    return beatCounter.Value;
                } else {
                    return 0;
                }
            }
        }

        private void Awake() 
        {
            //TODO: Create "Create" Function
            this._controller = AZombieController.Create(this, settings);
                
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
