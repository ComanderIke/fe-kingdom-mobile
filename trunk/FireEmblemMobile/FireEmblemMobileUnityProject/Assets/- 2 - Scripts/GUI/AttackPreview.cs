using Assets.Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackPreview : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI hpText;
    [SerializeField]
    private TextMeshProUGUI hpEnemyText;
    [SerializeField]
    private TextMeshProUGUI dmgText;
    [SerializeField]
    private TextMeshProUGUI hitText;
    [SerializeField]
    private TextMeshProUGUI attackCountText;
    [SerializeField]
    private Image HpBar;
    [SerializeField]
    private Image HpBarEnemy;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void UpdateValues(int maxHP, int hp, int maxHPEnemy, int hpEnemy, int dmg, int hit, int attackCount)
    {
        dmgText.text = "" + dmg;
        hitText.text = "" + hit+"%";
        attackCountText.text = " x " + attackCount;
        hpText.text = ""+hp;
        hpEnemyText.text = ""+hpEnemy;
        HpBar.fillAmount = MathUtility.MapValues(hp, 0, maxHP, 0, 1);
        HpBarEnemy.fillAmount = MathUtility.MapValues(hpEnemy, 0, maxHPEnemy, 0, 1);
    }
}
