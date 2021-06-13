
using UnityEngine;

namespace Game.WorldMapStuff.Controller
{
    public class LocationRenderer:MonoBehaviour
    {

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
            Debug.Log("Show AttackAble");
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
        
       
        public void Hide()
        {
            // Debug.Log(this);
            // Debug.Log(this.transform.parent.name);
            spriteRenderer.sprite = normalSprite;
            Walkable.SetActive(false);
           Attackable.SetActive(false);
           Enemy.SetActive(false);
        }

        public void Reset()
        {
            Hide();
        }
    }
}