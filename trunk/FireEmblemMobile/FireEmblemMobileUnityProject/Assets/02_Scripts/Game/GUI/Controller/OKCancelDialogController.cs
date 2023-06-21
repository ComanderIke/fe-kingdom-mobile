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
    // Start is called before the first frame update
    public void Show(string questionText, Action okAction)
    {
        QuestionText.SetText(questionText);
        action = okAction;
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
        gameObject.SetActive(false);
    }
}
