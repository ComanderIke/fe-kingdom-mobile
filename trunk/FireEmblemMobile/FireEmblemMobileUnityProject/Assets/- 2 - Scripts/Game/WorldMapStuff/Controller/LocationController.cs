using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.Systems;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Systems;
using UnityEngine;

public class LocationController : MonoBehaviour
{
    // Start is called before the first frame update
    public  IWorldMapLocationInputReceiver inputReceiver { get; set; }
    public LocationRenderer renderer;

    public WorldMapPosition worldMapPosition;
    private WM_Actor actor;
    public WM_Actor Actor
    {
        get => actor;
        set
        {
           // Debug.Log("Set Actor: "+gameObject.name);
            if (actor != null)
            {
                actor.TurnStateManager.onSelected -= OnSelectedActor;
                actor.TurnStateManager.UnitCanMove -= OnActorMoved;
                SetActive(false);
            }

            actor = value;
          
            if (actor != null)
            {
                SetActive(true);
                actor.location = this;
                if(actor.GameTransformManager.GameObject!=null)
                    actor.GameTransformManager.SetParent(transform);
                actor.TurnStateManager.onSelected += OnSelectedActor;
                actor.TurnStateManager.UnitCanMove += OnActorMoved;
            }
            worldMapPosition.UpdatePositionLocations();
        }
    }
    public void OnDestroy(){
        if(Actor!=null)
            Actor.TurnStateManager.onSelected -= OnSelectedActor;
    }

    void OnActorMoved(bool canMove)
    {
        if(!canMove)
            worldMapPosition.HideInteractableConnections();
    }
    void OnSelectedActor(bool selected)
    {
        
        if(selected)
            worldMapPosition.DrawInteractableConnections();


    }
    private bool active;

    public void SetActive(bool value)
    {
        active = value;
        gameObject.SetActive(value);
       
    }
    public bool IsActive()
    {
        return active;
    }

    public void SetPosition(float xValue)
    {
        transform.position = new Vector3(xValue, transform.position.y, transform.position.z);
        // if(actor!= null && actor.GameTransformManager.GameObject!=null)
        //     actor.GameTransformManager.SetPosition(transform.position);
    }
    public bool IsFree()
    {
        return actor == null;
    }
   

    private void OnEnable()
    {
        renderer = GetComponent<LocationRenderer>();
       
    }
    public void Select(bool selected)
    {
        renderer.ShowSelected(selected);
    }
  
    public void OnMouseDown()
    {
        inputReceiver.LocationClicked(this);
    }

    public void Hide()
    {
        renderer.Hide();
    }

    public void Reset()
    {
        renderer.Reset();
    }

    public LocationData GetSaveData()
    {
        return new LocationData(this);
    }

    public void LoadData(LocationData locationData)
    {
        Actor = locationData.actor;
       // locationData.name Also not helpful?
       // worldMapPosition = locationData.worldMapPosition; Not helpful?
    }
}
