using System;
using UnityEngine;

public abstract class UIMenu: MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    public Action onBackClicked;
    public void Show()
    {
        canvas.enabled = true;
    }

    public void Hide()
    {
        canvas.enabled = false;
    }
    public void BackClicked()
    {
        onBackClicked?.Invoke();
        Hide();
      
    }
}