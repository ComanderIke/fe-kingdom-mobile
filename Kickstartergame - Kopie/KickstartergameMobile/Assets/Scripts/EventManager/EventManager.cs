using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


public class EventManager : MonoBehaviour
{
    /*
    private Dictionary<string, UnityEvent> eventDictionary;

    private Dictionary<string, int> intParams;
    private Dictionary<string, string> stringParams;

    private static EventManager eventManager;

    public static EventManager instance
    {
        
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
            intParams = new Dictionary<string, int>();
            stringParams = new Dictionary<string, string>();
        }

        //Register Audio Events
        EventManager.StartListening("GameStart", gameStart);
        EventManager.StartListening("LevelStart", levelStart);
        EventManager.StartListening("inventoryItemUsed", inventoryItemUsed);
        EventManager.StartListening("testEvent", testEvent);

    }

    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        if (eventManager == null) return;
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
            instance.intParams.Clear();
            instance.stringParams.Clear();
        }
    }

    public static void PassInt(string ID, int param)
    {
		if (!instance.intParams.ContainsKey (ID))
			instance.intParams.Add (ID, param);
		else
			instance.intParams [ID] = param;
    }

    public static void PassString(string ID, string param)
    {
		if(!instance.stringParams.ContainsKey(ID))
        	instance.stringParams.Add(ID, param);
		else
			instance.stringParams [ID] = param;
    }


    //Audio Callbacks
    void gameStart()
    {

    }

    void levelStart()
    {

    }

    void inventoryItemUsed()
    {
        foreach (KeyValuePair<string, string> pair in stringParams)
        {
            Debug.Log(pair.Key.ToString() + "  -  " + pair.Value.ToString());
        }

        foreach (KeyValuePair<string, int> pair in intParams)
        {
            Debug.Log(pair.Key.ToString() + "  -  " + pair.Value.ToString());
        }
    }


    void testEvent()
    {
        foreach (KeyValuePair<string, string> pair in stringParams)
        {
            Debug.Log(pair.Key.ToString() + "  -  " + pair.Value.ToString());
        }

        foreach (KeyValuePair<string, int> pair in intParams)
        {
            Debug.Log(pair.Key.ToString() + "  -  " + pair.Value.ToString());
        }
    }
    */
}
