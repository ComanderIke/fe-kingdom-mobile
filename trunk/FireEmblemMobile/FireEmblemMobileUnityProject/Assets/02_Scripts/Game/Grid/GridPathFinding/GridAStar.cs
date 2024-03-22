using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Game.GameActors.Units.Interfaces;
using Game.Map;
using Game.States.Mechanics;
using UnityEngine;

namespace Game.Grid.GridPathFinding
{
    public class GridAStar : IPathFinder
    {
        private ArrayList closed;
        private ArrayList open;
        private Node[,] Nodes;
        private readonly int width;
        private readonly int height;
        private ITileChecker tileChecker;

        public GridAStar(ITileChecker tileChecker)
        {
            this.tileChecker = tileChecker;
            this.width = tileChecker.GetWidth();
            this.height = tileChecker.GetHeight();
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
  public MovementPath FindPath(int sx, int sy, int tx, int ty, IGridActor unit, int stopAfterDistance=100)
        {
            Nodes[sx, sy].CostFromStart = 0;
            Nodes[sx, sy].Depth = 0;
            closed.Clear();
            open.Clear();
            open.Add(Nodes[sx, sy]);
            MyDebug.LogTest("TX "+tx+" TY "+ty);
            Nodes[tx, ty].Parent = null;
            int maxDepth = 0;
            int maxSearchDistance = stopAfterDistance;
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
                        
                       
                        if (tileChecker.IsValidLocation(unit, sx, sy, xp, yp) ||
                            (xp == tx && yp == ty))
                        {
                            int nextStepCost = current.CostFromStart + tileChecker.GetMovementCost(xp, yp, unit);
                       
                               
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
                        }
                    }
                }
            }

            if (Nodes[tx, ty].Parent == null)
            {
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
        // public MovementPath FindPath(int sx, int sy, int tx, int ty, IGridActor unit, bool toAdjacentPos, IEnumerable<int> range, bool checkMovRange, bool isAttackTarget)
        // {
        //     Nodes[sx, sy].CostFromStart = 0;
        //     Nodes[sx, sy].Depth = 0;
        //     closed.Clear();
        //     open.Clear();
        //     open.Add(Nodes[sx, sy]);
        //     Nodes[tx, ty].Parent = null;
        //     int maxDepth = 0;
        //     int maxSearchDistance = 100;
        //     bool finished = false;
        //     var enumerable = range as int[] ?? range.ToArray();
        //     while ((maxDepth < maxSearchDistance) && (open.Count != 0))
        //     {
        //         var current = GetFirstInOpen();
        //         if (current == Nodes[tx, ty])
        //         {
        //             break;
        //         }
        //
        //         RemoveFromOpen(current);
        //         AddToClosed(current);
        //         for (int x = -1; x < 2; x++)
        //         {
        //             for (int y = -1; y < 2; y++)
        //             {
        //                 
        //                 if (x == 0 && y == 0)
        //                     continue;
        //                 if (x != 0 && y != 0) //no diagonal movement
        //                     continue;
        //                 int xp = x + current.X;
        //                 int yp = y + current.Y;
        //                 bool isAdjacent = false;
        //                 if (toAdjacentPos && tileChecker.IsTileAccessible(xp, yp, unit) && tileChecker.IsTileFree(xp, yp))
        //                 {
        //                     int delta = Mathf.Abs(xp - Nodes[tx, ty].X) + Mathf.Abs(yp - Nodes[tx, ty].Y);
        //                     var reverse = enumerable.Reverse();
        //                     if (reverse.Any(r => delta == r))
        //                     {
        //                         isAdjacent = true;
        //                         tx = xp;
        //                         ty = yp;
        //                         finished = true;
        //                     }
        //                 }
        //                
        //                 if (tileChecker.IsValidLocation(unit, sx, sy, xp, yp, isAdjacent) ||
        //                     (xp == tx && yp == ty))
        //                 {
        //                     int nextStepCost = current.CostFromStart + tileChecker.GetMovementCost(xp, yp, unit);
        //                
        //                        
        //                         var neighbor = Nodes[xp, yp];
        //                         if (nextStepCost < neighbor.CostFromStart)
        //                         {
        //                             if (InOpenList(neighbor))
        //                             {
        //                                 RemoveFromOpen(neighbor);
        //                             }
        //
        //                             if (InClosedList(neighbor))
        //                             {
        //                                 RemoveFromClosed(neighbor);
        //                             }
        //                         }
        //
        //                         if (!InOpenList(neighbor) && !InClosedList(neighbor))
        //                         {
        //                             int nodeRange = unit.MovementRange;
        //                             if (isAttackTarget)
        //                                 nodeRange = unit.MovementRange + unit.AttackRanges.Max();
        //                             if (!checkMovRange || nodeRange >= nextStepCost)
        //                             {
        //                                 neighbor.CostFromStart = nextStepCost;
        //                                 maxDepth = Mathf.Max(maxDepth, neighbor.SetParent(current));
        //                                 AddToOpen(neighbor);
        //                             }
        //                         }
        //
        //                         if (finished)
        //                         {
        //                             break;
        //                         }
        //                     
        //                 }
        //             }
        //             if (finished)
        //             {
        //                 break;
        //             }
        //         }
        //         if (finished)
        //         {
        //             break;
        //         }
        //     }
        //
        //     if (Nodes[tx, ty].Parent == null)
        //     {
        //         return null;
        //     }
        //
        //     var path = new MovementPath();
        //     var target = Nodes[tx, ty];
        //     while (target != Nodes[sx, sy])
        //     {
        //         path.PrependStep(target.X, target.Y);
        //         target = target.Parent;
        //     }
        //
        //     path.PrependStep(sx, sy);
        //     return path;
        // }

     
    }
}