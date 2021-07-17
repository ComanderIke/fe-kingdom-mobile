using System;
using System.Collections;
using System.Collections.Generic;
using Game.Dialog;
using Game.Utility;
using UnityEngine;


public class NPCController : MonoBehaviour
{
    public Action onClicked;

    public Conversation Conversation;
    // Start is called before the first frame update

    private void OnMouseDown()
    {
        if (!UIHelper.IsPointerOverGameObject())
        {
            Debug.Log("Start Dialog!");
            FindObjectOfType<DialogueManager>()?.ShowDialog(Conversation);
            onClicked?.Invoke();
        }
        else
        {
            Debug.Log("Dont start Dialog!");
        }
    }
}
