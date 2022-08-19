using System;
using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.GameActors.Units;
using UnityEngine;

public class EncounterPlayerUnitController : MonoBehaviour
{

    public Unit Unit;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public int baseSortOrder = 10;

    public float speed = 2f;

    private Transform follow;

    public float baseOffset = 0.0f;

    private float offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void Clicked()
    {
        if(!UIClickChecker.CheckUIObjectsInPosition())
            FindObjectOfType<UICharacterViewController>().Show(Unit);
    }

    // Update is called once per frame
    void Update()
    {

        if (offset == 0)
            offset = baseOffset;
        if (follow != null && Vector2.Distance(transform.position, follow.position) > offset)
        {
            //Debug.Log("Distance: "+Vector2.Distance(transform.position, follow.position)+" "+transform.position+" "+follow.position);
            transform.position = Vector2.MoveTowards(transform.position, follow.position, speed * Time.deltaTime);
        }
    }

    public void SetTarget(Transform target)
    {
        follow = target;
    }

    
    public void SetUnit(Unit unit)
    {
        this.Unit = unit;
    }
    public void SetSortOrder(int order)
    {
        spriteRenderer.sortingOrder =baseSortOrder+ order;
    }
    // public void SetActiveUnit(bool active)
    // {
    //     Debug.Log("Show Active Unit Effect On direct Unit?");
    //     }

    public void SetOffsetCount(int cnt)
    {
        this.offset = baseOffset * cnt;
      
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
