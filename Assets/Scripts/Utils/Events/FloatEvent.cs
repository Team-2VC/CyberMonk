using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Utils.Events
{
    /// <summary>
    /// The float event scriptable object definition.
    /// </summary>
    [CreateAssetMenu(fileName = "Game Event (Float)", menuName = "Events/Game Event (Float)")]
    public class FloatEvent : GenericEvent<float> { }
}
