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
        protected T constantValue;

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
                    return;
                }

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
        /// Resets the reference value.
        /// </summary>
        abstract public void Reset();

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
            get => this.variable != null ? this.variable.Value : this.constantValue;
            set
            {
                if(this.variable != null)
                {
                    this.variable.Value = value;
                }
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Resets the variable.
        /// </summary>
        public override void Reset()
        {
            this.variable?.Reset();
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
            get => this.variable != null ? this.variable.Value : this.constantValue;
            set
            {
                if (this.variable != null)
                {
                    this.variable.Value = value;
                }
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Resets the variable.
        /// </summary>
        public override void Reset()
        {
            this.variable?.Reset();
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
            get => this.variable != null ? this.variable.Value : this.constantValue;
            set
            {
                if (this.variable != null)
                {
                    this.variable.Value = value;
                }
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Resets the variable.
        /// </summary>
        public override void Reset()
        {
            this.variable?.Reset();
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
            get => this.variable != null ? this.variable.Value : this.constantValue;
            set
            {
                if (this.variable != null)
                {
                    this.variable.Value = value;
                }
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Resets the variable.
        /// </summary>
        public override void Reset()
        {
            this.variable?.Reset();
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
            get => this.variable != null ? this.variable.Value : this.constantValue;
            set
            {
                if (this.variable != null)
                {
                    this.variable.Value = value;
                }
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Resets the variable.
        /// </summary>
        public override void Reset()
        {
            this.variable?.Reset();
        }

        #endregion
    }
}
