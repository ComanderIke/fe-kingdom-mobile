using Assets.Scripts.Characters.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LevelUpOrbs : MonoBehaviour {

    

    // Use this for initialization
    void Start () {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane + 5));
        transform.LookAt(Camera.main.transform);
        CameraMovement.locked = true;
        CharacterScript.lockInput = true;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
       
    }
    List<OrbType> usedOrbs;
    public void LevelUp(Character c)
    {
        usedOrbs = new List<OrbType>();
        int totalSum = 0;
        foreach(Orb o in c.charclass.orbs)
        {
            totalSum += o.relativeProb;
        }
        for (int j=0; j< GetComponentsInChildren<OrbScript>().Length;j++)
        {
            int index = Random.Range(1, totalSum+1);
            int sum = 0;
            int i = 0;
            while(sum < index)
            {
                sum = sum + c.charclass.orbs[i++].relativeProb;
            }
            OrbType type = c.charclass.orbs[(int)Mathf.Max(0, i - 1)].type;
            if (!usedOrbs.Contains(type))
            {
                GetComponentsInChildren<OrbScript>()[j].SetOrb(c, c.charclass.orbs[(int)Mathf.Max(0, i - 1)]);
                usedOrbs.Add(type);
            }
            else
            {
                j--;
            }

        }
    }
}
