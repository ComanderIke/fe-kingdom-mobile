using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Monsters;
using Game.GameResources;
using Game.GUI;
using Game.Manager;
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
        [SerializeField] private StatsBarOnMap spBar;
        [SerializeField] private ISPBarRenderer spBars;
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private GameObject pointLight;
        [SerializeField] private Image weaponTypeIcon;
        [SerializeField] private Image moveTypeIcon;
        [SerializeField] private Animator weaponTypeAnimator;
        [SerializeField] private Animator moveTypeAnimator;
        [SerializeField] private Image EquippedItemIcon;
        //[SerializeField] private Image EquippedItemBackground;
        [SerializeField] private GameObject spriteMask;
        [SerializeField] private CanvasGroup alphaCanvas;
        [SerializeField] private CanvasGroup hoverCanvas;
        [SerializeField] public SpriteRenderer sprite;
        [SerializeField] public GameObject attackDamageObject;
        [SerializeField] private TextMeshProUGUI attackDamageText;
        [SerializeField] private TextMeshProUGUI xText;
        [SerializeField] private TextMeshProUGUI x2Text;
        
        [FormerlySerializedAs("unitBp")] public Unit unit;
        private static readonly int Effective = Animator.StringToHash("effective");
        private static readonly int Ineffective = Animator.StringToHash("ineffective");

        public void HideAttackDamage()
        {
            attackDamageObject.SetActive(false);
        }
        public void ShowAttackDamage(Unit compareUnit)
        {
            
            int dmg= unit.BattleComponent.BattleStats.GetDamageAgainstTarget(compareUnit);
            bool doubleAttack = unit.BattleComponent.BattleStats.CanDoubleAttack(compareUnit);
         
            attackDamageObject.SetActive(true);
            attackDamageText.SetText("" + dmg);
            xText.gameObject.SetActive(doubleAttack);
            x2Text.gameObject.SetActive(doubleAttack);
        }
        private void Start()
        {
            unit.HpValueChanged += HpValueChanged;
            // Unit.SpValueChanged += SpValueChanged;
            // Unit.SpBarsValueChanged += SpBarsValueChanged;
            if(unit!=null)
                unit.TurnStateManager.UnitWaiting += SetWaitingSprite;
            Unit.OnEquippedWeapon += OnEquippedWeapon;
            
           // hoverCanvas.alpha = 0;
            HpValueChanged();
            // SpValueChanged();
            // SpBarsValueChanged();
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
            hpBar.GetComponent<Image>().color = ColorManager.Instance.GetFactionColor(unit.Faction.Id);
            hpText.color = ColorManager.Instance.GetFactionColor(unit.Faction.Id);
            float intensity = 2;
            moveTypeIcon.sprite = unit.MoveType.icon;
            if(moveTypeIcon.sprite==null)
                moveTypeIcon.gameObject.SetActive(false);
            sprite.material.SetColor("_OutLineColor", ColorManager.Instance.GetFactionColor(unit.Faction.Id)*intensity);
            //spBar.GetComponent<Image>().color = ColorManager.Instance.GetFactionColor(Unit.Faction.Id);
            HpValueChanged();
            // SpBarsValueChanged();
            // SpValueChanged();
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
            weaponTypeIcon.sprite = unit.equippedWeapon.WeaponType.Icon;
            // EquippedItemBackground.color = ColorManager.Instance.GetFactionColor(unit.Faction.Id);
            // if (unit is Human human) {
            //     if (human.EquippedWeapon != null)
            //     {
            //         EquippedItemIcon.sprite = human.EquippedWeapon.Sprite;
            //         
            //     }
            //     else
            //     {
            //         EquippedItemIcon.sprite = null;
            //     }
            //         
            // }
            // else if(unit is Monster monster)
            // {
            //     EquippedItemIcon.sprite = monster.Weapon.Sprite; //FindObjectOfType<ResourceScript>().sprites.WolfClaw;
            // }
        }
        private void HpValueChanged()
        {
            if (hpBar != null && unit != null){
                hpBar.SetValue(unit.Hp, unit.MaxHp);
                hpText.SetText(""+unit.Hp);
            }
        }
       
        // private void SpValueChanged()
        // {
        //     if (spBar != null && unit != null)
        //         spBar.SetValue(unit.Sp, unit.Stats.MaxSp);
        // }
        // private void SpBarsValueChanged()
        // {
        //     if (spBars != null && unit != null)
        //         spBars.SetValue(unit.SpBars, unit.MaxSpBars);
        // }
      

        private void SetWaitingSprite(bool waiting)
        {

            GetComponentInChildren<SpriteRenderer>().color = !waiting ? Color.white : Color.grey;
        }


        public void ShowEffectiveness(Unit character)
        {
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
            moveTypeAnimator.SetBool(Effective, false);
            moveTypeAnimator.SetBool(Ineffective, false);
            weaponTypeAnimator.SetBool(Effective, false);
            weaponTypeAnimator.SetBool(Ineffective, false);
        }
    }
}