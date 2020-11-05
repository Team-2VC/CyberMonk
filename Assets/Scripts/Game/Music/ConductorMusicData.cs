using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Music
{
    /// <summary>
    /// Data used for the conductor.
    /// </summary>
    [CreateAssetMenu(fileName = "Conductor Music Data", menuName = "Conductor Music Data")]
    public class ConductorMusicData : ScriptableObject
    {
        [SerializeField, Range(1, 12)]
        [Tooltip("Represents the beats per measure.")]
        private int beatsPerMeasure;
        [SerializeField, Range(1, 200)]
        private int beatsPerMinute;
        [SerializeField]
        private float firstBeatOffset;
        [SerializeField]
        private AudioClip music;
        
        public float SecondsPerMeasure
            => this.BeatsPerMeasure * this.SecondsPerBeat;

        public float SecondsPerBeat
            => (60f / this.beatsPerMinute);

        public int BeatsPerMeasure
            => this.beatsPerMeasure;

        public float FirstBeatOffset
            => this.firstBeatOffset;

        public AudioClip Music
            => this.music;
    }
}
