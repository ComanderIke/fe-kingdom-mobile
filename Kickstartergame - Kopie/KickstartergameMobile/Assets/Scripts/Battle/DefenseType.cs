using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class DefenseType
{
    public string Name { get; set; }
    public int Hit { get; set; }
    public float Atk_Mult { get; set; }

    public DefenseType(string Name, float Atk_Mult, int Hit)
    {
        this.Name = Name;
        this.Atk_Mult = Atk_Mult;
        this.Hit = Hit;
    }
}
