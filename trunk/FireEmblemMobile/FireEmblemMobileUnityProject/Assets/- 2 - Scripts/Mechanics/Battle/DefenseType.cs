using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Unit/DefenseType", fileName = "DefenseType")]
public class DefenseType: ScriptableObject
{
    public int Hit;
    public float DamageMultiplier;


}
