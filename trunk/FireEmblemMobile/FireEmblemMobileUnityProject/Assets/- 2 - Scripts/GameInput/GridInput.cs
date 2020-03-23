using Assets.Core;
using Assets.GameActors.Units.Monsters;
using Assets.Grid;
using Assets.Map;
using Assets.Mechanics;
using UnityEngine;

namespace Assets.GameInput
{
    public class GridInput
    {
        public Vector2 ClickedField;
        public bool ConfirmClick;

        public GridInput()
        {
            ClickedField = new Vector2(-1, -1);
            InputSystem.OnClickedGrid += ClickedOnGrid;
        }

        public BigTile GetClickedBigTile(int centerX, int centerY, int x, int y)
        {
            var mainScript = GridGameManager.Instance;
            var selectedCharacter = mainScript.GetSystem<UnitSelectionSystem>().SelectedCharacter;
            var gridLogic = mainScript.GetSystem<MapSystem>().GridLogic;
            var clickedBigTile = new BigTile(new Vector2(centerX, centerY), new Vector2(centerX + 1, centerY),
                new Vector2(centerX, centerY + 1), new Vector2(centerX + 1, centerY + 1));

            if (!gridLogic.IsValidAndActive(clickedBigTile, selectedCharacter.Faction.Id))
            {
                clickedBigTile = new BigTile(new Vector2(x, y), new Vector2(x + 1, y), new Vector2(x, y + 1),
                    new Vector2(x + 1, y + 1));

                if (!gridLogic.IsValidAndActive(clickedBigTile, selectedCharacter.Faction.Id))
                {
                    clickedBigTile = new BigTile(new Vector2(x - 1, y), new Vector2(x, y), new Vector2(x - 1, y + 1),
                        new Vector2(x, y + 1));

                    if (!gridLogic.IsValidAndActive(clickedBigTile, selectedCharacter.Faction.Id))
                    {
                        clickedBigTile = new BigTile(new Vector2(x, y - 1), new Vector2(x + 1, y - 1),
                            new Vector2(x, y), new Vector2(x + 1, y));

                        if (!gridLogic.IsValidAndActive(clickedBigTile, selectedCharacter.Faction.Id))
                            clickedBigTile = new BigTile(new Vector2(x - 1, y - 1), new Vector2(x, y - 1),
                                new Vector2(x - 1, y), new Vector2(x, y));
                    }
                }
            }

            return clickedBigTile;
        }

        public void Reset()
        {
            ConfirmClick = false;
            ClickedField = new Vector2(-1, -1);
        }

        public Vector2 GetCenterPos(Vector2 clickedPos)
        {
            int centerX = (int) Mathf.Round(clickedPos.x - MapSystem.GRID_X_OFFSET) - 1;
            int centerY = (int) Mathf.Round(clickedPos.y) - 1;
            return new Vector2(centerX, centerY);
        }

        private void ClickedOnGrid(int x, int y, Vector2 clickedPos)
        {
            var mainScript = GridGameManager.Instance;
            var selectedCharacter = mainScript.GetSystem<UnitSelectionSystem>().SelectedCharacter;
            if (selectedCharacter != null)
            {
                if (ConfirmClick && ClickedField == new Vector2(x, y))
                {
                    selectedCharacter.ResetPosition();
                    InputSystem.OnClickedField(x, y);
                }
                else
                {
                    ConfirmClick = true;
                    ClickedField = new Vector2(x, y);
                    if (mainScript.GetSystem<MapSystem>().Tiles[x, y].IsActive)
                    {
                        if (selectedCharacter is Monster)
                        {
                            var centerPos = GetCenterPos(clickedPos);
                            var clickedBigTile = GetClickedBigTile((int) centerPos.x, (int) centerPos.y, x, y);
                            InputSystem.OnClickedMovableBigTile(selectedCharacter, clickedBigTile);
                        }
                        else
                        {
                            InputSystem.OnClickedMovableTile(selectedCharacter, x, y);
                            selectedCharacter.GameTransform.SetPosition(x, y);
                            selectedCharacter.GameTransform.DisableCollider();
                        }
                    }
                    else
                    {
                        mainScript.GetSystem<UnitSelectionSystem>().DeselectActiveCharacter();
                    }
                }
            }
            else
            {
                InputSystem.OnClickedField(x, y);
            }
        }
    }
}