using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Moonkey.Events
{
    /// <summary>
    /// Handles when the monkey failed an attack.
    /// </summary>
    [CreateAssetMenu(fileName = "Moonkey Failed Attack Event", menuName = "Moonkey Failed Attack Event")]
    public class MoonkeyFailedAttackEvent : Utils.Events.GenericEvent<Zombie.ZombieComponent> { }
}

