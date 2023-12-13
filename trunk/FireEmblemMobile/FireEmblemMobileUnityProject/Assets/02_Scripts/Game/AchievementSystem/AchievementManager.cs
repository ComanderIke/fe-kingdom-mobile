using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Game.GameActors.Players;
using LostGrace.AchievementSystem;

/// <summary>
/// Controls interactions with the Achievement System
/// </summary>
[System.Serializable]
public class AchievementManager : MonoBehaviour
{
    [Tooltip("The number of seconds an achievement will stay on the screen after being unlocked or progress is made.")]
    public float DisplayTime = 3;
    [Tooltip("The total number of achievements which can be on the screen at any one time.")]
    public int NumberOnScreen = 3;
    [Tooltip("If true, progress notifications will display their exact progress. If false it will show the closest bracket.")]
    public bool ShowExactProgress = false;
    [Tooltip("If true, achievement unlocks/progress update notifications will be displayed on the player's screen.")]
    public bool DisplayAchievements;
    [Tooltip("The location on the screen where achievement notifications should be displayed.")]
    public AchievementStackLocation StackLocation;
    [Tooltip("If true, the state of all achievements will be saved without any call to the manual save function (Recommended = true)")]
    public bool AutoSave;
    [Tooltip("The message which will be displayed on the UI if an achievement is marked as a spoiler.")]
    public string SpoilerAchievementMessage = "Hidden";
    [Tooltip("The sound which plays when an achievement is unlocked is displayed to a user. Sounds are only played when Display Achievements is true.")]
    public AudioClip AchievedSound;
   
    private AudioSource AudioSource;
   
    [SerializeField] public List<AchievementState> States = new List<AchievementState>();                       //List of achievement states (achieved, progress and last notification)
    [SerializeField] public List<AchievementInfromation> AchievementList = new List<AchievementInfromation>();  //List of all available achievements


    public static AchievementManager instance = null; //Singleton Instance
    public AchievenmentStack Stack;
    private AchievementEventsManager eventManager;
    void Awake()
    {
       if (instance == null)
       {
            instance = this;
            eventManager = new AchievementEventsManager(this);
            eventManager.InitEvents();
       }
       else if (instance != this)
       {
            Destroy(gameObject);
       }
       Debug.Log("SET DONT DESTROY ON LOAD");
       DontDestroyOnLoad(gameObject);
       AudioSource = gameObject.GetComponent<AudioSource>();
       Stack = GetComponentInChildren<AchievenmentStack>();
       LoadAchievementState();
    }

    private void OnDestroy()
    {
        eventManager?.Cleanup();
    }

    private void PlaySound (AudioClip Sound)
    {
        if(AudioSource != null)
        {
            AudioSource.clip = Sound;
            AudioSource.Play();
        }
    }
    # region Miscellaneous
    /// <summary>
    /// Does an achievement exist in the list
    /// </summary>
    /// <param name="Key">The Key of the achievement to test</param>
    /// <returns>true : if exists. false : does not exist</returns>
    public bool AchievementExists(string Key)
    {
        return AchievementExists(AchievementList.FindIndex(x => x.Key.Equals(Key)));
    }
    /// <summary>
    /// Does an achievement exist in the list
    /// </summary>
    /// <param name="Index">The index of the achievement to test</param>
    /// <returns>true : if exists. false : does not exist</returns>
    public bool AchievementExists(int Index)
    {
        return Index <= AchievementList.Count && Index >= 0;
    }
    /// <summary>
    /// Returns the total number of achievements which have been unlocked.
    /// </summary>
    public int GetAchievedCount()
    {
        int Count = (from AchievementState i in States
                    where i.Achieved == true
                    select i).Count();
        return Count;
    }
    /// <summary>
    /// Returns the current percentage of unlocked achievements.
    /// </summary>
    public float GetAchievedPercentage()
    {
        if(States.Count == 0)
        {
            return 0;
        }
        return (float)GetAchievedCount() / States.Count * 100;
    }
    #endregion

    #region Unlock and Progress
    /// <summary>
    /// Fully unlocks a progression or goal achievement.
    /// </summary>
    /// <param name="Key">The Key of the achievement to be unlocked</param>
    public void Unlock(string Key)
    {
        Unlock(FindAchievementIndex(Key));
    }
    /// <summary>
    /// Fully unlocks a progression or goal achievement.
    /// </summary>
    /// <param name="Index">The index of the achievement to be unlocked</param>
    public void Unlock(int Index)
    {
        if (!States[Index].Achieved)
        {
            States[Index].Progress = AchievementList[Index].ProgressGoal;
            States[Index].Achieved = true;
            if (AchievementList[Index].Chain &&AchievementList[Index].UnlockedAfter != null)
            {
                int unlockedIndex=FindAchievementIndex(AchievementList[Index].UnlockedAfter);
                AchievementList[unlockedIndex].Spoiler = false;
            }
            onAchievementUnlocked?.Invoke();
            DisplayUnlock(Index);
            AutoSaveStates();
        }
    }
    /// <summary>
    /// Set the progress of an achievement to a specific value. 
    /// </summary>
    /// <param name="Key">The Key of the achievement</param>
    /// <param name="Progress">Set progress to this value</param>
    public void SetAchievementProgress(string Key, float Progress)
    {
        SetAchievementProgress(FindAchievementIndex(Key), Progress);
    }
    /// <summary>
    /// Set the progress of an achievement to a specific value. 
    /// </summary>
    /// <param name="Index">The index of the achievement</param>
    /// <param name="Progress">Set progress to this value</param>
    public void SetAchievementProgress(int Index, float Progress)
    {
        if(AchievementList[Index].Progression)
        {
            if (States[Index].Progress >= AchievementList[Index].ProgressGoal)
            {
                Unlock(Index);
            }
            else
            {
                States[Index].Progress = Progress;
                DisplayUnlock(Index);
                AutoSaveStates();                
            }
        }
    }
    /// <summary>
    /// Adds the input amount of progress to an achievement. Clamps achievement progress to its max value.
    /// </summary>
    /// <param name="Key">The Key of the achievement</param>
    /// <param name="Progress">Add this number to progress</param>
    public void AddAchievementProgress(string Key, float Progress)
    {
        AddAchievementProgress(FindAchievementIndex(Key), Progress);
    }
    /// <summary>
    /// Adds the input amount of progress to an achievement. Clamps achievement progress to its max value.
    /// </summary>
    /// <param name="Index">The index of the achievement</param>
    /// <param name="Progress">Add this number to progress</param>
    public void AddAchievementProgress(int Index, float Progress)
    {
        while (AchievementList[Index].Chain && (States[Index].Achieved || States[Index].Claimed))
        {
            Debug.Log("UNIT DIED 3" + Index);
            Index = FindAchievementIndex(AchievementList[Index].UnlockedAfter);
        }
        if (AchievementList[Index].Progression)
        {
            if (States[Index].Progress + Progress >= AchievementList[Index].ProgressGoal)
            {
                Unlock(Index);
            }
            else
            {
                States[Index].Progress += Progress;
                DisplayUnlock(Index);
                AutoSaveStates();
            }
        }
    }
    #endregion

    #region Saving and Loading
    /// <summary>
    /// Saves progress and achieved states to player prefs. Used to allow reload of data between game loads. This function is automatically called if the Auto Save setting is set to true.
    /// </summary>
    public void SaveAchievementState()
    {
        for (int i = 0; i < States.Count; i++)
        {
            PlayerPrefs.SetString("AchievementState_" + i, JsonUtility.ToJson(States[i]));
        }
        PlayerPrefs.Save();
    }
    /// <summary>
    /// Loads all progress and achievement states from player prefs. This function is automatically called if the Auto Load setting is set to true.
    /// </summary>
    public void LoadAchievementState()
    {
        AchievementState NewState;
        States.Clear();

        for (int i = 0; i < AchievementList.Count; i++)
        {
            //Ensure that new project get default values
            if (PlayerPrefs.HasKey("AchievementState_" + i))
            {
                NewState = JsonUtility.FromJson<AchievementState>(PlayerPrefs.GetString("AchievementState_" + i));
                States.Add(NewState);
            }
            else { States.Add(new AchievementState()); }
            
        }
    }
    /// <summary>
    /// Clears all saved progress and achieved states.
    /// </summary>
    public void ResetAchievementState()
    {
        States.Clear();
        for (int i = 0; i < AchievementList.Count; i++)
        {
            PlayerPrefs.DeleteKey("AchievementState_" + i);
            States.Add(new AchievementState());
        }
        SaveAchievementState();
    }
    #endregion

    /// <summary>
    /// Find the index of an achievement with a cetain key
    /// </summary>
    /// <param name="Key">Key of achievevment</param>
    private int FindAchievementIndex(string Key)
    {
        return AchievementList.FindIndex(x => x.Key.Equals(Key));
    }
    /// <summary>
    /// Test if AutoSave is valid. If true, save list
    /// </summary>
    private void AutoSaveStates()
    {
        if (AutoSave)
        {
            SaveAchievementState();
        }
    }
    /// <summary>
    /// Display achievements progress to screen  
    /// </summary>
    /// <param name="Index">Index of achievement to display</param>
    private void DisplayUnlock(int Index)
    {
        if (DisplayAchievements && !AchievementList[Index].Spoiler || States[Index].Achieved)
        {
            //If not achieved
            // if (AchievementList[Index].Progression && States[Index].Progress < AchievementList[Index].ProgressGoal)
            // {
            //     int Steps = (int)AchievementList[Index].ProgressGoal / (int)AchievementList[Index].NotificationFrequency;
            //
            //     //Loop through all notification point backwards from last possible option
            //     for (int i = Steps; i > States[Index].LastProgressUpdate; i--)
            //     {
            //        //When it finds the largest valid notification point
            //        if (States[Index].Progress >= AchievementList[Index].NotificationFrequency * i)
            //        {
            //             PlaySound(ProgressMadeSound);
            //             States[Index].LastProgressUpdate = i;
            //             Stack.ScheduleAchievementDisplay(Index);
            //             return;
            //        }
            //     }
            // }
            // else
            // {
                PlaySound(AchievedSound);
                Stack.ScheduleAchievementDisplay(Index);
            // }
        }
    }

    public event Action onAchievementUnlocked;

    public void Claim(string key)
    {
        int index = FindAchievementIndex(key);
        States[index].Claimed = true;
        int amount = int.Parse(AchievementList[index].RewardAmount);
        switch (AchievementList[index].RewardType)
        {
            case RewardType.Grace:
                Player.Instance.Grace += amount; break;
        }
        onStateChanged?.Invoke();
        SaveAchievementState();
    }

    public event Action onStateChanged;
}