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
   
        private void OnDestroy()
        {
            Destroyable.HpValueChanged -= HpValueChanged;
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
        public bool IsOnPosition(int x, int y)
        {
            for (int x1 = X; x1 <= X; x1++)
            {
                for (int y1 = Y; y1 <= Y; y1++)
                {
                    if (x1 == x && y1 == y)
                        return true;
                }
            }
            return false;
        }

        void Update()
        {
            transform.localPosition = new Vector3((int) transform.localPosition.x, (int) transform.localPosition.y,
                (int) transform.localPosition.z);
#if UNITY_EDITOR
            sprite.sprite = Destroyable.SpriteNotDestroyed;
#endif
        }
        public int X
        {
            get
            {
                return (int)transform.localPosition.x;
            }
        }
        public int Y
        {
            get
            {
                return (int)transform.localPosition.y;
            }
        }

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