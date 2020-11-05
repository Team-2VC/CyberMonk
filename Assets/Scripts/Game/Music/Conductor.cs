using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Music
{
    /// <summary>
    /// The Conductor class definition.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class Conductor : MonoBehaviour
    {
        #region fields

        [SerializeField]
        private Utils.References.IntegerReference beatCounter;
        [SerializeField]
        private Utils.Events.GameEvent beatDownEvent;

        [SerializeField]
        private ConductorMusicData musicData;

        private float _songPosition = 0f;
        private AudioSource _audioSource;
        private float _originalDSPSongTime;
        private int currentBeat = 0;

        #endregion

        #region properties

        public float BeatsSongPosition
            => this._songPosition / this.musicData.SecondsPerBeat;

        private bool ContainsMusicData
            => this.musicData != null;

        #endregion

        #region methods

        /// <summary>
        /// Called when the conductor has started.
        /// </summary>
        private void Start()
        {
            this._audioSource = GetComponent<AudioSource>();
            this._originalDSPSongTime = (float)AudioSettings.dspTime;
            
            // Adds the audio clip from the music data.
            if(this.ContainsMusicData && this.musicData.Music != null)
            {
                this._audioSource.clip = this.musicData.Music;
            }

            this._audioSource.Play();
        }

        /// <summary>
        /// Update is called once per frame.
        /// </summary>
        private void Update()
        {
            if(this.ContainsMusicData && this._audioSource.isPlaying)
            {
                this._songPosition = (float)(AudioSettings.dspTime -
                    this._originalDSPSongTime - this.musicData.FirstBeatOffset);

                if (this.BeatsSongPosition > (this.currentBeat + 1))
                {
                    this.currentBeat++;
                    this.beatCounter.Value = this.currentBeat % this.musicData.BeatsPerMeasure;
                    this.beatDownEvent?.Call();
                }
            }
        }

        #endregion
    }
}
