using UnityEngine;
using System.Collections;

public class ExpScript : MonoBehaviour {

    public float currentXP = 0;
    public float goalXP = 0;
    public float addedExp = 0;
    private bool active = false;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            currentXP += Time.deltaTime * addedExp;
            if (currentXP >= goalXP)
            {
                currentXP = goalXP;
                active = false;
                GameObject.Destroy(GameObject.Find("ExperienceField(Clone)"));
            }
            gameObject.transform.localScale = new Vector3(currentXP / 100, 1, 1);
        }
	}
    public void addExp(int startExp, int addedExp)
    {
        currentXP = startExp;
        this.addedExp = addedExp;
        goalXP = currentXP + addedExp;
        active = true;
    }
}
