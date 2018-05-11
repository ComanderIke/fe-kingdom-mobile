using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class DefenseType
{
    public string Name { get; set; }
    public int Hit { get; set; }
    public float DamageMultiplier { get; set; }

    public DefenseType(string Name, float Atk_Mult, int Hit)
    {
        this.Name = Name;
        this.DamageMultiplier = Atk_Mult;
        this.Hit = Hit;
    }
}
