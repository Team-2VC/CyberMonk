using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CyberMonk.Tests
{
    /// <summary>
    /// The class for the shine bright monobehviour.
    /// This is not included in the game it's just
    /// used as a test object to sync beats.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class ShineBright : MonoBehaviour
    {

        private SpriteRenderer _renderer;

        /// <summary>
        /// Called when the object has started.
        /// </summary>
        private void Start()
        {
            this.HookEvents();
            this._renderer = this.GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Called when the object is enabled.
        /// </summary>
        private void OnEnable()
        {
            this.HookEvents();
        }

        /// <summary>
        /// Called to hook the events that this script uses.
        /// </summary>
        private void HookEvents()
        {
            if (Game.Conductor.Instance != null)
            {
                Game.Conductor.Instance.DownBeatEvent += OnDownBeat;
            }
        }

        /// <summary>
        /// Called when the object is disabled.
        /// </summary>
        private void OnDisable()
        {
            if(Game.Conductor.Instance != null)
            {
                Game.Conductor.Instance.DownBeatEvent -= OnDownBeat;
            }
        }

        /// <summary>
        /// Called on the down beat.
        /// </summary>
        private void OnDownBeat()
        {
            
            this._renderer.enabled = !this._renderer.enabled;
        }
    }
}
