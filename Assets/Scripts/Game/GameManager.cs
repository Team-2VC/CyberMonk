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
        private GameObject pausedGUIPrefab;

        public Utils.References.BooleanReference Paused
        {
            get => this.paused;
            set => this.paused = value;
        }

        public GameObject PausedPrefab
        {
            get => this.pausedGUIPrefab;
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


        private GameObject pausedGUI = null;

       
        private void Awake()
        {
            this.references.Reset();
        }

        private void OnEnable()
        {
            this.references.Paused.ChangedValueEvent += this.OnPaused;
        }

        private void OnDisable()
        {
            this.references.Paused.ChangedValueEvent -= this.OnPaused;
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
            Time.timeScale = paused ? 0f : 1f;

            if(paused)
            {
                if (this.pausedGUI == null)
                {
                    this.pausedGUI = Instantiate(this.references.PausedPrefab);
                }
                return;
            }
        }
    }
}
