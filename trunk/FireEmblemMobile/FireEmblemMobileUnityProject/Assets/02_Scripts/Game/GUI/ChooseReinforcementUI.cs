using System;
using Game.GameActors.Player;
using Game.GameActors.Units;
using Game.GUI.CharacterScreen;
using Game.GUI.Controller;
using Game.GUI.Other;
using Game.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    public class ChooseReinforcementUI : MonoBehaviour
    {
   
        public Action OnFinished { get; set; }
    
        [SerializeField] private TextMeshProUGUI chooseButtonText;
        [SerializeField] private UIDetailedCharacterViewController charView1;
   
        [SerializeField] private UIDetailedCharacterViewController charView2;
        [SerializeField] private UIDetailedCharacterViewController charView3;
        [SerializeField] private Button charView1Button;
        [SerializeField] private Button charView2Button;
        [SerializeField] private Button charView3Button;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI headline;

        public event Action<Unit> onUnitChosen;
        private Unit unit1, unit2, unit3;
        private bool unitChosen = false;
        public void Show(Unit unit1, Unit unit2, Unit unit3)
        {
            BottomUI.Hide();
            PlayerPhaseUI.StaticHide();
        
            chooseButtonText.SetText("</>Choose");
            canvasGroup.gameObject.SetActive(true);
            TweenUtility.FadeIn(canvasGroup);
            this.unit1 = unit1;
            this.unit2 = unit2;
            this.unit3 = unit3;
            unitChosen = false;
            selected = null;
            UpdateUI();
        }

        private UIDetailedCharacterViewController selected;
    
        public void CharView1Clicked()
        {
            if (selected != null)
                selected.Deselect();
            selected = charView1;
            chooseButtonText.SetText("<bounce>Choose");
            selected.Select();
        }
        public void CharView2Clicked()
        {
            if (selected != null)
                selected.Deselect();
            selected = charView2;
            chooseButtonText.SetText("<bounce>Choose");
            selected.Select();
        }
        public void CharView3Clicked()
        {
            if (selected != null)
                selected.Deselect();
            selected = charView3;
            chooseButtonText.SetText("<bounce>Choose");
            selected.Select();
        }

   
        public void ChooseClicked()
        {
            MyDebug.LogInput("Choose Clicked");
            if (selected == null||unitChosen)
                return;
            unitChosen = true;
            Player.Instance.Party.AddMember(selected.unit);
            MyDebug.LogTODO("Add Unit to PAr");
            MonoUtility.DelayFunction(()=>
            {
            
                onUnitChosen?.Invoke(selected.unit);
                Hide();
            }, 0.5f);
        }

        void UpdateUI()
        {
            if(unit1!=null)
                charView1.Show(unit1, true);
            if(unit2!=null)
                charView2.Show(unit2, true);
            if(unit3!=null)
                charView3.Show(unit3, true);
        }

        public void Hide()
        {
            TweenUtility.FadeOut(canvasGroup).setOnComplete(()=>
            {
                MyDebug.LogTest("Choose Skill Renderer Finished");
                OnFinished?.Invoke();
                // chooseSkill1.Hide();
                // chooseSkill2.Hide();
                // chooseSkill3.Hide();
                canvasGroup.gameObject.SetActive(false);
            });
        
        }
    
    }
}