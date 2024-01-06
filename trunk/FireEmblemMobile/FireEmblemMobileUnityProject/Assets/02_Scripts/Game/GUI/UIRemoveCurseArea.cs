using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using __2___Scripts.Game.Utility;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.WorldMapStuff.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class UIRemoveCurseArea : MonoBehaviour
    {
        private List<Curse> curses;
        public int curseIndex = 0;
        [SerializeField] private Image selectedCurse;
        [SerializeField] private Image prevCurse;
        [SerializeField] private Image nextCurse;
        [SerializeField] public int removeCurseCost = 200;
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private Button removeCurseButton;
        [SerializeField] private Color tooExpensiveTextColor;
        [SerializeField] private float faithPriceReduction = 5;
        [SerializeField] private GameObject noCurseArea;
        [SerializeField] private GameObject cursedArea;
        public void Show(Unit unit)
        {
            gameObject.SetActive(true);
            curses = unit.Curses;
            if (curses.Count == 0)
            {
                noCurseArea.gameObject.SetActive(true);
                cursedArea.gameObject.SetActive(false);
                
            }
            else
            {
                noCurseArea.gameObject.SetActive(false);
                cursedArea.gameObject.SetActive(true);
            }
            
           
            curseIndex = 0;
            bool affordable = Player.Instance.Party.CanAfford(removeCurseCost);
            removeCurseButton.interactable =affordable ;
            costText.color = affordable ? Color.white:tooExpensiveTextColor;
            costText.SetText(""+CalculateFaithPriceReduction(unit.Stats.CombinedAttributes().FAITH));
            UpdateUI();
           
        }

        public int CalculateFaithPriceReduction(int faith)
        {
        
            return (int)Math.Max(0,(removeCurseCost)*(1-((faith*faithPriceReduction)/100f)));
        }

        void UpdateUI()
        {
            if (curses.Count == 0)
            {
                selectedCurse.sprite=null;
                nextCurse.sprite = null;
                prevCurse.sprite = null;
                return;
            }
                
            int prevIndex = curseIndex - 1;
            if (prevIndex < 0)
            {
                prevIndex = curses.Count - 1;
            }

            int nextIndex = curseIndex + 1;
            if (nextIndex >= curses.Count)
            {
                nextIndex = 0;
            }
            Debug.Log("Curse Index: "+curseIndex+" next: "+nextIndex+" prev: "+prevIndex);
            Debug.Log(curses[curseIndex].Name+" "+curses[nextIndex].Name+" "+curses[prevIndex].Name);
            prevCurse.sprite = curses[prevIndex].Icon;
            nextCurse.sprite = curses[nextIndex].Icon;
            selectedCurse.sprite = curses[curseIndex].Icon;
            nextCurse.transform.parent.gameObject.SetActive(nextIndex!=curseIndex&& (nextIndex !=prevIndex||nextIndex>curseIndex));
            prevCurse.transform.parent.gameObject.SetActive(prevIndex!=curseIndex&& (prevIndex!= nextIndex||prevIndex<curseIndex));
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void NextClicked()
        {
            Debug.Log("Next");
            curseIndex++;
            if (curseIndex >= curses.Count)
                curseIndex = 0;
            UpdateUI();
        }

        public void PrevClicked()
        {
            Debug.Log("Prev");
            curseIndex--;
            if (curseIndex < 0)
                curseIndex = curses.Count-1;
            UpdateUI();
        }

       
    }
}