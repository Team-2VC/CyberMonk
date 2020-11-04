using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CyberMonk.Game.Zombie
{

    public enum ZombieType {
        MELEE, 
        RANGE,
        TANK
    }


    public abstract class AZombieController
    {

        private readonly ZombieComponent _component;
        private readonly ZombieType _type;

        public abstract AZombieRhythmController RhythmController {
            get;
        }

        public abstract AZombieTargetController TargetController {
            get;
        }

        public ZombieComponent Component => this._component;     
        public ZombieType Type => this._type;   
        
        public AZombieController(ZombieComponent component, ZombieType type){
            this._component = component;
            this._type = type;
        }


        public static AZombieController Create(ZombieComponent component, ZombieSettings settings){
            return null;
        }

        


    }
}
