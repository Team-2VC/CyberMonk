using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace CyberMonk.Game.Level
{
   
    /// <summary>
    /// Loading Scene.
    /// </summary>
    public class LoadingScene
    {

        public event System.Action SceneLoadedEvent
            = delegate { };

        private float _currentLoadTime = 0f;
        private float _maxLoadTime;

        private bool _executingCoroutine = false;
        private string _sceneToLoad;

        public bool CanLoadScene
            => this._currentLoadTime >= this._maxLoadTime && !this._executingCoroutine;

        public LoadingScene(string sceneToLoad, float maxLoadTime)
        {
            this._sceneToLoad = sceneToLoad;
            this._maxLoadTime = maxLoadTime;
        }

        public void Update()
        {
            if (this._currentLoadTime < this._maxLoadTime)
            {
                this._currentLoadTime += Time.deltaTime;
            }
        }

        public IEnumerator LoadSceneAsync()
        {
            this._executingCoroutine = true;
            AsyncOperation operation = SceneManager.LoadSceneAsync(this._sceneToLoad);
            yield return new WaitUntil(() => operation.isDone);
            this.SceneLoadedEvent();
        }
    }

    /// <summary>
    /// The static class that handles level loading.
    /// </summary>
    public static class LevelLoader
    {
        private static LoadingScene _currentLoadingScene = null;
       
        public static bool IsLoading
            => _currentLoadingScene != null;

        public static void Update()
        {
            _currentLoadingScene?.Update();
        }

        public static IEnumerator EnumerateLoadSceneAsync()
        {
            if(_currentLoadingScene == null)
            {
                return null;
            }

            if(_currentLoadingScene.CanLoadScene)
            {
                return _currentLoadingScene.LoadSceneAsync();
            }
            return null;
        }

        private static void OnCompleted()
        {
            Debug.Log("On Completed Loading the level.");
            _currentLoadingScene.SceneLoadedEvent -= OnCompleted;
            _currentLoadingScene = null;
        }

        public static void LoadLevel(LevelLoadData levelData)
        {
            if(IsLoading)
            {
                return;
            }

            if(levelData.ShowLoadingSceen)
            {
                SceneManager.LoadScene("LoadingScreen");
                GenerateLoadingScene(levelData.RandomLoadTime, levelData);
                return;
            }

            GenerateLoadingScene(0f, levelData);
        }

        private static void GenerateLoadingScene(float loadingTime, LevelLoadData levelData)
        {
            _currentLoadingScene = new LoadingScene(levelData.LevelToLoad, loadingTime);
            _currentLoadingScene.SceneLoadedEvent += OnCompleted;
        }
    }
}


