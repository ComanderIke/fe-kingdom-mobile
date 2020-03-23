using System.IO;
using System.Net.Mime;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Core;
using Assets.GameActors.Players;
using UnityEngine;

namespace Assets.SerializedData
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

        public static void SaveGame(string filename)
        {
           
            var data = new GameData(Player.Instance, GameManager.Instance.GameProgress);
            SaveData(filename,data);
        }

        public static void LoadGame(string filename)
        {
            var data = LoadData(Application.persistentDataPath + "/saves/"+filename);
            Player.Instance.LoadPlayer(data.Player);
            GameManager.Instance.GameProgress = data.GameProgress;
        }
    }
}
