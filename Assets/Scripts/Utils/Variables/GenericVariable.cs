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
        [SerializeField]
        private T originalValue;

        public event System.Action<T> ChangedValueEvent
            = delegate { };

        #endregion

        #region properties

        public T Value
        {
            get => this.value;
            set
            {
                if(!this.value.Equals(value))
                {
                    this.ChangedValueEvent(value);
                }
                this.value = value;
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Resets the value to its original.
        /// </summary>
        public void Reset()
        {
            this.value = originalValue;
        }

        #endregion
    }
}
