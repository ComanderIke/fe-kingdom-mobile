using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Grid
{
    public class AStar2x2
    {
        private PathFindingNode[,] accesibilities;
        private ArrayList closed;
        private ArrayList open;
        private BigTile start;
        private BigTile end;
        private List<Node2x2> nodes;
        private int gridWidth;
        private int gridHeight;
        public AStar2x2(PathFindingNode[,] accesibilities)
        {
            this.accesibilities = accesibilities;
            this.gridWidth = accesibilities.GetLength(0);
            this.gridHeight = accesibilities.GetLength(1);
        }
        public MovementPath GetPath(BigTile start, BigTile end)
        {
            this.start = start;
            this.end = end;
            nodes = new List<Node2x2>();
            return FindPath(start, end);
        }

        private void addToClosed(Node2x2 node)
        {
            closed.Add(node);
        }

        private void addToOpen(Node2x2 node)
        {
            open.Add(node);
        }
        private Node2x2 getFirstInOpen()
        {
            return (Node2x2)open[0];
        }
        private bool InClosedList(Node2x2 node)
        {
            return closed.Contains(node);
        }

        private bool InOpenList(Node2x2 node)
        {
            return open.Contains(node);
        }
        private void removeFromClosed(Node2x2 node)
        {
            closed.Remove(node);
        }

        private void removeFromOpen(Node2x2 node)
        {
            open.Remove(node);
        }
        private bool isValidLocation(Vector2 pos)
        {
            bool invalid = (pos.x < 0) || (pos.y < 0) || (pos.x >= gridWidth) || (pos.y >= gridHeight);
            if (!invalid)
            {
                invalid = !accesibilities[(int)pos.x, (int)pos.y].Accesible;
            }
            return !invalid;
        }
        private bool isValidLocation(BigTile position)
        {
            return isValidLocation(position.BottomLeft()) && isValidLocation(position.BottomRight()) && isValidLocation(position.TopLeft()) && isValidLocation(position.TopRight());
        }
        public MovementPath FindPath(BigTile start, BigTile end)
        {
            Node2x2 startNode = new Node2x2(start, 0);
            startNode.depth = 0;
            startNode.costfromStart = 0;
            nodes.Add(startNode);
            closed.Clear();
            open.Clear();
            open.Add(startNode);
            Node2x2 endNode = new Node2x2(end, 0);
            endNode.parent = null;
            nodes.Add(endNode);
            int maxDepth = 0;
            int maxSearchDistance = 100;
            while ((maxDepth < maxSearchDistance) && (open.Count != 0))
            {
                Node2x2 current = getFirstInOpen();
                if (current == endNode)
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
                        BigTile currentPos = current.Position;
                        BigTile newPos = new BigTile(new Vector2(currentPos.BottomLeft().x + x, currentPos.BottomLeft().y + y), new Vector2(currentPos.BottomRight().x + x, currentPos.BottomRight().y + y), new Vector2(currentPos.TopLeft().x + x, currentPos.TopLeft().y + y), new Vector2(currentPos.TopRight().x + x, currentPos.TopRight().y + y));
                        if (isValidLocation(newPos) || newPos.Equals(end))
                        {
                            int nextStepCost = current.costfromStart + 1;
                            Node2x2 neighbour = nodes[xp, yp];
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
    }
}
