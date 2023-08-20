using System;
using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using _02_Scripts.Game.GameActors.Items.Consumables;
using Game.GameActors.Units.Skills;
using Game.Manager;
using Game.Map;
using UnityEngine;

namespace LostGrace
{

    public class UIAreaTypePreview : MonoBehaviour
    {
        [SerializeField] private GameObject gridTilePrefab;
        [SerializeField] private RectTransform rectTransformCopyYAnchored;
        private RectTransform rectTransform;

        private void OnEnable()
        {
            rectTransform = GetComponent<RectTransform>();
            //Show(SkillTargetArea.Star, 2, EffectType.Heal, 2);
        }

        private void Update()
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x,
                rectTransformCopyYAnchored.anchoredPosition.y);
        }

        private UITargetAreaTile[,] grid = new UITargetAreaTile[5, 5];

        public void Show(SkillTargetArea targetArea, int currentSize, EffectType effectType, int upgradedSize, bool rooted)
        {

           // Debug.Log("SHOW TARGET AREA" +targetArea+" "+size+" "+effectType);
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
            var positions = GetTargetAreaPositions(currentSize, targetArea, characterPos);
            var positionsUpg = GetTargetAreaPositions(upgradedSize, targetArea, characterPos);
            positions.Add(characterPos);
        
            foreach (var pos in positionsUpg)
            {
                grid[(int)pos.x, (int)pos.y].Show(effectType);
                grid[(int)pos.x, (int)pos.y].Blink();
            }
            foreach (var pos in positions)
            {
                grid[(int)pos.x, (int)pos.y].Show(effectType);
                grid[(int)pos.x, (int)pos.y].StopBlink();
            }
            if(rooted)
                grid[(int)characterPos.x,(int)characterPos.y].Show(EffectType.Neutral);
            
        }

        void AddToPosition(ref List<Vector2> positions, Vector2 pos)
        {
            if(!(pos.x<0|| pos.y<0||pos.x>=5||pos.y>=5))
                positions.Add(pos);
        }

    
        public List<Vector2> GetTargetAreaPositions(int size, SkillTargetArea targetArea, Vector2 characterPos)
        {
            var positions = new List<Vector2>();
  
            if (size > 0)
            {
                if (targetArea == SkillTargetArea.Block)
                {
                    for (int i = 0; i < size + 1; i++)
                    {
                        for (int j = 0; j < size + 1; j++)
                        {
                            AddToPosition(ref positions, characterPos + new Vector2(i, j));
                            AddToPosition(ref positions, characterPos + new Vector2(-i, j));
                            AddToPosition(ref positions, characterPos + new Vector2(i, -j));
                            AddToPosition(ref positions, characterPos + new Vector2(-i, -j));
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
                            AddToPosition(ref positions, characterPos + new Vector2(-i, 0));
                            AddToPosition(ref positions, characterPos + new Vector2(i, 0));
                        }

                        if (targetArea == SkillTargetArea.NormalLine || targetArea == SkillTargetArea.Star ||
                            targetArea == SkillTargetArea.Cross)
                        {
                            AddToPosition(ref positions, characterPos + new Vector2(0, -i));  
                            AddToPosition(ref positions, characterPos + new Vector2(0, i));
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
                                    AddToPosition(ref positions, characterPos + new Vector2(i, j));
                                    AddToPosition(ref positions, characterPos + new Vector2(-i, j));
                                    AddToPosition(ref positions, characterPos + new Vector2(i, -j));
                                    AddToPosition(ref positions, characterPos + new Vector2(-i, -j));
                                }
                            }
                        }
                    }
                }

                
            }
            return positions;
        }

        public void Hide()
        {
            transform.DeleteAllChildrenImmediate();
        }
    }
}
