using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Utils.Variables
{

    /// <summary>
    /// Defintion for the string variable scriptable object.
    /// </summary>
    [CreateAssetMenu(fileName = "String Variable", menuName = "Variables/String Variable")]
    public class StringVariable : GenericVariable<string> { }
}
