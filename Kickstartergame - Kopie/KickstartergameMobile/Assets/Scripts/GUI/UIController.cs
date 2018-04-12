using Assets.Scripts.AI.AttackPatterns;
using Assets.Scripts.Characters;
using Assets.Scripts.Engine;
using Assets.Scripts.Events;
using Assets.Scripts.GameStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UIController : MonoBehaviour, Controller {



    [Header("Input Fields")]
    [SerializeField]
    GameObject gameOverScreen;
    [SerializeField]
    GameObject winScreen;
    [SerializeField]
    GameObject mapUI;
    [SerializeField]
    GameObject bottomUI;
    [SerializeField]
    TopUI topUI;
    [SerializeField]
    TopUI topUIEnemy;

    [SerializeField]
    public AttackUIController attackUIController;
    [SerializeField]
    public ReactUIController reactUIController;
    [SerializeField]
    GameObject attackPreview;
    [SerializeField]
    GameObject attackPattern;
    [SerializeField]
    GameObject playerTurnAnimation;
    [SerializeField]
    GameObject aiTurnAnimation;
    [SerializeField]
    Button deselectButton;

    private Dictionary<string, GameObject> activeUnitEffects;
    private MainScript mainScript;
    private GameObject tileCursor;
    private RessourceScript ressources;
    private List<GameObject> attackableEnemyEffects;
	void Start () {
        mainScript = MainScript.GetInstance();
        EventContainer.attackPatternUsed += ShowAttackPattern;
        EventContainer.showCursor += SpawnTileCursor;
        EventContainer.hideCursor += HideTileCursor;
        EventContainer.unitShowActiveEffect += SpawnActiveUnitEffect;
        EventContainer.selectedActiveCharacter += ShowDeselectButton;
        EventContainer.deselectActiveCharacter += HideDeselectButton;

        activeUnitEffects = new Dictionary<string, GameObject>();
        attackableEnemyEffects = new List<GameObject>();
        ressources = FindObjectOfType<RessourceScript>();
    }
    void ShowDeselectButton()
    {
        deselectButton.gameObject.SetActive(true);
    }
    void HideDeselectButton()
    {
        deselectButton.gameObject.SetActive(false);
    }
    public void DeselectButtonClicked()
    {
        EventContainer.deselectButtonClicked();
    }
    private void SpawnTileCursor(int x, int y)
    {
        if(x==0 && y == 0)
        {
            Debug.Log("WTF WHY CURSOR POSITIONNULL");
        }
        tileCursor = GameObject.Instantiate(ressources.prefabs.moveCursor, GameObject.FindGameObjectWithTag("World").transform);
        tileCursor.transform.localPosition = new Vector3(x, y, tileCursor.transform.localPosition.z);
        tileCursor.name = "TileCursor";
    }
    public void ShowAttackableEnemy(int x, int y)
    {
        foreach(GameObject gameobj in attackableEnemyEffects)
        {
            if ((int)gameobj.transform.localPosition.x == x && (int)gameobj.transform.localPosition.y == y)
                return;
        }
        GameObject go = GameObject.Instantiate(ressources.prefabs.attackableEnemyPrefrab, GameObject.FindGameObjectWithTag("World").transform);
        go.transform.localPosition = new Vector3(x, y, go.transform.localPosition.z);
        attackableEnemyEffects.Add(go);
    }
    public void HideAttackableEnemy() {
        foreach(GameObject go in attackableEnemyEffects)
        {
            Destroy(go);
        }
        attackableEnemyEffects.Clear();
    }
    private void HideTileCursor()
    {
        Destroy(tileCursor);
    }
    private void SpawnActiveUnitEffect(LivingObject unit, bool spawn, bool disableOthers)
    {
        //foreach (KeyValuePair<string, GameObject> pair in activeUnitEffects)
        //{
        //    pair.Value.SetActive(true);
        //}
        if (activeUnitEffects.ContainsKey(unit.Name))
        {
            GameObject go = activeUnitEffects[unit.Name];

            activeUnitEffects.Remove(unit.Name);
            Destroy(go);
        }
        
        if (spawn)
        {
            GameObject go = GameObject.Instantiate(ressources.prefabs.moveCursor, GameObject.FindGameObjectWithTag("World").transform);
            go.transform.localPosition = new Vector3(unit.GridPosition.x, unit.GridPosition.y, go.transform.localPosition.z);
            activeUnitEffects.Add(unit.Name, go);
            go.name = "ActiveUnitEffect";
           
        }
        else
        {
            if(disableOthers)
                foreach(KeyValuePair<string, GameObject> pair in activeUnitEffects)
                {
                    pair.Value.SetActive(false);
                }
        }
    }
    public void HideAllActiveUnitEffects()
    {
        foreach (KeyValuePair<string, GameObject> pair in activeUnitEffects)
        {
            pair.Value.SetActive(false);
        }
    }
    public void ShowAllActiveUnitEffects()
    {
        foreach (KeyValuePair<string, GameObject> pair in activeUnitEffects)
        {
            pair.Value.SetActive(true);
        }
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
        HideAllActiveUnitEffects();
        attackUIController.Show(attacker,defender);
    }
    public void ShowReactUI(LivingObject attacker, LivingObject defender)
    {
        HideAllActiveUnitEffects();
        reactUIController.Show(attacker, defender);
    }

    public void HideFightUI()
    {
        if(attackUIController.isActiveAndEnabled)
            attackUIController.Hide();
        if(mainScript.GetSystem<TurnManager>().ActivePlayer.IsHumanPlayer)
            ShowAllActiveUnitEffects();
    }
    public void ShowGameOver()
    {
        gameOverScreen.SetActive(true);
    }
    public void ShowWinScreen()
    {
        winScreen.SetActive(true);
    }
    public void PlayerTurnAnimation()
    {

        Instantiate(playerTurnAnimation, new Vector3(), Quaternion.identity, GameObject.Find("Canvas").transform).transform.localPosition = new Vector3();
    }
    public void EnemyTurnAnimation()
    {
        HideAllActiveUnitEffects();
        Instantiate(aiTurnAnimation, new Vector3(), Quaternion.identity, GameObject.Find("Canvas").transform).transform.localPosition = new Vector3();
    }
    public void HideReactUI()
    {
        if (reactUIController.isActiveAndEnabled)
            reactUIController.Hide();
        if (mainScript.GetSystem<TurnManager>().ActivePlayer.IsHumanPlayer)
            ShowAllActiveUnitEffects();
    }
    public void ShowAttackPattern(LivingObject user, AttackPattern pattern )
    {
        attackPattern.SetActive(true);
        attackPattern.GetComponent<AttackPatternUI>().Show(user.Name, pattern.Name);
    }

    public void ShowTopUI(LivingObject c)
    {
        if(c.Player.ID != mainScript.GetSystem<TurnManager>().ActivePlayer.ID)
        {
            topUIEnemy.Show(c);
            topUI.Hide();
        }
        else
        {
            topUIEnemy.Hide();
            topUI.Show(c);
        }
       

        
    }

    public void ShowMapUI()
    {
        mapUI.SetActive(true);
    }
    public void HideMapUI()
    {
        mapUI.SetActive(false);
    }

    public void UndoClicked()
    {
        EventContainer.undo();
    }
    public void EndTurnClicked()
    {
        EventContainer.endTurn();
    }
    public void ShowAttackPreview(LivingObject attacker, LivingObject defender)
    {
        attackPreview.SetActive(true);
        attackPreview.GetComponent<AttackPreview>().UpdateValues(attacker.Stats.MaxHP,attacker.Stats.HP, defender.Stats.MaxHP, defender.Stats.HP,attacker.BattleStats.GetDamageAgainstTarget(defender),attacker.BattleStats.GetHitAgainstTarget(defender), attacker.BattleStats.GetAttackCountAgainst(defender));
        //Vector3 attackPreviewPos;
        //if (defender.GridPosition is BigTilePosition)
        //  attackPreviewPos = Camera.main.WorldToScreenPoint(new Vector3(pos.x + GridManager.GRID_X_OFFSET ,pos.y + 1.0f, -0.05f));
        //else
        //    attackPreviewPos = Camera.main.WorldToScreenPoint(new Vector3(pos.x + GridManager.GRID_X_OFFSET + 0.5f, pos.y + 1.0f, -0.05f));
        //attackPreviewPos.z = 0;
        //attackPreview.transform.localPosition = new Vector3(attackPreviewPos.x-540,attackPreviewPos.y-960,0);//TODO WHY MAGIC NUMBERS?
    }
    public void HideAttackPreview()
    {
        attackPreview.SetActive(false);
    }
}
