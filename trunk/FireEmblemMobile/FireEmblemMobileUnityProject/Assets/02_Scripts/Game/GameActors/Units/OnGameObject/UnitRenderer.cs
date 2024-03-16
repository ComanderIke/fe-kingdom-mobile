using System;
using Game.GameActors.Factions;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Monsters;
using Game.GameMechanics;
using Game.GUI;
using Game.GUI.Other;
using Game.Manager;
using Game.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

using Utility;

namespace Game.GameActors.Units.OnGameObject
{
    public class UnitRenderer : MonoBehaviour
    {

        [SerializeField]Color waitingBlueColor;
        [SerializeField]Color waitingRedColor;
        [SerializeField]Color normalBlueColor;
        [SerializeField]Color normalRedColor;
        [SerializeField] private GameObject curseVfxPrefab;
        [SerializeField] private GameObject curseResistedVfxPrefab;
        [SerializeField] private Image hpBarImage;
        [SerializeField]private CanvasGroup grayOutCanvasObject;
        [SerializeField] private ParticleSystem canMoveVFX;
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
        [SerializeField] private Image dropableItem;
        
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

        private GameObject curseInstantiated;
        public void ShowCurse()
        {
            if (curseInstantiated != null)
                return;
            
            curseInstantiated = Instantiate(curseVfxPrefab, transform);
            curseInstantiated.transform.Translate(new Vector3(.5f,0,0),Space.Self);
            
        }

        public void HideCurse()
        {
            if (curseInstantiated == null)
                return;
            Destroy(curseInstantiated);
        }
        private void Start()
        {
            Faction.UnitAddedStatic += FactionChanged;
            unit.HpValueChanged += HpValueChanged;
            if (unit != null && unit.TurnStateManager != null)
            {
                unit.TurnStateManager.UnitWaiting += SetWaitingSprite;

                unit.TurnStateManager.UnitCanMove += ToogleMoveEffect;
            }

            Unit.OnEquippedWeapon += OnEquippedWeapon;
            unit.OnAddCurse += CurseChanged;
            unit.OnRemoveCurse += CurseChanged;
            unit.OnCurseResisted += CurseResisted;
            if(unit.IsCursed())
                ShowCurse();
            dropableItem.gameObject.SetActive(unit.DropableItem!=null);
           // hoverCanvas.alpha = 0;
            HpValueChanged();
        }

        void CurseResisted()
        {
            var go = Instantiate(curseResistedVfxPrefab, transform) as GameObject;
            go.transform.Translate(new Vector3(.5f,.5f,0),Space.Self);

        }

        void CurseChanged(Curse curse)
        {
            if(unit.IsCursed())
                ShowCurse();
            else
            {
                HideCurse();
            }
        }

        void FactionChanged(Unit unit)
        {
            if (unit.Equals(this.unit))
            {
                MyDebug.LogTest("WHUUUUT");
                Init();
            }
                
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
            MyDebug.LogTest("INIT UNIT: "+ColorManager.Instance.GetFactionColor(unit.Faction.Id)+" "+unit.Faction.Id);
            hpBar.GetComponent<Image>().color = ColorManager.Instance.GetFactionColor(unit.Faction.Id);
            if(unit.Faction.IsPlayerControlled)
                hpBarAlternate.SetAllyColors();
            else 
            {
                hpBarAlternate.SetEnemyColors();
            }
            hpText.color=ColorManager.Instance.GetFactionColor(unit.Faction.Id);
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
            Faction.UnitAddedStatic -= FactionChanged;
            unit.HpValueChanged -= HpValueChanged;
            unit.OnAddCurse -= CurseChanged;
            unit.OnRemoveCurse -= CurseChanged;
            unit.OnCurseResisted -= CurseResisted;
            // Unit.SpValueChanged -= SpValueChanged;
            // Unit.SpBarsValueChanged -= SpBarsValueChanged;
            if(unit!=null)
                unit.TurnStateManager.UnitCanMove -= ToogleMoveEffect;
            if(unit!=null)
                unit.TurnStateManager.UnitWaiting -= SetWaitingSprite;
            Unit.OnEquippedWeapon -= OnEquippedWeapon;
        }

        void ToogleMoveEffect(bool canMove)
        {
            // Debug.Log("Unit can move: "+canMove);
            if(canMove)
                canMoveVFX.Play();
            else
                canMoveVFX.Stop();
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
                MyDebug.LogTest("EquippedWeapon null");
                return;
            }

            if (weaponTypeIcon == null)
            {
                MyDebug.LogTest("WeaponTypeIcon is null");
            }
            if (unit.equippedWeapon.WeaponType == null)
            {
                MyDebug.LogTest("WeaponType is null" +unit.name+" "+unit.equippedWeapon.Name);
            }
            if (unit.equippedWeapon.WeaponType.Icon == null)
            {
                MyDebug.LogTest("Icon is null");
            }
            weaponTypeIcon.sprite = unit.equippedWeapon.WeaponType.Icon;
           
        }
        private void HpValueChanged()
        {
            MyDebug.LogTest("HPVALUECHANGED");
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
            // Debug.Log("SHOW HP BAR");
            if (hpBarAlternate != null && unit != null){
                // if (afterBattleHp > unit.Hp)
                // {
                //     hpBarAlternate.UpdateValues(unit.MaxHp, afterBattleHp, unit.Hp);
                // }
                // else
                    hpBarAlternate.UpdateValues(unit.MaxHp, unit.Hp, afterBattleHp);
                
            }
        }
        private void SetWaitingSprite(bool waiting)
        {

            MyDebug.LogTest("SETWAITING"+unit.IsPlayerControlled());
             hpBarImage.color = waiting ? (unit.IsPlayerControlled()?waitingBlueColor:waitingRedColor) : (unit.IsPlayerControlled()?normalBlueColor:normalRedColor);
            hpText.color = waiting ? (unit.IsPlayerControlled()?waitingBlueColor:waitingRedColor):ColorManager.Instance.GetFactionColor(unit.Faction.Id);
            //grayOutCanvasObject.alpha=waiting?.6f:1f;
            GetComponentInChildren<SpriteRenderer>().color = !waiting ? Color.white : Color.grey;
        }


        public void ShowEffectiveness(Unit character)
        {
            //Debug.Log("Show Effectivness: "+unit.name+" ");
            //Debug.Log(unit.GridComponent.GridPosition);
           // Debug.Log(this.GetInstanceID());
            // if (character.BattleComponent.IsEffective(unit.MoveType))
            // {
            //     float eff = character.BattleComponent.GetEffectiveCoefficient(unit.MoveType);
            //     
            //     if(eff<1)
            //     {
            //         moveTypeAnimator.SetBool(Effective, false);
            //         moveTypeAnimator.SetBool(Ineffective, true);
            //     }
            //     else
            //     {
            //         moveTypeAnimator.SetBool(Effective, true);
            //         moveTypeAnimator.SetBool(Ineffective, false);
            //     }
            // }
            // else
            // {
            //     moveTypeAnimator.SetBool(Effective, false);
            //     moveTypeAnimator.SetBool(Ineffective, false);
            // }

            if (character.BattleComponent.IsEffective(unit.equippedWeapon.WeaponType))
            {
                float eff= character.BattleComponent.GetEffectiveCoefficient(unit.equippedWeapon.WeaponType);
                if(eff<1)
                {
                    // weaponTypeAnimator.SetBool(Effective, false);
                    // weaponTypeAnimator.SetBool(Ineffective, true);
                }
                else 
                {
                    
                    // weaponTypeAnimator.SetBool(Effective, true);
                    // weaponTypeAnimator.SetBool(Ineffective, false);
                }
            }
            else
            {
                // weaponTypeAnimator.SetBool(Effective, false);
                // weaponTypeAnimator.SetBool(Ineffective, false);
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
                // Debug.Log("HidePreviewHP"+unit.name);
                hpBarAlternate.UpdateValuesWithoutDamagePreview(unit.MaxHp, unit.Hp,0);
                
            }
        }

        
    }
}