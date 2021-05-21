using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUIController:MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Image image;
    public Image BackgroundImage;
    public Sprite selectedSprite;
    public Sprite normalSprite;
    private bool selected;
    public void SetText(string memberName)
    {
        nameText.SetText(memberName);
    }

    public void SetSprite(Sprite mapSprite)
    {
        image.sprite = mapSprite;
    }

    public void Clicked()
    {
        selected = !selected;
        BackgroundImage.sprite = selected?selectedSprite:normalSprite;
    }
}