﻿using System.Collections.Generic;

public class Column
{
    public List<EncounterNode> children;
    public int index;
    public bool battle = false;

    public Column()
    {
        children = new List<EncounterNode>();
    }
}