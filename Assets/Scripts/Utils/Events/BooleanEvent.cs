using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Utils.Events
{
    /// <summary>
    /// A Game event that utilizes a boolean input.
    /// </summary>
    [CreateAssetMenu(fileName = "Game Event (Boolean)", menuName = "Game Event (Boolean)")]
    public class BooleanEvent : GenericEvent<bool> { }
}


