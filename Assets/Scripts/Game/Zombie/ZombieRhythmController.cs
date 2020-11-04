using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Zombie
{

    public abstract class AZombieRhythmController
    {
        protected readonly AZombieController _controller;

        public AZombieRhythmController(AZombieController controller){
            this._controller = controller;
        }

        public virtual void HookEvents(){
            if (Game.Conductor.Instance != null)
            {
                Game.Conductor.Instance.DownBeatEvent += OnDownBeat;
            }
        }

        public void UnhookEvents()
        {
            if(Game.Conductor.Instance != null)
            {
                Game.Conductor.Instance.DownBeatEvent -= OnDownBeat;
            }
        }


        protected abstract void OnDownBeat();

        

    }

    public class MeleeZombieRhythmController: AZombieRhythmController
    {
        //TODO: Add parameters
        public MeleeZombieRhythmController(): base(null)
        {
            

        }

        //TODO: Do this thing
        protected override void OnDownBeat()
        {
            // throw new System.NotImplementedException();
        }
    }
}


