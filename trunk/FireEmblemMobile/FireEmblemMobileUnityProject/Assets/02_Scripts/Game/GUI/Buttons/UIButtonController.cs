using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonController : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI description;

    public void SetValues(Sprite sprite, int cost, string description)
    {
        image.sprite = sprite;
        this.cost.SetText(""+cost);
        this.description.SetText(description);
    }
    public void SetValues(Sprite sprite, string costText, string description)
    {
        image.sprite = sprite;
        this.cost.SetText(""+costText);
        this.description.SetText(description);
    }
}