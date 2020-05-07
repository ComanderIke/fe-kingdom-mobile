using Assets.Core;
using Assets.GameEngine.GameStates;
using Assets.GameActors.Units;
using Assets.GameInput;
using Assets.GameResources;
using Assets.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;
using UnityEngine.UI;
using Assets.GameEngine;
using Assets.Game.Manager;

namespace Assets.GUI
{
    public class UiSystem : MonoBehaviour, IEngineSystem
    {
        #region Events
        #endregion

        [SerializeField] private Canvas mainCanvas = default;
        [Header("Screens")]
        [SerializeField] public BattleRenderer BattleRenderer;
        [SerializeField] private GameObject winScreen = default;
        [SerializeField] private GameObject gameOverScreen = default;
        [SerializeField] private LevelUpScreenController levelUpScreen = default;
     

        [Header("UI Sections")]
        [SerializeField] private CanvasGroup fadeImage = default;

        [SerializeField] private GameObject bottomUi = default;
        [SerializeField] private TopUi topUi = default;
        [SerializeField] private GameObject attackPreview = default;
        [Header("Buttons")]
        [SerializeField] private Button deselectButton = default;

        private GridGameManager gridGameManager;
        private GameplayInput gameplayInput;
        private GameObject tileCursor;
        private ResourceScript resources;


        private void Start()
        {
            gridGameManager = GridGameManager.Instance;
            gameplayInput = new GameplayInput();
            
            UnitSelectionSystem.OnSelectedCharacter += SelectedCharacter;
            UnitSelectionSystem.OnDeselectCharacter += HideDeselectButton;
            
            UnitSelectionSystem.OnEnemySelected += ShowTopUi;
            UnitSelectionSystem.OnSelectedInActiveCharacter += ShowTopUi;

            UnitActionSystem.OnCheckAttackPreview += ShowAttackPreview;
            Unit.OnUnitLevelUp += ShowLevelUpScreen;
            InputSystem.OnDragOverTile += HideAttackPreview;
            InputSystem.OnDragReset += HideAttackPreview;
            GameplayInput.OnViewUnit += ShowTopUi;
            Unit.OnExpGained += ExpGained;
            InputSystem.OnInputActivated += FadeOut;
            InputSystem.OnInputDeactivated += FadeIn;
            resources = FindObjectOfType<ResourceScript>();
        }
        private void FadeIn()
        {
            LeanTween.alphaCanvas(fadeImage, 1f, 0.2f).setEaseOutQuad();
        }
        private void FadeOut()
        {
            LeanTween.alphaCanvas(fadeImage, 0.0f, 0.2f).setEaseInQuad();
        }
        private void ExpGained(Unit unit, int exp, int exp2)
        {
            ShowTopUi(unit);
        }
        private void SelectedCharacter(Unit u)
        {
            ShowDeselectButton();
            ShowTopUi(u);
        }
        private void ShowDeselectButton()
        {
            deselectButton.gameObject.SetActive(true);
        }

        private void HideDeselectButton()
        {
            deselectButton.gameObject.SetActive(false);
        }

        public void ShowLevelUpScreen(string name, int levelBefore, int levelAfter, int [] stats, int[] statIncreases)
        {
            AnimationQueue.Add(()=> { 
                InputSystem.OnSetActive(false, this);
                levelUpScreen.Show(name, levelBefore, levelAfter, stats, statIncreases); },
                ()=> InputSystem.OnSetActive(true, this));
            
        }
        public void DeselectButtonClicked()
        {
            gameplayInput.DeselectUnit();
            //EventContainer.deselectButtonClicked();
        }

        private void SpawnTileCursor(int x, int y)
        {
            if (x == 0 && y == 0)
            {
                Debug.Log("WTF WHY CURSOR POSITION NULL");
            }

            tileCursor = Instantiate(resources.Prefabs.MoveCursor, GameObject.FindGameObjectWithTag("World").transform);
            tileCursor.transform.localPosition = new Vector3(x, y, tileCursor.transform.localPosition.z);
            tileCursor.name = "TileCursor";
        }



        private void HideTileCursor()
        {
            Destroy(tileCursor);
        }

       

     

        public int GetUiHeight()
        {
            return (int) (topUi.GetComponent<RectTransform>().rect.height +
                          bottomUi.GetComponent<RectTransform>().rect.height);
        }

        public int GetReferenceHeight()
        {
            return (int) mainCanvas.GetComponent<CanvasScaler>().referenceResolution.y;
        }

        public void HideBottomUi()
        {
            bottomUi.SetActive(false);
        }

        public void ShowBottomUi()
        {
            bottomUi.SetActive(true);
        }

        public void HideTopUi()
        {
            Debug.Log("Hide TopUi");
            topUi.gameObject.SetActive(false);
        }

        public void ShowFightUi(Unit attacker, Unit defender)
        {
            
            ShowAttackPreview(attacker, defender);
        }

        public void HideFightUi()
        {
            if (BattleRenderer.isActiveAndEnabled)
                BattleRenderer.Hide();
            
        }

        public void ShowGameOver()
        {
            gameOverScreen.SetActive(true);
        }

        public void ShowWinScreen()
        {
            winScreen.SetActive(true);
        }

       

        public void ShowTopUi(Unit c)
        {
            HideAttackPreview();
            topUi.Show(c);
        }


        public void UndoClicked()
        {
            UnitActionSystem.TriggerUndo?.Invoke();
        }

        public void EndTurnClicked()
        {
            TurnSystem.OnTriggerEndTurn();
        }

       

        public void ShowAttackPreview(Unit attacker, Unit defender)
        {

            

            attackPreview.GetComponent<AttackPreviewUI>().UpdateValues(attacker, defender, gridGameManager.GetSystem<BattleSystem>().GetBattlePreview(attacker, defender), attacker.CharacterSpriteSet.FaceSprite, defender.CharacterSpriteSet.FaceSprite);
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
            attackPreview.GetComponent<AttackPreviewUI>().Hide();
            
        }

        private void OnDestroy()
        {

        }
    }
}