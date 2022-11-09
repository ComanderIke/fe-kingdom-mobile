using System;
using System.Collections;
using System.Collections.Generic;
using Game.Dialog;
using Game.GameActors.Players;
using Game.Systems;
using LostGrace;
using UnityEngine;
using UnityEngine.UI;

public class RestoreGraceController : UIMenu, IDataPersistance
{
    [SerializeField] private CanvasGroup detailPanel;
    [SerializeField] private CanvasGroup buttonBackGroup;
    [SerializeField] private CanvasGroup gracePanel;
    [SerializeField] private CanvasGroup titleGroup;
    [SerializeField] private CanvasGroup tabGroup;
    [SerializeField] private GoddessUI goddessUI;
    [SerializeField] private CanvasGroup Fade;
    [SerializeField] private UIRessourceAmount graceAmount;
    [SerializeField] private MetaUpgradeController upgradeController;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private Conversation tutorialConversation;
    // [SerializeField] private Button backButton;
    [SerializeField] private GameObject tutorialRaycastBlocker;

[SerializeField] private MetaUpgradeDetailPanelController detailPanelController;
    public override void Show()
    {
        StartCoroutine(ShowCoroutine());
    }
    
    IEnumerator ShowCoroutine()
    {
        base.Show();
        graceAmount.Amount = 0;
        detailPanel.alpha = 0;
        buttonBackGroup.alpha = 0;
        gracePanel.alpha = 0;
        titleGroup.alpha = 0;
        tabGroup.alpha = 0;
        tutorialRaycastBlocker.gameObject.SetActive(true);
        detailPanelController.SetButtonInteractable(false);
        yield return new WaitForSeconds(.5f);
        upgradeController.Show();
        TweenUtility.FadeIn(tabGroup);
        TweenUtility.FadeIn(titleGroup);
        yield return new WaitForSeconds(.5f);
        TweenUtility.FadeIn(gracePanel);
        TweenUtility.FadeIn(detailPanel);
        TweenUtility.FadeIn(buttonBackGroup);

        yield return new WaitForSeconds(.6f);
        goddessUI.Show();
        yield return new WaitForSeconds(4.5f);
        //if (GameConfig.Instance.config.tutorial)
            yield return TutorialCoroutine();
        // else
        // {
        //  tutorialRaycastBlocker.gameObject.SetActive(false);
        //     detailPanelController.SetButtonInteractable(true);
        // }

    }
    IEnumerator TutorialCoroutine()
    {
        graceAmount.Amount += 100;
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
        StartCoroutine(HideCoroutine());
    }
    IEnumerator HideCoroutine()
    {
        TweenUtility.FadeOut(buttonBackGroup);
        TweenUtility.FadeOut(titleGroup);
        TweenUtility.FadeOut(gracePanel);
        goddessUI.Hide();
        yield return new WaitForSeconds(0.5f);
           
        TweenUtility.FadeOut(detailPanel);
        TweenUtility.FadeOut(tabGroup);
        yield return new WaitForSeconds(2.0f);
            
        TweenUtility.FadeIn(Fade).setOnComplete(()=>
        {
            base.Hide();
            parent?.Show();
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
