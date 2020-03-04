using Assets.Core;
using Assets.GameActors.Units;
using Assets.GameResources;
using Assets.Mechanics;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.GUI
{
    public class UiSystem : MonoBehaviour, IEngineSystem
    {
        #region Events

        public delegate void OnContinuePressedEvent();

        public static OnContinuePressedEvent OnContinuePressed;

        public delegate void OOnFrontalAttackAnimationEndEvent();

        public static OOnFrontalAttackAnimationEndEvent OnFrontalAttackAnimationEnd;

        public delegate void OnAttackUiVisibleEvent(bool visible);

        public static OnAttackUiVisibleEvent OnAttackUiVisible;

        public delegate void OnDeselectButtonClickedEvent();

        public static OnDeselectButtonClickedEvent OnDeselectButtonClicked;

        public delegate void OnShowCursorEvent(int x, int y);

        public static OnShowCursorEvent OnShowCursor;

        public delegate void OnHideCursorEvent();

        public static OnHideCursorEvent OnHideCursor;


        #endregion
        [SerializeField] private Canvas mainCanvas = default;
        [Header("Screens")]
        [SerializeField] public AttackUiController AttackUiController;
        [SerializeField] private GameObject winScreen = default;
        [SerializeField] private GameObject gameOverScreen = default;


        [Header("UI Sections")]
        [SerializeField] private GameObject bottomUi = default;
        [SerializeField] private TopUi topUi = default;
        [SerializeField] private TopUi topUiEnemy = default;
        [SerializeField] private GameObject attackPreview = default;
        [Header("Buttons")]
        [SerializeField] private Button deselectButton = default;


        [Header("Animations")]
        [SerializeField] private GameObject playerTurnAnimation = default;
        [SerializeField] private GameObject aiTurnAnimation = default;

        private Dictionary<string, GameObject> activeUnitEffects;
        private MainScript mainScript;
        private GameObject tileCursor;
        private ResourceScript resources;
        private List<GameObject> attackableEnemyEffects;
        private List<GameObject> attackableFieldEffects;

        private void Start()
        {
            mainScript = MainScript.Instance;
            OnShowCursor += SpawnTileCursor;
            OnHideCursor += HideTileCursor;
            Unit.UnitShowActiveEffect += SpawnActiveUnitEffect;
            UnitActionSystem.OnSelectedCharacter += ShowDeselectButton;
            UnitActionSystem.OnDeselectCharacter += HideDeselectButton;

            activeUnitEffects = new Dictionary<string, GameObject>();
            attackableEnemyEffects = new List<GameObject>();
            attackableFieldEffects = new List<GameObject>();
            resources = FindObjectOfType<ResourceScript>();
        }

        private void ShowDeselectButton()
        {
            deselectButton.gameObject.SetActive(true);
        }

        private void HideDeselectButton()
        {
            deselectButton.gameObject.SetActive(false);
        }

        public void DeselectButtonClicked()
        {
            OnDeselectButtonClicked();
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

        public void ShowAttackableEnemy(int x, int y)
        {
            if (attackableEnemyEffects.Any(gameObj => (int) gameObj.transform.localPosition.x == x && (int) gameObj.transform.localPosition.y == y))
            {
                return;
            }

            var go = Instantiate(resources.Prefabs.AttackableEnemyPrefab,
                GameObject.FindGameObjectWithTag("World").transform);
            go.transform.localPosition = new Vector3(x, y, go.transform.localPosition.z);
            attackableEnemyEffects.Add(go);
        }

        public void HideAttackableEnemy()
        {
            foreach (var go in attackableEnemyEffects)
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
                var go = activeUnitEffects[unit.Name];

                activeUnitEffects.Remove(unit.Name);
                Destroy(go);
            }

            if (spawn)
            {
                var go = Instantiate(resources.Prefabs.MoveCursor,
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
            HideAllActiveUnitEffects();
            AttackUiController.Show(attacker, defender);
        }

        public void HideFightUi()
        {
            if (AttackUiController.isActiveAndEnabled)
                AttackUiController.Hide();
            if (mainScript.PlayerManager.ActivePlayer.IsPlayerControlled)
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
            Instantiate(playerTurnAnimation, new Vector3(), Quaternion.identity, mainCanvas.transform).transform
                .localPosition = new Vector3();
        }

        public void EnemyTurnAnimation()
        {
            HideAllActiveUnitEffects();
            Instantiate(aiTurnAnimation, new Vector3(), Quaternion.identity, mainCanvas.transform).transform
                .localPosition = new Vector3();
        }

        public void ShowTopUi(Unit c)
        {
            
            if (c.Player.Id != mainScript.PlayerManager.ActivePlayer.Id)
            {
                topUiEnemy.Show(c);

                topUi.Hide();
            }
            else
            {
                topUiEnemy.Hide();

                topUi.Show(c);
            }
        }


        public void UndoClicked()
        {
            UnitActionSystem.OnUndo();
        }

        public void EndTurnClicked()
        {
            TurnSystem.OnEndTurn();
        }

        public void ShowAttackableField(int x, int y)
        {
            if (attackableFieldEffects.Any(gameObj => (int) gameObj.transform.localPosition.x == x && (int) gameObj.transform.localPosition.y == y))
            {
                return;
            }

            var go = Instantiate(resources.Particles.EnemyField,
                GameObject.FindGameObjectWithTag("World").transform);
            go.transform.localPosition = new Vector3(x + 0.5f, y + 0.5f, go.transform.localPosition.z - 0.1f);
            attackableFieldEffects.Add(go);
        }

        public void HideAttackableField()
        {
            Debug.Log("hideAttackableField!");
            foreach (var go in attackableFieldEffects)
            {
                Destroy(go);
            }

            attackableFieldEffects.Clear();
        }

        public void ShowAttackPreview(Unit attacker, Unit defender)
        {
            ShowAttackableEnemy(defender.GridPosition.X, defender.GridPosition.Y);
            attackPreview.SetActive(true);
            attackPreview.GetComponent<AttackPreview>().UpdateValues(attacker.Stats.MaxHp, attacker.Hp,
                defender.Stats.MaxHp, defender.Hp, attacker.BattleStats.GetDamageAgainstTarget(defender),
                attacker.BattleStats.GetAttackCountAgainst(defender));
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
            OnAttackUiVisible = null;
            OnDeselectButtonClicked = null;
            OnFrontalAttackAnimationEnd = null;
            OnHideCursor = null;
            OnShowCursor = null;
            OnContinuePressed = null;
        }
    }
}