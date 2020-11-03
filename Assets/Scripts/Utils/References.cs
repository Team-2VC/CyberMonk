using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The namespace definition.
/// </summary>
namespace CyberMonk.Utils.References
{
    /// <summary>
    /// The Abstract Reference class implementation.
    /// </summary>
    /// <typeparam name="T">The type of the reference.</typeparam>
    public abstract class GenericReference<T>
    {

        #region fields

        [SerializeField]
        [Tooltip("Determines whether or not the reference is a constant.")]
        private bool constant;
        [SerializeField]
        [Tooltip("The constant value if the given reference is a constant value.")]
        private T constantValue;

        private T _original = default;
        private bool _hasBeenSet = false;

        public event System.Action<T> ChangedValueEvent
            = delegate { };

        #endregion

        #region properties

        /// <summary>
        /// Gets the reference value.
        /// </summary>
        protected abstract T ReferenceValue
        {
            set;
            get;
        }

        /// <summary>
        /// The value property.
        /// </summary>
        public T Value
        {
            get
            {
                return this.constant ? this.constantValue : this.ReferenceValue;
            }
            set
            {
                if (this.constant)
                {
                    this.SetOriginal(this.constantValue);
                    return;
                }

                if (this.DidChange(this.ReferenceValue, value))
                {
                    this.CallChangeValueEvent(value);
                }

                this.SetOriginal(this.ReferenceValue);
                this.ReferenceValue = value;
            }
        }

        /// <summary>
        /// Determines whether the reference is a constant reference.
        /// </summary>
        protected bool Constant
            => this.constant;

        #endregion

        #region methods

        /// <summary>
        /// Sets the original value for safe keeping.
        /// </summary>
        /// <param name="original">The original value.</param>
        private void SetOriginal(T original)
        {
            if (this._hasBeenSet)
            {
                return;
            }

            this._original = original;
            this._hasBeenSet = true;
        }

        /// <summary>
        /// Resets the reference value.
        /// </summary>
        public void Reset()
        {
            this.Value = this._original;
        }

        /// <summary>
        /// Determines if the value does change.
        /// </summary>
        /// <param name="prev">The previous value</param>
        /// <param name="next">The next value</param>
        /// <returns>True if the value changed, false otherwise.</returns>
        abstract protected bool DidChange(T prev, T next);

        /// <summary>
        /// Calls the change value event.
        /// </summary>
        /// <param name="v">The value.</param>
        protected virtual void CallChangeValueEvent(T v)
        {
            this.ChangedValueEvent(v);
        }

        #endregion
    }

    /// <summary>
    /// The Float Reference Definition.
    /// </summary>
    [System.Serializable]
    public class FloatReference : GenericReference<float>
    {

        #region fields

        [SerializeField]
        private Variables.FloatVariable variable;

        #endregion

        #region properties

        /// <summary>
        /// The Reference value definition.
        /// </summary>
        protected override float ReferenceValue
        {
            get => this.variable.Value;
            set => this.variable.Value = value;
        }

        #endregion

        #region methods

        protected override bool DidChange(float prev, float next)
        {
            return prev != next;
        }

        #endregion
    }

    /// <summary>
    /// The Integer Reference implementation.
    /// </summary>
    [System.Serializable]
    public class IntegerReference : GenericReference<int>
    {

        #region fields

        [SerializeField]
        private Variables.IntegerVariable variable;

        #endregion

        #region properties

        protected override int ReferenceValue
        {
            get => variable.Value;
            set => variable.Value = value;
        }

        #endregion

        #region methods

        protected override bool DidChange(int prev, int next)
        {
            return prev != next;
        }

        #endregion
    }

    /// <summary>
    /// The boolean reference class.
    /// </summary>
    [System.Serializable]
    public class BooleanReference : GenericReference<bool>
    {
        #region fields

        [SerializeField]
        private Variables.BooleanVariable variable;

        #endregion

        #region properties

        protected override bool ReferenceValue
        {
            get => variable.Value;
            set => variable.Value = value;
        }

        #endregion

        #region methods

        protected override bool DidChange(bool prev, bool next)
        {
            return prev != next;
        }

        #endregion
    }

    /// <summary>
    /// The string reference.
    /// </summary>
    [System.Serializable]
    public class StringReference : GenericReference<string>
    {

        #region fields

        [SerializeField]
        private Variables.StringVariable variable;

        #endregion

        #region propreties

        protected override string ReferenceValue
        {
            get => variable.Value;
            set => variable.Value = value;
        }

        #endregion

        #region methods

        protected override bool DidChange(string prev, string next)
        {
            return prev != next;
        }

        #endregion
    }

    /// <summary>
    /// The Vector2 Reference.
    /// </summary>
    [System.Serializable]
    public class Vector2Reference : GenericReference<Vector2>
    {
        #region fields

        [SerializeField]
        private Variables.Vector2Variable variable;

        #endregion

        #region properties

        protected override Vector2 ReferenceValue
        {
            get => variable.Value;
            set => variable.Value = value;
        }

        #endregion

        #region methods

        protected override bool DidChange(Vector2 prev, Vector2 next)
        {
            return prev != next;
        }

        #endregion
    }
}
