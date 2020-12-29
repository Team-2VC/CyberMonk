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
        [SerializeField]
        private LevelLoaderReference reference;
        
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
            this.reference.Loader?.Update();
            
            IEnumerator asyncLoadLevel = this.reference.Loader?.EnumerateLoadSceneAsync();
            if(asyncLoadLevel != null)
            {
                this.StartCoroutine(asyncLoadLevel);
            }
        }
    }
}

