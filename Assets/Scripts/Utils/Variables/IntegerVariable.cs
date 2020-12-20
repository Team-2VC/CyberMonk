using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Utils.Variables
{

    /// <summary>
    /// Defintion for the integer variable scriptable object.
    /// </summary>
    [CreateAssetMenu(fileName = "Integer Variable", menuName = "Variables/Integer Variable")]
    public class IntegerVariable : GenericVariable<int> { }
}
