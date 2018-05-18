using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour {

    private static MainController mainController;

    private string currentSceneName;
    private string nextSceneName;
    private AsyncOperation resourceUnloadTask;
    private AsyncOperation sceneLoadTask;
    private enum SceneState { Reset, Preload, Load, Unload, Postload, Ready, Run, Count}
    private SceneState sceneState;
    private delegate void UpdateDelegate();
    private UpdateDelegate[] updateDelegates;

    public static void SwitchScene(string nextSceneName)
    {
        if (mainController != null)
        {
            if(mainController.currentSceneName != nextSceneName)
            {
                mainController.nextSceneName = nextSceneName;
            }
        }
    }

    private void Awake()
    {
        Object.DontDestroyOnLoad(gameObject);

        if (mainController == null)
            mainController = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        updateDelegates = new UpdateDelegate[(int)SceneState.Count];

        updateDelegates[(int)SceneState.Reset] = UpdateSceneReset;
        updateDelegates[(int)SceneState.Preload] = UpdateScenePreload;
        updateDelegates[(int)SceneState.Load] = UpdateSceneLoad;
        updateDelegates[(int)SceneState.Unload] = UpdateSceneUnload;
        updateDelegates[(int)SceneState.Postload] = UpdateScenePostload;
        updateDelegates[(int)SceneState.Ready] = UpdateSceneReady;
        updateDelegates[(int)SceneState.Run] = UpdateSceneRun;

        nextSceneName = "MainMenu";
        sceneState = SceneState.Reset;
    }

    private void Update()
    {
        if (updateDelegates[(int)sceneState] != null)
            updateDelegates[(int)sceneState]();
    }

    //attach the new scene controller to start cascade of loading
    void UpdateSceneReset()
    {
        System.GC.Collect();
        sceneState = SceneState.Preload;
    }

    // handle anything that needs to happen before loading
    void UpdateScenePreload()
    {
        sceneLoadTask = SceneManager.LoadSceneAsync(nextSceneName);
        sceneState = SceneState.Load;
    }

    // show the loading screen until it's loaded
    void UpdateSceneLoad()
    {
        if (sceneLoadTask.isDone)
        {
            sceneState = SceneState.Unload;
        }
        else
        {
            // Update Scene Loading Progress
            // LoadingScreen etc..
        }
    }

    // clean up unused resources by unloading them
    void UpdateSceneUnload()
    {
        if(resourceUnloadTask == null)
        {
            resourceUnloadTask = Resources.UnloadUnusedAssets();
        }
        else
        {
            if (resourceUnloadTask.isDone)
            {
                resourceUnloadTask = null;
                sceneState = SceneState.Postload;
            }
        }
    }

    // handle anything that needs to happen immediately after loading
    void UpdateScenePostload()
    {
        currentSceneName = nextSceneName;
        sceneState = SceneState.Ready;
    }

    // handle anything that needs to happen immediatly before running
    void UpdateSceneReady()
    {
        // run a GC Pass
        // if some assets loaded in the scene are currently unused
        // but may be used later DON'T do this here
        System.GC.Collect();
        sceneState = SceneState.Run;
    }

    // wait for scene change
    void UpdateSceneRun()
    {
        if(currentSceneName != nextSceneName)
        {
            sceneState = SceneState.Reset;
        }
    }

    private void OnDestroy()
    {
        if(updateDelegates != null)
        {
            for(int i=0; i < (int)SceneState.Count; i++)
            {
                updateDelegates[i] = null;
            }
            updateDelegates = null;
        }
        if (mainController != null)
            mainController = null;
    }
    
}
