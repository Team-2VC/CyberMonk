using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CyberMonk.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private int _defaultComboCount = 0;

        [SerializeField]
        private int _defaultComboMultiplier = 1;

        [SerializeField]
        private int _defaultTotalScore = 0;

        [SerializeField]
        private Utils.References.IntegerReference _comboCounter;

        [SerializeField]
        private Utils.References.IntegerReference _comboMultiplier;

        [SerializeField]
        private Utils.References.IntegerReference _totalScore;

       
        void Awake()
        {
            this._comboCounter.Value = this._defaultComboCount;
            this._comboMultiplier.Value = this._defaultComboMultiplier;
            this._totalScore.Value = this._defaultTotalScore;
        }

        
    }

}
