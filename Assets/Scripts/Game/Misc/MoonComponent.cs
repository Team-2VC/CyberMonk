using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Misc
{
    /// <summary>
    /// The moon component.
    /// </summary>
    public class MoonComponent : MonoBehaviour
    {
        [SerializeField]
        private Utils.References.Vector2Reference moonPosition;

        private void Start()
        {
            this.moonPosition.Value = (Vector2)this.transform.position;
        }
    }
}

