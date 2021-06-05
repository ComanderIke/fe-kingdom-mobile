using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Game.Systems;
using UnityEngine;

namespace SerializedData
{
    public class SaveSystem
    {

        private static void SaveData(string filename, SaveData data)
        {
            var formatter = new BinaryFormatter();
            if (!Directory.Exists(Application.persistentDataPath + "/saves"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/saves");
            }
            
            string path = Application.persistentDataPath+ "/saves/" + filename+".fe";
            var jsonData=JsonUtility.ToJson(data);
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
                Debug.LogError("Failed to write to File: {path} with exception: {e}");
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
                Debug.LogError("Failed to read from File: {path} with exception: {e}");
                result = "";
            }

            return false;
        }

        private static object LoadData(string path)
        {
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
                SaveData data = new SaveData();
                JsonUtility.FromJsonOverwrite(json, data);
                return data;
            }
            else
            {
                Debug.LogError("Save File not found in " +path);
                return null;
            }
        }

        public static void SaveGame(string filename, object saveData)
        {
            
            SaveData(filename,saveData);
        }

        public static object LoadGame(string filename)
        {
            return LoadData(Application.persistentDataPath + "/saves/" + filename);
        }
    }
}
