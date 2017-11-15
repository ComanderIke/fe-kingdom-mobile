using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Grid.PathFinding
{
    public class AStar
    {
        private ArrayList closed;
        private ArrayList open;
        public Node[,] nodes;
        private int width;
        private int height;
        GridManager gridManager;
        public AStar(GridManager gridManager, int width, int height)
        {
            this.gridManager = gridManager;
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
        public void Reset()
        {
            nodes = new Node[width, height];
            closed = new ArrayList();
            open = new ArrayList();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    nodes[i, j] = new Node(i, j, 1000);
                }
            }
        }
        private void addToClosed(Node node)
        {
            closed.Add(node);
        }
        private void addToOpen(Node node)
        {
            open.Add(node);
        }
        private bool InClosedList(Node node)
        {
            return closed.Contains(node);
        }
        private Node getFirstInOpen()
        {
            return (Node)open[0];
        }
        private bool InOpenList(Node node)
        {
            return open.Contains(node);
        }
        public bool nodeFaster(int x, int y, int c)
        {
            if (nodes[x, y].c < c)
                return false;
            return true;
        }

        private void removeFromClosed(Node node)
        {
            closed.Remove(node);
        }

        private void removeFromOpen(Node node)
        {
            open.Remove(node);
        }
       
        public MovementPath findPath(int sx, int sy, int tx, int ty, int team, bool toadjacentPos, List<int> range)
        {
            nodes[sx, sy].costfromStart = 0;
            nodes[sx, sy].depth = 0;
            closed.Clear();
            open.Clear();
            open.Add(nodes[sx, sy]);
            nodes[tx, ty].parent = null;
            int maxDepth = 0;
            int maxSearchDistance = 100;
            while ((maxDepth < maxSearchDistance) && (open.Count != 0))
            {
                Node current = getFirstInOpen();
                if (current == nodes[tx, ty])
                {
                    break;
                }
                removeFromOpen(current);
                addToClosed(current);
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        if (x == 0 && y == 0)
                            continue;
                        if (x != 0 && y != 0)   //no diagonal movement
                            continue;
                        int xp = x + current.x;
                        int yp = y + current.y;
                        bool isAdjacent = false;
                        if (toadjacentPos)
                        {
                            int delta = Mathf.Abs(xp - nodes[tx, ty].x) + Mathf.Abs(yp - nodes[tx, ty].y);
                            range.Reverse();
                            foreach (int r in range)
                            {
                                if (delta == r)
                                {

                                    isAdjacent = true;

                                    MainScript m = MainScript.GetInstance();
                                    if (m.AttackRangeFromPath < r)
                                    {
                                        m.AttackRangeFromPath = r;
                                        // break;
                                    }
                                }
                            }
                            range.Reverse();
                        }
                        if (gridManager.GridLogic.IsValidLocation(team, sx, sy, xp, yp, isAdjacent) || (xp == tx && yp == ty))
                        {
                            int nextStepCost = current.costfromStart + 1;
                            Node neighbour = nodes[xp, yp];
                            if (nextStepCost < neighbour.costfromStart)
                            {
                                if (InOpenList(neighbour))
                                {
                                    removeFromOpen(neighbour);
                                }
                                if (InClosedList(neighbour))
                                {
                                    removeFromClosed(neighbour);
                                }
                            }
                            if (!InOpenList(neighbour) && !InClosedList(neighbour))
                            {
                                neighbour.costfromStart = nextStepCost;
                                maxDepth = Mathf.Max(maxDepth, neighbour.setParent(current));
                                addToOpen(neighbour);
                            }
                        }
                    }
                }
            }
            if (nodes[tx, ty].parent == null)
            {
                return null;
            }
            MovementPath path = new MovementPath();
            Node target = nodes[tx, ty];
            while (target != nodes[sx, sy])
            {
                path.prependStep(target.x, target.y);
                target = target.parent;
            }
            path.prependStep(sx, sy);
            return path;

        }
        public MovementPath getPath(int x, int y, int x2, int y2, int team, bool toadjacentPos, List<int> range)
        {
            MainScript.GetInstance().AttackRangeFromPath = 0;
            nodes = new Node[width, height];
            closed = new ArrayList();
            open = new ArrayList();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    nodes[i, j] = new Node(i, j, 1000);
                }
            }

            return findPath(x, y, x2, y2, team, toadjacentPos, range);
        }
    }
}
