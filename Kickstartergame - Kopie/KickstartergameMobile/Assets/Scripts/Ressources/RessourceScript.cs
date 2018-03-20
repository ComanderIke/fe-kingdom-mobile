using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceScript : MonoBehaviour {

    public Sprite swordActiveSprite;
    public Sprite archerActiveSprite;
    public Sprite axeActiveSprite;
    public Sprite lancerActiveSprite;
    public Sprite mammothSprite;
    public Sprite sabertoothSprite;
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private Sprite[] specialAttackSprites;
    [SerializeField]
    private GameObject[] specialAttackPrefabs;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public Sprite GetSprite(int spriteId)
    {
        if(spriteId>= 0 && spriteId < sprites.Length)
            return sprites[spriteId];
        return null;
    }
    public Sprite GetSpecialAttackSprite(int spriteId)
    {
        if (spriteId >= 0 && spriteId < specialAttackSprites.Length)
            return specialAttackSprites[spriteId];
        return null;
    }
    public GameObject GetSpecialAttackPrefab(int prefabId)
    {
        if (prefabId >= 0 && prefabId < specialAttackPrefabs.Length)
            return specialAttackPrefabs[prefabId];
        return null;
    }
}
