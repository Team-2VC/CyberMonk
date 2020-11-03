using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Utils.Variables
{
    /// <summary>
    /// The Generic Variable abstract class definition.
    /// </summary>
    /// <typeparam name="T">The type of variable.</typeparam>
    public abstract class GenericVariable<T> : ScriptableObject
    {

        #region fields

        [SerializeField]
        private T value;

        #endregion

        #region properties

        public T Value
        {
            get => this.value;
            set => this.value = value;
        }

        #endregion
    }
}
