using CyberMonk.Game.Zombie.Target;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Zombie
{

    namespace Target
    {

        /// <summary>
        /// The reason why the target got hit.
        /// </summary>
        public enum DeactivationReason
        {
            // Called when the target is hit by the player.
            REASON_PLAYER_HIT,
            // Called when the target runs out of time.
            REASON_RUN_OUT_OF_TIME,
            // Called when the target was forced to deactivate
            // because of other targets
            REASON_FORCED
        }

        /// <summary>
        /// The data for why the zombie target got deactivated.
        /// </summary>
        public struct DeactivationData
        {
            private DeactivationReason _reason;
            private object _data;

            public object Data
                => this._data;

            public DeactivationReason Reason
                => this._reason;

            public DeactivationData(DeactivationReason reason, object otherData = null)
            {
                this._reason = reason;
                this._data = null;
            }
        }

        /// <summary>
        /// The data containing the zombie targets.
        /// </summary>
        [System.Serializable]
        public struct ZombieTargetsData
        {
            #region fields

            [SerializeField]
            private GameObject prefab;

            [SerializeField]
            private List<Vector2> zombieTargetPositions;

            [SerializeField]
            private bool randomizeTargetOrder;

            #endregion

            #region properties

            public GameObject Prefab
            {
                get => this.prefab;
            }

            public List<Vector2> ZombieTargetPositions
                => this.zombieTargetPositions;

            public bool RandomizeTargets
                => this.randomizeTargetOrder;

            #endregion
        }

        /// <summary>
        /// The wrapper class containing the information for the zombie target.
        /// </summary>
        public class ZombieTargetWrapper
        {

            #region fields

            private AZombieController _parent;
            private GameObject _prefab;
            private Vector2 _spawnPosition;

            private int _targetIndex;

            private TargetComponent _component;

            #endregion

            public int TargetIndex
                => this._targetIndex;

            #region constructor

            public ZombieTargetWrapper(AZombieController parent, Vector2 position, GameObject prefab, int targetIndex)
            {
                this._parent = parent;
                this._prefab = prefab;
                this._spawnPosition = position;
                this._targetIndex = targetIndex;
                this._component = null;
            }

            /// <summary>
            /// Spawns the target.
            /// </summary>
            public void SpawnTarget()
            {
                if(this._parent.Component == null)
                {
                    return;
                }

                Vector2 transformPosition = (Vector2)this._parent.Component.transform.position;
                Vector2 position = this._spawnPosition + transformPosition;
                GameObject spawned = GameObject.Instantiate<GameObject>(this._prefab, position, Quaternion.identity);
                spawned.transform.parent = this._parent.Component.transform;

                // Sets the wrapper component.
                TargetComponent component = spawned.GetComponent<TargetComponent>();
                if(component != null)
                {
                    component.SetWrapper(this);
                }

                this._component = component;
            }

            /// <summary>
            /// Called when the target is deactivated.
            /// </summary>
            /// <param name="data">The data for why the target was deactivated.</param>
            public void OnDeactivated(DeactivationData data)
            {
                this._parent.TargetController.OnDeactivate(
                    this.TargetIndex, data);
                
                // Dereference the component.
                if(this._component != null)
                {
                    this._component = null;
                }
            }

            /// <summary>
            /// Forces the target to be deactivated.
            /// </summary>
            public void ForceDeactivate()
            {
                if(this._component != null)
                {
                    this._component.ForceDeactivate();
                    this._component = null;
                }
            }

            #endregion
        }
    }

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
    /// The state of the zombie.
    /// </summary>
    public enum ZombieState
    {
        STATE_DANCING,
        STATE_IDLE,
        STATE_ATTACKED,
        STATE_LAUNCHED
    }

    /// <summary>
    /// Controls how the targets are handled for each zombie.
    /// </summary>
    public class ZombieTargetController
    {

        /// <summary>
        /// Iterates through each target when the targets 
        /// are active.
        /// </summary>
        public class TargetsIterator
        {
            #region fields

            private Target.ZombieTargetsData _targetsData;
           
            private List<Target.ZombieTargetWrapper> _targets;
            private int currentTarget;

            private AZombieController _parent;

            #endregion

            #region constructor

            public TargetsIterator(AZombieController parent, Target.ZombieTargetsData data)
            {
                this._targetsData = data;
                this._parent = parent;
                
                this._targets = new List<Target.ZombieTargetWrapper>();
                this.currentTarget = -1;
                
                this.SetupTargets();
            }

            #endregion

            #region methods

            /// <summary>
            /// This sets up the targets.
            /// </summary>
            private void SetupTargets()
            {
                if(this._targetsData.RandomizeTargets)
                {
                    int positionCount = this._targetsData.ZombieTargetPositions.Count;
                    List<Vector2> positionsFilled = new List<Vector2>();

                    while(this._targets.Count < positionCount)
                    {
                        Vector2 position;
                        do
                        {
                            int randomTarget = UnityEngine.Random.Range(0, positionCount);
                            position = this._targetsData.ZombieTargetPositions[randomTarget];
                        }
                        while (positionsFilled.Contains(position));

                        Target.ZombieTargetWrapper wrapper = new Target.ZombieTargetWrapper(
                            this._parent, position, this._targetsData.Prefab, this._targets.Count);
                        
                        this._targets.Add(wrapper);
                        positionsFilled.Add(position);
                    }
                    return;
                }

                foreach(Vector2 position in this._targetsData.ZombieTargetPositions)
                {
                    Target.ZombieTargetWrapper wrapper = new Target.ZombieTargetWrapper(
                        this._parent, position, this._targetsData.Prefab, this._targets.Count);
                    this._targets.Add(wrapper);
                }
            }

            /// <summary>
            /// Gets the next zombie target.
            /// </summary>
            /// <returns>The next zombie target in the list.</returns>
            public Target.ZombieTargetWrapper Next()
            {

                if(!this.HasNext())
                {
                    return null;
                }

                return this._targets[++this.currentTarget];
            }

            /// <summary>
            /// Determines whether the iterator has a next value.
            /// </summary>
            /// <returns>True if the iterator has a next.</returns>
            public bool HasNext()
            {
                if(this._targets.Count <= 0)
                {
                    return false;
                }

                if((this.currentTarget + 1) >= this._targets.Count)
                {
                    return false;
                }

                return true;
            }

            /// <summary>
            /// Resets the iterator.
            /// </summary>
            public void Reset()
            {
                // Deactivates each of the target wrappers.
                foreach(ZombieTargetWrapper wrapper in this._targets)
                {
                    wrapper.ForceDeactivate();
                }

                this.currentTarget = -1;
            }

            #endregion
        }

        #region events

        /// <summary>
        /// Called when all targets are deactivated.
        /// </summary>
        public event System.Action OnTargetsDeactivated
            = delegate { };

        #endregion

        #region fields

        private bool _targetsActive = false;
        private TargetsIterator _targetsIterator;

        /// <summary>
        /// Current Target represents the previous target that we have in the iterator.
        /// Next Target represents the current target that we have in the iterator.
        /// </summary>
        private Target.ZombieTargetWrapper _currentTarget;

        private AZombieController _controller;
        private ZombieReferences? references = null;

        private int _previousTargetClicked = -1;

        #endregion

        #region properties

        public bool TargetsActive
        {
            get => this._targetsActive;
            set => this._targetsActive = value;
        }

        #endregion

        #region constructor

        public ZombieTargetController(AZombieController controller, Target.ZombieTargetsData data)
        {
            this._controller = controller;
            this._targetsIterator = new TargetsIterator(controller, data);
            
            this._currentTarget = null;
        }

        #endregion

        #region methods

        /// <summary>
        /// Hooks the events from the target controller.
        /// </summary>
        public virtual void HookEvents()
        {
            if(this.references == null)
            {
                this.references = this._controller.Component.References;
            }

            ZombieReferences reference = this.references.Value;
            reference.BeatDownEvent += this.OnDownBeat;
        }

        /// <summary>
        /// Called to unhook the events.
        /// </summary>
        public virtual void UnHookEvents()
        {
            ZombieReferences reference = this.references.Value;
            reference.BeatDownEvent += this.OnDownBeat;
        }

        /// <summary>
        /// Called when the beat is on down
        /// </summary>
        private void OnDownBeat()
        {
            if(this._targetsActive)
            {
                // TODO: Test
                this._currentTarget = this._targetsIterator.Next();
                this._currentTarget?.SpawnTarget();
            }
        }

        /// <summary>
        /// Called when the target at the index is deactivated.
        /// </summary>
        /// <param name="targetIndex">The target index.</param>
        /// <param name="data">The data.</param>
        internal void OnDeactivate(int targetIndex, DeactivationData data)
        {
            if(data.Reason == DeactivationReason.REASON_RUN_OUT_OF_TIME)
            {
                // TODO: hook up the targets to the event and damage player.
                Debug.Log("Damage the player & deactivate targets.");
                this.OnTargetsDeactivated();
                this.ResetTargets();
                return;
            }

            if(this._previousTargetClicked + 1 != targetIndex)
            {
                // TODO: Hook up the targets to the event and damage player. 
                Debug.Log("Damage the player & deactivate targets.");
                this.OnTargetsDeactivated();
                this.ResetTargets();
                return;
            }

            // TODO: check if the target index is the last target 
            this._previousTargetClicked = targetIndex;
        }

        /// <summary>
        /// Resets the targets for the zombie.
        /// </summary>
        private void ResetTargets()
        {
            this._currentTarget = null;
            this._targetsActive = false;
            this._previousTargetClicked = -1;

            this._targetsIterator.Reset();
        }

        #endregion
    }

    /// <summary>
    /// The abstract class that handles the state of the zombies.
    /// </summary>
    public abstract class AZombieStateController
    {
        // TODO: Implementation.

        #region fields

        protected ZombieState _state;
        protected readonly AZombieController _controller;

        #endregion

        #region constructor

        public AZombieStateController(AZombieController controller)
        {
            this._controller = controller;
            this._state = ZombieState.STATE_IDLE;
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
        private readonly ZombieTargetController _targetController;

        #endregion

        #region properties

        public virtual ZombieTargetController TargetController
            => this._targetController;

        public abstract AZombieStateController StateController
        {
            get;
        }

        public ZombieComponent Component => this._component;

        public ZombieType Type => this._type;

        #endregion

        #region constructor

        public AZombieController(ZombieComponent component, ZombieSettings settings)
        {
            this._component = component;
            this._type = settings.Type;

            this._targetController = new ZombieTargetController(this, settings.TargetsData);
        }

        #endregion

        #region methods

        /// <summary>
        /// Used to hook the events.
        /// </summary>
        public virtual void HookEvents()
        {
            this.StateController?.HookEvents();
            this.TargetController.HookEvents();
        }

        /// <summary>
        /// Used to unhook the events.
        /// </summary>
        public virtual void UnHookEvents()
        {
            this.StateController?.UnHookEvents();
            this.TargetController.UnHookEvents();
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