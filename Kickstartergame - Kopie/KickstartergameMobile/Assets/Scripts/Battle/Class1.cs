using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AttackType
{
    public int Hit { get; set; }
    public float DamageMultiplier { get; set; }
    public string Name { get; set; }
    public int SpeedPenalty { get; set; }
    public AttackType(string Name, float DamageMultiplier, int Hit, int SpeedPenalty)
    {
        this.Name = Name;
        this.DamageMultiplier = DamageMultiplier;
        this.Hit = Hit;
        this.SpeedPenalty = SpeedPenalty;
    }
}
