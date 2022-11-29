using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using TMPro;
using UnityEngine;

public class UIRecruitCharacterController : UIButtonController
{
    public new TextMeshProUGUI name;
    // Start is called before the first frame update
    public void SetValues(Unit unit)
    {
        name.SetText(unit.name);
        SetValues(unit.visuals.CharacterSpriteSet.FaceSprite, 50, "Class");
    }
}
