using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Zombie
{

    /// <summary>
    /// The Zombie Type Enum.
    /// </summary>
    public enum ZombieType
    {
        MELEE,
        RANGE,
        TANK
    }

    /// <summary>
    /// Controls how the targets are handled for each zombie.
    /// </summary>
    public abstract class AZombieTargetController
    {
        // TODO: Implementation
    }

    /// <summary>
    /// The abstract class that handles the state of the zombies.
    /// </summary>
    public abstract class AZombieStateController
    {
        // TODO: Implementation.

        #region fields

        protected readonly AZombieController _controller;

        #endregion

        #region constructor

        public AZombieStateController(AZombieController controller)
        {
            this._controller = controller;
        }

        #endregion

        #region methods

        /// <summary>
        /// Called to hook the events.
        /// </summary>
        public virtual void HookEvents()
        {
            ZombieReferences references = this._controller.Component.References;
            references.BeatDownEvent += this.OnDownBeat;
        }

        /// <summary>
        /// Called to unhook the events.
        /// </summary>
        public virtual void UnHookEvents()
        {
            ZombieReferences references = this._controller.Component.References;
            references.BeatDownEvent -= OnDownBeat;
        }

        /// <summary>
        /// Handles when a down beat event is applied.
        /// </summary>
        abstract protected void OnDownBeat();

        #endregion
    }

    /// <summary>
    /// The abstract class that handles the animations of each zombie,
    /// also adds an animation controller at runtime.
    /// </summary>
    public abstract class AZombieAnimationController
    {
        #region fields

        protected readonly Animator _animator;

        #endregion

        #region constructor

        public AZombieAnimationController(AZombieController controller, RuntimeAnimatorController animatorController)
        {
            this._animator = controller.Component.GetComponent<Animator>()
                ?? controller.Component.gameObject.AddComponent<Animator>();
            this._animator.runtimeAnimatorController = animatorController;
            // TODO: More settings.
        }

        #endregion

        #region methods

        /// <summary>
        /// Hooks the events to the animator.
        /// </summary>
        public virtual void HookEvents() 
        {
            // TODO: 
        }

        /// <summary>
        /// Unhooks the events to the animator.
        /// </summary>
        public virtual void UnHookEvents()
        {
            // TODO: 
        }

        #endregion
    }

    /// <summary>
    /// The abstract zombie controller class.
    /// List of class Components:
    /// - ZombieTargetController
    /// - ZombieAnimationController
    /// - ZombieAttackController
    /// - ZombieStateController
    /// </summary>
    public abstract class AZombieController
    {
        #region fields

        private readonly ZombieComponent _component;
        private readonly ZombieType _type;

        #endregion

        #region properties

        public abstract AZombieTargetController TargetController
        {
            get;
        }

        public abstract AZombieStateController StateController
        {
            get;
        }

        public ZombieComponent Component => this._component;

        public ZombieType Type => this._type;

        #endregion

        #region constructor

        public AZombieController(ZombieComponent component, ZombieType type)
        {
            this._component = component;
            this._type = type;
        }

        #endregion

        #region methods

        /// <summary>
        /// Used to hook the events.
        /// </summary>
        public virtual void HookEvents()
        {
            this.StateController?.HookEvents();
        }

        /// <summary>
        /// Used to unhook the events.
        /// </summary>
        public virtual void UnHookEvents()
        {
            this.StateController?.UnHookEvents();
        }

        #endregion

        #region static_methods

        public static AZombieController Create(ZombieComponent component, ZombieSettings settings)
        {
            switch (settings.Type)
            {
                case ZombieType.MELEE:
                    return new Melee.MeleeZombieController(component, settings);
            }

            return null;
        }

        #endregion
    }
}