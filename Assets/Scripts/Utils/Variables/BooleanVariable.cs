using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Utils.Variables
{

    /// <summary>
    /// The definition for the boolean variable scriptable object.
    /// </summary>
    [CreateAssetMenu(fileName = "Bool Variable", menuName = "Bool Variable")]
    public class BooleanVariable : GenericVariable<bool> { }
}
