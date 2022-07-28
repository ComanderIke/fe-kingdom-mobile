using System;
using System.Linq;
using Game.Grid;
using TMPro;
using UnityEngine;

namespace Game.GUI
{
    [ExecuteInEditMode]
    public class UIObjectiveController : IObjectiveUI
    {
        [SerializeField] private TextMeshProUGUI victoryObjectivesText;
        [SerializeField] private TextMeshProUGUI defeatConditionsText;
        [SerializeField] private TextMeshProUGUI chapterNameText;
        [SerializeField] private BattleMap chapter;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnEnable()
        {
            if(chapter!=null)
                UpdateContent();
        }

        private void UpdateContent()
        {
            chapterNameText.SetText(chapter.name);
            VictoryDefeatCondition[] victoryConditions = chapter.victoryDefeatConditions.Where(condition => condition.victory).ToArray();
            VictoryDefeatCondition[] defeatConditions = chapter.victoryDefeatConditions.Where(condition => condition.victory==false).ToArray();
            string text = victoryConditions[0].description;
            for (int i = 1; i < victoryConditions.Length; i++)
            {
                text +=  "\n"+victoryConditions[i].description;
            }
            victoryObjectivesText.SetText(text);
            text = defeatConditions[0].description;
            for (int i = 1; i < defeatConditions.Length; i++)
            {
                text +=  "\n"+defeatConditions[i].description;
            }
            defeatConditionsText.SetText(text);
        }
        public override void Show(BattleMap chapter)
        {
            this.chapter = chapter;
            GetComponent<Canvas>().enabled = true;
            UpdateContent();

        }
        

        public override void Hide()
        {
            GetComponent<Canvas>().enabled = false;
        }
    }
}
