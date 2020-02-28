using Assets.Scripts.AI.AttackPatterns;
using Assets.Scripts.Characters;
using Assets.Scripts.Engine;
using Assets.Scripts.GameStates;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UISystem : MonoBehaviour, EngineSystem {

    #region Events
    public delegate void OnContinuePressed();
    public static OnContinuePressed onContinuePressed;

    public delegate void oOnFrontalAttackAnimationEnd();
    public static oOnFrontalAttackAnimationEnd onFrontalAttackAnimationEnd;

    public delegate void OnAttackUIVisible(bool visible);
    public static OnAttackUIVisible onAttackUIVisible;

    public delegate void OnReactUIVisible(bool visible);
    public static OnReactUIVisible onReactUIVisible;

    public delegate void OnDodgeClicked();
    public static OnDodgeClicked onDodgeClicked;

    public delegate void OnGuardClicked();
    public static OnGuardClicked onGuardClicked;

    public delegate void OnDeselectButtonClicked();
    public static OnDeselectButtonClicked onDeselectButtonClicked;

    public delegate void OnCounterClicked();
    public static OnCounterClicked onCounterClicked;

    public delegate void OnShowCursor(int x, int y);
    public static OnShowCursor onShowCursor;

    public delegate void OnHideCursor();
    public static OnHideCursor onHideCursor;
    #endregion

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
    Canvas mainCanvas;
    [SerializeField]
    public AttackUIController attackUIController;
    [SerializeField]
    public ReactUIController reactUIController;
    [SerializeField]
    GameObject attackPreview;
    [SerializeField]
    GameObject playerTurnAnimation;
    [SerializeField]
    GameObject aiTurnAnimation;
    [SerializeField]
    Button deselectButton;
    public UnityEvent onDeselectClicked;

    private Dictionary<string, GameObject> activeUnitEffects;
    private MainScript mainScript;
    private GameObject tileCursor;
    private RessourceScript ressources;
    private List<GameObject> attackableEnemyEffects;
    private List<GameObject> attackableFieldEffects;
    void Start () {
        mainScript = MainScript.instance;
        UISystem.onShowCursor += SpawnTileCursor;
        UISystem.onHideCursor += HideTileCursor;
        Unit.onUnitShowActiveEffect += SpawnActiveUnitEffect;
        UnitActionSystem.onSelectedCharacter += ShowDeselectButton;
        UnitActionSystem.onDeselectCharacter += HideDeselectButton;

        activeUnitEffects = new Dictionary<string, GameObject>();
        attackableEnemyEffects = new List<GameObject>();
        attackableFieldEffects = new List<GameObject>();
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
        onDeselectClicked.Invoke();
        //EventContainer.deselectButtonClicked();
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
    private void SpawnActiveUnitEffect(Unit unit, bool spawn, bool disableOthers)
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
    public int GetUIHeight()
    {
        return (int)(topUI.GetComponent<RectTransform>().rect.height+ bottomUI.GetComponent<RectTransform>().rect.height);
    }
    public int GetReferenceHeight()
    {
        return (int)mapUI.GetComponent<CanvasScaler>().referenceResolution.y;
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

    public void ShowFightUI(Unit attacker, Unit defender)
    {
        HideAllActiveUnitEffects();
        attackUIController.Show(attacker,defender);
    }
    public void ShowReactUI(Unit attacker, Unit defender)
    {
        HideAllActiveUnitEffects();
        reactUIController.Show(attacker, defender);
    }

    public void HideFightUI()
    {
        if(attackUIController.isActiveAndEnabled)
            attackUIController.Hide();
        if(mainScript.PlayerManager.ActivePlayer.IsPlayerControlled)
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

        Instantiate(playerTurnAnimation, new Vector3(), Quaternion.identity, mainCanvas.transform).transform.localPosition = new Vector3();
    }
    public void EnemyTurnAnimation()
    {
        HideAllActiveUnitEffects();
        Instantiate(aiTurnAnimation, new Vector3(), Quaternion.identity, mainCanvas.transform).transform.localPosition = new Vector3();
    }
    public void HideReactUI()
    {
        if (reactUIController.isActiveAndEnabled)
            reactUIController.Hide();
        if (mainScript.PlayerManager.ActivePlayer.IsPlayerControlled)
            ShowAllActiveUnitEffects();
    }

    public void ShowTopUI(Unit c)
    {
        if(c.Player.ID != mainScript.PlayerManager.ActivePlayer.ID)
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
        UnitActionSystem.onUndo();
    }
    public void EndTurnClicked()
    {
        TurnSystem.onEndTurn();
    }
    public void ShowAttackableField(int x, int y)
    {
        foreach (GameObject gameobj in attackableFieldEffects)
        {
            if ((int)gameobj.transform.localPosition.x == x && (int)gameobj.transform.localPosition.y == y)
                return;
        }
        GameObject go = GameObject.Instantiate(ressources.particles.enemyField, GameObject.FindGameObjectWithTag("World").transform);
        go.transform.localPosition = new Vector3(x+0.5f, y+0.5f, go.transform.localPosition.z-0.1f);
        attackableFieldEffects.Add(go);
    }
    public void HideAttackableField()
    {
        Debug.Log("hideAttackableField!");
        foreach (GameObject go in attackableFieldEffects)
        {
            Destroy(go);
        }
        attackableFieldEffects.Clear();
    }
    public void ShowAttackPreview(Unit attacker, Unit defender)
    {
        ShowAttackableEnemy((int)defender.GridPosition.x, (int)defender.GridPosition.y);
        attackPreview.SetActive(true);
        attackPreview.GetComponent<AttackPreview>().UpdateValues(attacker.Stats.MaxHP,attacker.HP, defender.Stats.MaxHP, defender.HP,attacker.BattleStats.GetDamageAgainstTarget(defender),attacker.BattleStats.GetHitAgainstTarget(defender), attacker.BattleStats.GetAttackCountAgainst(defender));
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
        HideAttackableEnemy();
        attackPreview.SetActive(false);
    }

    private void OnDestroy()
    {
        onAttackUIVisible = null;
        onReactUIVisible = null;
        onDodgeClicked = null;
        onGuardClicked = null;
        onCounterClicked = null;
        onDeselectButtonClicked = null;
        onFrontalAttackAnimationEnd = null;
        onHideCursor = null;
        onShowCursor = null;
        onContinuePressed = null;
    }
}
