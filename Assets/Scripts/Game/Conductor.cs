using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game
{
    /// <summary>
    /// The Conductor class definition.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class Conductor : MonoBehaviour
    {

        public event System.Action DownBeatEvent = delegate { };

        [Tooltip("The song beats per minute.")]
        [SerializeField, Range(0.1f, 150)]
        private float beatsPerMinute;
        [SerializeField]
        [Tooltip("The number of beats in the entire song.")]
        private float beatsPerSongLoop;
        [SerializeField]
        private float firstBeatOffset;

        private float _songPosition = 0f;
        private float _prevSongPositionInBeats;
        private AudioSource _audioSource;
        private int _completedLoops = 0;
        private float _originalDSPSongTime;

        //The current position of the song within the loop in beats.
        private float _loopPositionInBeats;

        private static Conductor _instance;

        public float BeatsPerSecond
            => 60f / this.beatsPerMinute;

        public float BeatsSongPosition
            => this._songPosition / this.BeatsPerSecond;

        // Reference for the instance.
        public static Conductor Instance
            => _instance;

        /// <summary>
        /// First function that gets called.
        /// </summary>
        private void Awake()
        {
            _instance = this;
        }

        /// <summary>
        /// Called when the conductor has started.
        /// </summary>
        private void Start()
        {
            
            //Load the AudioSource attached to the Conductor GameObject
            this._audioSource = GetComponent<AudioSource>();

            //Record the time when the music starts
            this._originalDSPSongTime = (float)AudioSettings.dspTime;

            //Start the music
            this._audioSource.Play();
        }

        /// <summary>
        /// Update is called once per frame.
        /// </summary>
        private void Update()
        {
            //determine how many seconds since the song started
            this._songPosition = (float)(AudioSettings.dspTime - this._originalDSPSongTime - firstBeatOffset);

           /*  this._prevSongPositionInBeats = (int)this.BeatsSongPosition;
            int absDifference = (int)Mathf.Abs(this._prevSongPositionInBeats - this.BeatsSongPosition);
            Debug.Log(absDifference);
            if(absDifference >= 1)
            {
                this.DownBeatEvent();
            } */

            if (this.BeatsSongPosition >= (this._completedLoops + 1) * this.beatsPerSongLoop)
            {
                this._completedLoops++;
                this.DownBeatEvent();
            }
        }
    }
}
