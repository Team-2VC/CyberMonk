using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;


namespace CyberMonk.UI
{
    public class ScoreUI : MonoBehaviour
    {

        // Scritable Objects
        [SerializeField]
        private Utils.References.IntegerReference _comboCounter;

        [SerializeField]
        private Utils.References.IntegerReference _comboMultiplier;

        [SerializeField]
        private Utils.References.IntegerReference _totalScore;

        // TextMeshes
        [SerializeField]
        private TextMeshProUGUI _tmpComboCounter;

        [SerializeField]
        private TextMeshProUGUI _tmpComboMultiplier;

        [SerializeField]
        private TextMeshProUGUI _tmpTotalScore;


        // Update is called once per frame
        void Update()
        {
            this._tmpComboCounter.text = this._comboCounter.Value.ToString() + " Combo";
            this._tmpComboMultiplier.text = "x" + this._comboMultiplier.Value.ToString();
            this._tmpTotalScore.text = this._totalScore.Value.ToString();
        }
    }

}
