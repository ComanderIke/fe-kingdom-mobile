using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Core
{
    public class MainController : MonoBehaviour
    {
        private static MainController _instance;

        private string currentSceneName;
        public GameObject LoadingScreen;
        private float loadTime;
        public float MinLoadTime = 5f;
        private string nextSceneName;
        private AsyncOperation resourceUnloadTask;
        private AsyncOperation sceneLoadTask;
        private SceneState sceneState;
        public Slider Slider;
        private UpdateDelegate[] updateDelegates;

        public static void SwitchScene(string nextSceneName)
        {
            if (_instance != null)
                if (_instance.currentSceneName != nextSceneName)
                    _instance.nextSceneName = nextSceneName;
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            updateDelegates = new UpdateDelegate[(int) SceneState.Count];

            updateDelegates[(int) SceneState.Reset] = UpdateSceneReset;
            updateDelegates[(int) SceneState.PreLoading] = UpdateScenePreload;
            updateDelegates[(int) SceneState.Load] = UpdateSceneLoad;
            updateDelegates[(int) SceneState.Unload] = UpdateSceneUnload;
            updateDelegates[(int) SceneState.PostLoading] = UpdateScenePostload;
            updateDelegates[(int) SceneState.Ready] = UpdateSceneReady;
            updateDelegates[(int) SceneState.Run] = UpdateSceneRun;

            nextSceneName = "MainMenu";
            sceneState = SceneState.Reset;
        }

        private void Update()
        {
            if (updateDelegates[(int) sceneState] != null)
                updateDelegates[(int) sceneState]();
        }

        //attach the new scene controller to start cascade of loading
        private void UpdateSceneReset()
        {
            GC.Collect();
            sceneState = SceneState.PreLoading;
        }

        // handle anything that needs to happen before loading
        private void UpdateScenePreload()
        {
            sceneLoadTask = SceneManager.LoadSceneAsync(nextSceneName);
            sceneState = SceneState.Load;
            LoadingScreen.SetActive(true);
            Slider.value = 0;
            loadTime = 0;
        }

        // show the loading screen until it's loaded
        private void UpdateSceneLoad()
        {
            if (sceneLoadTask.isDone && loadTime >= MinLoadTime)
            {
                sceneState = SceneState.Unload;
                Slider.value = 0;
                LoadingScreen.SetActive(false);
            }
            else
            {
                float progress = Mathf.Clamp01(loadTime / MinLoadTime); //sceneLoadTask.progress / .9f);
                Slider.value = progress;
                loadTime += Time.deltaTime;
                // Update Scene Loading Progress
                // LoadingScreen etc..
            }
        }

        // clean up unused resources by unloading them
        private void UpdateSceneUnload()
        {
            if (resourceUnloadTask == null)
            {
                resourceUnloadTask = Resources.UnloadUnusedAssets();
            }
            else
            {
                if (resourceUnloadTask.isDone)
                {
                    resourceUnloadTask = null;
                    sceneState = SceneState.PostLoading;
                }
            }
        }

        // handle anything that needs to happen immediately after loading
        private void UpdateScenePostload()
        {
            currentSceneName = nextSceneName;
            sceneState = SceneState.Ready;
        }

        // handle anything that needs to happen immediatly before running
        private void UpdateSceneReady()
        {
            // run a GC Pass
            // if some assets loaded in the scene are currently unused
            // but may be used later DON'T do this here
            GC.Collect();
            sceneState = SceneState.Run;
        }

        // wait for scene change
        private void UpdateSceneRun()
        {
            if (currentSceneName != nextSceneName) sceneState = SceneState.Reset;
        }

        private void OnDestroy()
        {
            if (updateDelegates != null)
            {
                for (var i = 0; i < (int) SceneState.Count; i++) updateDelegates[i] = null;
                updateDelegates = null;
            }

            if (_instance != null)
                _instance = null;
        }

        private enum SceneState
        {
            Reset,
            PreLoading,
            Load,
            Unload,
            PostLoading,
            Ready,
            Run,
            Count
        }

        private delegate void UpdateDelegate();
    }
}