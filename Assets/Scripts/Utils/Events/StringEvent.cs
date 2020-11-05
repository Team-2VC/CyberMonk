using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Utils.Events
{
    /// <summary>
    /// The string game event.
    /// </summary>
    [CreateAssetMenu(fileName = "Game Event (String)", menuName = "Game Event (String)")]
    public class StringEvent : GenericEvent<string> { }
}