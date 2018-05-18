using Assets.Scripts.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AI
{
    public class AI_GameData
    {
        private GridSystem gridManager;

        public AI_GameData()
        {
            gridManager=MainScript.instance.GetSystem<GridSystem>();
        }

        public List<Unit> GetAttackTargets(Unit c)
        {
            int x = c.GridPosition.x;
            int z = c.GridPosition.y;
            List<Unit> characters = new List<Unit>();
            foreach (int range in c.Stats.AttackRanges)
            {
                GetAttackableCharacters(c, x, z, range, characters, new List<int>());
            }
            return characters;
        }

        public List<Unit> GetAttackableTargetsAtLocation(Vector3 location, Unit character)
        {
            List<Unit> attackTargets = new List<Unit>();
            int x = (int)location.x;
            int z = (int)location.z;
            foreach (int range in character.Stats.AttackRanges)
            {
                GetAttackableCharacters(character, x, z, range, attackTargets, new List<int>());
            }
            return attackTargets;
        }
      
        public void GetAttackableCharacters(Unit character, int x, int y, int range, List<Unit> characters, List<int> direction)
        {
            if (range <= 0)
            {
                Unit c = gridManager.Tiles[x, y].character;

                if (c != null && c.Player.ID != character.Player.ID && c.IsAlive())
                {
                    bool contains = false;
                    foreach (Unit a in characters)
                    {
                        if (a == c)
                        {
                            contains = true;

                        }
                    }
                    if (!contains)
                    {
                        characters.Add(c);
                    }
                }
                return;
            }
            if (!direction.Contains(2))
            {
                if (gridManager.GridLogic.CheckAttackField(x + 1, y))
                {
                    List<int> newdirection = new List<int>(direction);
                    newdirection.Add(1);
                    GetAttackableCharacters(character, x + 1, y, range - 1, characters, newdirection);
                }
            }
            if (!direction.Contains(1))
            {
                if (gridManager.GridLogic.CheckAttackField(x - 1, y))
                {
                    List<int> newdirection = new List<int>(direction);
                    newdirection.Add(2);
                    GetAttackableCharacters(character, x - 1, y, range - 1, characters, newdirection);
                }
            }
            if (!direction.Contains(4))
            {
                if (gridManager.GridLogic.CheckAttackField(x, y + 1))
                {
                    List<int> newdirection = new List<int>(direction);
                    newdirection.Add(3);
                    GetAttackableCharacters(character, x, y + 1, range - 1, characters, newdirection);
                }
            }
            if (!direction.Contains(3))
            {
                if (gridManager.GridLogic.CheckAttackField(x, y - 1))
                {
                    List<int> newdirection = new List<int>(direction);
                    newdirection.Add(4);
                    GetAttackableCharacters(character, x, y - 1, range - 1, characters, newdirection);
                }
            }

        }
    }
}
