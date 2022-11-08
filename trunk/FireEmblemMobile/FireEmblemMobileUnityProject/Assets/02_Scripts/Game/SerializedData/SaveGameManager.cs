using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using Game.Systems;
using UnityEngine;
using UnityEngine.Rendering;

namespace LostGrace
{
    public interface IDataPersistance
    {
        void LoadData(SaveData data);
        void SaveData(ref SaveData data);
    }
    public class SaveGameManager : MonoBehaviour
    {
        public const int SaveFileCount = 3;
        private const string SaveFileName = "save";
        public static SaveData currentSaveData;
        private static List<IDataPersistance> dataPersistanceObjects= new List<IDataPersistance>();




        public static void RegisterDataPersistanceObject(IDataPersistance obj)
        {
            if (!dataPersistanceObjects.Contains(obj))
            {
                dataPersistanceObjects.Add(obj);
            }
        }
    
        public static void NewGame(int slot, string label)
        {
            currentSaveData = new SaveData(slot, label);
            Save(slot);
        }
        public static void Save()
        {
            if(currentSaveData!=null)
                Save(currentSaveData.saveSlot);
            else
            {
                Debug.LogError("No Save Data found!");
            }
        }
        public static void Save(int slotNumber)
        {
            
            foreach (IDataPersistance dataPersistance in dataPersistanceObjects)
            {
                dataPersistance.SaveData(ref currentSaveData);
            }
            
            var formatter = new BinaryFormatter();
            string pathFolder = Application.persistentDataPath + "/saves/";
            if (!Directory.Exists(pathFolder))
            {
                Directory.CreateDirectory(pathFolder);
            }

            currentSaveData.saveSlot = slotNumber;
            string path =  Path.Combine(pathFolder , SaveFileName+slotNumber+".fe");
            var jsonData=JsonUtility.ToJson(currentSaveData, true);
            Debug.Log("Save Game: " + path);
            if (WriteToFile(path, jsonData))
            {
                Debug.Log("Save Successfull!");
            }
            // var stream = new FileStream(path, FileMode.Create);
          
            
            //formatter.Serialize(stream, data);
            //stream.Close();
        }
        private static bool WriteToFile(string path, string data)
        {
            try
            {
                File.WriteAllText(path, data);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to write to File: {path} with exception: "+e.Message);
            }

            return false;
        }
        private static bool LoadFromFile(string path, out string result)
        {
            try
            {
                result= File.ReadAllText(path);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to read from File: {path} with exception: "+e.Message);
                result = "";
            }

            return false;
        }
        public static void Load(int slot)
        {
            string path = Path.Combine(Application.persistentDataPath+"/saves", SaveFileName+slot+".fe");
            if (File.Exists(path))
            {
                // var formatter = new BinaryFormatter();
                // var stream =  new FileStream(path, FileMode.Open);
                // var data = formatter.Deserialize(stream); //as Saveata;
                // stream.Close();
               
                Debug.Log("Load Data from: "+path);
                var json = "";
                if (LoadFromFile(path, out json))
                {
                    Debug.Log("Loading Successfull!");
                }

                currentSaveData = new SaveData(0,"tmp");
                JsonUtility.FromJsonOverwrite(json, currentSaveData);
                Debug.Log("FileSlotNameBer: "+currentSaveData.fileLabel);
                
            }
            else
            {
                Debug.LogError("Save File not found in " +path);
                
            }

            if (currentSaveData == null)
            {
                Debug.LogError("No data was found. Initializing new game data.");
                NewGame(slot, "File "+slot);
            }

            foreach (IDataPersistance dataPersistance in dataPersistanceObjects)
            {
                dataPersistance.LoadData(currentSaveData);
            }
        }
        
        public static string[] GetLoadFiles()
        {
            string folder = Application.persistentDataPath + "/saves/";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            
            return Directory.GetFiles(folder);
        }
        private void OnApplicationQuit()
        {
            Debug.Log("ApplicationQuit");
            if (currentSaveData != null)
            {
                Debug.Log("Saving...");
                Save(currentSaveData.saveSlot);
            }
        }

        private void OnApplicationPause(bool pauseStatus)//Android no guarantue to call OnApplicationQuit
        {
            Debug.Log("ApplicationPause"+pauseStatus);
            if (pauseStatus)
            {
                if(currentSaveData!=null)
                    Save(currentSaveData.saveSlot);
            }
        }

        public static bool FileSlotExists(int slot)
        {
            string folder = Application.persistentDataPath + "/saves/";
            Debug.Log("Check Exists: "+File.Exists(Path.Combine(folder, SaveFileName + slot + ".fe")));
            return File.Exists(Path.Combine(folder, SaveFileName + slot + ".fe"));
        }

        public static string GetFileSlotName(int slot)
        {
            Load(slot);
            Debug.Log("FileSlotName: "+currentSaveData.fileLabel);
            return currentSaveData.fileLabel;
        }
    }
}
