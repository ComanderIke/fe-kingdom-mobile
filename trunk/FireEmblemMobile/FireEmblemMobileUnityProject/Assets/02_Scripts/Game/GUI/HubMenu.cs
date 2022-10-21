using System.Collections;
using System.Collections.Generic;
using System.IO;
using Game.Dialog;
using Game.GameActors.Players;
using Game.GameResources;
using Game.Menu;
using Game.Systems;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.UI;
using Menu;
using SerializedData;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LostGrace
{
    public class HubMenu : UIMenu
    {
        [SerializeField] private UIMenu characterSelectMenu;
        [SerializeField] private UIMenu upgradeMenu;
        [Header("SaveLoadStuff: ")]
        public GameObject ContinueButton;
        [SerializeField] private TMP_InputField saveNameField = default;
        [SerializeField] private GameObject saveDialog = default;
        [SerializeField] private GameObject loadDialog = default;
        [SerializeField] private GameObject loadArea = default;
        [SerializeField] private GameObject loadFilePrefab = default;
        private string[] saveFiles;
        private string lastestSaveFile;
        public GameObject LoadButton;
        public GameObject SaveButton;
       [Header("Canvas Groups: ")]
        [SerializeField] private CanvasGroup titleCanvasGroup;
        [SerializeField] private CanvasGroup newGameButtonCanvasGroup;
        [SerializeField] private CanvasGroup tutorialButtonCanvasGroup;
        [SerializeField] private CanvasGroup upgradeButtonCanvasGroup;
        [SerializeField] private CanvasGroup backButtonCanvasGroup;
        [SerializeField] private CanvasGroup Fade;
        [Header("Buttons: ")]
        [SerializeField] private Button newCampaignButton;
        [SerializeField] private Button tutorialButton;
        [SerializeField] private Button upgradesButton;
        [SerializeField] private Button backButton;
        [Header("Experimental: ")]
        [SerializeField] private GoddessUI goddessUI;
        [SerializeField] private DialogueManager dialogueManager;
        [SerializeField] private Conversation introConversation;
        public override void Show()
        {
            //ContinueButton.SetActive(SaveData.currentSaveData!=null);
            //SaveButton.SetActive(SaveData.currentSaveData!=null);
            //LoadButton.SetActive(GetLoadFiles()!=0);
            StartCoroutine(ShowCoroutine());
            newCampaignButton.interactable = false;
            upgradesButton.interactable = false;
            backButton.interactable = false;
            tutorialButton.interactable = false;

        }

        IEnumerator ShowCoroutine()
        {
            base.Show();
            yield return new WaitForSeconds(.5f);
            TweenUtility.FadeIn(titleCanvasGroup);
            TweenUtility.FadeIn(newGameButtonCanvasGroup);
            TweenUtility.FadeIn(upgradeButtonCanvasGroup);
            TweenUtility.FadeIn(backButtonCanvasGroup);
            TweenUtility.FadeIn(tutorialButtonCanvasGroup);
            yield return new WaitForSeconds(.6f);
            goddessUI.Show();
            yield return new WaitForSeconds(4.5f);
            dialogueManager.ShowDialog(introConversation);
            dialogueManager.dialogEnd += IntroFinished;

        }

        void IntroFinished()
        {
            goddessUI.Hide();
            tutorialButton.interactable = true;
            newCampaignButton.interactable = false;
            upgradesButton.interactable = false;
            backButton.interactable = false;
        }
        IEnumerator HideCoroutine()
        {
           
            TweenUtility.FadeOut(upgradeButtonCanvasGroup);
            TweenUtility.FadeOut(newGameButtonCanvasGroup);
            TweenUtility.FadeOut(tutorialButtonCanvasGroup);
           
            yield return new WaitForSeconds(.4f);
            TweenUtility.FadeOut(titleCanvasGroup);
            TweenUtility.FadeOut(backButtonCanvasGroup);

            TweenUtility.FadeIn(Fade).setOnComplete(()=>
            {
                base.Hide();
                parent?.Show();
                TweenUtility.FadeOut(Fade);
            });
        }
        public override void Hide()
        {
            StartCoroutine(HideCoroutine());
        }
        public override void BackClicked()
        {
            Hide();
            
        }
        public void ContinueClicked()
        {
            Hide();
            //LoadScene(Scenes.Campaign1);
            //LoadGame(lastestSaveFile);
        }
        public void NewGameClicked()
        {
            StartCoroutine(CharacterSelectCoroutine());
           
        }
        public void TutorialClicked()
        {
            SceneController.LoadSceneAsync(Scenes.Battle1, false);
        }
        IEnumerator CharacterSelectCoroutine()
        {
            TweenUtility.FadeOut(upgradeButtonCanvasGroup);
            TweenUtility.FadeOut(backButtonCanvasGroup);
            TweenUtility.FadeOut(tutorialButtonCanvasGroup);
            yield return new WaitForSeconds(.4f);
            TweenUtility.FadeOut(titleCanvasGroup);
            TweenUtility.FadeOut(newGameButtonCanvasGroup);
           

            TweenUtility.FadeIn(Fade).setOnComplete(()=>
            {
                base.Hide();
                characterSelectMenu.Show();
                TweenUtility.FadeOut(Fade);
            });
        }
        public void UpgradeClicked()
        {
            
            upgradeMenu.Show();
            Hide();
        }
       
       
        
        public void SaveGameClicked()
        {
            SaveSystem.SaveGame(saveNameField.text.Trim(), new SaveData(Player.Instance, Campaign.Instance,EncounterTree.Instance));
            LeanTween.scale(saveDialog, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInBack)
                .setOnComplete(HideSaveDialog);
        }
        public void LoadCampaignScene(Campaign campaign)
        {
            SaveData.Reset();
            SceneController.LoadSceneAsync(campaign.scene, true);
            // SceneController.LoadSceneAsync(Scenes.UI, true);
            // SceneController.LoadSceneAsync(Scenes.WM_Gameplay, true);
        }
        public void LoadCampaignClicked(int campaignIndex)
        {
            Campaign.Instance.LoadConfig(GameData.Instance.campaigns[campaignIndex]);
            LoadCampaignScene(Campaign.Instance);
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
    }
}
