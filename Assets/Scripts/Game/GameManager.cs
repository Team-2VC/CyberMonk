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

        public void Reset()
        {
            this.comboCounter?.Reset();
            this.comboMultiplier?.Reset();
            this.totalScore?.Reset();
            this.currentWave?.Reset();
        }
    }

    /// <summary>
    /// The Game Manager references.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private GameManagerReferences references;
       
        private void Awake()
        {
            this.references.Reset();
        }
    }
}
