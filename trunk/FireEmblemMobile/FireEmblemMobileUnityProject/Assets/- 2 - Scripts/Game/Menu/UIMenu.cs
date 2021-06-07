using UnityEngine;

public abstract class UIMenu: MonoBehaviour
{
    [SerializeField] private Canvas canvas;
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
        Hide();
    }
}