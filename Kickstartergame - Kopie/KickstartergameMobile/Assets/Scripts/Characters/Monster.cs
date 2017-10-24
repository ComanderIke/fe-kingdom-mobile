using Assets.Scripts.Characters.Attributes;
using Assets.Scripts.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    public class Monster :LivingObject
    {
        public BigTile Position { get; set; }

        public Monster(string name, MonsterType type) : base()
        {
            this.name = name;
            movRange = 3;
            stats = new Stats(100, 5, 5, 5, 5, 5);
            AttackRanges = new List<int>();
            AttackRanges.Add(1);
            HP = stats.maxHP;
            level = 1;
        }
        public override void SetPosition(int x, int y)
        {
            OldPosition = new Vector2(this.x, this.y);
            MainScript m = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>();
            if (Position != null)
            {
                m.gridScript.fields[(int)Position.BottomLeft().x, (int)Position.BottomLeft().y].character = null;
                m.gridScript.fields[(int)Position.BottomRight().x, (int)Position.BottomRight().y].character = null;
                m.gridScript.fields[(int)Position.TopLeft().x, (int)Position.TopLeft().y].character = null;
                m.gridScript.fields[(int)Position.TopRight().x, (int)Position.TopRight().y].character = null;
            }
            this.x = x;
            this.y = y;
            gameObject.transform.localPosition = new Vector3(x, y, 0);
            Position = new BigTile(new Vector2(x, y), new Vector2(x + 1, y), new Vector2(x, y + 1), new Vector2(x + 1, y + 1));
            m.gridScript.fields[(int)Position.BottomLeft().x, (int)Position.BottomLeft().y].character = this;
            m.gridScript.fields[(int)Position.BottomRight().x, (int)Position.BottomRight().y].character = this;
            m.gridScript.fields[(int)Position.TopLeft().x, (int)Position.TopLeft().y].character = this;
            m.gridScript.fields[(int)Position.TopRight().x, (int)Position.TopRight().y].character = this;
            
        }
    }
}
