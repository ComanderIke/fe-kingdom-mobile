using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonController : MonoBehaviour
{
    public Image image;
    public Image selectedImage;
    [SerializeField] protected CanvasGroup canvasGroup;
    [SerializeField] protected Button button;
    private bool selected = false;
    
    public void SetValues(Sprite sprite, bool greyedOut=false)
    {
        image.sprite = sprite;
    }
    
    public void Select()
    {
        selected = true;
        UpdateUI();
    }
    public void Deselect()
    {
        selected = false;
        UpdateUI();
    }

    protected virtual void UpdateUI()
    {
        selectedImage.gameObject.SetActive( selected);
    }

    public void SetInteractable(bool interactable)
    {
        button.interactable = interactable;
    }
}