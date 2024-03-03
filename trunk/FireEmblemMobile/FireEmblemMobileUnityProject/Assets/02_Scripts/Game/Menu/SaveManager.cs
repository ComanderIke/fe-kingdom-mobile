using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Menu
{
    public class SaveManager
    {
        private string [] saveFiles;
        [SerializeField] private GameObject loadButtonPrefab = default;
        [SerializeField] private GameObject loadArea = default;

        public void GetLoadFiles()
        {
            if (!Directory.Exists(Application.persistentDataPath + "/saves/"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/saves/");
            }

            saveFiles = Directory.GetFiles(Application.persistentDataPath + "/saves/");
            
        }

        public void ShowLoadScreen()
        {
            GetLoadFiles();
            foreach (var button in loadArea.GetComponentsInChildren<Button>())
            {
                Object.Destroy(button.gameObject);
            }
            foreach (string file in saveFiles)
            {
                var buttonObject = Object.Instantiate(loadButtonPrefab);
                buttonObject.transform.SetParent(loadArea.transform, false);
                buttonObject.GetComponent<Button>().onClick.AddListener(()=>{});
                buttonObject.GetComponentInChildren<TextMeshProUGUI>().text =
                    file.Replace(Application.persistentDataPath + "/saves/", "");
            }
        }
    }
}
