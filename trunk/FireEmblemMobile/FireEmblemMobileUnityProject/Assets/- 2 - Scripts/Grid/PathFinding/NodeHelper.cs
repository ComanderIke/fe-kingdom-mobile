using Assets.Scripts.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NodeHelper
{
    int width;
    int height;
    public NodeHelper(int width, int height)
    {
        this.width = width;
        this.height = height;
        nodes = new Node[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                nodes[i, j] = new Node(i, j, 1000);
            }
        }
    }
    public Node[,] nodes;

    public bool nodeFaster(int x, int y, int c)
    {
        if (nodes[x, y].c < c)
            return false;
        return true;
    }
    public void Reset()
    {
        nodes = new Node[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                nodes[i, j] = new Node(i, j, 1000);
            }
        }
    }
}
