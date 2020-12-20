using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace CyberMonk.Game.Level
{
    
    /// <summary>
    /// The level loader.
    /// </summary>
    public class LevelLoaderComponent : MonoBehaviour
    {
        
        private static bool _instantiated = false;

        /// <summary>
        /// Called when the level loader has been awakened.
        /// </summary>
        private void Start()
        {
            if(_instantiated)
            {
                Destroy(this.gameObject);
                return;
            }

            _instantiated = true;
            DontDestroyOnLoad(this.gameObject);
        }

        private void Update()
        {
            LevelLoader.Update();
            
            IEnumerator asyncLoadLevel = LevelLoader.EnumerateLoadSceneAsync();
            if(asyncLoadLevel != null)
            {
                this.StartCoroutine(asyncLoadLevel);
            }
        }
    }
}

