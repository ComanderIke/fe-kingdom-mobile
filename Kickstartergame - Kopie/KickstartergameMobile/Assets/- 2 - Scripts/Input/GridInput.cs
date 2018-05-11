using Assets.Scripts.Characters;
using Assets.Scripts.Events;
using Assets.Scripts.GameStates;
using Assets.Scripts.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Input
{
    public class GridInput
    {
        public bool confirmClick = false;
        public Vector2 clickedField;

        public GridInput()
        {
            clickedField = new Vector2(-1, -1);
            EventContainer.clickedOnGrid += ClickedOnGrid;
        }

        public BigTile GetClickedBigTile(int centerX, int centerY, int x, int y)
        {
            MainScript mainScript = MainScript.GetInstance();
            LivingObject selectedCharacter = mainScript.GetSystem<UnitSelectionSystem>().SelectedCharacter;
            GridLogic gridLogic = mainScript.GetSystem<GridSystem>().GridLogic;
            BigTile clickedBigTile = new BigTile(new Vector2(centerX, centerY), new Vector2(centerX + 1, centerY), new Vector2(centerX, centerY + 1), new Vector2(centerX + 1, centerY + 1));

            if (!gridLogic.IsValidAndActive(clickedBigTile, selectedCharacter.Player.ID))
            {

                clickedBigTile = new BigTile(new Vector2(x, y), new Vector2(x + 1, y), new Vector2(x, y + 1), new Vector2(x + 1, y + 1));

                if (!gridLogic.IsValidAndActive(clickedBigTile, selectedCharacter.Player.ID))
                {


                    clickedBigTile = new BigTile(new Vector2(x - 1, y), new Vector2(x, y), new Vector2(x - 1, y + 1), new Vector2(x, y + 1));

                    if (!gridLogic.IsValidAndActive(clickedBigTile, selectedCharacter.Player.ID))
                    {

                        clickedBigTile = new BigTile(new Vector2(x, y - 1), new Vector2(x + 1, y - 1), new Vector2(x, y), new Vector2(x + 1, y));

                        if (!gridLogic.IsValidAndActive(clickedBigTile, selectedCharacter.Player.ID))
                        {
                            clickedBigTile = new BigTile(new Vector2(x - 1, y - 1), new Vector2(x, y - 1), new Vector2(x - 1, y), new Vector2(x, y));
                        }
                    }
                }
            }
            return clickedBigTile;
        }
        public void Reset()
        {
            confirmClick = false;
            clickedField = new Vector2(-1, -1);
        }
        public Vector2 GetCenterPos(Vector2 clickedPos)
        {
            int centerX = (int)Mathf.Round(clickedPos.x - GridSystem.GRID_X_OFFSET) - 1;
            int centerY = (int)Mathf.Round(clickedPos.y) - 1;
            return new Vector2(centerX, centerY);
        }
        private void ClickedOnGrid(int x, int y, Vector2 clickedPos)
        {
            MainScript mainScript = MainScript.GetInstance();
            LivingObject selectedCharacter = mainScript.GetSystem<UnitSelectionSystem>().SelectedCharacter;
            if (selectedCharacter != null)
            {
                if (confirmClick && clickedField == new Vector2(x, y))
                {
                    selectedCharacter.ResetPosition();
                    EventContainer.clickedOnField(x, y);
                }
                else
                {
                    
                    confirmClick = true;
                    clickedField = new Vector2(x, y);
                    if (mainScript.GetSystem<GridSystem>().Tiles[x, y].isActive)
                    {
                        if (selectedCharacter is Monster)
                        {
                            Vector2 centerPos = GetCenterPos(clickedPos);
                            BigTile clickedBigTile = GetClickedBigTile((int)centerPos.x, (int)centerPos.y, x, y);
                            EventContainer.monsterClickedOnActiveBigTile(selectedCharacter, clickedBigTile);
                        }
                        else
                        {
                            EventContainer.unitClickedOnActiveTile(selectedCharacter, x, y);
                            Debug.Log("OnACiveTileClicked!");
                            selectedCharacter.GameTransform.SetPosition(x, y);
                            selectedCharacter.GameTransform.DisableCollider();
                        }
                    }
                    else
                        mainScript.GetSystem<UnitSelectionSystem>().DeselectActiveCharacter();
                }
            }
            else
                EventContainer.clickedOnField(x, y);
        }
    }
}
