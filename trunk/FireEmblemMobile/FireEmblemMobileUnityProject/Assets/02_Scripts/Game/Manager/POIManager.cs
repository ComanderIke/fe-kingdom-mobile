using System.Collections;
using Game.Dialog;
using Game.WorldMapStuff.Model;
using Menu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class POIManager : UIMenu
    {
        [SerializeField] private POIController poiTutorial;
        [SerializeField] private POIController poiIAP;
        [SerializeField] private POIController poiUpgrades;
        [SerializeField] private POIController poiEternalFlame;
        [SerializeField] private POIController poiStatue;
        [SerializeField] private POIController poiPortal;
        [SerializeField] private POIController poiLore;

        [SerializeField] private UIMenu characterSelectMenu;
        [SerializeField] private UIMenu upgradeMenu;
        [SerializeField] private UIMenu goBackMenu;
        [SerializeField] private GameObject backGround;
        [SerializeField] private GameObject sanctuary;
      
        [Header("SaveLoadStuff: ")]

        private string[] saveFiles;
        private string lastestSaveFile;
   
       [Header("Canvas Groups: ")]
        [SerializeField] private CanvasGroup titleCanvasGroup;
       [SerializeField] private CanvasGroup backButtonCanvasGroup;
        [SerializeField] private CanvasGroup Fade;
        [Header("Buttons: ")]
        [SerializeField] private Button backButton;

        [Header("Experimental: ")] 
        [SerializeField] private GoddessUI goddessUI;
        [SerializeField] private DialogueManager dialogueManager;
        [SerializeField] private Conversation introConversation;
        [SerializeField] private EnemyArmyData tutorialBattleData;
           
        public void POIClicked(int index)
        {
            Debug.Log("POI Clicked: "+index);
        }
        
        public override void Show()
        {
            //ContinueButton.SetActive(SaveData.currentSaveData!=null);
            //SaveButton.SetActive(SaveData.currentSaveData!=null);
            //LoadButton.SetActive(GetLoadFiles()!=0);
            base.Show();
            StartCoroutine(ShowCoroutine());
            poiPortal.SetInteractable(false);
            poiUpgrades.SetInteractable(false);
            backButton.interactable = false;
            poiTutorial.SetInteractable(false);
            sanctuary.gameObject.SetActive(true);
            backGround.gameObject.SetActive(false);

        }

       
        IEnumerator ShowCoroutine()
        {
            //tutorialButtonCanvasGroup.alpha = 1;
            yield return new WaitForSeconds(.5f);
            TweenUtility.FadeIn(titleCanvasGroup);
            TweenUtility.FadeIn(backButtonCanvasGroup);
 
            if (GameConfig.Instance.tutorialEnabled)
                yield return TutorialCoroutine();
            else
            {
                //tutorialButton.interactable = true;
                poiPortal.SetInteractable(true);
                poiUpgrades.SetInteractable(true);
                backButton.interactable = true;
            }
        }

        IEnumerator TutorialCoroutine()
        {
            yield return new WaitForSeconds(.6f);
            goddessUI.Show();
            yield return new WaitForSeconds(4.5f);
            dialogueManager.ShowDialog(introConversation);
            dialogueManager.dialogEnd += IntroFinished;
        }

        void IntroFinished()
        {
            dialogueManager.dialogEnd -= IntroFinished;
            goddessUI.Hide();
            poiTutorial.SetInteractable(true);
            poiPortal.SetInteractable(true);
            poiUpgrades.SetInteractable(true);
            backButton.interactable = false;
        }
        IEnumerator HideCoroutine(UIMenu nextMenu)
        {
            
           
            yield return new WaitForSeconds(.4f);
            TweenUtility.FadeOut(titleCanvasGroup);
            TweenUtility.FadeOut(backButtonCanvasGroup);

            TweenUtility.FadeIn(Fade).setOnComplete(()=>
            {
                sanctuary.gameObject.SetActive(false);
                backGround.gameObject.SetActive(true);
                base.Hide();
                nextMenu?.Show();
                TweenUtility.FadeOut(Fade);
            });
        }
        public override void Hide()
        {
            StartCoroutine(HideCoroutine(goBackMenu));
        }
        public override void BackClicked()
        {
            Hide();
            
        }
        public void ContinueClicked()
        {
            Hide();
        }
        public void NewGameClicked()
        {
            if (SaveGameManager.HasOngoingCampaignSaveData())
            {
                MenuActionController.StartGame();
            }
            else
            {
                StartCoroutine(HideCoroutine(characterSelectMenu));
            }

        }
        public void TutorialClicked()
        {
            SceneTransferData.Instance.Reset();
            SceneTransferData.Instance.EnemyArmyData = tutorialBattleData;
            SceneTransferData.Instance.TutorialBattle1 = true;
            SceneController.LoadSceneAsync(Scenes.Battle1, false);
        }
        
       
        public void UpgradeClicked()
        {
            StartCoroutine(HideCoroutine(upgradeMenu));
        }
   
    }
}

    
