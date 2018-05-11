using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    public class GridPosition
    {

        public int x=-1;
        public int y=-1;
        private bool facingLeft=false;
        public bool FacingLeft { get
            {
                return facingLeft;
            }
            set
            {
                facingLeft = value;
                if (facingLeft)
                    character.GameTransform.SetYRotation(180);
                else
                    character.GameTransform.SetYRotation(0);
            }

        }

        protected LivingObject character;
        protected GridSystem gridScript;

        public GridPosition(LivingObject character)
        {
            this.character = character;
            
        }
        public Vector2 GetPos()
        {
            return new Vector2(x, y);
        }
        public virtual void SetPosition(int newX, int newY)
        {
            if(gridScript==null)
                gridScript = MainScript.GetInstance().GetSystem<GridSystem>();
            if (x!=-1&&y!=-1)
                gridScript.Tiles[x, y].character = null;
            gridScript.Tiles[newX, newY].character = character;
            x = newX;
            y = newY;
        }
        protected int DeltaPos(int x2, int y2)
        {
            return Math.Abs(x - x2) + Math.Abs(y - y2);
        }
        public virtual void RemoveCharacter()
        {
            if(gridScript==null)
                gridScript = MainScript.GetInstance().GetSystem<GridSystem>();
            gridScript.Tiles[x, y] = null;
        }
        public virtual bool CanAttack(List<int> range, GridPosition enemyPosition)
        {
            if (!(enemyPosition is BigTilePosition))
            {
                return range.Contains(DeltaPos(enemyPosition.x, enemyPosition.y));
            }
            else
            {
                BigTilePosition bigTile = (BigTilePosition)enemyPosition;
                if(range.Contains(DeltaPos((int)bigTile.Position.BottomLeft().x, (int)bigTile.Position.BottomLeft().y))){
                    return true;
                }
                else if (range.Contains(DeltaPos((int)bigTile.Position.BottomRight().x, (int)bigTile.Position.BottomRight().y))){
                    return true;
                }
                else if (range.Contains(DeltaPos((int)bigTile.Position.TopRight().x, (int)bigTile.Position.TopRight().y))){
                    return true;
                }
                else if (range.Contains(DeltaPos((int)bigTile.Position.TopLeft().x, (int)bigTile.Position.TopLeft().y))){
                    return true;
                }
            }
            return false;
        }
    }
}
