using Assets.Scripts.AI.AttackPatterns;
using Assets.Scripts.Characters;
using Assets.Scripts.Engine;
using Assets.Scripts.Events;
using Assets.Scripts.GameStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour, Controller {

    public delegate void AttackButtonCLicked();
    public static AttackButtonCLicked attacktButtonCLicked;

    [Header("Input Fields")]
    [SerializeField]
    GameObject bottomUI;
    [SerializeField]
    TopUI topUI;
    [SerializeField]
    Image faceSpriteLeft;
    [SerializeField]
    Image[] inventorySprites;
    [SerializeField]
    Text hpLeft;
    [SerializeField]
    Text atkLeft;
    [SerializeField]
    Text spdLeft;
    [SerializeField]
    Text defLeft;
    [SerializeField]
    Text accLeft;
    [SerializeField]
    public AttackUIController attackUIController;
    [SerializeField]
    public ReactUIController reactUIController;
    [SerializeField]
    GameObject attackPreview;
    [SerializeField]
    GameObject attackPattern;

    private MainScript mainScript;

	void Start () {
        mainScript = MainScript.GetInstance();
        EventContainer.attackPatternUsed += ShowAttackPattern;
    }

    public void HideBottomUI()
    {
        bottomUI.SetActive(false);
    }

    public void ShowBottomUI()
    {
        bottomUI.SetActive(true);
    }

    public void HideTopUI()
    {
        topUI.gameObject.SetActive(false);
    }

    public void ShowFightUI(LivingObject attacker, LivingObject defender)
    {
        attackUIController.Show(attacker,defender);
    }
    public void ShowReactUI(LivingObject attacker, LivingObject defender)
    {
        reactUIController.Show(attacker, defender);
    }

    public void HideFightUI()
    {
        attackUIController.Hide() ;
    }


    public void HideReactUI()
    {
        reactUIController.Hide();
    }
    public void ShowAttackPattern(LivingObject user, AttackPattern pattern )
    {
        attackPattern.SetActive(true);
        attackPattern.GetComponent<AttackPatternUI>().Show(user.Name, pattern.Name);
    }

    public void ShowTopUI(LivingObject c)
    {
        //topUI.gameObject.SetActive(true);

        hpLeft.text = c.Stats.HP+" / "+c.Stats.MaxHP;
        atkLeft.text = ""+c.Stats.Attack;
        spdLeft.text = "" + c.Stats.Speed;
        accLeft.text = "" + c.Stats.Accuracy;
        defLeft.text = "" + c.Stats.Defense;
        faceSpriteLeft.sprite = c.Sprite;
        Debug.Log(c.Sprite);
        foreach (Image i in inventorySprites)
        {
            i.enabled = false;
        }
        if (c.GetType() == typeof(Human))
        {
            Human character = (Human)c;
            for (int i = 0; i < character.Inventory.items.Count; i++)
            {
                inventorySprites[i].sprite = character.Inventory.items[i].Sprite;
                inventorySprites[i].enabled = true;
            }
        }
    }

    public void SkipFightButtonClicked()
    {
        attacktButtonCLicked();
    }
    public void UndoClicked()
    {
        EventContainer.undo();
    }
    public void EndTurnClicked()
    {
        EventContainer.endTurn();
    }
    public void ShowAttackPreview(LivingObject attacker, LivingObject defender, Vector2 pos)
    {
        Debug.Log("ShowAttackPreview" +pos);
        attackPreview.SetActive(true);
        attackPreview.GetComponent<AttackPreview>().UpdateValues(attacker.BattleStats.GetDamageAgainstTarget(defender),attacker.BattleStats.GetHitAgainstTarget(defender), attacker.BattleStats.CanDoubleAttack(defender) ? 2 : 1);
        Vector3 attackPreviewPos;
        if (defender.GridPosition is BigTilePosition)
          attackPreviewPos = Camera.main.WorldToScreenPoint(new Vector3(pos.x + GridManager.GRID_X_OFFSET ,pos.y + 1.0f, -0.05f));
        else
            attackPreviewPos = Camera.main.WorldToScreenPoint(new Vector3(pos.x + GridManager.GRID_X_OFFSET + 0.5f, pos.y + 1.0f, -0.05f));
        attackPreviewPos.z = 0;
        attackPreview.transform.position = attackPreviewPos;
    }
    public void HideAttackPreview()
    {
        attackPreview.SetActive(false);
    }
}
