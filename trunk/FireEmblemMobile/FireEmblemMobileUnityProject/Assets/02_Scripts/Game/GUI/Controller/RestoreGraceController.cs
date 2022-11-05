using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using LostGrace;
using UnityEngine;

public class RestoreGraceController :UIMenu
{
    [SerializeField] private CanvasGroup detailPanel;
    [SerializeField] private CanvasGroup buttonBackGroup;
    [SerializeField] private CanvasGroup gracePanel;
    [SerializeField] private CanvasGroup titleGroup;
    [SerializeField] private CanvasGroup tabGroup;
    [SerializeField] private GoddessUI goddessUI;
    [SerializeField] private CanvasGroup Fade;
    [SerializeField] private UIRessourceAmount graceAmount;
    public override void Show()
    {
        StartCoroutine(ShowCoroutine());
    }
    IEnumerator ShowCoroutine()
    {
        base.Show();
        detailPanel.alpha = 0;
        buttonBackGroup.alpha = 0;
        gracePanel.alpha = 0;
        titleGroup.alpha = 0;
        tabGroup.alpha = 0;
        yield return new WaitForSeconds(.5f);
        TweenUtility.FadeIn(tabGroup);
        TweenUtility.FadeIn(titleGroup);
        yield return new WaitForSeconds(.5f);
        TweenUtility.FadeIn(gracePanel);
        TweenUtility.FadeIn(detailPanel);
        TweenUtility.FadeIn(buttonBackGroup);

        yield return new WaitForSeconds(.6f);
        goddessUI.Show();
        yield return new WaitForSeconds(4.5f);
       // if (GameConfig.Instance.config.tutorial)
            yield return TutorialCoroutine();

    }
    IEnumerator TutorialCoroutine()
    {
        graceAmount.Amount += 100;
        yield return new WaitForSeconds(4.5f);
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
}
