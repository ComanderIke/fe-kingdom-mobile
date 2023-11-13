using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OKCancelDialogController : MonoBehaviour
{
    public TextMeshProUGUI QuestionText;


    private Action action;
    private Action cancelAction;
    // Start is called before the first frame update
    public void Show(string questionText, Action okAction, Action cancelAction=null)
    {
        QuestionText.SetText(questionText);
        action = okAction;
        this.cancelAction = cancelAction;
        gameObject.SetActive(true);
    }

    public void OkClicked()
    {
        Debug.Log("OK Clicked!");
        action?.Invoke();
        gameObject.SetActive(false);
    }
    public void CancelClicked()
    {
        Debug.Log("Cancel Clicked!");
        cancelAction?.Invoke();
        gameObject.SetActive(false);
    }
}
