using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonController : MonoBehaviour
{
    public Image image;
   
    public TextMeshProUGUI description;
    [SerializeField] protected CanvasGroup canvasGroup;
    [SerializeField] protected Button button;
    public Color normalBgColor;
    public Color selectedBgColor;
    private bool selected = false;
    [SerializeField] private Image backGround;
    
    public void SetValues(Sprite sprite, string description, bool greyedOut=false)
    {
        image.sprite = sprite;
     
        this.description.SetText(description);
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
        backGround.color = selected ? selectedBgColor : normalBgColor;
    }
}