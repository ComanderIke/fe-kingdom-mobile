using Assets.Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackPreview : MonoBehaviour {

    [SerializeField]
    private Text dmgText;
    [SerializeField]
    private Text hitText;
    [SerializeField]
    private Text attackCountText;
    [SerializeField]
    private Image dmgBar;
    [SerializeField]
    private Image hitBar;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void UpdateValues(int dmg, int hit, int attackCount)
    {
        dmgText.text = "" + dmg;
        hitText.text = "" + hit;
        attackCountText.text = "" + attackCount;
        dmgBar.fillAmount = MathUtility.MapValues(dmg, 0, 10, 0, 1);
        hitBar.fillAmount = MathUtility.MapValues(hit, 0, 100, 0, 1);
    }
}
