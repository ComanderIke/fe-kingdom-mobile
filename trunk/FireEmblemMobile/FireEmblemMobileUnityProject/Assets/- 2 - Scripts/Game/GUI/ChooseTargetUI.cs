using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.GameActors.Units;
using Game.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseTargetUI : MonoBehaviour, IChooseTargetUI, IClickedReceiver
{
    private Canvas canvas;

    public GameObject SelectedCharacterCirclePrefab;

    public Transform CharacterCircleSpawnParent;

    public TextMeshProUGUI nameText;
    public Image icon;
    public TextMeshProUGUI descriptionText;

    public LayoutGroup topLayout;

    public LayoutGroup bottomLayout;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void Show(Unit selectedUnit, ITargetableObject targetableObject)
    {
        CharacterCircleSpawnParent.DeleteAllChildren();
        var go = Instantiate(SelectedCharacterCirclePrefab, transform);
        var uiController = go.GetComponent<CharacterUIController>();
        uiController.parentController = this;
        uiController.ShowActive(selectedUnit);
           
        nameText.SetText(targetableObject.GetName());
        descriptionText.SetText(targetableObject.GetDescription());
        icon.sprite=targetableObject.GetIcon();
  
        LayoutRebuilder.ForceRebuildLayoutImmediate(topLayout.transform as RectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(bottomLayout.transform as RectTransform);
        canvas.enabled = true;
    }

    public void Hide()
    {
        canvas.enabled = false;
        CharacterCircleSpawnParent.DeleteAllChildren();
    }

    public void Clicked(Unit unit)
    {
        throw new System.NotImplementedException();
    }
}
