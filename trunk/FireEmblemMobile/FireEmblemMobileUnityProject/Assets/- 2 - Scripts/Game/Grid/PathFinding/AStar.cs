using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Map;
using UnityEngine;

namespace Game.Grid.PathFinding
{
    public class AStar
    {
        private ArrayList closed;
        private ArrayList open;
        public Node[,] Nodes;
        private readonly int width;
        private readonly int height;
        private readonly MapSystem gridManager;

        public AStar(MapSystem gridManager, int width, int height)
        {
            this.gridManager = gridManager;
            this.width = width;
            this.height = height;
            Nodes = new Node[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Nodes[i, j] = new Node(i, j, 1000);
                }
            }
            closed = new ArrayList();
            open = new ArrayList();
        }

        private void Reset()
        {
            Nodes = new Node[width, height];
            closed = new ArrayList();
            open = new ArrayList();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Nodes[i, j] = new Node(i, j, 1000);
                }
            }
        }

        private void AddToClosed(Node node)
        {
            closed.Add(node);
        }

        private void AddToOpen(Node node)
        {
            open.Add(node);
        }

        private bool InClosedList(Node node)
        {
            return closed.Contains(node);
        }

        private Node GetFirstInOpen()
        {
            return (Node) open[0];
        }

        private bool InOpenList(Node node)
        {
            return open.Contains(node);
        }

        private void RemoveFromClosed(Node node)
        {
            closed.Remove(node);
        }

        private void RemoveFromOpen(Node node)
        {
            open.Remove(node);
        }

        public MovementPath FindPath(int sx, int sy, int tx, int ty, int team, bool toAdjacentPos, List<int> range)
        {
            Nodes[sx, sy].CostFromStart = 0;
            Nodes[sx, sy].Depth = 0;
            closed.Clear();
            open.Clear();
            open.Add(Nodes[sx, sy]);
            Nodes[tx, ty].Parent = null;
            int maxDepth = 0;
            int maxSearchDistance = 100;
            bool finished = false;
            while ((maxDepth < maxSearchDistance) && (open.Count != 0))
            {
                var current = GetFirstInOpen();
                if (current == Nodes[tx, ty])
                {
                    break;
                }

                RemoveFromOpen(current);
                AddToClosed(current);
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        
                        if (x == 0 && y == 0)
                            continue;
                        if (x != 0 && y != 0) //no diagonal movement
                            continue;
                        int xp = x + current.X;
                        int yp = y + current.Y;
                        bool isAdjacent = false;
                        if (toAdjacentPos && gridManager.GridLogic.IsTileAccessible(new Vector2(xp, yp)) && gridManager.GridLogic.IsTileFree(new Vector2(xp, yp)))
                        {
                            int delta = Mathf.Abs(xp - Nodes[tx, ty].X) + Mathf.Abs(yp - Nodes[tx, ty].Y);
                            range.Reverse();
                            foreach (int r in range.Where(r => delta == r))
                            {
                                isAdjacent = true;
                                
                                //if (GridGameManager.Instance.GetSystem<InputSystem>().AttackRangeFromPath < r)
                                //{
                                    //Debug.Log("AttackRange " + r + " " +tx+" "+ty+" "+ xp + " " + yp);
                                    //GridGameManager.Instance.GetSystem<InputSystem>().AttackRangeFromPath = r;
                                    tx = xp;
                                    ty = yp;
                                    finished = true;
                                    break;
                                //}
                            }
                            range.Reverse();
                        }
                       
                        if (gridManager.GridLogic.IsValidLocation(team, sx, sy, xp, yp, isAdjacent) ||
                            (xp == tx && yp == ty))
                        {
                           
                            int nextStepCost = current.CostFromStart + 1;
                            var neighbor = Nodes[xp, yp];
                            if (nextStepCost < neighbor.CostFromStart)
                            {
                                if (InOpenList(neighbor))
                                {
                                    RemoveFromOpen(neighbor);
                                }

                                if (InClosedList(neighbor))
                                {
                                    RemoveFromClosed(neighbor);
                                }
                            }

                            if (!InOpenList(neighbor) && !InClosedList(neighbor))
                            {
                                neighbor.CostFromStart = nextStepCost;
                                maxDepth = Mathf.Max(maxDepth, neighbor.SetParent(current));
                                AddToOpen(neighbor);
                            }
                            if (finished)
                            {
                                break;
                            }
                        }
                    }
                    if (finished)
                    {
                        break;
                    }
                }
                if (finished)
                {
                    break;
                }
            }

            if (Nodes[tx, ty].Parent == null)
            {
                Debug.Log("Parent null");
                return null;
            }

            var path = new MovementPath();
            var target = Nodes[tx, ty];
            while (target != Nodes[sx, sy])
            {
                path.PrependStep(target.X, target.Y);
                target = target.Parent;
            }

            path.PrependStep(sx, sy);
            return path;
        }

        public MovementPath GetPath(int x, int y, int x2, int y2, int team, bool toAdjacentPos, List<int> range)
        {
            //GridGameManager.Instance.GetSystem<InputSystem>().AttackRangeFromPath = 0;
            Reset();

            return FindPath(x, y, x2, y2, team, toAdjacentPos, range);
        }
    }
}