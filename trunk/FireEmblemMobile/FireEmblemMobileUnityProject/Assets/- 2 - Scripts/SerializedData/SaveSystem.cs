using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SerializedData
{
    public class SaveSystem
    {

        private static void SaveData(string filename, GameData data)
        {
            var formatter = new BinaryFormatter();
            if (!Directory.Exists(Application.persistentDataPath + "/saves"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/saves");
            }
            
            string path = Application.persistentDataPath+ "/saves/" + filename+".fe";
            var stream = new FileStream(path, FileMode.Create);
            Debug.Log("Save Game: " + path);
            formatter.Serialize(stream, data);
            stream.Close();

        }

        private static GameData LoadData(string path)
        {
            if (File.Exists(path))
            {
                var formatter = new BinaryFormatter();
                var stream =  new FileStream(path, FileMode.Open);
                var data = formatter.Deserialize(stream) as GameData;
                stream.Close();
                Debug.Log("Loaded Data from: "+path);
                return data;
            }
            else
            {
                Debug.LogError("Save File not found in " +path);
                return null;
            }
        }

        public static void SaveGame(string filename, PlayerData playerData, GameProgress gameProgress)
        {
           
            var data = new GameData(playerData, gameProgress);
            SaveData(filename,data);
        }

        public static GameData LoadGame(string filename)
        {
            return LoadData(Application.persistentDataPath + "/saves/" + filename);
        }
    }
}
