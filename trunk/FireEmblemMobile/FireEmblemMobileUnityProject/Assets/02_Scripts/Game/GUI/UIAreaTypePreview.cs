using System;
using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using _02_Scripts.Game.GameActors.Items.Consumables;
using Game.GameActors.Units.Skills;
using Game.Map;
using UnityEngine;

namespace LostGrace
{
    [ExecuteInEditMode]
    public class UIAreaTypePreview : MonoBehaviour
    {
        [SerializeField] private GameObject gridTilePrefab;


        private void OnEnable()
        {
            //Show(SkillTargetArea.Star, 2, EffectType.Heal, 2);
        }

        private UITargetAreaTile[,] grid = new UITargetAreaTile[5, 5];

        public void Show(SkillTargetArea targetArea, int size, EffectType effectType, int upgradedSize)
        {

            Debug.Log("SHOW TARGET AREA");
            transform.DeleteAllChildrenImmediate();
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    var go = Instantiate(gridTilePrefab, transform);
                    grid[i, j] = go.GetComponent<UITargetAreaTile>();
                }
            }

            Vector2 characterPos = new Vector2(2, 2);
            List<Vector2> positions = new List<Vector2>();
            positions.Add(characterPos);
            if (size > 0)
            {
                if (targetArea == SkillTargetArea.Block)
                {
                    for (int i = 0; i < size + 1; i++)
                    {
                        for (int j = 0; j < size + 1; j++)
                        {
                            positions.Add(characterPos + new Vector2(i, j));
                            positions.Add(characterPos + new Vector2(-i, j));
                            positions.Add(characterPos + new Vector2(i, -j));
                            positions.Add(characterPos + new Vector2(-i, -j));
                        }
                    }
                }
                else
                {
                    for (int i = 1; i < size + 1; i++)
                    {
                        if (targetArea == SkillTargetArea.Line || targetArea == SkillTargetArea.Star ||
                            targetArea == SkillTargetArea.Cross)
                        {
                            positions.Add(characterPos + new Vector2(-i, 0));
                            positions.Add(characterPos + new Vector2(i, 0));
                        }

                        if (targetArea == SkillTargetArea.NormalLine || targetArea == SkillTargetArea.Star ||
                            targetArea == SkillTargetArea.Cross)
                        {
                            positions.Add(characterPos + new Vector2(0, -i));
                            positions.Add(characterPos + new Vector2(0, i));
                        }
                    }

                    if (targetArea == SkillTargetArea.Star)
                    {
                        for (int i = 0; i < size; i++)
                        {
                            for (int j = 0; j < size; j++)
                            {
                                if (i != 0 && j != 0 && (i + j) <= size)
                                {
                                    positions.Add(characterPos + new Vector2(i, j));
                                    positions.Add(characterPos + new Vector2(-i, j));
                                    positions.Add(characterPos + new Vector2(i, -j));
                                    positions.Add(characterPos + new Vector2(-i, -j));
                                }
                            }
                        }
                    }

                    


                }
                foreach (var pos in positions)
                {
                    grid[(int)pos.x, (int)pos.y].Show(effectType);
                }
            }
        }
    }
}
