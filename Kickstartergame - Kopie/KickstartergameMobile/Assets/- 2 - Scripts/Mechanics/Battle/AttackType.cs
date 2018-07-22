using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
[CreateAssetMenu(menuName ="GameData/Unit/AttackType", fileName ="AttackType")]
public class AttackType : ScriptableObject
{
    public int Hit;
    public float DamageMultiplier;
    public int SpeedPenalty;
}
