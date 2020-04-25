using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Assets.GameResources;
using Assets.Grid;
using Assets.GameActors.Units.Monsters;
using Assets.Mechanics;
using Assets.GameInput;
using Assets.GameActors.Units;
using Assets.Core.GameStates;
using Assets.Core;
using Assets.Manager;
using Assets.Audio;

public class PlayerInputFeedback :MonoBehaviour
{
    [SerializeField] private Transform uiContainer = default;
    [SerializeField] private Transform worldContainer = default;
    private ResourceScript resources;
    private GameObject moveCursor;
    private GameObject moveCursorStart;
    private List<GameObject> instantiatedMovePath;
    private Dictionary<string, GameObject> activeUnitEffects;
    private GridGameManager gridGameManager;
    
    [SerializeField] private GameObject playerTurnAnimation = default;
    [SerializeField] private GameObject aiTurnAnimation = default;
    void Start ()
    {
        instantiatedMovePath = new List<GameObject>();
    
        resources = FindObjectOfType<ResourceScript>();
        activeUnitEffects = new Dictionary<string, GameObject>();
        gridGameManager = GridGameManager.Instance;
        InputSystem.OnDragReset += HideMoventPath;
        InputSystem.OnMovementPathUpdated += DrawMovementPath;
        UnitSelectionSystem.OnDeselectCharacter += ShowAllActiveUnitEffects;
        UnitSelectionSystem.OnSelectedCharacter += SelectedCharacter;
        InputSystem.OnSetActive += InputActive;
        BattleState.OnEnter += HideAllActiveUnitEffects;
        BattleState.OnExit += OnExitBattleState;
        TurnSystem.OnStartTurn += OnStartTurn;
    }
  
    private void OnStartTurn()
    {
        
        if (!gridGameManager.FactionManager.ActiveFaction.IsPlayerControlled)
        {
            Debug.Log("AITurn");
            gridGameManager.GetSystem<AudioSystem>().ChangeMusic("EnemyTheme", "PlayerTheme", true);

            EnemyTurnAnimation();

            HideAllActiveUnitEffects();
        }
        else
        {
            Debug.Log("PlayerTurn");
            gridGameManager.GetSystem<AudioSystem>().ChangeMusic("PlayerTheme", "EnemyTheme", true);
            PlayerTurnAnimation();
        }
        
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
    private void InputActive(bool active)
    {
        if (active)
            ShowAllActiveUnitEffects();
        else
            HideAllActiveUnitEffects();
    }
    private void SelectedCharacter(Unit u)
    {
        HideAllActiveUnitEffects();
        if (!u.UnitTurnState.HasMoved)
            SpawnActiveUnitEffect(u, true, false);
    }
    private void SpawnActiveUnitEffect(Unit unit, bool spawn, bool disableOthers)
    {
        //foreach (KeyValuePair<string, GameObject> pair in activeUnitEffects)
        //{
        //    pair.Value.SetActive(true);
        //}
        if (activeUnitEffects.ContainsKey(unit.Name))
        {
            var go = activeUnitEffects[unit.Name];

            activeUnitEffects.Remove(unit.Name);
            GameObject.Destroy(go);
        }

        if (spawn)
        {
            var go = GameObject.Instantiate(resources.Prefabs.MoveCursor,
                GameObject.FindGameObjectWithTag("World").transform);
            go.transform.localPosition =
                new Vector3(unit.GridPosition.X, unit.GridPosition.Y, go.transform.localPosition.z);
            activeUnitEffects.Add(unit.Name, go);
            go.name = "ActiveUnitEffect";
        }
        else
        {
            if (disableOthers)
                foreach (var pair in activeUnitEffects)
                {
                    pair.Value.SetActive(false);
                }
        }
    }
    public void HideAllActiveUnitEffects()
    {
        foreach (var pair in activeUnitEffects)
        {
            pair.Value.SetActive(false);
        }
    }

    public void ShowAllActiveUnitEffects()
    {
        foreach (var pair in activeUnitEffects)
        {
            pair.Value.SetActive(true);
        }
    }
    public void DrawMovementPath(List<Vector2> mousePath, int startX, int startY)
    {
        HideMoventPath();
        instantiatedMovePath = new List<GameObject>();
        if (moveCursor != null)
            GameObject.Destroy(moveCursor);
        if (moveCursorStart != null)
            GameObject.Destroy(moveCursorStart);
        if (mousePath.Count == 0)
        {
            moveCursor = GameObject.Instantiate(resources.Prefabs.MoveCursor, worldContainer);
            moveCursor.transform.localPosition = new Vector3(startX,
                startY, moveCursor.transform.localPosition.z);
        }
        else
        {
            moveCursorStart = GameObject.Instantiate(resources.Prefabs.MoveArrowDot, worldContainer);
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

            var dot = GameObject.Instantiate(resources.Prefabs.MoveArrowDot, worldContainer);
            dot.transform.localPosition = new Vector3(v.x + 0.5f, v.y + 0.5f, -0.03f);
            if (i == mousePath.Count - 1)
            {
                moveCursor = GameObject.Instantiate(resources.Prefabs.MoveCursor, worldContainer);
                moveCursor.transform.localPosition = new Vector3(v.x, v.y, moveCursor.transform.localPosition.z);
                dot.GetComponent<SpriteRenderer>().sprite = resources.Sprites.MoveArrowHead;
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
                    DrawCurvedMovementPathSection(dot, v, vBefore, vAfter);
                }
                else
                {
                    vBefore = new Vector2(startX, startY);
                    DrawCurvedMovementPathSection(dot, v, vBefore, vAfter);
                }
            }
            instantiatedMovePath.Add(dot);
        }
    }

    public void HideMoventPath()
    {
        for(int i=0; i< instantiatedMovePath.Count; i++)
        {
            GameObject.Destroy(instantiatedMovePath[i]);
        }
        instantiatedMovePath.Clear();
        if (moveCursor != null)
            GameObject.Destroy(moveCursor);
        if (moveCursorStart != null)
            GameObject.Destroy(moveCursorStart);
    }
    public void DrawCurvedMovementPathSection(GameObject dot, Vector2 v, Vector2 vBefore, Vector2 vAfter)
    {
        if (vBefore.x == vAfter.x)
        {
            dot.GetComponent<SpriteRenderer>().sprite = resources.Sprites.MoveArrowStraight;
            dot.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (vBefore.y == vAfter.y)
        {
            dot.GetComponent<SpriteRenderer>().sprite = resources.Sprites.MoveArrowStraight;
            dot.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            dot.GetComponent<SpriteRenderer>().sprite = resources.Sprites.MoveArrowCurve;
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