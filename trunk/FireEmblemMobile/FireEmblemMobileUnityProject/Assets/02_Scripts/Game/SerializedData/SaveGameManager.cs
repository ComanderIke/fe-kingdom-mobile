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
        private const string backupExtension = ".bak";
        public static SaveData currentSaveData;
        private static List<IDataPersistance> dataPersistanceObjects= new List<IDataPersistance>();


        public static void UnregisterDataPersistanceObject(IDataPersistance obj)
        {
            if (dataPersistanceObjects.Contains(obj))
            {
                Debug.LogError("Unregister Persitance Object: "+obj.ToString());
                dataPersistanceObjects.Remove(obj);
            }
        }

        public static void RegisterDataPersistanceObject(IDataPersistance obj)
        {
            if (!dataPersistanceObjects.Contains(obj))
            {
                Debug.LogError("Register Persitance Object: "+obj.ToString());
                dataPersistanceObjects.Add(obj);
            }
        }
    
        public static void NewGame(int slot, string label)
        {
            currentSaveData = new SaveData(slot, label);
            Debug.Log("New Game: "+slot+" "+label);
            Save(slot);
        }
        public static void Save()
        {
           // Debug.LogError("SAVING IS DISABLED");
            if(currentSaveData!=null)
                Save(currentSaveData.saveSlot);
            else
            {
                Debug.LogError("No Save Data found!");
            }
        }
        public static void Save(int slotNumber)
        {
            try
            {
                Debug.Log("Try Save!");
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
                Debug.Log("Try Save2!");
                currentSaveData.saveSlot = slotNumber;
                string path = Path.Combine(pathFolder, SaveFileName + slotNumber + ".fe");
                string backupPath = path + backupExtension;
                //Debug.Log(currentSaveData.encounterTreeData);
               // Debug.Log();
                var jsonData = JsonUtility.ToJson(currentSaveData, true);
                Debug.Log("Save JsonFile: "+jsonData);
                Debug.Log("Save Game: " + path);
                if (WriteToFile(path, jsonData))
                {
                    Debug.Log("Save Successfull!");
                }

                Load(slotNumber);
                Debug.Log("Until Here Save Successfull!");
                if (currentSaveData != null)
                {
                    Debug.Log("Copy File!");
                    File.Copy(path, backupPath, true);
                }
                else
                {
                    Debug.LogError("Save File could not be verified and backup could not be created");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("File could not be saved: Slot: "+slotNumber+"\n"+e);
            }
            // var stream = new FileStream(path, FileMode.Create);


            //formatter.Serialize(stream, data);
            //stream.Close();
        }

        private static bool AttemptRollback(string fullPath)
        {
            bool success = false;
            string backUpFilePath = fullPath + backupExtension;
            try
            {
                if (File.Exists(backUpFilePath))
                {
                    File.Copy(backUpFilePath, fullPath, true);
                    success = true;
                    Debug.LogWarning("had to roll back to backup file at: "+backUpFilePath);
                }
                else
                {
                    Debug.LogError("Tried to rollback but no backup file exists at: "+backupExtension+"\n");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to roll back to backup file at: "+backupExtension+"\n"+e);
            }
            
            return success;
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
        public static void Load(int slot, bool allowRollback=true)
        {
            // Debug.LogError("LOADING IS DISABLED");
            // return;
            string path = Path.Combine(Application.persistentDataPath+"/saves", SaveFileName+slot+".fe");
            try
            {


                if (File.Exists(path))
                {
                    // var formatter = new BinaryFormatter();
                    // var stream =  new FileStream(path, FileMode.Open);
                    // var data = formatter.Deserialize(stream); //as Saveata;
                    // stream.Close();

                    Debug.Log("Load Data from: " + path);
                    var json = "";
                    if (LoadFromFile(path, out json))
                    {
                        Debug.Log("Loading Successfull!");
                    }

                    currentSaveData = new SaveData(0, "tmp");
                    Debug.Log("JsonFile: "+json);
                    JsonUtility.FromJsonOverwrite(json, currentSaveData);
                    Debug.Log("FileSlotNameBer: " + currentSaveData.fileLabel);
                    if (currentSaveData == null)
                    {
                        throw new Exception("Data is null!");
                    }
                }
                else
                {
                    Debug.LogError("Save File not found in " + path);

                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("Failed to load data file. Attempting to roll back");
                if (allowRollback)
                {
                    bool rollbackSuccess = AttemptRollback(path);
                    if (rollbackSuccess)
                    {
                        Load(slot, false);
                    }
                }

            }


            // if (currentSaveData == null)
            // {
            //     Debug.LogError("No data was found. Initializing new game data.");
            //     NewGame(slot, "File "+slot);
            // }

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
            if (pauseStatus)
            {
                if(currentSaveData!=null)
                    Save(currentSaveData.saveSlot);
            }
        }

        public static bool FileSlotExists(int slot)
        {
            string folder = Application.persistentDataPath + "/saves/";
            Debug.Log("Check Exists: " + Path.Combine(folder, SaveFileName + slot + ".fe"));
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
