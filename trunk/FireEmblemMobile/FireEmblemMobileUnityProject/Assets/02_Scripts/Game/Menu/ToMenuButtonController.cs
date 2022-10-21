using System.Collections;
using System.Collections.Generic;
using Game.GUI;
using UnityEngine;

public class ToMenuButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        gameObject.SetActive(MainMenuController.Instance != null);
    }

    public void OnClick()
    {
        MainMenuController.Instance.Show();
    }
}
