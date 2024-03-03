using System;
using TMPro;
using UnityEngine;

namespace Game.GUI.Controller
{
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
            MyDebug.LogInput("OK Clicked!");
            action?.Invoke();
            gameObject.SetActive(false);
        }
        public void CancelClicked()
        {
            MyDebug.LogInput("Cancel Clicked!");
            cancelAction?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
