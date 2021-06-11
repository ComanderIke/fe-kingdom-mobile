using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Grid;
using Game.Mechanics;
using UnityEngine;

namespace Game.Graphics
{
    [Serializable]
    public class Visuals
    {
        [SerializeField]
        public List<CharacterSpriteSet> characterSpriteSets;
        [SerializeField]
        public List<IUnitEffectVisual> unitVisualEffects;

        public CharacterSpriteSet LoadCharacterSpriteSet(string id)
        {
            foreach (var spriteSet in characterSpriteSets)
            {
                if (spriteSet.name == id)
                    return spriteSet;
                
            }

            return null;
        }
        // [SerializeField] private Transform uiContainer = default;
        // [SerializeField] private ExpBarController expBarController = default;
        // [SerializeField] private Canvas MainCanvas = default;
        // private ResourceScript resources;

   
        // private Dictionary<string, GameObject> activeUnitEffects;
        // private GridGameManager gridGameManager;
        //
        // [SerializeField] private GameObject playerTurnAnimation = default;
        // [SerializeField] private GameObject aiTurnAnimation = default;
        //

         private void Start ()
         {
             //GridInputSystem.OnResetInput += moveArrowVisual.HideMovementPath;
             //InputPathManager.OnReset += ()=>moveArrowVisual.DrawMovementPath(null, );
             
           //  UnitSelectionSystem.OnDeselectCharacter += DeselectedCharacter;
             // MovementState.OnEnter += attackTargetVisual.HideAttackableField;
             // BattleState.OnEnter += attackTargetVisual.HideAttackableField;
             // UnitActionSystem.OnCheckAttackPreview += OnCheckAttackPreview;
             // GridInputSystem.OnMovementPathUpdated += moveArrowVisual.OnMovementPathUpdated;
             //     resources = FindObjectOfType<ResourceScript>();
             //     activeUnitEffects = new Dictionary<string, GameObject>();
             //     gridGameManager = GridGameManager.Instance;
             //     
             //     PlayerTurnTextAnimation.OnStarted += TurnAnimationStarted;
             //     PlayerTurnTextAnimation.OnFinished += TurnAnimationFinished;
             //    
              //    GridInputSystem.OnDraggedOnActiveField += HideAttackableEnemy;
                  
             //     UnitSelectionSystem.OnSelectedCharacter += SelectedCharacter;
             //
             //     GridInputSystem.OnSetActive += InputActive;
             //     GridInputSystem.OnDragReset += HideAttackableEnemy;
             //     BattleState.OnEnter += HideAllActiveUnitEffects;
             //     BattleState.OnEnter += HideAttackableEnemy;
             //     BattleState.OnExit += OnExitBattleState;
             //     TurnSystem.OnStartTurn += OnStartTurn;
             //     Unit.OnUnitActiveStateUpdated += OnActiveUnitStateUpdate;
                
             //     MovementState.OnEnter += HideAllActiveUnitEffects;
             //     MovementState.OnMovementFinished += (Unit u)=>HideMovementPath();

              
             //     MovementState.OnMovementFinished += (Unit u)=>HideAttackableEnemy();
                  
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
        private void DeselectedCharacter()
        {
            //ShowAllActiveUnitEffects();
            // attackTargetVisual.HideAttackableField();
           // HideAttackableEnemy();
        }
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

        private void OnCheckAttackPreview(IBattleActor u, IBattleActor defender)
        {
            // attackTargetVisual.HideAttackableEnemy();
            // if( defender is IGridActor gridActor)
            //     attackTargetVisual.ShowAttackableEnemy(gridActor.GridPosition.X, gridActor.GridPosition.Y);
        }
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
        public IUnitEffectVisual LoadUnitEffectVisual(string id)
        {
            foreach (var vfx in unitVisualEffects)
            {
                if (vfx.name == id)
                    return vfx;
                
            }

            return null;
        }
    }
}