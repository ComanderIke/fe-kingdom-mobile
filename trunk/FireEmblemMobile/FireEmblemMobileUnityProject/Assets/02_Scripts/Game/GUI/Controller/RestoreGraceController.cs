using System;
using System.Collections;
using Game.Dialog;
using Game.GameActors.Player;
using Game.Menu;
using Game.SerializedData;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GUI.Controller
{
    public class RestoreGraceController : UIMenu, IDataPersistance
    {
        [SerializeField] private CanvasGroup detailPanel;
        [SerializeField] private CanvasGroup buttonBackGroup;
        [SerializeField] private UIRessourceAmount gracePanel;
        [SerializeField] private CanvasGroup titleGroup;
        [FormerlySerializedAs("tabGroup")] [SerializeField] private CanvasGroup upgradesGroup;
        [SerializeField] private GoddessUI goddessUI;
        [SerializeField] private CanvasGroup Fade;
        [SerializeField] private MetaUpgradeController upgradeController;
        [SerializeField] private DialogueManager dialogueManager;
        [SerializeField] private Conversation tutorialConversation;
        // [SerializeField] private Button backButton;
        [SerializeField] private GameObject tutorialRaycastBlocker;

        [SerializeField] private MetaUpgradeDetailPanelController detailPanelController;

        void Start()
        {
       
        }

        void GraceValueChanged()
        {
            gracePanel.Amount = Player.Instance.Grace;
            detailPanelController.UpdateUI();
            upgradeController.UpdateUI();
        }
        public override void Show()
        {
            StartCoroutine(ShowCoroutine());
        }

        void UpgradeSelected(UIMetaUpgradeButton metaUpgradeButton)
        {
            if(metaUpgradeButton!=null)
                detailPanelController.Show(metaUpgradeButton.metaUpgradeBp);
            else
            {
                detailPanelController.Hide();
            }
        }

    
        IEnumerator ShowCoroutine()
        {
            base.Show();
            gracePanel.Amount = Player.Instance.Grace;
            Player.Instance.onGraceValueChanged += GraceValueChanged;
            detailPanel.alpha = 0;
            buttonBackGroup.alpha = 0;
            gracePanel.GetComponent<CanvasGroup>().alpha = 0;
            titleGroup.alpha = 0;
            upgradesGroup.alpha = 0;
            tutorialRaycastBlocker.gameObject.SetActive(true);
            detailPanelController.SetButtonInteractable(false);
            yield return new WaitForSeconds(.5f);
        
            upgradeController.onSelected -= UpgradeSelected;
            upgradeController.onSelected += UpgradeSelected;
            upgradeController.Show();
            TweenUtility.FadeIn(upgradesGroup);
            TweenUtility.FadeIn(titleGroup);
            yield return new WaitForSeconds(.5f);
            TweenUtility.FadeIn(gracePanel.GetComponent<CanvasGroup>());
            TweenUtility.FadeIn(detailPanel);
            TweenUtility.FadeIn(buttonBackGroup);

            // yield return new WaitForSeconds(.6f);
            // goddessUI.Show();
            // yield return new WaitForSeconds(4.5f);
            // //if (GameConfig.Instance.config.tutorial)
            //     yield return TutorialCoroutine();
            //TutorialSequenceFinished();
        
            // DONT DO THIS WHEN TUTORIAL(After tutorial finished)
            tutorialRaycastBlocker.gameObject.SetActive(false);
            detailPanelController.SetButtonInteractable(true);
        
            // else
            // {
            //  tutorialRaycastBlocker.gameObject.SetActive(false);
            //     detailPanelController.SetButtonInteractable(true);
            // }

        }
        IEnumerator TutorialCoroutine()
        {
            Player.Instance.Grace += 100;
            yield return new WaitForSeconds(3.0f);
            dialogueManager.ShowDialog(tutorialConversation);
            dialogueManager.dialogEnd += TutorialSequenceFinished;
        }
        void TutorialSequenceFinished()
        {
            dialogueManager.dialogEnd -= TutorialSequenceFinished;
            goddessUI.Hide();
            tutorialRaycastBlocker.gameObject.SetActive(false);
            detailPanelController.SetButtonInteractable(true);
        }
        public override void Hide()
        {
            Player.Instance.onGraceValueChanged -= GraceValueChanged;
            upgradeController.onSelected -= UpgradeSelected;
            StartCoroutine(HideCoroutine());
        }
        IEnumerator HideCoroutine()
        {
            TweenUtility.FadeOut(buttonBackGroup);
            TweenUtility.FadeOut(titleGroup);
            TweenUtility.FadeOut(gracePanel.GetComponent<CanvasGroup>());
            goddessUI.Hide();
            yield return new WaitForSeconds(0.5f);
           
            TweenUtility.FadeOut(detailPanel);
            TweenUtility.FadeOut(upgradesGroup);
            yield return new WaitForSeconds(2.0f);
            
            TweenUtility.FadeIn(Fade).setOnComplete(()=>
            {
                base.Hide();
                TweenUtility.FadeOut(Fade);
            });
        }

        public void LoadData(SaveData data)
        {
            Debug.Log("Load Data form RestoreGrace: "+gameObject.name);
        
        }

        public void SaveData(ref SaveData data)
        {
            throw new NotImplementedException();
        }
    }
}
