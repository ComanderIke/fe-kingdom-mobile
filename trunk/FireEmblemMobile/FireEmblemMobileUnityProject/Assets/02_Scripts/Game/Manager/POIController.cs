using System;
using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace LostGrace
{
    public class POIController : MonoBehaviour
    {
        [SerializeField] private UnityEvent onClickEvent;
        //[SerializeField] private POIManager manager;
        [SerializeField] private Material selectedMaterial;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private SpriteRenderer sr;
        [SerializeField]private bool interactable = true;
        void Start()
        {
        
        }

        void Update()
        {
        
        }

        private void OnMouseDown()
        {
            if (interactable&&!UIClickChecker.CheckUIObjectsInPosition())
            {
                MyDebug.LogInput("POI Clicked: "+gameObject);
                sr.material = selectedMaterial;
                onClickEvent?.Invoke();
            }
        }

        private void OnMouseExit()
        {
            sr.material = defaultMaterial;
        }

        public void SetInteractable(bool b)
        {
            interactable = b;
        }
    }
}
