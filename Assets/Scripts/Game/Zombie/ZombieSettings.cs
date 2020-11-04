using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CyberMonk.Game.Zombie
{

    [CreateAssetMenu(fileName = "Zombie Settings", menuName = "Zombie Settings")]
    public class ZombieSettings : ScriptableObject
    {
        [SerializeField]
        private ZombieType type;

        public ZombieType Type => this.type;
    }
}