using UnityEngine;
using System.Collections.Generic;
using Assets.GameResources;
using Assets.Grid;
using Assets.Mechanics;
using Assets.GameInput;
using Assets.GameActors.Units;
using System.Linq;
using Assets.Audio;
using Assets.GUI;
using Assets.Game.GameStates;
using Assets.Game.Manager;

public class PlayerInputFeedback : MonoBehaviour
{
    [SerializeField] private Transform uiContainer = default;
    [SerializeField] private Transform worldContainer = default;
    [SerializeField] private ExpBarController expBarController = default;
    [SerializeField] private Canvas MainCanvas = default;
    private ResourceScript resources;
    private GameObject moveCursor;
    private GameObject moveCursorStart;
    private List<GameObject> instantiatedMovePath= new List<GameObject>();
    private Dictionary<string, GameObject> activeUnitEffects;
    private GridGameManager gridGameManager;
    
    [SerializeField] private GameObject playerTurnAnimation = default;
    [SerializeField] private GameObject aiTurnAnimation = default;
   
    private List<GameObject> attackableEnemyEffects;
    private List<GameObject> attackableFieldEffects;
    private void Start ()
    {
        instantiatedMovePath = new List<GameObject>();
        resources = FindObjectOfType<ResourceScript>();
        activeUnitEffects = new Dictionary<string, GameObject>();
        gridGameManager = GridGameManager.Instance;
        attackableEnemyEffects = new List<GameObject>();
        attackableFieldEffects = new List<GameObject>();
        PlayerTurnTextAnimation.OnStarted += TurnAnimationStarted;
        PlayerTurnTextAnimation.OnFinished += TurnAnimationFinished;
        //InputSystem.OnDragReset += HideMovementPath;
        InputSystem.OnMovementPathUpdated += DrawMovementPath;
        //InputSystem.OnMovementPathUpdated += OnMovementPathUpdated;
        InputSystem.OnDraggedOnActiveField += HideAttackableEnemy;
        UnitSelectionSystem.OnDeselectCharacter += DeselectedCharacter;
        UnitSelectionSystem.OnSelectedCharacter += SelectedCharacter;

        InputSystem.OnSetActive += InputActive;
        InputSystem.OnDragReset += HideAttackableEnemy;
        BattleState.OnEnter += HideAllActiveUnitEffects;
        BattleState.OnEnter += HideAttackableEnemy;
        BattleState.OnExit += OnExitBattleState;
        TurnSystem.OnStartTurn += OnStartTurn;
        Unit.OnUnitActiveStateUpdated += OnActiveUnitStateUpdate;
        MovementState.OnEnter += HideAttackableField;
        BattleState.OnEnter += HideAttackableField;
        MovementState.OnEnter += HideAllActiveUnitEffects;
        MovementState.OnMovementFinished += (Unit u)=>HideMovementPath();
        BattleState.OnEnter += HideMovementPath;
        UnitActionSystem.OnCheckAttackPreview += OnCheckAttackPreview;
        MovementState.OnMovementFinished += (Unit u)=>HideAttackableEnemy();
        GridRenderer.OnRenderEnemyTile += OnRenderEnemyTile;
        Unit.OnExpGained += ExpGained;
        InputSystem.OnInputActivated += InputActivated;
        UnitSelectionSystem.OnEnemySelected += EnemySelected;
        BattleRenderer.OnAttackConnected += SlashAnimation;
        Unit.OnUnitDamaged += UnitDamaged;
        InstantiateGameObjects();//Performance Do this on Start
        DrawMovementPath(new List<Vector2>() {new Vector2(1,0), new Vector2(2, 0), new Vector2(3, 0) },0,0);
        HideMovementPath();
    }
    private void InstantiateGameObjects()
    {
        ShowAttackableField(0, 0);
        ShowAttackableField(0, 1);
        ShowAttackableField(0, 2);
        ShowAttackableField(0, 3);
        ShowAttackableField(0, 4);
        HideAttackableField();
    }
    private void UnitDamaged(Unit unit , int damage)
    {
        DamagePopUp.Create(unit.GameTransform.GetCenterPosition(), damage, Color.red);
    }
    private void SlashAnimation(Unit attacker, Unit defender)
    {
        Vector3 spawnPos = defender.GameTransform.GetPosition();
        spawnPos.x += 0.5f;
        spawnPos.y += 0.5f;//Spawn in Middle of GridCell
        if(attacker.Faction.Id==0)
            Instantiate(resources.Prefabs.slashBlue, spawnPos, Quaternion.identity);
        else
            Instantiate(resources.Prefabs.slashRed, spawnPos, Quaternion.identity);
    }
    private void EnemySelected(Unit u) {
        HideAllActiveUnitEffects();
    }
    private void DeselectedCharacter()
    {
        ShowAllActiveUnitEffects();
        HideMovementPath();
        HideAttackableField();
        HideAttackableEnemy();
    }
    bool setActiveUnitEffectsWhenInputIsActive = false;
    private void InputActivated()
    {
        if (setActiveUnitEffectsWhenInputIsActive)
            ShowAllActiveUnitEffects();
        setActiveUnitEffectsWhenInputIsActive = false;
    }
    private void ExpGained(Unit unit, int currentExp, int expGained)
    {
        AnimationQueue.Add(() => { 
            InputSystem.OnSetActive(false, this); 
            expBarController.Show(currentExp, expGained);
        }, ()=> InputSystem.OnSetActive(true, this));
        //AnimationQueue.OnAllAnimationsEnded += SetInputToOldState;
    }
    private void TurnAnimationStarted()
    {
        InputSystem.OnSetActive(false, this);
    }
    private void TurnAnimationFinished()
    {
        //Set to State Before TurnAnimationStarted
        InputSystem.OnSetActive(true, this);
    }
    //private void OnMovementPathUpdated(List<Vector2> mousePath, int startX, int startY)
    //{
    //    HideAttackableEnemy();
    //}
    private void OnActiveUnitStateUpdate(Unit u, bool canMove, bool disableOthers)
    {
        if (u.Faction != null && u.Faction.Id != gridGameManager.FactionManager.GetPlayerControlledFaction().Id)
            return;
        HideActiveUnitEffect(u);
        if (disableOthers)
            HideAllActiveUnitEffects();
        if (canMove)
        {
            SpawnActiveUnitEffect(u);
        }
    }

    private void OnRenderEnemyTile(int x, int y, Unit enemy, int playerId)
    {
        if (enemy != null && enemy.Faction.Id != playerId)
            ShowAttackableField(x, y);
    }
    private void OnCheckAttackPreview(Unit u, Unit defender)
    {
        HideAttackableEnemy();
        ShowAttackableEnemy(defender.GridPosition.X, defender.GridPosition.Y);
    }
    private void OnStartTurn()
    {
        if (!gridGameManager.FactionManager.ActiveFaction.IsPlayerControlled)
        {
            gridGameManager.GetSystem<AudioSystem>().ChangeMusic("EnemyTheme", "PlayerTheme", true);

            EnemyTurnAnimation();

            HideAllActiveUnitEffects();
        }
        else
        {
            gridGameManager.GetSystem<AudioSystem>().ChangeMusic("PlayerTheme", "EnemyTheme", true);
            PlayerTurnAnimation();
        }
        
    }
    public void ShowAttackableEnemy(int x, int y)
    {
        if (attackableEnemyEffects.Any(gameObj => (int)gameObj.transform.localPosition.x == x && (int)gameObj.transform.localPosition.y == y))
        {
            attackableEnemyEffects.Find(gameObj => (int)gameObj.transform.localPosition.x == x && (int)gameObj.transform.localPosition.y == y)
                .SetActive(true);
            return;
        }

        var go = Instantiate(resources.Prefabs.AttackableEnemyPrefab,
            GameObject.FindGameObjectWithTag("World").transform);
        go.transform.localPosition = new Vector3(x, y, go.transform.localPosition.z);
        attackableEnemyEffects.Add(go);
    }
    public void ShowAttackableField(int x, int y)
    {
        //Debug.Log("Show Attackable Field: "+x+" " +y );
        if (attackableFieldEffects.Any(gameObj => !gameObj.activeSelf||(gameObj.transform.localPosition.x-0.5f==x && gameObj.transform.localPosition.y-0.5f == y)))
        {
            GameObject go2 = attackableFieldEffects.Find(gameObj => gameObj.transform.localPosition.x - 0.5f == x && gameObj.transform.localPosition.y - 0.5f == y||!gameObj.activeSelf);
            go2.transform.localPosition = new Vector3(x + 0.5f, y + 0.5f, go2.transform.localPosition.z);
            go2.SetActive(true);
            return;
        }
       
        var go = Instantiate(resources.Prefabs.attackIconPrefab,
            null);
        go.transform.localPosition = new Vector3(x + 0.5f, y + 0.5f, go.transform.localPosition.z);
        attackableFieldEffects.Add(go);
    }

    public void HideAttackableField()
    {
        foreach (var go in attackableFieldEffects)
        {
            go.SetActive(false);
            
        }

        //attackableFieldEffects.Clear();
    }

    public void HideAttackableEnemy()
    {
        foreach (var go in attackableEnemyEffects)
        {
            go.SetActive(false);
        }

        //attackableEnemyEffects.Clear();
    }
    public void PlayerTurnAnimation()
    {
        Instantiate(playerTurnAnimation, new Vector3(), Quaternion.identity, uiContainer).transform
            .localPosition = new Vector3();
    }
    public void EnemyTurnAnimation()
    {

        Instantiate(aiTurnAnimation, new Vector3(), Quaternion.identity, uiContainer).transform
            .localPosition = new Vector3();
    }
    private void OnExitBattleState()
    {
        if (GridGameManager.Instance.FactionManager.ActiveFaction.IsPlayerControlled)
            ShowAllActiveUnitEffects();
    }
    private void InputActive(bool active, object caller)
    {
        if (active)
            ShowAllActiveUnitEffects();
        else
            HideAllActiveUnitEffects();
    }
    private void SelectedCharacter(Unit u)
    {
        HideAllActiveUnitEffects();
        if (u.Ap != 0)
        {
            SpawnActiveUnitEffect(u);
        }
    }
    private void HideActiveUnitEffect(Unit unit)
    {
        //if (activeUnitEffects.ContainsKey(unit.Name))
        //{
        //    var gob = activeUnitEffects[unit.Name];
        //    gob.SetActive(false);
        //    //activeUnitEffects.Remove(unit.Name);
        //    //GameObject.Destroy(gob);
        //}
    }
    private void SpawnActiveUnitEffect(Unit unit)
    {
        //HideActiveUnitEffect(unit);
        //Debug.Log("Spawn ActiveUnitEffect: [" + unit.GridPosition.X + "/" + unit.GridPosition.Y + "]");

        //var go = GameObject.Instantiate(resources.Prefabs.ActiveUnitField,
        //    GameObject.FindGameObjectWithTag("World").transform);
        //go.transform.localPosition =
        //    new Vector3(unit.GridPosition.X, unit.GridPosition.Y, go.transform.localPosition.z);
        //activeUnitEffects.Add(unit.Name, go);

        //go.name = "ActiveUnitEffect";
    }
    public void HideAllActiveUnitEffects()
    {
        //setActiveUnitEffectsWhenInputIsActive = false;
        //foreach (var pair in activeUnitEffects)
        //{
        //    pair.Value.SetActive(false);
        //}
        //InputSystem.OnInputActivated -= ShowAllActiveUnitEffects;
    }
    public void ShowAllActiveUnitEffects()
    {
        //if (!InputSystem.Active)
        //{
        //    InputSystem.OnInputActivated += ShowAllActiveUnitEffects;
        //    return;
        //}
        //InputSystem.OnInputActivated -= ShowAllActiveUnitEffects;
        ////Debug.Log("ShowAllActiveUnitEffects "+activeUnitEffects.Count);
        //foreach (var pair in activeUnitEffects)
        //{
        //    pair.Value.SetActive(true);
        //}
    }
    private bool MovementPathVisible = false;
    public void DrawMovementPath(List<Vector2> mousePath, int startX, int startY)
    {
        HideMovementPath();
        MovementPathVisible = true;
 
        if (mousePath.Count == 0)
        {
            if (moveCursor == null)
            {
                moveCursor = GameObject.Instantiate(resources.Prefabs.MoveCursor, worldContainer);
                moveCursor.name = "MoveCursor";
            }
            moveCursor.transform.localPosition = new Vector3(startX,
                startY, moveCursor.transform.localPosition.z);
            moveCursor.SetActive(true);
            if (moveCursorStart == null)
            {
                moveCursorStart = GameObject.Instantiate(resources.Prefabs.MoveArrowDot, worldContainer);
                moveCursorStart.name = "MoveCursorStart";
            }
            moveCursorStart.SetActive(true);
            moveCursorStart.transform.localPosition = new Vector3(startX + 0.5f,
                startY + 0.5f, -0.03f);
            moveCursorStart.GetComponent<SpriteRenderer>().sprite = resources.Sprites.StandOnArrowStartNeutral;
        }
        else
        {
            if (moveCursorStart == null)
            {
                moveCursorStart = GameObject.Instantiate(resources.Prefabs.MoveArrowDot, worldContainer);
                moveCursorStart.name = "MoveCursorStart";
            }
          
            moveCursorStart.SetActive(true);
            moveCursorStart.transform.localPosition = new Vector3(startX + 0.5f,
                startY + 0.5f, -0.03f);
            moveCursorStart.GetComponent<SpriteRenderer>().sprite = resources.Sprites.StandOnArrowStart;
            var v = new Vector2(startX, startY);
            if (v.x - mousePath[0].x > 0)
                moveCursorStart.transform.rotation = Quaternion.Euler(0, 0, 180);
            else if (v.x - mousePath[0].x < 0)
                moveCursorStart.transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (v.y - mousePath[0].y > 0)
                moveCursorStart.transform.rotation = Quaternion.Euler(0, 0, 270);
            else if (v.y - mousePath[0].y < 0)
                moveCursorStart.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        for (var i = 0; i < mousePath.Count; i++)
        {
            var v = mousePath[i];
            GameObject dot = null;
            if (i >= instantiatedMovePath.Count)
            {
                dot = Instantiate(resources.Prefabs.MoveArrowDot, worldContainer);
                instantiatedMovePath.Add(dot);
            }
            else
            {
                dot = instantiatedMovePath[i];
                dot.SetActive(true);
            }
            dot.transform.localPosition = new Vector3(v.x + 0.5f, v.y + 0.5f, -0.03f);
            SpriteRenderer dotSpriteRenderer = dot.GetComponent<SpriteRenderer>();
            if (i == mousePath.Count - 1)
            {
                if (moveCursor == null)
                {
                    moveCursor = Instantiate(resources.Prefabs.MoveCursor, worldContainer);
                    moveCursor.name = "MoveCursor";
                }
                moveCursor.SetActive(true);
                moveCursor.transform.localPosition = new Vector3(v.x, v.y, moveCursor.transform.localPosition.z);
                
                dotSpriteRenderer.sprite = resources.Sprites.MoveArrowHead;
                if (i != 0)
                {
                    if (v.x - mousePath[i - 1].x > 0)
                        dot.transform.rotation = Quaternion.Euler(0, 0, 180);
                    else if (v.x - mousePath[i - 1].x < 0)
                        dot.transform.rotation = Quaternion.Euler(0, 0, 0);
                    else if (v.y - mousePath[i - 1].y > 0)
                        dot.transform.rotation = Quaternion.Euler(0, 0, 270);
                    else if (v.y - mousePath[i - 1].y < 0)
                        dot.transform.rotation = Quaternion.Euler(0, 0, 90);
                }
                else
                {
                    if (v.x - startX > 0)
                        dot.transform.rotation = Quaternion.Euler(0, 0, 180);
                    else if (v.x - startX < 0)
                        dot.transform.rotation = Quaternion.Euler(0, 0, 0);
                    else if (v.y - startY > 0)
                        dot.transform.rotation = Quaternion.Euler(0, 0, 270);
                    else if (v.y - startY < 0)
                        dot.transform.rotation = Quaternion.Euler(0, 0, 90);
                }
            }
            else
            {
                var vAfter = mousePath[i + 1];
                Vector2 vBefore;
                if (i != 0)
                {
                    vBefore = mousePath[i - 1];
                    DrawCurvedMovementPathSection(dot, dotSpriteRenderer, v, vBefore, vAfter);
                }
                else
                {
                    vBefore = new Vector2(startX, startY);
                    DrawCurvedMovementPathSection(dot, dotSpriteRenderer, v, vBefore, vAfter);
                }
            }
            
        }
    }
    public void HideMovementPath()
    {
        if (!MovementPathVisible)
            return;
        MovementPathVisible = false;
        for(int i=0; i< instantiatedMovePath.Count; i++)
        {
            instantiatedMovePath[i].SetActive(false);
        }
        //instantiatedMovePath.Clear();
        if (moveCursor != null)
            moveCursor.SetActive(false);//GameObject.Destroy(moveCursor);
        if (moveCursorStart != null)
            moveCursorStart.SetActive(false);
    }
    public void DrawCurvedMovementPathSection(GameObject dot, SpriteRenderer sr, Vector2 v, Vector2 vBefore, Vector2 vAfter)
    {
        if (vBefore.x == vAfter.x)
        {
            sr.sprite = resources.Sprites.MoveArrowStraight;
            dot.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (vBefore.y == vAfter.y)
        {
            sr.sprite = resources.Sprites.MoveArrowStraight;
            dot.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            sr.sprite = resources.Sprites.MoveArrowCurve;
            if (vBefore.x - vAfter.x > 0)
            {
                if (vBefore.y - vAfter.y > 0)
                {
                    if (vBefore.x == v.x)
                        dot.transform.rotation = Quaternion.Euler(0, 0, 90);
                    else
                        dot.transform.rotation = Quaternion.Euler(0, 0, 270);
                }

                else
                {
                    if (vBefore.x == v.x)
                        dot.transform.rotation = Quaternion.Euler(0, 0, 180);
                    else
                        dot.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            else if (vBefore.x - vAfter.x < 0)
            {
                if (vBefore.y - vAfter.y > 0)
                {
                    if (vBefore.x == v.x)
                        dot.transform.rotation = Quaternion.Euler(0, 0, 0);
                    else
                        dot.transform.rotation = Quaternion.Euler(0, 0, 180);
                }
                else
                {
                    if (vBefore.x == v.x)
                        dot.transform.rotation = Quaternion.Euler(0, 0, 270);
                    else
                        dot.transform.rotation = Quaternion.Euler(0, 0, 90);
                }
            }
        }
    }
}