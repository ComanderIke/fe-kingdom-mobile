using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using UnityEngine;

public class EncounterPlayerUnitController : MonoBehaviour
{
    public GameObject blueRing;

    public Unit Unit;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public int baseSortOrder = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUnit(Unit unit)
    {
        this.Unit = unit;
    }
    public void SetSortOrder(int order)
    {
        spriteRenderer.sortingOrder =baseSortOrder+ order;
    }
    public void SetActiveUnit(bool active)
    {
        blueRing.SetActive(active);
    }
}
