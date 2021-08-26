using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.Mechanics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public Image faceSpriteLeft;

    public Image faceSpriteRight;
    [SerializeField] private TextMeshProUGUI attackCount = default;
    [SerializeField] private GameObject attackCountX = default;
    [SerializeField] private TextMeshProUGUI dmgValue = default;
    [SerializeField] private TextMeshProUGUI hitValue = default;
    [SerializeField] private TextMeshProUGUI dmgValueRight = default;
    [SerializeField] private TextMeshProUGUI hitValueRight = default;
    [SerializeField] private TextMeshProUGUI attackCountRight = default;
    [SerializeField] private GameObject attackCountRightX = default;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Show(BattleSimulation battleSimulation)
    {
        Unit attacker = (Unit) battleSimulation.Attacker;
        Unit defender = (Unit) battleSimulation.Defender;
        faceSpriteLeft.sprite = attacker.visuals.CharacterSpriteSet.FaceSprite;
        faceSpriteRight.sprite = defender.visuals.CharacterSpriteSet.FaceSprite;
        dmgValue.text = "" + battleSimulation.AttackerDamage;
        hitValue.text = "" + battleSimulation.AttackerHit;
        dmgValueRight.text = "" + battleSimulation.DefenderDamage;
        hitValueRight.text = "" + battleSimulation.DefenderHit;
        attackCountX.SetActive(battleSimulation.AttackerAttackCount>1);
        attackCount.gameObject.SetActive(attackCountX.activeSelf);
        attackCount.text = "" + battleSimulation.AttackerAttackCount;
        attackCountRightX.SetActive(battleSimulation.DefenderAttackCount > 1);
        attackCountRight.gameObject.SetActive(attackCountRightX.activeSelf);
        attackCountRight.text = "" + battleSimulation.DefenderAttackCount;
    }
}
