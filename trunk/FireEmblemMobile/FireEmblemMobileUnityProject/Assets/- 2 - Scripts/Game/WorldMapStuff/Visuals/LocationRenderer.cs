﻿using ICSharpCode.NRefactory;
using UnityEngine;

namespace Game.WorldMapStuff.Controller
{
    public class LocationRenderer:MonoBehaviour
    {
        public WorldMapPosition location;
        public SpriteRenderer spriteRenderer;
        public Sprite selectedSprite;
        public Sprite normalSprite;
        public Sprite moveSprite;
        public Sprite enemySprite;
        public GameObject Walkable;
        public GameObject Attackable;
        public GameObject Enemy;

        public void ShowAttackable()
        {
            spriteRenderer.sprite = enemySprite;
            Attackable.SetActive(true);
            Walkable.SetActive(false);
            Enemy.SetActive(true);
        }
        public void ShowEnemy()
        {
            spriteRenderer.sprite = enemySprite;
            Attackable.SetActive(false);
            Walkable.SetActive(false);
            Enemy.SetActive(true);
        }
        public void ShowWalkable()
        {
            Walkable.SetActive(true);
            Attackable.SetActive(false);
            Enemy.SetActive(false);
            spriteRenderer.sprite = moveSprite;
        }
        public void ShowSelected(bool selected)
        {
            spriteRenderer.sprite = selected ? selectedSprite:normalSprite;
        }
        
        public void DrawInteractableConnections()
        {
            foreach (WorldMapPosition connection in location.Connections)
            {
               
                if(connection.Actor!=null&&!connection.Actor.Faction.IsActive())
                    connection.renderer.ShowEnemy();
                else
                {
                    connection.renderer.ShowWalkable();
                }
            }
        }
        public void HideInteractableConnections()
        {
            foreach (WorldMapPosition connection in location.Connections)
            {
                connection.renderer.Hide();
            }
        }
        private void Hide()
        {
            spriteRenderer.sprite = normalSprite;
            Walkable.SetActive(false);
           Attackable.SetActive(false);
        }

        public void Reset()
        {
            Hide();
        }
    }
}