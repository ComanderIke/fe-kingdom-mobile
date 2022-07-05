using System;
using System.IO;
using System.Linq;
using Game.GameActors.Players;
using Game.GameResources;
using Game.Manager;
using Game.Systems;
using Game.WorldMapStuff.Manager;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.UI;
using Menu;
using SerializedData;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.GUI
{
    public class MainMenuController : MonoBehaviour
    {
        public static MainMenuController Instance;
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
        [SerializeField] private UIMenu upgradeMenu;
        [SerializeField] private Canvas mainMenuCanvas;
        [SerializeField] private Canvas mainCanvas;
      
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                ShowMainMenu();
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        public void ShowMainMenu()
        {
            
            Debug.Log("Continue Button: "+ContinueButton.name);
            ContinueButton.SetActive(SaveData.currentSaveData!=null);
            SaveButton.SetActive(SaveData.currentSaveData!=null);
            LoadButton.SetActive(GetLoadFiles()!=0);
            mainMenuCanvas.enabled=true;
            optionsMenu.Hide();
            campaignMenu.Hide();
            menuCam.gameObject.SetActive(true);
            menuBackground.SetActive((true));
        }

        public Camera menuCam;
        public GameObject menuBackground;
        public void HideMenu()
        {
            mainMenuCanvas.enabled = false;
            optionsMenu.Hide();
            campaignMenu.Hide();
            menuCam.gameObject.SetActive(false);
            menuBackground.SetActive((false));
        }
     
        

       
        public void LoadCampaignScene(Campaign campaign)
        {
            HideMenu();
            SaveData.Reset();
            SceneController.LoadSceneAsync(campaign.scene, true);
            // SceneController.LoadSceneAsync(Scenes.UI, true);
            // SceneController.LoadSceneAsync(Scenes.WM_Gameplay, true);
        }

        public void LoadGame(string name)
        {
            SaveData.currentSaveData= SaveSystem.LoadGame(name);

            CampaignConfig first = null;
            foreach (var c in GameData.Instance.campaigns)
            {
                if (c.campaignId == SaveData.currentSaveData.campaignData.campaignId)
                {
                    first = c;
                    break;
                }
            }

            Player.Instance.LoadData(SaveData.currentSaveData.playerData);
            Campaign.Instance.LoadData(SaveData.currentSaveData.campaignData);
            
            LoadCampaignScene(Campaign.Instance);

            LeanTween.scale(loadDialog, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInBack)
                .setOnComplete(HideLoadDialog);
        }
        
        #region LoadSaveDialog
        public void OpenSaveDialog()
        {
            saveDialog.gameObject.SetActive(true);
            LeanTween.scale(saveDialog, Vector3.one, 0.3f).setEase(LeanTweenType.easeOutBack);
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
        #endregion
        
        #region Clicks

        public void LoadCampaignClicked(int campaignIndex)
        {
            Campaign.Instance.LoadConfig(GameData.Instance.campaigns[campaignIndex]);
            LoadCampaignScene(Campaign.Instance);
        }

        public void OptionsClicked()
        {
            optionsMenu.Show();
        }
        public void NewGameClicked()
        {
            campaignMenu.Show();
            mainMenuCanvas.enabled = false;
            //SceneController.SwitchScene("Base");
        }
        public void UpgradeClicked()
        {
            upgradeMenu.Show();
        }

        public void ExitClicked()
        {
            Application.Quit();
        }
        public void SaveGameClicked()
        {
            SaveSystem.SaveGame(saveNameField.text.Trim(), new SaveData(Player.Instance, Campaign.Instance,EncounterTree.Instance));
            LeanTween.scale(saveDialog, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInBack)
                .setOnComplete(HideSaveDialog);
        }
        public void ContinueClicked()
        {
            HideMenu();
            //LoadScene(Scenes.Campaign1);
            //LoadGame(lastestSaveFile);
        }
        #endregion
        
    }
}
