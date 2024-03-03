using Game.SerializedData;
using GameEngine;
using UnityEngine;

namespace Game.EncounterAreas.Serialization
{
    public class SerializedActionSystem : IEngineSystem
    {
        private static SerializedActionSystem instance;
        public static SerializedActionSystem Instance => instance ??= new SerializedActionSystem();
        // private List<SerializedAction> actions = new List<SerializedAction>();
       
        public void Init()
        {
            
        }

        // public void Update()
        // {
        //     for (int i= actions.Count-1; i >=0; i--)
        //     {
        //         
        //         actions.Remove(actions[i]);
        //     }
        // }

        public void Add(SerializedAction action)
        {
           
            action.Save(SaveGameManager.currentSaveData);
        }

        public void Deactivate()
        {
            Debug.Log("TODO Save Data?");
        }

        public void Activate()
        {
            Debug.Log("TODO Load Data Here?");
        }
    }
}