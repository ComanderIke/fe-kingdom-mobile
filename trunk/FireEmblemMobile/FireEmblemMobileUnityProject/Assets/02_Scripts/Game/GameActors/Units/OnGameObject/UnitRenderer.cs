using System;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Monsters;
using Game.GameResources;
using Game.GUI;
using Game.Manager;
using LostGrace;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

using Utility;

namespace Game.GameActors.Units.OnGameObject
{
    public class UnitRenderer : MonoBehaviour
    {

        [SerializeField] private StatsBarOnMap hpBar;
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private GameObject pointLight;
        [SerializeField] private Image weaponTypeIcon;
        [SerializeField] private Image moveTypeIcon;
        [SerializeField] private Animator weaponTypeAnimator;
        [SerializeField] private Animator moveTypeAnimator;
        [SerializeField] private AttackPreviewStatBar hpBarAlternate;
        [SerializeField] private TextMeshProUGUI internPositionText;
        [SerializeField] private TextMeshProUGUI tilePositionText;
        [SerializeField] private TextMeshProUGUI originPositionText;
        //[SerializeField] private Image EquippedItemBackground;
        [SerializeField] private GameObject spriteMask;
        [SerializeField] private CanvasGroup alphaCanvas;
        [SerializeField] private CanvasGroup hoverCanvas;
        [SerializeField] public SpriteRenderer sprite;
        //[SerializeField] public GameObject attackDamageObject;
        //[SerializeField] private TextMeshProUGUI attackDamageText;
        [SerializeField] private TextMeshProUGUI xText;
        [SerializeField] private TextMeshProUGUI x2Text;
        
        [FormerlySerializedAs("unitBp")] public Unit unit;
        private static readonly int Effective = Animator.StringToHash("effective");
        private static readonly int Ineffective = Animator.StringToHash("ineffective");

        public void HideAttackDamage()
        {
            //attackDamageObject.SetActive(false);
        }
        public void ShowAttackDamage(Unit compareUnit)
        {
            
            int dmg= unit.BattleComponent.BattleStats.GetDamageAgainstTarget(compareUnit);
            bool doubleAttack = unit.BattleComponent.BattleStats.CanDoubleAttack(compareUnit);
         
            //attackDamageObject.SetActive(true);
            //attackDamageText.SetText("" + dmg);
            xText.gameObject.SetActive(doubleAttack);
            x2Text.gameObject.SetActive(doubleAttack);
        }
        private void Start()
        {
            unit.HpValueChanged += HpValueChanged;
    
            if(unit!=null)
                unit.TurnStateManager.UnitWaiting += SetWaitingSprite;
            Unit.OnEquippedWeapon += OnEquippedWeapon;
            
           // hoverCanvas.alpha = 0;
            HpValueChanged();
        }

        private void Update()
        {
            if (unit != null&& GameConfig.Instance.ConfigProfile.debugModeEnabled)
            {
                internPositionText.enabled = true;
                tilePositionText.enabled = true;
                originPositionText.enabled = true;
                internPositionText.SetText("Grid: "+unit.GridComponent.GridPosition.X+"/"+unit.GridComponent.GridPosition.Y);
                tilePositionText.SetText("Tile: "+unit.GridComponent.Tile.X+"/"+unit.GridComponent.Tile.Y);
                originPositionText.SetText("Origin: "+unit.GridComponent.OriginTile.X+"/"+unit.GridComponent.OriginTile.Y);
            }
            else
            {
                internPositionText.enabled = false;
                tilePositionText.enabled = false;
                originPositionText.enabled = false;
            }
        }

        public void HideHover()
        {
           // hoverCanvas.alpha = 0;
        }
        public void ShowHover()
        {
         //   hoverCanvas.alpha = 1;
        }
        public void Init()
        {
            //hpBarAlternate.SetColor();
            hpBar.GetComponent<Image>().color = ColorManager.Instance.GetFactionColor(unit.Faction.Id);
            hpText.color = ColorManager.Instance.GetFactionColor(unit.Faction.Id);
            float intensity = 2;
            moveTypeIcon.sprite = unit.MoveType.icon;
            if(moveTypeIcon.sprite==null)
                moveTypeIcon.gameObject.SetActive(false);
            sprite.material.SetColor("_OutLineColor", ColorManager.Instance.GetFactionColor(unit.Faction.Id)*intensity);
  
            HpValueChanged();
   
            OnEquippedWeapon();
            if(!GridGameManager.Instance.FactionManager.IsActiveFaction(unit.Faction))
            {
                //pointLight.SetActive(false);
                Color color = Color.blue;
                GetComponentInChildren<SpriteRenderer>().flipX = true;

                //spriteMask.SetActive(false);
            }
            else {
                //pointLight.SetActive(true);
                GetComponentInChildren<SpriteRenderer>().flipX = false;
                //spriteMask.SetActive(true);
            }
        }
        void OnDestroy()
        {
            unit.HpValueChanged -= HpValueChanged;
            // Unit.SpValueChanged -= SpValueChanged;
            // Unit.SpBarsValueChanged -= SpBarsValueChanged;
            if(unit!=null)
                unit.TurnStateManager.UnitWaiting -= SetWaitingSprite;
            Unit.OnEquippedWeapon -= OnEquippedWeapon;
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
            if (unit.equippedWeapon == null)
            {
                Debug.Log("EquippedWeapon null");
                return;
            }

            if (weaponTypeIcon == null)
            {
                Debug.Log("WeaponTypeIcon is null");
            }
            if (unit.equippedWeapon.WeaponType == null)
            {
                Debug.Log("WeaponType is null" +unit.name+" "+unit.equippedWeapon.Name);
            }
            if (unit.equippedWeapon.WeaponType.Icon == null)
            {
                Debug.Log("Icon is null");
            }
            weaponTypeIcon.sprite = unit.equippedWeapon.WeaponType.Icon;
           
        }
        private void HpValueChanged()
        {
            if (hpBar != null && unit != null){
                hpBar.SetValue(unit.Hp, unit.MaxHp, false);
                hpText.SetText(""+unit.Hp);
            }
            if (hpBarAlternate != null && unit != null){
                hpBarAlternate.UpdateValuesAnimated(unit.MaxHp, unit.Hp);
                
            }
        }

        public void ShowPreviewHp(int afterBattleHp)
        {
            Debug.Log("SHOW HP BAR");
            if (hpBarAlternate != null && unit != null){
                hpBarAlternate.UpdateValues(unit.MaxHp, unit.Hp, afterBattleHp);
                
            }
        }
        private void SetWaitingSprite(bool waiting)
        {

            GetComponentInChildren<SpriteRenderer>().color = !waiting ? Color.white : Color.grey;
        }


        public void ShowEffectiveness(Unit character)
        {
            //Debug.Log("Show Effectivness: "+unit.name+" ");
            //Debug.Log(unit.GridComponent.GridPosition);
           // Debug.Log(this.GetInstanceID());
            if (character.BattleComponent.IsEffective(unit.MoveType))
            {
                float eff = character.BattleComponent.GetEffectiveCoefficient(unit.MoveType);
                
                if(eff<1)
                {
                    moveTypeAnimator.SetBool(Effective, false);
                    moveTypeAnimator.SetBool(Ineffective, true);
                }
                else
                {
                    moveTypeAnimator.SetBool(Effective, true);
                    moveTypeAnimator.SetBool(Ineffective, false);
                }
            }
            else
            {
                moveTypeAnimator.SetBool(Effective, false);
                moveTypeAnimator.SetBool(Ineffective, false);
            }

            if (character.BattleComponent.IsEffective(unit.equippedWeapon.WeaponType))
            {
                float eff= character.BattleComponent.GetEffectiveCoefficient(unit.equippedWeapon.WeaponType);
                if(eff<1)
                {
                    weaponTypeAnimator.SetBool(Effective, false);
                    weaponTypeAnimator.SetBool(Ineffective, true);
                }
                else 
                {
                    
                    weaponTypeAnimator.SetBool(Effective, true);
                    weaponTypeAnimator.SetBool(Ineffective, false);
                }
            }
            else
            {
                weaponTypeAnimator.SetBool(Effective, false);
                weaponTypeAnimator.SetBool(Ineffective, false);
            }
        }

        public void HideTemporaryVisuals()
        {
            HideAttackDamage();
            if (moveTypeAnimator != null)
            {
                moveTypeAnimator.SetBool(Effective, false);
                moveTypeAnimator.SetBool(Ineffective, false);
            }

            if (weaponTypeAnimator != null)
            {

                weaponTypeAnimator.SetBool(Effective, false);
                weaponTypeAnimator.SetBool(Ineffective, false);
            }
        }


        public void HidePreviewHp()
        {
            if (hpBarAlternate != null && unit != null){
                Debug.Log("HidePreviewHP"+unit.name);
                hpBarAlternate.UpdateValuesWithoutDamagePreview(unit.MaxHp, unit.Hp,0);
                
            }
        }
    }
}