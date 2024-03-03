using System;
using System.Collections;
using System.Collections.Generic;
using Game.EncounterAreas.Encounters;
using Game.EncounterAreas.Management;
using Game.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.EncounterAreas.Controller
{
    public class EncounterNodeClickController : MonoBehaviour
    {
        public EncounterNode encounterNode;
        private AreaGameManager areaGameManager;
        private bool IsPointerOverUIObject() {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        private void Start()
        {
            areaGameManager=FindObjectOfType<AreaGameManager>();
            areaGameManager.SubscribeNodeClickController(this);

        }

  

        private void OnMouseDown()
        {
            dragTime = 0;
            //Debug.Log("NodeOnMouseDown");
            if (!IsPointerOverUIObject())
            {
                // Debug.Log("NoUI");
                ServiceProvider.Instance.StartChildCoroutine(DelayBy1Frame(()=>  areaGameManager.NodeClicked(encounterNode)));
          
            }
        }

 
        private float dragTime = 0;
        private Coroutine coroutine;
        private void OnMouseDrag()
        {
            if (!IsPointerOverUIObject())
            {
                dragTime += Time.deltaTime;
         
                coroutine=ServiceProvider.Instance.StartChildCoroutine(DelayBy1Frame(()=>  FindObjectOfType<AreaGameManager>().NodeHolding(encounterNode, dragTime)));
          
            }
        }


        private void OnMouseUp()
        {
            if(coroutine!=null)
                ServiceProvider.Instance.StopChildCoroutine(coroutine);
        }

        private void OnMouseExit()
        {
        
            dragTime =0;
        }

  

        IEnumerator DelayBy1Frame(Action action)
        {
            yield return null;
            action?.Invoke();
        }
    }
}
