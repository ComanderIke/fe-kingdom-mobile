using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Game.WorldMapStuff.Model;
using LostGrace;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class SceneController : MonoBehaviour
    {
        private static SceneController _instance;


       
        private float loadTime;
        public float MinLoadTime = 0f;

        private AsyncOperation resourceUnloadTask;
      //  private AsyncOperation sceneLoadTask;
        private SceneState sceneState;
        [SerializeField] private LoadingScreen LoadingScreen;
        [SerializeField] private Image progressBar = default;
         [SerializeField] private TextMeshProUGUI clickContinueText = default;
        [SerializeField] private TextMeshProUGUI progressText = default;
        [SerializeField] private TextMeshProUGUI tipsText = default;
        [SerializeField] private CanvasGroup alphaCanvas = default;
        public string[] Tips;
        private UpdateDelegate[] updateDelegates;
        public static event Action OnSceneReady;
        public static event Action OnSceneCompletelyFinished;
        public static event Action OnBeforeSceneReady;
        public static event Action FinishedUnloading;

        private List<SceneLoadData> scenesToLoad = new List<SceneLoadData>();

        class SceneLoadData
        {
            public Scenes scene;
            public bool additive;
            public string name;
            public bool unload;
            public AsyncOperation task;
            
        }
       
        public static void LoadSceneAsync(Scenes buildIndex, bool additive)
        {
            string pathToScene = SceneUtility.GetScenePathByBuildIndex((int)buildIndex);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(pathToScene);
            Debug.Log(sceneName);
            if (_instance != null)
            {
               
                //Debug.Log("Current Scene Name: "+_instance.currentSceneName);
                _instance.PrepareLoadScene();
                _instance.scenesToLoad.Add(new SceneLoadData(){name=sceneName, scene=buildIndex, additive = additive});
                Debug.Log("Load Scene: " + sceneName);
                
            }
        }
        public static void UnLoadSceneAsync(Scenes buildIndex)
        {
            Debug.Log("Try Unload Scene:" + buildIndex);
            string pathToScene = SceneUtility.GetScenePathByBuildIndex((int)buildIndex);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(pathToScene);
            Debug.Log(sceneName);
            Debug.Log(_instance);
            Debug.Log(SceneManager.GetSceneByName(sceneName).isLoaded);
            if (_instance != null&& SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                
                _instance.scenesToLoad.Add(new SceneLoadData(){name=sceneName, scene=buildIndex, unload = true});
                Debug.Log("Unload Scene: " + sceneName);
                _instance.sceneState = SceneState.Reset;
            }
        }

        void PrepareLoadScene()
        {
            LoadingScreen.Show();
            LoadingScreen.onBlack += StartLoadingProcess;
            
        }

        void StartLoadingProcess()
        {
            LoadingScreen.onBlack -= StartLoadingProcess;
            sceneState = SceneState.Reset;
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
                Debug.Log("==========================DESTROY SCENE CONTROLLer");
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
            updateDelegates[(int) SceneState.Run] = UpdateIdle;

            //nextSceneName = "MainMenu";
            sceneState = SceneState.Run;
        }

        private void Update()
        {
            if (updateDelegates[(int) sceneState] != null)
                updateDelegates[(int) sceneState]();
        }

        //attach the new scene controller to start cascade of loading
        private void UpdateSceneReset()
        {
            Debug.Log("Reset");
            GC.Collect();
           
            sceneState = SceneState.PreLoading;
        }

        // handle anything that needs to happen before loading
        private void UpdateScenePreload()
        {
            Debug.Log("Preload");
            foreach (var scene in scenesToLoad)
            {
                //Debug.Log("Scene in scenesToLoad: " +scene);
                if (scene.unload)
                {
                    //Debug.Log("UNLOAD " + scene);
                    scene.task=SceneManager.UnloadSceneAsync(scene.name);
                }
                else
                {
                    //Debug.Log("LOAD " + scene);
                    scene.task = SceneManager.LoadSceneAsync(scene.name,
                        scene.additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
                    scene.task.allowSceneActivation = false;
                   
                }
            }
            sceneState = SceneState.Load;
           
            OnBeforeSceneReady?.Invoke();
            progressBar.fillAmount = 0;
            progressText.text = "0%";
            loadTime = 0;
            clickContinueText.gameObject.SetActive(false);
            StartCoroutine(GenerateTip());

        }

       

        // show the loading screen until it's loaded
        private void UpdateSceneLoad()
        {
            //Debug.Log("UpdateSceneLoad");
            if (loadTime >= MinLoadTime)
            {
                foreach (var scene in scenesToLoad.Where(s=>s.task!=null))
                {
                    scene.task.allowSceneActivation = true;
                }
            }

            bool done = true;
            foreach (var scene in scenesToLoad.Where(s=>s.task!=null))
            {

                if (!scene.task.isDone)
                {
                    done = false;
                    //Debug.Log("Progress: "+scene.task.progress);
                }
                else
                {
                    if (!scene.unload)
                    {
                        var activeScene = SceneManager.GetSceneByBuildIndex((int)scene.scene);
                        if (activeScene.IsValid())
                            SceneManager.SetActiveScene(activeScene);
                    }
                }
            }

            if (done)
            {
              
                bool unload = true;
                foreach (var scene in scenesToLoad)
                {
                    if (!scene.unload)
                    {
                        unload = false; //If there is one normal loaded scene wait for loading screen click to continue!
                    }
                }

                clickContinueText.gameObject.SetActive(true);
                if (Input.touchCount > 0|| unload)
                {
                    sceneState = SceneState.Unload;
                    progressBar.fillAmount = 0;
                    progressText.text = "0%";
                    LoadingScreen.Hide();
                    OnSceneReady?.Invoke();
                }
            }
            else
            {
                
                float sumProgress = 0;
                foreach (var scene in scenesToLoad.Where(s=>s.task!=null))
                {
                    sumProgress+=scene.task.progress;
                }
                
                sumProgress /= scenesToLoad.Count(s=>s.task!=null);
              
                float progress = Mathf.Clamp01( sumProgress / .9f);//loadTime / MinLoadTime);
                progressBar.fillAmount = progress;
                progressText.text = ((int)(progress*100))+"%";
                //Debug.Log("Progress: "+progress);
                loadTime += Time.deltaTime;
            }
      
        }

        // clean up unused resources by unloading them
        private void UpdateSceneUnload()
        {
            Debug.Log("UpdateSceneUnLoad");
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
            Debug.Log("UpdateScenePostLoad");
            for (int i=scenesToLoad.Count-1; i >=0; i--)
            {
                if (scenesToLoad[i].task != null)
                {
                    scenesToLoad.Remove(scenesToLoad[i]);
                }
            }

            scenesToLoad.Clear();
            
            sceneState = SceneState.Ready;
        }

        // handle anything that needs to happen immediatly before running
        private void UpdateSceneReady()
        {
            Debug.Log("Ready");
            // run a GC Pass
            // if some assets loaded in the scene are currently unused
            // but may be used later DON'T do this here
            GC.Collect();
            sceneState = SceneState.Run;
            OnSceneCompletelyFinished?.Invoke();
           
        }

        // wait for scene change
        private void UpdateIdle()
        {
            //Debug.Log("UpdateIdle");
           // if ( scenesToLoad.Count!=0) sceneState = SceneState.Reset;
            
        }
        

        private void OnDestroy()
        {
            if (_instance == this)
            {
                if (updateDelegates != null)
                {
                    for (var i = 0; i < (int) SceneState.Count; i++) updateDelegates[i] = null;
                    updateDelegates = null;
                }
                Debug.Log("NULL SCENE CONTROLLER");
                _instance = null;
            }
        }

        private int tipCount;

        private IEnumerator GenerateTip()
        {
            tipCount = UnityEngine.Random.Range(0, Tips.Length);
            tipsText.text = Tips[tipCount];
            
            while (sceneState != SceneState.Ready)
            {
                yield return new WaitForSeconds(3f);
                
                LeanTween.alphaCanvas(alphaCanvas, 0, 0.5f);
                yield return new WaitForSeconds(0.5f);
                tipCount++;
                if (tipCount >= Tips.Length)
                    tipCount = 0;
                tipsText.text = Tips[tipCount];
                LeanTween.alphaCanvas(alphaCanvas, 1, 0.5f);
            }
            yield return null;

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


        public static void LoadScene(Scenes encounterArea, bool b)
        {
            SceneManager.LoadScene((int)encounterArea);
        }
    }
}