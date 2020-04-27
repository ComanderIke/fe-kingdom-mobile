using Assets.Core;
using Assets.GameInput;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Grid.PathFinding
{
    public class AStar2X2
    {
        private readonly PathFindingNode[,] accessibilities;
        private ArrayList closed;
        private ArrayList open;
        private List<Node2X2> nodes;
        private readonly int gridWidth;
        private readonly int gridHeight;
        private readonly Tile[,] fields;

        public AStar2X2(PathFindingNode[,] accessibilities)
        {
            this.accessibilities = accessibilities;
            gridWidth = accessibilities.GetLength(0);
            gridHeight = accessibilities.GetLength(1);
            fields = GridGameManager.Instance.GetSystem<Map.MapSystem>().Tiles;
        }

        public MovementPath GetPath(BigTile start, BigTile end)
        {
            open = new ArrayList();
            closed = new ArrayList();
            nodes = new List<Node2X2>();
            return FindPath(start, end);
        }

        public MovementPath GetPath(BigTile start, BigTile end, int team, bool adjacent, List<int> attackRanges)
        {
            open = new ArrayList();
            closed = new ArrayList();
            nodes = new List<Node2X2>();
            return FindPath(start, end, team, adjacent, attackRanges);
        }

        private void AddToClosed(Node2X2 node)
        {
            closed.Add(node);
        }

        private void AddToOpen(Node2X2 node)
        {
            open.Add(node);
        }

        private Node2X2 GetFirstInOpen()
        {
            return (Node2X2) open[0];
        }

        private bool InClosedList(Node2X2 node)
        {
            return closed.Contains(node);
        }

        private bool InOpenList(Node2X2 node)
        {
            return open.Contains(node);
        }

        private void RemoveFromClosed(Node2X2 node)
        {
            closed.Remove(node);
        }

        private void RemoveFromOpen(Node2X2 node)
        {
            open.Remove(node);
        }

        private bool IsValidLocation(Vector2 pos, bool isAdjacent, int team)
        {
            bool invalid = (pos.x < 0) || (pos.y < 0) || (pos.x >= gridWidth) || (pos.y >= gridHeight);
            if (!invalid)
            {
                invalid = !accessibilities[(int) pos.x, (int) pos.y].Accessible;
                if (fields[(int) pos.x, (int) pos.y].Unit != null)
                {
                    if (fields[(int) pos.x, (int) pos.y].Unit.Faction.Id != team)
                    {
                        invalid = true;
                    }

                    if (isAdjacent) //TODO passthrough should be ok but not stopping on it
                    {
                        invalid = true;
                    }
                }
            }

            return !invalid;
        }

        private bool IsValidLocation(Vector2 pos)
        {
            bool invalid = (pos.x < 0) || (pos.y < 0) || (pos.x >= gridWidth) || (pos.y >= gridHeight);
            if (!invalid)
            {
                invalid = !accessibilities[(int) pos.x, (int) pos.y].Accessible;
            }

            return !invalid;
        }

        private bool IsValidLocation(BigTile position)
        {
            return IsValidLocation(position.BottomLeft()) && IsValidLocation(position.BottomRight()) &&
                   IsValidLocation(position.TopLeft()) && IsValidLocation(position.TopRight());
        }

        private bool IsValidLocation(BigTile position, int team, bool isAdjacent)
        {
            return IsValidLocation(position.BottomLeft(), isAdjacent, team) &&
                   IsValidLocation(position.BottomRight(), isAdjacent, team) &&
                   IsValidLocation(position.TopLeft(), isAdjacent, team) &&
                   IsValidLocation(position.TopRight(), isAdjacent, team);
        }

        public MovementPath FindPath(BigTile start, BigTile end)
        {
            var startNode = new Node2X2(start, 0) {Depth = 0, CostFromStart = 0};
            nodes.Add(startNode);
            closed.Clear();
            open.Clear();
            open.Add(startNode);
            var endNode = new Node2X2(end, 0) {Parent = null};
            nodes.Add(endNode);
            int maxDepth = 0;
            int maxSearchDistance = 100;
            while ((maxDepth < maxSearchDistance) && (open.Count != 0))
            {
                var current = GetFirstInOpen();
                if (current == endNode)
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
                        var currentPos = current.Position;
                        var newPos = new BigTile(
                            new Vector2(currentPos.BottomLeft().x + x, currentPos.BottomLeft().y + y),
                            new Vector2(currentPos.BottomRight().x + x, currentPos.BottomRight().y + y),
                            new Vector2(currentPos.TopLeft().x + x, currentPos.TopLeft().y + y),
                            new Vector2(currentPos.TopRight().x + x, currentPos.TopRight().y + y));
                        if (IsValidLocation(newPos) || newPos.Equals(end))
                        {
                            int nextStepCost = current.CostFromStart + 1;
                            var neighbor = new Node2X2(newPos, 0);
                            if (nodes.Contains(neighbor))
                            {
                                neighbor = nodes[nodes.IndexOf(neighbor)];
                            }
                            else
                            {
                                nodes.Add(neighbor);
                            }

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

            if (endNode.Parent == null)
            {
                return null;
            }

            var path = new MovementPath();
            var target = endNode;
            while (target != startNode)
            {
                path.PrependStep(target.Position.BottomLeft().x + 0.5f, target.Position.BottomLeft().y + 0.5f);
                target = target.Parent;
            }

            path.PrependStep(startNode.Position.BottomLeft().x + 0.5f, startNode.Position.BottomLeft().y + 0.5f);
            return path;
        }

        public MovementPath FindPath(BigTile start, BigTile end, int team, bool adjacent, List<int> attackRanges)
        {
            var startNode = new Node2X2(start, 0) {Depth = 0, CostFromStart = 0};
            nodes.Add(startNode);
            closed.Clear();
            open.Clear();
            open.Add(startNode);
            var endNode = new Node2X2(end, 0) {Parent = null};
            nodes.Add(endNode);
            int maxDepth = 0;
            int maxSearchDistance = 100;
            while ((maxDepth < maxSearchDistance) && (open.Count != 0))
            {
                var current = GetFirstInOpen();
                if (current == endNode)
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
                        var currentPos = current.Position;
                        var newPos = new BigTile(
                            new Vector2(currentPos.BottomLeft().x + x, currentPos.BottomLeft().y + y),
                            new Vector2(currentPos.BottomRight().x + x, currentPos.BottomRight().y + y),
                            new Vector2(currentPos.TopLeft().x + x, currentPos.TopLeft().y + y),
                            new Vector2(currentPos.TopRight().x + x, currentPos.TopRight().y + y));
                        bool isAdjacent = false;
                        if (adjacent)
                        {
                            int delta = Mathf.Abs((int) newPos.BottomLeft().x - (int) end.BottomLeft().x) +
                                        Mathf.Abs((int) newPos.BottomLeft().y - (int) end.BottomLeft().y);
                            attackRanges.Reverse();
                            foreach (int r in attackRanges.Where(r => delta == r))
                            {
                                isAdjacent = true;

                                GridGameManager m = GridGameManager.Instance;
                                Debug.Log("TODO");
                                //if (m.GetSystem<InputSystem>().AttackRangeFromPath < r)
                                //{
                                //    m.GetSystem<InputSystem>().AttackRangeFromPath = r;
                                //    // break;
                                //}
                            }

                            attackRanges.Reverse();
                        }

                        if (IsValidLocation(newPos, team, isAdjacent) || newPos.Equals(end))
                        {
                            int nextStepCost = current.CostFromStart + 1;
                            var neighbor = new Node2X2(newPos, 0);
                            if (nodes.Contains(neighbor))
                            {
                                neighbor = nodes[nodes.IndexOf(neighbor)];
                            }
                            else
                            {
                                nodes.Add(neighbor);
                            }

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

            if (endNode.Parent == null)
            {
                return null;
            }

            var path = new MovementPath();
            var target = endNode;
            while (target != startNode)
            {
                path.PrependStep(target.Position.BottomLeft().x + 0.5f, target.Position.BottomLeft().y + 0.5f);
                target = target.Parent;
            }

            path.PrependStep(startNode.Position.BottomLeft().x + 0.5f, startNode.Position.BottomLeft().y + 0.5f);
            return path;
        }
    }
}