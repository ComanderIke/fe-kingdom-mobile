﻿using System;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameInput;
using Game.GameResources;
using Game.Grid;
using Game.Manager;
using Game.Map;
using Game.Mechanics;
using Game.Mechanics.Battle;
using GameEngine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utility;

namespace Game.GUI
{
    public class UiSystem : MonoBehaviour, IEngineSystem
    {
        public IAttackPreviewUI attackPreviewUI;
        public IObjectiveUI objectiveUI;
        public IUnitPlacementUI unitPlacementUI;
        public Canvas MainUICanvas;

        public static event Action OnShowAttackPreview;
        public static event Action OnHideAttackPreview;
        //public UIFactionCharacterCircleController characterCircleController;
        public UIPartyCharacterCircleController characterCircleController;
        public void Init()
        {
           
            //gridGameManager = GridGameManager.Instance;
            //     gameplayInput = new GameplayInput();
            //     

            //     UnitSelectionSystem.OnDeselectCharacter += HideDeselectButton;
            //     
            //     UnitSelectionSystem.OnEnemySelected += ShowTopUi;
            //     UnitSelectionSystem.OnSelectedInActiveCharacter += ShowTopUi;
            //
            //    // 
            //     Unit.OnUnitLevelUp += ShowLevelUpScreen;
            //     GridInputSystem.OnDragOverTile += HideAttackPreview;
            //     GridInputSystem.OnDragReset += HideAttackPreview;
            //     
            //     GameplayInput.OnViewUnit += ShowTopUi;
            //     Unit.OnExpGained += ExpGained;
            //     GridInputSystem.OnInputStateChanged += InputStateChanged;
            //     resources = FindObjectOfType<ResourceScript>();
            //     var pointer = new PointerEventData(EventSystem.current); // pointer event for Execute Verhindert LagSpike bei erstem click
            //     ExecuteEvents.Execute(deselectButton.gameObject, pointer, ExecuteEvents.pointerEnterHandler);
            //     ExecuteEvents.Execute(deselectButton.gameObject, pointer, ExecuteEvents.submitHandler);
            //     ExecuteEvents.Execute(deselectButton.gameObject, pointer, ExecuteEvents.pointerDownHandler);
            //     ExecuteEvents.Execute(deselectButton.gameObject, pointer, ExecuteEvents.pointerUpHandler);
            //     HideDeselectButton();//Start with Button ACtive for Performance Reasons
            //     attackPreview.Hide();
        }

        private void OnEnable()
        {
            attackPreviewOnMapShown = new List<Unit>();
        }

        private void OnDisable()
        {
           // Deactivate();
        }

        public void Deactivate()
        {
            Debug.Log("DEACTIVATE UI SYSTEM================================");
            UnitSelectionSystem.OnSelectedCharacter -= SelectedCharacter;
            UnitSelectionSystem.OnSelectedInActiveCharacter -= SelectedCharacter;
            UnitSelectionSystem.OnEnemySelected -= SelectedEnemyCharacter;
            UnitSelectionSystem.OnDeselectCharacter -= DeselectedCharacter;
            UnitActionSystem.OnCheckAttackPreview -= ShowAttackPreviewUI;
            UnitActionSystem.OnUpdateAttackPreview -= UpdateAttackPreviewUI;
            GridInputSystem.OnResetInput -= HideAttackPreviewUI;//TODO Remove somehow
            GridInputSystem.OnResetInput -=  HideAttackPreviewOnMapHPBar;//TODO Remove somehow
            UnitActionSystem.OnCheckAttackPreview -= ShowAttackPreviewOnMapHPBar;
            UnitActionSystem.OnUpdateAttackPreview -= ShowAttackPreviewOnMapHPBar;
            SelectionUI.OnBackClicked -= HideAttackPreviewUI;
            SelectionUI.OnBackClicked -= HideAttackPreviewOnMapHPBar;
            GridGameManager.Instance.GetSystem<GridSystem>().cursor.OnCursorPositionChanged -= (Vector2Int v)=>
            {
                HideAttackPreviewUI();
                HideAttackPreviewOnMapHPBar();
            };
        }

        public void Activate()
        {
            SelectionUI.OnBackClicked += HideAttackPreviewUI;
            SelectionUI.OnBackClicked += HideAttackPreviewOnMapHPBar;
            UnitSelectionSystem.OnSelectedCharacter += SelectedCharacter;
            UnitSelectionSystem.OnSelectedInActiveCharacter += SelectedCharacter;
            UnitSelectionSystem.OnEnemySelected += SelectedEnemyCharacter;
            UnitSelectionSystem.OnDeselectCharacter += DeselectedCharacter;
            UnitActionSystem.OnCheckAttackPreview += ShowAttackPreviewUI;
            UnitActionSystem.OnCheckAttackPreview += ShowAttackPreviewOnMapHPBar;
            UnitActionSystem.OnUpdateAttackPreview += ShowAttackPreviewOnMapHPBar;
            UnitActionSystem.OnUpdateAttackPreview += UpdateAttackPreviewUI;
            GridInputSystem.OnResetInput += HideAttackPreviewUI;//TODO Remove somehow
            GridInputSystem.OnResetInput += HideAttackPreviewOnMapHPBar;
            GridGameManager.Instance.GetSystem<GridSystem>().cursor.OnCursorPositionChanged += (Vector2Int v)=>
            {
                HideAttackPreviewUI();
                HideAttackPreviewOnMapHPBar();
            };
        }

        public void ShowObjectiveCanvas(BattleMap chapter)
        {
            objectiveUI.Show(chapter);
        }
        private void UpdateAttackPreviewUI(BattlePreview battlePreview)
        {
            if (battlePreview.Attacker is Unit attacker)
            {
                if (battlePreview.Defender is Unit defender)
                    attackPreviewUI.Show(battlePreview, attacker.visuals, defender.visuals);
                else if (battlePreview.TargetObject != null &&
                         battlePreview.TargetObject is Destroyable dest)
                {
         
                    attackPreviewUI.Show(battlePreview, attacker.visuals, dest.Sprite);
                }

            }
        }

        private List<Unit> attackPreviewOnMapShown;
        private void ShowAttackPreviewOnMapHPBar(BattlePreview battlePreview)
        {
            if (battlePreview.Defender is Unit defender)
            {
                defender.visuals.unitRenderer.ShowPreviewHp(battlePreview.DefenderStats.AfterBattleHp);
                attackPreviewOnMapShown.Add(defender);
            }
        }
        private void ShowAttackPreviewUI(BattlePreview battlePreview)
        {
            OnShowAttackPreview?.Invoke();
            if (battlePreview.Attacker is Unit attacker)
            {
                if (battlePreview.Defender is Unit defender)
                    attackPreviewUI.Show(battlePreview, attacker.visuals, defender.visuals);
                else if (battlePreview.TargetObject != null &&
                         battlePreview.TargetObject is Destroyable dest)
                {
         
                    attackPreviewUI.Show(battlePreview, attacker.visuals, dest.Sprite);
                }

            }
        }
        private void HideAttackPreviewOnMapHPBar()
        {
            Debug.Log("HideAttackPreviewOnMapHPBar"+attackPreviewOnMapShown.Count);
            for (int i = attackPreviewOnMapShown.Count - 1; i >= 0; i--)
            {
                attackPreviewOnMapShown[i].visuals.unitRenderer.HidePreviewHp();
                attackPreviewOnMapShown.RemoveAt(i);
            }
        }

        private void HideAttackPreviewUI()
        {
            //Debug.Log("HIDE ATTACK PREVIEW");
            OnHideAttackPreview?.Invoke();
            attackPreviewUI.Hide();
        }
        
        public void SelectedCharacter(IGridActor actor)
        {
            //Debug.Log("DO STUFF HERE");
            // if(actor is Unit u)
            //     characterCircleController.Clicked(u);//characterCircleController.Select(u);
        }
        public void SelectedEnemyCharacter(IGridActor actor)
        {
            // if(actor is Unit u)
            //     enemyCharacterUI.Show(u);
        }
        private void DeselectedCharacter(IGridActor actor)
        {
            //characterUI.Hide();
            // enemyCharacterUI.Hide();
        }
        // #region Events
        // #endregion
        //
        // [SerializeField] private Canvas mainCanvas = default;
        // [Header("Screens")]
        // [SerializeField] public BattleRenderer BattleRenderer;
        // [SerializeField] private GameObject winScreen = default;
        // [SerializeField] private GameObject gameOverScreen = default;
        // [SerializeField] private LevelUpScreenController levelUpScreen = default;
        //
        //
        // [Header("UI Sections")]
        // [SerializeField] private CanvasGroup fadeImage = default;
        //
        // [SerializeField] private GameObject bottomUi = default;
        // [SerializeField] private TopUi topUi = default;
        // [SerializeField] private AttackPreviewUI attackPreview = default;
        // [Header("Buttons")]
        // [SerializeField] private Button deselectButton = default;
        //
        // private GridGameManager gridGameManager;
        // private GameplayInput gameplayInput;
        // private GameObject tileCursor;
        // private ResourceScript resources;
        //
        //
        // private void Start()
        // {
        //     
        // }
        //
        // private void InputStateChanged(bool active)
        // {
        //     if(active)
        //         FadeOut();
        //     else
        //         FadeIn();
        // }
        // private void FadeIn()
        // {
        //     LeanTween.alphaCanvas(fadeImage, 1f, 0.2f).setEaseOutQuad();
        // }
        // private void FadeOut()
        // {
        //     LeanTween.alphaCanvas(fadeImage, 0.0f, 0.2f).setEaseInQuad();
        // }
        // private void ExpGained(Unit unit, int exp, int exp2)
        // {
        //     ShowTopUi(unit);
        // }
    
        // private void ShowDeselectButton()
        // {
        //     deselectButton.gameObject.SetActive(true);
        // }
        //
        // private void HideDeselectButton()
        // {
        //     deselectButton.gameObject.SetActive(false);
        // }
        //
        // public void ShowLevelUpScreen(string name, int levelBefore, int levelAfter, int [] stats, int[] statIncreases)
        // {
        //     AnimationQueue.Add(()=> { 
        //             GridInputSystem.SetActive(false);
        //             levelUpScreen.Show(name, levelBefore, levelAfter, stats, statIncreases); },
        //         ()=> GridInputSystem.SetActive(true));
        //     
        // }
        // public void DeselectButtonClicked()
        // {
        //     gameplayInput.DeselectUnit();
        //     //EventContainer.deselectButtonClicked();
        // }
        //
        // private void SpawnTileCursor(int x, int y)
        // {
        //     if (x == 0 && y == 0)
        //     {
        //         Debug.Log("WTF WHY CURSOR POSITION NULL");
        //     }
        //
        //     tileCursor = Instantiate(resources.Prefabs.MoveCursor, GameObject.FindGameObjectWithTag("World").transform);
        //     tileCursor.transform.localPosition = new Vector3(x, y, tileCursor.transform.localPosition.z);
        //     tileCursor.name = "TileCursor";
        // }
        //
        //
        //
        // private void HideTileCursor()
        // {
        //     Destroy(tileCursor);
        // }
        //
        //
        //
        //
        //
        // public int GetUiHeight()
        // {
        //     return (int) (topUi.GetComponent<RectTransform>().rect.height +
        //                   bottomUi.GetComponent<RectTransform>().rect.height);
        // }
        //
        // public int GetReferenceHeight()
        // {
        //     return (int) mainCanvas.GetComponent<CanvasScaler>().referenceResolution.y;
        // }
        //
        // public void HideBottomUi()
        // {
        //     bottomUi.SetActive(false);
        // }
        //
        // public void ShowBottomUi()
        // {
        //     bottomUi.SetActive(true);
        // }
        //
        // public void HideTopUi()
        // {
        //     Debug.Log("Hide TopUi");
        //     topUi.gameObject.SetActive(false);
        // }
        //
        // public void ShowFightUi(Unit attacker, Unit defender)
        // {
        //     
        //     ShowAttackPreview(attacker, defender);
        // }
        //
        // public void HideFightUi()
        // {
        //     if (BattleRenderer.isActiveAndEnabled)
        //         BattleRenderer.Hide();
        //     
        // }
        //
        // public void ShowGameOver()
        // {
        //     gameOverScreen.SetActive(true);
        // }
        //
        // public void ShowWinScreen()
        // {
        //     winScreen.SetActive(true);
        // }
        //
        //
        //
        // public void ShowTopUi(Unit c)
        // {
        //     HideAttackPreview();
        //     topUi.Show(c);
        // }
        //
        //
        // public void UndoClicked()
        // {
        //     UnitActionSystem.TriggerUndo?.Invoke();
        // }
        //
        // public void EndTurnClicked()
        // {
        //     TurnSystem.OnTriggerEndTurn();
        // }
        //
        //
        //
        // public void ShowAttackPreview(Unit attacker, Unit defender)
        // {
        //     attackPreview.UpdateValues(attacker, defender, gridGameManager.GetSystem<BattleSystem>().GetBattlePreview(attacker, defender), attacker.CharacterSpriteSet.FaceSprite, defender.CharacterSpriteSet.FaceSprite);
        // }
        //
        // public void HideAttackPreview()
        // {
        //     attackPreview.Hide();
        //     
        // }
        //
        // private void OnDestroy()
        // {
        //
        // }
        public void HideObjectiveCanvas()
        {
            objectiveUI.Hide();
        }

    
        public void HideMainCanvas()
        {
            MainUICanvas.enabled = false;
        }

        public void ShowMainCanvas()
        {
            MainUICanvas.enabled = true;
        }
    }
}