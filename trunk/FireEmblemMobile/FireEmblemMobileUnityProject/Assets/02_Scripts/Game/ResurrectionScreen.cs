using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.Utility;
using UnityEngine;

namespace LostGrace
{
    public class ResurrectionScreen : MonoBehaviour
    {
        public GameObject resurrectionPrefab;
        public event Action<Unit> onUnitChosen;
        [SerializeField] Canvas canvas;
        public void Show(List<Unit> deadUnits)
        {
            canvas.enabled = true;
            transform.DeleteAllChildren();
            foreach (var unit in deadUnits)
            {
                var go = Instantiate(resurrectionPrefab, transform);
                go.GetComponent<ResurrectionUI>().SetUnit(unit);
                go.GetComponent<ResurrectionUI>().OnClicked+=ChooseUnit;
            }
            
        }

        public void ChooseUnit(Unit unit)
        {
            onUnitChosen?.Invoke(unit);
            Hide();
        }

        public void Hide()
        {
            canvas.enabled = false;
        }
    }
}
