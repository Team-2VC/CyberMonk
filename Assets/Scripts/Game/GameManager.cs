using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CyberMonk.Game
{

    [System.Serializable]
    public class GameManagerReferences
    {
        [SerializeField]
        private Utils.References.IntegerReference comboCounter;
        [SerializeField]
        private Utils.References.IntegerReference comboMultiplier;
        [SerializeField]
        private Utils.References.IntegerReference totalScore;
        [SerializeField]
        private Utils.References.IntegerReference currentWave;
        [SerializeField]
        private Utils.References.BooleanReference paused;

        [SerializeField]
        private Utils.Events.GameEvent gameOverEvent;
        [SerializeField]
        private Utils.Events.GameEvent moonkeyDeathEvent;

        [SerializeField]
        private GameObject pausedGUIPrefab;
        [SerializeField]
        private GameObject gameOverGUIPrefab;

        public Utils.References.BooleanReference Paused
        {
            get => this.paused;
            set => this.paused = value;
        }

        public GameObject PausedPrefab
        {
            get => this.pausedGUIPrefab;
        }

        public GameObject GameOverPrefab
            => this.gameOverGUIPrefab;

        public Utils.Events.GameEvent GameOverEvent
        {
            get => this.gameOverEvent;
            set
            {
                if (this.gameOverEvent != null)
                {
                    this.gameOverEvent = value;
                }
            }
        }

        public Utils.Events.GameEvent MoonkeyDeathEvent
        {
            get => this.moonkeyDeathEvent;
            set
            {
                if(this.moonkeyDeathEvent != null)
                {
                    this.moonkeyDeathEvent = value;
                }
            }
        }

        public void Reset()
        {
            this.comboCounter?.Reset();
            this.comboMultiplier?.Reset();
            this.totalScore?.Reset();
            this.currentWave?.Reset();
            this.paused?.Reset();
        }
    }

    /// <summary>
    /// The Game Manager references.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private GameManagerReferences references;

        protected bool PausedPressed
            => Input.GetKeyDown(KeyCode.Escape);

        private GameObject _currentMenu = null;
       
        private void Awake()
        {
            this.references.Reset();
        }

        private void OnEnable()
        {
            this.references.Paused.ChangedValueEvent += this.OnPaused;
            this.references.MoonkeyDeathEvent += this.OnMoonkeyDeath;
            this.references.GameOverEvent += this.OnGameOver;
        }

        private void OnDisable()
        {
            this.references.Paused.ChangedValueEvent -= this.OnPaused;
            this.references.MoonkeyDeathEvent -= this.OnMoonkeyDeath;
            this.references.GameOverEvent -= this.OnGameOver;
        }

        private void Update()
        {
            if(!this.references.Paused.Value && this.PausedPressed)
            {
                this.references.Paused.Value = true;
            }
        }

        private void OnPaused(bool paused)
        {
            float timeScale = 1f;
            if (paused && this._currentMenu == null)
            {
                timeScale = 0f;
                this._currentMenu = Instantiate(this.references.PausedPrefab);
            }

            Time.timeScale = timeScale;
        }
        
        private void OnGameOver()
        {
            if(this._currentMenu != null)
            {
                Destroy(this._currentMenu.gameObject);
            }

            if(this.references.Paused.Value)
            {
                this.references.Paused.Value = false;
                Time.timeScale = 1f;
            }

            this._currentMenu = Instantiate(this.references.GameOverPrefab);
        }

        private void OnMoonkeyDeath()
        {
            // TODO: reimplement accounting for more than 1 monkey.
            this.references.GameOverEvent?.Call();
        }
    }
}
