using System;
using Game.GameResources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EncounterToolTip:MonoBehaviour
{
    public TextMeshProUGUI nameLabel;
    public TextMeshProUGUI description;
    public Image icon;
    public Button moveButton;
    public Action<EncounterNode> moveClicked;
    private EncounterNode encounterNode;

    public void Updatevalues(EncounterNode encounterNode, string name, Vector3 worldPos, bool movable, Action<EncounterNode> movClicked)
    {
        this.encounterNode = encounterNode;
        this.nameLabel.text = name;
        icon.sprite = encounterNode.sprite;
        description.text = encounterNode.description;
        transform.position = Camera.main.WorldToScreenPoint(worldPos);
        moveButton.interactable = movable;
        moveClicked = movClicked;
    }

    public void MoveClicked()
    {
        Debug.Log("MOVE CLICKED");
        gameObject.SetActive(false);
        moveClicked?.Invoke(encounterNode);
    }
}