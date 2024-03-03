using System.Collections.Generic;
using System.Linq;
using Game.DataAndReferences.Data;
using Game.EncounterAreas.Encounters.Battle;
using Game.GameActors.Items.Consumables;
using Game.GameActors.Player;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills.Active;
using Game.GameInput.Raycasts;
using Game.GUI.CharacterCircleUI;
using Game.GUI.Controller;
using Game.GUI.Convoy;
using Game.GUI.Interface;
using Game.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI.Other
{
    [ExecuteInEditMode]
    public class UIUnitPlacement : IUnitPlacementUI
    {
        [SerializeField]
        private GameObject unitPrefab;
        // [SerializeField]
        // private Camera camera;
        private RaycastManager RaycastManager { get; set; }
        [SerializeField]
        private List<Unit> units;

        public GameObject unitFieldAnimationPrefab;

        private bool dragInitiated;
        private bool dragStarted;
        [SerializeField] private GameObject PrepUI;
        [SerializeField] private Button ShowPrepUIButton;
        [SerializeField] private Button StartButton;

        [SerializeField] private IUnitSelectionUI unitSelectionUI;
        [SerializeField] private UIObjectiveController conditionUI;
        [SerializeField] private UIFactionCharacterCircleController charCircleController;
        [SerializeField] private Image darkenBackground;
        private List<Unit> selectedUnits;
        // Update is called once per frame
 

        private Canvas canvas;
        void Start()
        {
            canvas = GetComponent<Canvas>();
      
        }

        private void OnEnable()
        {
            RaycastManager = new RaycastManager();
            unitSelectionUI.unitSelectionChanged += InvokeSelectionChanged;
            //  UpdateValues();
        }

        private void OnDisable()
        {
            unitSelectionUI.unitSelectionChanged -= InvokeSelectionChanged;
        }

        private void InvokeSelectionChanged(List<Unit> units)
        {
            unitSelectionChanged?.Invoke(units);
            selectedUnits = units;
            StartButton.interactable = units.Count!=0;
        }

        // private void UpdateValues()
        // {
        //     for (int i=layoutGroup.childCount-1; i>=0; i--){
        //         DestroyImmediate(layoutGroup.transform.GetChild(i).gameObject);
        //     }
        //
        //     if(units!=null)
        //         foreach (Unit u in units)
        //         {
        //            
        //             var go = Instantiate(unitPrefab, layoutGroup, false);
        //             go.GetComponent<UIUnitDragObject>().UnitPlacement = this;
        //             go.GetComponent<UIUnitDragObject>().SetUnitSprite(u.visuals.CharacterSpriteSet.MapSprite);
        //         }
        // }

        public void StartClicked()
        {
            OnFinished?.Invoke();
            Hide();
        }

        public override void Show(List<Unit> units, BattleMap chapter)
        {
            this.units = units;
            this.chapter = chapter;
            if (selectedUnits == null|| selectedUnits.Count==0)
                selectedUnits = units;
            //UpdateValues();
            GetComponent<Canvas>().enabled = true;
            // Debug.Log("party size: "+Player.Instance.Party.members.Count);
            charCircleController.Show(units);
        
        }
    
        public override void Hide()
        {
            if (gameObject == null)
                return;
            GetComponent<Canvas>().enabled = false;
        }



        private void HideGrid()
        {
            PrepUI.SetActive(false);
            ShowPrepUIButton.gameObject.SetActive(true);
        }
        public void ShowGrid()
        {
            //darkenBackground.enabled = true;
            PrepUI.SetActive(true);
            ShowPrepUIButton.gameObject.SetActive(false);
        }
        public void MapButtonCLicked()
        {
            Debug.Log("Map Button Clicked!");
            //darkenBackground.enabled = false;
            HideGrid();
        }
        public void ConditionButtonClicked()
        {
            conditionUI.Show(chapter);
            Hide();
        }
        public void Show()
        {
            GetComponent<Canvas>().enabled = true;

        }

        //[SerializeField] private SmokeScreenSkillEffectMixin smokeScreenSkillEffectMixin;
        public void ExitButtonClicked()
        {
            // SaveGameManager.Save();
            // SceneController.LoadSceneAsync(Scenes.TestScene, false);
            Player.Instance.Party.Convoy.AddItem(GameBPData.Instance.GetItemByName("SmokeBomb"));
            if (Player.Instance.Party.Convoy.ContainsItem(new Bomb(null, "SmokeBomb", "", 0, 0, 0, null)))
            {
                var smokeBomb =
                    Player.Instance.Party.Convoy.Items.First(a => a.item is Bomb && a.item.Name == "SmokeBomb").item as Bomb;
                ((SelfTargetSkillMixin)smokeBomb.skill.FirstActiveMixin).Activate(null);
                Hide();
                //smokeScreenSkillEffectMixin.Activate(null, 1);
            }
            else
            {
                foreach (var member in Player.Instance.Party.members)
                {
                    if (member.CombatItem1 != null && member.CombatItem1.item.GetName() == "SmokeBomb")
                    {
                        var smokeBomb = (Bomb)member.CombatItem1.item;
                        ((SelfTargetSkillMixin)smokeBomb.skill.FirstActiveMixin).Activate(null);
                        Hide();
                        return;
                    }
                }
            }
        
            //GameSceneController.Instance.LoadEncounterAreaBeforeBattle();
        }
        public void UnitButtonClicked()
        {
            unitSelectionUI.Show(units,selectedUnits);
            Hide();
        }

        [SerializeField] private UIConvoyController convoyController;
        public void ConvoyButtonClicked()
        {
            Debug.Log("ConvoyButtonClicked");
            convoyController.Show();
            convoyController.OnHide += OnConvoyHide;
            Hide();
        }

        void OnConvoyHide()
        {
            convoyController.OnHide -= OnConvoyHide;
            Show();
        }
        public void PlaceholderButtonCLicked()
        {
            Debug.Log("Placeholder Button Clicked!");
        }
        // public override void OnDrag(UIUnitDragObject uiUnitDragObject, PointerEventData eventData)
        // {
        //     uiUnitDragObject.rectTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;
        // }

        // public override void OnEndDrag(UIUnitDragObject uiUnitDragObject, PointerEventData eventData)
        // {
        //
        //     Debug.Log(eventData.position);
        //
        //     var screenRay = Camera.main.ScreenPointToRay(eventData.position);
        //     // Perform Physics2D.GetRayIntersection from transform and see if any 2D object was under transform.position on drop.
        //     RaycastHit2D hit2D = Physics2D.GetRayIntersection(screenRay);
        //     if (hit2D)
        //     {
        //         Debug.Log(hit2D.transform.gameObject.name);
        //         var dropComponent = hit2D.transform.gameObject.GetComponent<IDropHandler>();
        //         if (dropComponent != null)
        //             dropComponent.OnDrop(eventData);
        //     }
        // }

        private void OnDestroy()
        {
            if(convoyController!=null)
                convoyController.OnHide -= OnConvoyHide;
        }
    }
}
