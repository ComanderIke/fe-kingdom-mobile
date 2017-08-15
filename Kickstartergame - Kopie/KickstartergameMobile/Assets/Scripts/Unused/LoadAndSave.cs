using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Characters.Classes;

public class LoadAndSave : MonoBehaviour {

    public static SaveObject character;
	// Use this for initialization
	void Start () {
        Player p = new Player(1, Color.blue, "yolo", true);
        Character c = new Character("super", CharacterClassType.Archer);
        p.addCharacter(c);
        character = new SaveObject(p, 1);
	}
    float time = 0.0f;
    // Update is called once per frame
    bool save = false;
    bool load = false;
	void Update () {
        time += Time.deltaTime;
        if (time > 3 &&save&&!load)
        {
            Load();
            load = true;
            Debug.Log(character.player.characters[0].name);
        }
        else if (time > 3&&!load)
        {
            time = 0;
            save = true;
            Save();
            Debug.Log(character.player.characters[0].name);
            character.player.name = "YOLO";
        }
        
    }

    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        bf.Serialize(file, character);
        file.Close();
    }
    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            character = (SaveObject)bf.Deserialize(file);
            file.Close();
        }
    }
}
