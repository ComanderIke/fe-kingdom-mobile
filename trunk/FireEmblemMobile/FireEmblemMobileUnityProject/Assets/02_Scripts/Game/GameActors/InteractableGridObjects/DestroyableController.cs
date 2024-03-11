using System;
using Game.GameActors.Factions;
using Game.GUI.Other;
using Game.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GameActors.InteractableGridObjects
{
    [ExecuteInEditMode]
    public class DestroyableController : MonoBehaviour
    {
    
        public Destroyable Destroyable;
        public FactionId factionID = 0;
        [SerializeField] private TextMeshProUGUI hpText;
        public StatsBarOnMap hpBar;

        public SpriteRenderer sprite;
        public GameObject canvasTransform;
        private int x = 0;
        private int y = 0;
        private void OnDestroy()
        {
            Destroyable.HpValueChanged -= HpValueChanged;
        }

        private void OnEnable()
        {
            x=(int)transform.localPosition.x;
            y = (int)transform.localPosition.y;
        }

        public void Init()
        {
            Destroyable.HpValueChanged += HpValueChanged;
            HpValueChanged();
            hpBar.GetComponent<Image>().color = ColorManager.Instance.GetFactionColor(Destroyable.Faction.Id);
            if (Destroyable.Faction.Id != 0)
            {
                canvasTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(.9f,
                    canvasTransform.GetComponent<RectTransform>().anchoredPosition.y);
            }
            else
            {
                canvasTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,
                    canvasTransform.GetComponent<RectTransform>().anchoredPosition.y);
            }
            hpText.color = ColorManager.Instance.GetFactionColor(Destroyable.Faction.Id);
   
        }
        private void HpValueChanged()
        {
            if (hpBar != null && Destroyable != null){
                hpBar.SetValue(Destroyable.Hp, Destroyable.MaxHp, false);
                hpText.SetText(""+Destroyable.Hp);
            }
        }
        public bool IsOnPosition(int xPos, int yPos)
        {
            if (x == xPos && y == yPos)
                return true;
            for (int x1 = x+1; x1 <= x; x1++)
            {
                for (int y1 = y; y1 <= y; y1++)
                {
                    if (x1 == xPos && y1 == yPos)
                        return true;
                }
            }
            return false;
        }

        void Update()
        {
            var transform1 = transform;
            var localPosition = transform1.localPosition;
            localPosition = new Vector3((int) localPosition.x, (int) localPosition.y,
                (int) localPosition.z);
            transform1.localPosition = localPosition;
#if UNITY_EDITOR
            sprite.sprite = Destroyable.SpriteNotDestroyed;
#endif
        }
        public int X => x;
        public int Y => y;

        public Vector3 GetCenterPosition()
        {
            return transform.position + new Vector3(.5f, .5f, 0);
        }

        public void Die()
        {
            sprite.sprite = Destroyable.Sprite;
            hpBar.Hide();
            hpText.gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }
}