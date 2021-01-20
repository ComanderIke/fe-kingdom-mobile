using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using Game.GameActors.Units;
using Game.GameResources;
using Game.Grid;
using Game.GUI;
using Game.GUI.PopUpText;
using Game.GUI.Text;
using Game.Manager;
using Game.Mechanics;
using UnityEngine;
using Utility;

namespace Game.GameInput
{
    [Serializable]
    public class Visuals : MonoBehaviour
    {
        // [SerializeField] private Transform uiContainer = default;
        // [SerializeField] private ExpBarController expBarController = default;
        // [SerializeField] private Canvas MainCanvas = default;
        // private ResourceScript resources;
        [SerializeField]
        private MoveArrowVisual moveArrowVisual;
        // private Dictionary<string, GameObject> activeUnitEffects;
        // private GridGameManager gridGameManager;
        //
        // [SerializeField] private GameObject playerTurnAnimation = default;
        // [SerializeField] private GameObject aiTurnAnimation = default;
        //
        // private List<GameObject> attackableEnemyEffects;
        // private List<GameObject> attackableFieldEffects;
         private void Start ()
         {
             //GridInputSystem.OnResetInput += moveArrowVisual.HideMovementPath;
             InputPathManager.OnReset += moveArrowVisual.HideMovementPath;
             InputPathManager.OnMovementPathUpdated += moveArrowVisual.DrawMovementPath;
             BattleState.OnEnter += moveArrowVisual.HideMovementPath;
             UnitSelectionSystem.OnDeselectCharacter += moveArrowVisual.HideMovementPath;

             // GridInputSystem.OnMovementPathUpdated += moveArrowVisual.OnMovementPathUpdated;
             //     resources = FindObjectOfType<ResourceScript>();
             //     activeUnitEffects = new Dictionary<string, GameObject>();
             //     gridGameManager = GridGameManager.Instance;
             //     attackableEnemyEffects = new List<GameObject>();
             //     attackableFieldEffects = new List<GameObject>();
             //     PlayerTurnTextAnimation.OnStarted += TurnAnimationStarted;
             //     PlayerTurnTextAnimation.OnFinished += TurnAnimationFinished;
             //    
             //     GridInputSystem.OnDraggedOnActiveField += HideAttackableEnemy;
             //     UnitSelectionSystem.OnDeselectCharacter += DeselectedCharacter;
             //     UnitSelectionSystem.OnSelectedCharacter += SelectedCharacter;
             //
             //     GridInputSystem.OnSetActive += InputActive;
             //     GridInputSystem.OnDragReset += HideAttackableEnemy;
             //     BattleState.OnEnter += HideAllActiveUnitEffects;
             //     BattleState.OnEnter += HideAttackableEnemy;
             //     BattleState.OnExit += OnExitBattleState;
             //     TurnSystem.OnStartTurn += OnStartTurn;
             //     Unit.OnUnitActiveStateUpdated += OnActiveUnitStateUpdate;
             //     MovementState.OnEnter += HideAttackableField;
             //     BattleState.OnEnter += HideAttackableField;
             //     MovementState.OnEnter += HideAllActiveUnitEffects;
             //     MovementState.OnMovementFinished += (Unit u)=>HideMovementPath();

             //    // UnitActionSystem.OnCheckAttackPreview += OnCheckAttackPreview;
             //     MovementState.OnMovementFinished += (Unit u)=>HideAttackableEnemy();
             //     GridRenderer.OnRenderEnemyTile += OnRenderEnemyTile;
             //     Unit.OnExpGained += ExpGained;
             //     GridInputSystem.OnInputActivated += InputActivated;
             //     UnitSelectionSystem.OnEnemySelected += EnemySelected;
             //     BattleRenderer.OnAttackConnected += SlashAnimation;
             //     Unit.OnUnitDamaged += UnitDamaged;
             //     InstantiateGameObjects();//Performance Do this on Start
             //     DrawMovementPath(new List<Vector2>() {new Vector2(1,0), new Vector2(2, 0), new Vector2(3, 0) },0,0);
             //     HideMovementPath();
         }
        // private void InstantiateGameObjects()
        // {
        //     ShowAttackableField(0, 0);
        //     ShowAttackableField(0, 1);
        //     ShowAttackableField(0, 2);
        //     ShowAttackableField(0, 3);
        //     ShowAttackableField(0, 4);
        //     HideAttackableField();
        // }
        // private void UnitDamaged(Unit unit , int damage)
        // {
        //     DamagePopUp.Create(unit.GameTransform.GetCenterPosition(), damage, Color.red);
        // }
        // private void SlashAnimation(Unit attacker, Unit defender)
        // {
        //     Vector3 spawnPos = defender.GameTransform.GetPosition();
        //     spawnPos.x += 0.5f;
        //     spawnPos.y += 0.5f;//Spawn in Middle of GridCell
        //     if(attacker.Faction.Id==0)
        //         Instantiate(resources.Prefabs.slashBlue, spawnPos, Quaternion.identity);
        //     else
        //         Instantiate(resources.Prefabs.slashRed, spawnPos, Quaternion.identity);
        // }
        // private void EnemySelected(Unit u) {
        //     HideAllActiveUnitEffects();
        // }
        // private void DeselectedCharacter()
        // {
        //     ShowAllActiveUnitEffects();
        //     HideMovementPath();
        //     HideAttackableField();
        //     HideAttackableEnemy();
        // }
        // bool setActiveUnitEffectsWhenInputIsActive = false;
        // private void InputActivated()
        // {
        //     if (setActiveUnitEffectsWhenInputIsActive)
        //         ShowAllActiveUnitEffects();
        //     setActiveUnitEffectsWhenInputIsActive = false;
        // }
        // private void ExpGained(Unit unit, int currentExp, int expGained)
        // {
        //     AnimationQueue.Add(() => { 
        //         GridInputSystem.OnSetActive(false, this); 
        //         expBarController.Show(currentExp, expGained);
        //     }, ()=> GridInputSystem.OnSetActive(true, this));
        //     //AnimationQueue.OnAllAnimationsEnded += SetInputToOldState;
        // }
        // private void TurnAnimationStarted()
        // {
        //     GridInputSystem.OnSetActive(false, this);
        // }
        // private void TurnAnimationFinished()
        // {
        //     //Set to State Before TurnAnimationStarted
        //     GridInputSystem.OnSetActive(true, this);
        // }
        // //private void OnMovementPathUpdated(List<Vector2> mousePath, int startX, int startY)
        // //{
        // //    HideAttackableEnemy();
        // //}
        // private void OnActiveUnitStateUpdate(Unit u, bool canMove, bool disableOthers)
        // {
        //     if (u.Faction != null && u.Faction.Id != gridGameManager.FactionManager.GetPlayerControlledFaction().Id)
        //         return;
        //     HideActiveUnitEffect(u);
        //     if (disableOthers)
        //         HideAllActiveUnitEffects();
        //     if (canMove)
        //     {
        //         SpawnActiveUnitEffect(u);
        //     }
        // }
        //
        // private void OnRenderEnemyTile(int x, int y, Unit enemy, int playerId)
        // {
        //     if (enemy != null && enemy.Faction.Id != playerId)
        //         ShowAttackableField(x, y);
        // }
        // private void OnCheckAttackPreview(Unit u, Unit defender)
        // {
        //     HideAttackableEnemy();
        //     ShowAttackableEnemy(defender.GridPosition.X, defender.GridPosition.Y);
        // }
        // private void OnStartTurn()
        // {
        //     if (!gridGameManager.FactionManager.ActiveFaction.IsPlayerControlled)
        //     {
        //         gridGameManager.GetSystem<AudioSystem>().ChangeMusic("EnemyTheme", "PlayerTheme", true);
        //
        //         EnemyTurnAnimation();
        //
        //         HideAllActiveUnitEffects();
        //     }
        //     else
        //     {
        //         gridGameManager.GetSystem<AudioSystem>().ChangeMusic("PlayerTheme", "EnemyTheme", true);
        //         PlayerTurnAnimation();
        //     }
        //
        // }
        // public void ShowAttackableEnemy(int x, int y)
        // {
        //     if (attackableEnemyEffects.Any(gameObj => (int)gameObj.transform.localPosition.x == x && (int)gameObj.transform.localPosition.y == y))
        //     {
        //         attackableEnemyEffects.Find(gameObj => (int)gameObj.transform.localPosition.x == x && (int)gameObj.transform.localPosition.y == y)
        //             .SetActive(true);
        //         return;
        //     }
        //
        //     var go = Instantiate(resources.Prefabs.AttackableEnemyPrefab,
        //         GameObject.FindGameObjectWithTag("World").transform);
        //     go.transform.localPosition = new Vector3(x, y, go.transform.localPosition.z);
        //     attackableEnemyEffects.Add(go);
        // }
        // public void ShowAttackableField(int x, int y)
        // {
        //     //Debug.Log("Show Attackable Field: "+x+" " +y );
        //     if (attackableFieldEffects.Any(gameObj => !gameObj.activeSelf||(gameObj.transform.localPosition.x-0.5f==x && gameObj.transform.localPosition.y-0.5f == y)))
        //     {
        //         GameObject go2 = attackableFieldEffects.Find(gameObj => gameObj.transform.localPosition.x - 0.5f == x && gameObj.transform.localPosition.y - 0.5f == y||!gameObj.activeSelf);
        //         go2.transform.localPosition = new Vector3(x + 0.5f, y + 0.5f, go2.transform.localPosition.z);
        //         go2.SetActive(true);
        //         return;
        //     }
        //
        //     var go = Instantiate(resources.Prefabs.attackIconPrefab,
        //         null);
        //     go.transform.localPosition = new Vector3(x + 0.5f, y + 0.5f, go.transform.localPosition.z);
        //     attackableFieldEffects.Add(go);
        // }
        //
        // public void HideAttackableField()
        // {
        //     foreach (var go in attackableFieldEffects)
        //     {
        //         go.SetActive(false);
        //     
        //     }
        //
        //     //attackableFieldEffects.Clear();
        // }
        //
        // public void HideAttackableEnemy()
        // {
        //     foreach (var go in attackableEnemyEffects)
        //     {
        //         go.SetActive(false);
        //     }
        //
        //     //attackableEnemyEffects.Clear();
        // }
        // public void PlayerTurnAnimation()
        // {
        //     Instantiate(playerTurnAnimation, new Vector3(), Quaternion.identity, uiContainer).transform
        //         .localPosition = new Vector3();
        // }
        // public void EnemyTurnAnimation()
        // {
        //
        //     Instantiate(aiTurnAnimation, new Vector3(), Quaternion.identity, uiContainer).transform
        //         .localPosition = new Vector3();
        // }
        // private void OnExitBattleState()
        // {
        //     if (GridGameManager.Instance.FactionManager.ActiveFaction.IsPlayerControlled)
        //         ShowAllActiveUnitEffects();
        // }
        // private void InputActive(bool active, object caller)
        // {
        //     if (active)
        //         ShowAllActiveUnitEffects();
        //     else
        //         HideAllActiveUnitEffects();
        // }
        // private void SelectedCharacter(Unit u)
        // {
        //     HideAllActiveUnitEffects();
        //     SpawnActiveUnitEffect(u);
        // }
        // private void HideActiveUnitEffect(Unit unit)
        // {
        //     //if (activeUnitEffects.ContainsKey(unit.Name))
        //     //{
        //     //    var gob = activeUnitEffects[unit.Name];
        //     //    gob.SetActive(false);
        //     //    //activeUnitEffects.Remove(unit.Name);
        //     //    //GameObject.Destroy(gob);
        //     //}
        // }
        // private void SpawnActiveUnitEffect(Unit unit)
        // {
        //     //HideActiveUnitEffect(unit);
        //     //Debug.Log("Spawn ActiveUnitEffect: [" + unit.GridPosition.X + "/" + unit.GridPosition.Y + "]");
        //
        //     //var go = GameObject.Instantiate(resources.Prefabs.ActiveUnitField,
        //     //    GameObject.FindGameObjectWithTag("World").transform);
        //     //go.transform.localPosition =
        //     //    new Vector3(unit.GridPosition.X, unit.GridPosition.Y, go.transform.localPosition.z);
        //     //activeUnitEffects.Add(unit.Name, go);
        //
        //     //go.name = "ActiveUnitEffect";
        // }
        // public void HideAllActiveUnitEffects()
        // {
        //     //setActiveUnitEffectsWhenInputIsActive = false;
        //     //foreach (var pair in activeUnitEffects)
        //     //{
        //     //    pair.Value.SetActive(false);
        //     //}
        //     //InputSystem.OnInputActivated -= ShowAllActiveUnitEffects;
        // }
        // public void ShowAllActiveUnitEffects()
        // {
        //     //if (!InputSystem.Active)
        //     //{
        //     //    InputSystem.OnInputActivated += ShowAllActiveUnitEffects;
        //     //    return;
        //     //}
        //     //InputSystem.OnInputActivated -= ShowAllActiveUnitEffects;
        //     ////Debug.Log("ShowAllActiveUnitEffects "+activeUnitEffects.Count);
        //     //foreach (var pair in activeUnitEffects)
        //     //{
        //     //    pair.Value.SetActive(true);
        //     //}
        // }
        //
        // 
    }
}