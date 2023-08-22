using System;
using UnityEngine;

public abstract class UIMenu: MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] protected UIMenu parent;

    public Action onBackClicked;
  

    public virtual void Show()
    {
        canvas.enabled = true;
        
        
    }

    public virtual void Hide()
    {
        canvas.enabled = false;

    }
    public virtual void BackClicked()
    {
        Hide();
        parent?.Show();
    }
}