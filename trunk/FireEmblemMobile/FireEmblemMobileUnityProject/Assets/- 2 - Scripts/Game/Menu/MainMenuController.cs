using System;
using System.IO;
using Game.GameActors.Players;
using Game.Manager;
using Game.Systems;
using Game.WorldMapStuff.Manager;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.UI;
using Menu;
using SerializedData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private TMP_InputField saveNameField = default;
        [SerializeField] private GameObject saveDialog = default;
        [SerializeField] private GameObject loadDialog = default;
        [SerializeField] private GameObject loadArea = default;
        [SerializeField] private GameObject loadFilePrefab = default;
        private string[] saveFiles;
        private string lastestSaveFile;
        public GameObject ContinueButton;
        public GameObject LoadButton;
        public GameObject SaveButton;
        [SerializeField] private UIMenu optionsMenu;
        [SerializeField] private UIMenu campaignMenu;


        private void Start()
        {
            ContinueButton.SetActive(GameManager.Instance.SessionManager.WorldMapLoaded);
            SaveButton.SetActive(GameManager.Instance.SessionManager.WorldMapLoaded);
            LoadButton.SetActive(GetLoadFiles()!=0);
        }

        public void NewGameClicked()
        {
            campaignMenu.Show();
            //SceneController.SwitchScene("Base");
        }
        public void OpenSaveDialog()
        {
            saveDialog.gameObject.SetActive(true);
            LeanTween.scale(saveDialog, Vector3.one, 0.3f).setEase(LeanTweenType.easeOutBack);
        }
        public void ContineClicked()
        {
            SceneController.SwitchScene(Scenes.WorldMap);
            //LoadGame(lastestSaveFile);
        }
        public void OptionsClicked()
        {
            Debug.Log("TODO Open OptionsMenu");
            optionsMenu.Show();
        }

        public void SaveGameClicked()
        {
            SaveSystem.SaveGame(saveNameField.text.Trim(), new SaveData(Player.Instance, Campaign.Instance));
            LeanTween.scale(saveDialog, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInBack)
                .setOnComplete(HideSaveDialog);
        }
        public void LoadGame(string name)
        {
            SaveData.currentSaveData= SaveSystem.LoadGame(name);
            

         
            SceneController.SwitchScene(Scenes.WorldMap);

            LeanTween.scale(loadDialog, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInBack)
                .setOnComplete(HideLoadDialog);
        }

        void HideSaveDialog()
        {
            saveDialog.SetActive(false);
        }
        void HideLoadDialog()
        {
            loadDialog.SetActive(false);
        }
        public void OpenLoadDialog()
        {
            loadDialog.gameObject.SetActive(true);
            ShowLoadScreen();
            LeanTween.scale(loadDialog, Vector3.one, 0.3f).setEase(LeanTweenType.easeOutBack);

        }
        public int GetLoadFiles()
        {
            if (!Directory.Exists(Application.persistentDataPath + "/saves/"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/saves/");
            }

            saveFiles = Directory.GetFiles(Application.persistentDataPath + "/saves/");
            return saveFiles.Length;
        }

        public void ShowLoadScreen()
        {
            GetLoadFiles();
            foreach (var button in loadArea.GetComponentsInChildren<Button>())
            {
                Destroy(button.gameObject);
            }
            foreach (string file in saveFiles)
            {
                var buttonObject = Instantiate(loadFilePrefab);
                string fileName = file.Replace(Application.persistentDataPath + "/saves/", "");
                buttonObject.transform.SetParent(loadArea.transform, false);
                buttonObject.GetComponent<Button>().onClick.AddListener(() => { LoadGame(fileName);});
                buttonObject.GetComponentInChildren<TextMeshProUGUI>().text = fileName;
            }
        }
        public void ExitClicked()
        {
            Application.Quit();
        }
    }
}
