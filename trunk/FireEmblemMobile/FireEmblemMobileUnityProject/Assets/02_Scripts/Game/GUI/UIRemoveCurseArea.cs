using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using __2___Scripts.Game.Utility;
using Game.GameActors.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class UIRemoveCurseArea : MonoBehaviour
    {
        private List<Curse> curses;
        private int curseIndex = 0;
        [SerializeField] private Image selectedCurse;
        [SerializeField] private Image prevCurse;
        [SerializeField] private Image nextCurse;
        public void Show(Unit unit)
        {
            curses = unit.Curses;
            
            gameObject.SetActive(true);
            curseIndex = 0;
            UpdateUI();
           
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
            nextCurse.gameObject.SetActive(nextIndex!=curseIndex&& (nextIndex !=prevIndex||nextIndex>curseIndex));
            prevCurse.gameObject.SetActive(prevIndex!=curseIndex&& (prevIndex!= nextIndex||prevIndex<curseIndex));
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