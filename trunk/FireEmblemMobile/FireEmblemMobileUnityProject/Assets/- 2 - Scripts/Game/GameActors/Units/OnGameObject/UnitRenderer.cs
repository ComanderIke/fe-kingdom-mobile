using Assets.Game.Manager;
using Assets.GameActors.Units.Humans;
using Assets.GameInput;
using Assets.GameResources;
using Assets.GUI;
using Assets.Mechanics;
using Assets.Utility;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.GameActors.Units.OnGameObject
{
    public class UnitRenderer : MonoBehaviour
    {

        [SerializeField] private StatsBarOnMap hpBar;
        [SerializeField] private StatsBarOnMap spBar;
        [SerializeField] private GameObject pointLight;

        [SerializeField] private TextMeshProUGUI ApText;
        [SerializeField] private Image EquippedItemIcon;
        [SerializeField] private Image EquippedItemBackground;
        [SerializeField] private GameObject spriteMask;
        [SerializeField] private CanvasGroup alphaCanvas;
        [SerializeField] private SpriteRenderer sprite;
        public Unit Unit;

        private void Start()
        {
            Unit.HpValueChanged += HpValueChanged;
            Unit.SpValueChanged += SpValueChanged;
            Unit.UnitWaiting += SetWaitingSprite;
            Unit.ApValueChanged += ApValueChanged;
            Human.OnEquippedWeapon += OnEquippedWeapon;
            HpValueChanged();
            SpValueChanged();
        }
        public void Init()
        {
            hpBar.GetComponent<Image>().color = ColorManager.Instance.GetFactionColor(Unit.Faction.Id);
            float intensity = 2;
            sprite.material.SetColor("_OutLineColor", ColorManager.Instance.GetFactionColor(Unit.Faction.Id)*intensity);
            //spBar.GetComponent<Image>().color = ColorManager.Instance.GetFactionColor(Unit.Faction.Id);
            HpValueChanged();
            SpValueChanged();
            ApValueChanged();
            OnEquippedWeapon();
            if(Unit.Faction.Id != GridGameManager.Instance.FactionManager.ActivePlayerNumber)
            {
                pointLight.SetActive(false);
                Color color = Color.blue;
                GetComponentInChildren<SpriteRenderer>().flipX = true;

                //spriteMask.SetActive(false);
            }
            else {
                pointLight.SetActive(true);
                GetComponentInChildren<SpriteRenderer>().flipX = false;
                //spriteMask.SetActive(true);
            }
        }
        void Destroy()
        {
            Unit.HpValueChanged -= HpValueChanged;
            Unit.SpValueChanged -= SpValueChanged;
            Unit.UnitWaiting -= SetWaitingSprite;
            Unit.ApValueChanged -= ApValueChanged;
            Human.OnEquippedWeapon -= OnEquippedWeapon;
        }
        public void SetVisible(bool visible)
        {
            if (visible)
            {
                alphaCanvas.alpha = 1;
                sprite.color = new Color(1, 1, 1, 1);
            }
            else
            {
                alphaCanvas.alpha = 0f;
                sprite.color = new Color(0, 0, 0,1f);
            }
        }
        private void OnEquippedWeapon()
        {
            EquippedItemBackground.color = ColorManager.Instance.GetFactionColor(Unit.Faction.Id);
            if (Unit is Human human) {
                if (human.EquippedWeapon != null)
                {
                    EquippedItemIcon.sprite = human.EquippedWeapon.Sprite;
                    
                }
                else
                {
                    EquippedItemIcon.sprite = null;
                }
                    
            }
            else
            {
                EquippedItemIcon.sprite = FindObjectOfType<ResourceScript>().Sprites.WolfClaw;
            }
        }
        private void HpValueChanged()
        {
            if (hpBar != null && Unit != null)
                hpBar.SetHealth(Unit.Hp, Unit.Stats.MaxHp);
        }
        private void ApValueChanged()
        {
            if (ApText != null && Unit != null)
            {
                ApText.text = "" + Unit.Ap;
                ApText.color = ColorManager.Instance.GetFactionColor(Unit.Faction.Id);
                if (Unit.Faction.Id != GridGameManager.Instance.FactionManager.ActiveFaction.Id)
                {

                    //ApText.transform.GetChild(0).gameObject.SetActive(false);
                }
                else if (Unit.Ap == 0)
                {
                    ApText.color = ColorManager.Instance.MainGreyColor;
                    //ApText.transform.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    //ApText.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }
        private void SpValueChanged()
        {
            if (spBar != null && Unit != null)
                spBar.SetHealth(Unit.Sp, Unit.Stats.MaxSp);
        }
      

        private void SetWaitingSprite(Unit unit, bool waiting)
        {
            if (unit == Unit)
            {
                GetComponentInChildren<SpriteRenderer>().color = !waiting ? Color.white : Color.grey;
            }
        }



    }
}