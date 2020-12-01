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
        STATE_ATTACKED,
        STATE_LAUNCHED
    }

    public enum AttackOutcome
    {
        OUTCOME_FAILED,
        OUTCOME_SUCCESS,
        OUTCOME_NORMAL // Determines whether the attack was a normal attack
    }

    /// <summary>
    /// The zombie sound types.
    /// </summary>
    public enum ZombieSoundType
    {
        SOUND_LAUNCH
    }


    /// <summary>
    /// The zombies sound controller.
    /// </summary>
    public abstract class AZombieSoundController
    {
        public struct CurrentSoundData
        {
            public FMOD.Studio.EventInstance currentSound;
            public ZombieSoundType soundType;

            public bool IsPlaying
            {
                get
                {
                    FMOD.Studio.PLAYBACK_STATE state;
                    this.currentSound.getPlaybackState(out state);
                    return state != FMOD.Studio.PLAYBACK_STATE.STOPPED && state != FMOD.Studio.PLAYBACK_STATE.STOPPING;
                }
            }
        }

        private CurrentSoundData? _currentSoundData;
        protected readonly AZombieController _controller;
        protected readonly Dictionary<ZombieSoundType, FMOD.Studio.EventInstance> _sounds;

        protected CurrentSoundData? CurrentSound
            => this._currentSoundData;

        public AZombieSoundController(AZombieController controller, ZombieSoundData soundData)
        {
            this._controller = controller;
            this._sounds = soundData.GetSoundsList(controller.Type);
            this._currentSoundData = null;
        }

        /// <summary>
        /// Hooks the events of the zombie.
        /// </summary>
        abstract public void HookEvents();

        /// <summary>
        /// Unhooks the events of the zombie.
        /// </summary>
        abstract public void UnHookEvents();

        /// <summary>
        /// Plays the given sound.
        /// </summary>
        /// <param name="sound">The sound.</param>
        protected void PlaySound(ZombieSoundType sound)
        {
            if (this._currentSoundData.HasValue)
            {
                CurrentSoundData soundData = this._currentSoundData.Value;
                FMOD.Studio.EventInstance currentSound = this._currentSoundData.Value.currentSound;
                if (this._currentSoundData.Value.IsPlaying)
                {
                    currentSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                }
            }

            if (this._sounds.ContainsKey(sound))
            {
                CurrentSoundData newSoundData = new CurrentSoundData();
                newSoundData.currentSound = this._sounds[sound];
                newSoundData.soundType = sound;

                this._currentSoundData = newSoundData;
                this._currentSoundData.Value.currentSound.start();
            }
        }
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

            #region properties

            /// <summary>
            /// Gets the last target in the iterator.
            /// </summary>
            public Target.ZombieTargetWrapper Last
            {
                get
                {
                    if(this._targets.Count <= 0)
                    {
                        return null;
                    }

                    return this._targets[this._targets.Count - 1];
                }
            }

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

        #region fields

        public event System.Action<AttackOutcome> AttackEvent
            = delegate { };

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
            reference.BeatDownEvent -= this.OnDownBeat;
        }

        /// <summary>
        /// Called when the beat is on down
        /// </summary>
        private void OnDownBeat()
        {
            if (this._targetsActive)
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
                this.ResetTargets();
                this.AttackEvent(AttackOutcome.OUTCOME_FAILED);
                return;
            }

            if(this._previousTargetClicked + 1 != targetIndex)
            {
                // TODO: Hook up the targets to the event and damage player. 
                Debug.Log("Damage the player & deactivate targets.");
                this.ResetTargets();
                this.AttackEvent(AttackOutcome.OUTCOME_FAILED);
                return;
            }

            if(this._targetsIterator.Last != null && this._targetsIterator.Last.TargetIndex == targetIndex)
            {
                this.AttackEvent(AttackOutcome.OUTCOME_SUCCESS);
                return;
            }

            this._previousTargetClicked = targetIndex;
            this.AttackEvent(AttackOutcome.OUTCOME_NORMAL);
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
    /// Implements the movement for the launch.
    /// </summary>
    public class ZombieLaunchController
    {

        private AZombieMovementController _parent;
        private Rigidbody2D _rigidbody;

        private Vector2 _startPosition;
        private Vector2 _centerPosition;
        private Vector2 _endPosition;

        private ZombieMovementData.ZombieLaunchData _launchData;
        private float _currentLaunchDuration = 0f;

        public bool Completed
            => this._currentLaunchDuration >= this._launchData.LaunchDuration;

        public float LaunchDuration
            => this._currentLaunchDuration;

        protected float DurationPercentage
            => this._currentLaunchDuration / this._launchData.LaunchDuration;

        public ZombieLaunchController(AZombieMovementController parent, ZombieMovementData.ZombieLaunchData launchData)
        {
            this._parent = parent;
            this._rigidbody = parent.Rigidbody;
            this._startPosition = parent.Controller.Component.transform.position;
            this._endPosition = parent.Controller.Component.References.MoonPosition;
            this._launchData = launchData;

            // Calculates the middle position.
            Vector2 midPosition = new Vector2();
            midPosition.y += launchData.LaunchHeight;
            midPosition.x = (this._startPosition.x + this._endPosition.x / 2.0f);
            this._centerPosition = midPosition;
        }

        /// <summary>
        /// Updates the movement of the launch.
        /// </summary>
        public void Update()
        {
            if(!this.Completed)
            {
                this._currentLaunchDuration += Time.fixedDeltaTime;
                this._rigidbody.MovePosition(this.BezierCurve(this.DurationPercentage));
            }
        }

        /// <summary>
        /// Gets the bezier curve used for the movement of the launch.
        /// </summary>
        /// <param name="travelPercentage">The travel percentage.</param>
        /// <returns>The point where to travel to.</returns>
        private Vector2 BezierCurve(float travelPercentage)
        {
            Vector2 rayAB = Vector2.Lerp(this._startPosition, this._centerPosition, travelPercentage);
            Vector2 rayBC = Vector2.Lerp(this._centerPosition, this._endPosition, travelPercentage);

            return Vector2.Lerp(rayAB, rayBC, travelPercentage);
        }
    }



    /// <summary>
    /// The abstract class that handles the movement of the zombie.
    /// Checklist:
    /// - Zombies need to be in x amount of range to move to the player.
    /// - While targeted player is attacking, search for a new target (if there one)
    /// - Move every other beat towards the player if in range.
    /// </summary>
    public abstract class AZombieMovementController
    {

        #region fields

        public event System.Action LaunchEndEvent
            = delegate { };
 
        protected AZombieController _controller;
        private ZombieLaunchController _launchController = null;
        private ZombieMovementData.ZombieLaunchData _launchData;

        #endregion

        #region properties

        public abstract Rigidbody2D Rigidbody
        {
            get;
        }

        public AZombieController Controller
        {
            get => this._controller;
        }

        protected bool Launched
            => this._launchController != null;

        #endregion

        #region constructor

        public AZombieMovementController(AZombieController controller, ZombieMovementData movementData)
        {
            this._controller = controller;
            this._launchData = movementData.LaunchData;
        }

        #endregion

        #region methods

        /// <summary>
        /// Hooks the events.
        /// </summary>
        public virtual void HookEvents()
        {
            ZombieReferences references = this._controller.Component.References;
            references.BeatDownEvent += this.OnDownBeat;

            this._controller.AttackedEvent += this.OnAttackBegin;
            this._controller.AttackEvent += this.OnAttack;
            this._controller.LaunchBeginEvent += this.OnLaunched;
        }

        /// <summary>
        /// Unhooks the events.
        /// </summary>
        public virtual void UnHookEvents()
        {
            ZombieReferences references = this._controller.Component.References;
            references.BeatDownEvent -= this.OnDownBeat;

            this._controller.AttackedEvent -= this.OnAttackBegin;
            this._controller.AttackEvent -= this.OnAttack;
            this._controller.LaunchBeginEvent -= this.OnLaunched;
        }

        /// <summary>
        /// Updates the physics of the zombie, used to update movement.
        /// </summary>
        public void PhysicsUpdate()
        {
            if(this.Launched)
            {
                this.UpdateLaunched();
                return;
            }

            this.UpdateMovement();
        }

        protected virtual void OnAttackBegin(Moonkey.MoonkeyComponent attacker) { }

        protected virtual void OnAttack(AttackOutcome outcome) { }

        abstract protected void OnDownBeat();

        /// <summary>
        /// Updates the movement of the zombie, called
        /// in the Update function.
        /// </summary>
        abstract protected void UpdateMovement();

        protected virtual void OnLaunched()
        {
            this._launchController = new ZombieLaunchController(this, this._launchData);
        }
        
        protected void UpdateLaunched()
        {
            if(this._launchController == null)
            {
                return;
            }

            this._launchController.Update();
            
            if(this._launchController.Completed)
            {
                this.LaunchEndEvent();
                this._launchController = null;
                return;
            }
        }

        #endregion
    }

    /// <summary>
    /// The abstract class that handles the state of the zombies.
    /// </summary>
    public abstract class AZombieStateController
    {
        #region fields

        public event System.Action LaunchBeginEvent
            = delegate { };

        protected ZombieState _state;
        protected readonly AZombieController _controller;

        #endregion

        #region properties

        /// <summary>
        /// Determines whether the zombie is open for an attack.
        /// </summary>
        public abstract bool OpenForAttack
        {
            get;
        }

        public abstract Moonkey.MoonkeyComponent Attacker
        {
            get;
        }

        public ZombieState State
            => this._state;

        #endregion

        #region constructor

        public AZombieStateController(AZombieController controller)
        {
            this._controller = controller;
            this._state = ZombieState.STATE_DANCING;
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
            this._controller.AttackedEvent += this.OnAttacked;
            this._controller.AttackEvent += this.OnAttack;
            this._controller.LaunchEndEvent += this.OnLaunchEnd;
        }

        /// <summary>
        /// Called to unhook the events.
        /// </summary>
        public virtual void UnHookEvents()
        {
            ZombieReferences references = this._controller.Component.References;
            references.BeatDownEvent -= this.OnDownBeat;
            this._controller.AttackedEvent -= this.OnAttacked;
            this._controller.AttackEvent -= this.OnAttack;
            this._controller.LaunchEndEvent -= this.OnLaunchEnd;
        }

        /// <summary>
        /// Called when the beat goes down.
        /// </summary>
        private void OnDownBeat()
        {
            this.OnBeat();
        }

        /// <summary>
        /// Handles when a down beat event is applied.
        /// </summary>
        abstract protected void OnBeat();

        /// <summary>
        /// Called when the zombie is attacked.
        /// </summary>
        /// <param name="component">The moonkey component.</param>
        abstract protected void OnAttacked(Moonkey.MoonkeyComponent component);

        /// <summary>
        /// Called when the attack has ended.
        /// </summary>
        /// <param name="outcome">The attack outcome.</param>
        abstract protected void OnAttack(AttackOutcome outcome);

        /// <summary>
        /// Called to launch the zombie.
        /// </summary>
        protected void Launch()
        {
            this.OnLaunchBegin();
            this.LaunchBeginEvent();
        }

        /// <summary>
        /// Called when the zombie is launched.
        /// </summary>
        abstract protected void OnLaunchBegin();

        /// <summary>
        /// Called when the launch has ended.
        /// </summary>
        abstract protected void OnLaunchEnd();

        #endregion
    }

    /// <summary>
    /// The abstract class that handles the animations of each zombie,
    /// also adds an animation controller at runtime.
    /// </summary>
    public abstract class AZombieAnimationController
    {
        #region fields

        protected readonly ZombieGraphics _graphics;
        protected readonly Animator _animator;

        #endregion

        #region constructor

        public AZombieAnimationController(AZombieController controller, RuntimeAnimatorController animatorController)
        {
            this._graphics = controller.Component.Graphics;

            this._animator = this._graphics.GraphicsAnimator;
           
            if(this._animator != null)
            {
                this._animator.runtimeAnimatorController = animatorController;
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Hooks the events to the animator.
        /// </summary>
        public virtual void HookEvents() { }

        /// <summary>
        /// Unhooks the events to the animator.
        /// </summary>
        public virtual void UnHookEvents() { }

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

        public event System.Action<Moonkey.MoonkeyComponent> AttackedEvent
            = delegate { };

        public event System.Action<AttackOutcome> AttackEvent
        {
            add => this._targetController.AttackEvent += value;
            remove => this._targetController.AttackEvent -= value;
        }

        public event System.Action LaunchBeginEvent
        {
            add
            {
                if(this.StateController != null)
                {
                    this.StateController.LaunchBeginEvent += value;
                }
            }
            remove
            {
                if(this.StateController != null)
                {
                    this.StateController.LaunchBeginEvent -= value;
                }
            }
        }

        public event System.Action LaunchEndEvent
        {
            add
            {
                if(this.MovementController != null)
                {
                    this.MovementController.LaunchEndEvent += value;
                }
            }
            remove
            {
                if(this.MovementController != null)
                {
                    this.MovementController.LaunchEndEvent -= value;
                }
            }
        }

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

        public abstract AZombieMovementController MovementController
        {
            get;
        }

        public abstract AZombieSoundController SoundController
        {
            get;
        }

        public abstract AZombieAnimationController AnimationController
        {
            get;
        }

        public ZombieComponent Component => this._component;

        public ZombieType Type 
            => this._type;

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
            this.TargetController?.HookEvents();
            this.MovementController?.HookEvents();
            this.SoundController?.HookEvents();
            this.AnimationController?.HookEvents();
        }

        /// <summary>
        /// Used to unhook the events.
        /// </summary>
        public virtual void UnHookEvents()
        {
            this.StateController?.UnHookEvents();
            this.TargetController.UnHookEvents();
            this.MovementController?.UnHookEvents();
            this.SoundController?.UnHookEvents();
            this.AnimationController?.UnHookEvents();
        }

        /// <summary>
        /// Updates the physics of the zombie.
        /// </summary>
        public virtual void PhysicsUpdate()
        {
            this.MovementController?.PhysicsUpdate();
        }

        /// <summary>
        /// Called when the moonkey component attacks this zombie.
        /// </summary>
        /// <param name="component">The moonkey component reference.</param>
        /// <returns>True if the monkey was succesfully attacked, false otherwise.</returns>
        public virtual TryZombieAttackOutcome OnMoonkeyAttack(Moonkey.MoonkeyComponent component)
        {
            // TODO: Add check for player if its already attacking

            if(this.StateController.OpenForAttack)
            {
                this.AttackedEvent(component);
                return TryZombieAttackOutcome.OUTCOME_SUCCESS;
            }

            return TryZombieAttackOutcome.OUTCOME_FAILED_ZOMBIE_ATTACKING;
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