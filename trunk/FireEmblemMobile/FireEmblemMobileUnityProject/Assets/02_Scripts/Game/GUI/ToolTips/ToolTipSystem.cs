using Game.GameActors.Items;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Units;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.Skills.Base;
using Game.GameMechanics;
using Game.GUI.Base;
using Game.GUI.EncounterUI.Merchant;
using Game.States.Mechanics.Battle;
using UnityEngine;

namespace Game.GUI.ToolTips
{
    public class ToolTipSystem : MonoBehaviour
    {
        [SerializeField] private Canvas tooltipCanvas;
        private static ToolTipSystem instance;
        public ItemToolTip ItemToolTip;
        public WeaponToolTip WeaponToolTip;
        public SkillToolTip skillToolTip;
        public BlessingTooltip blessingTooltip;
        public SkillToolTip curseTooltip;
        public AttributeValueTooltipUI AttributeValueTooltipUI;
        public CombatStatValueTooltipUI CombatStatalueTooltipUI;
   
        public UITimeOfDayTooltip TimeOfDayTooltip;
        public UIMoralityBarTooltip MoralityBarTooltip;
        public void Awake()
        {
            instance = this;
        }

        static void CloseAllToolTips()
        {
       
      
            instance.WeaponToolTip.gameObject.SetActive(false);

            if(instance.MoralityBarTooltip!=null)
                instance.MoralityBarTooltip.gameObject.SetActive(false);
            instance.skillToolTip.gameObject.SetActive(false);
      
            //instance.skillTreeToolTip.gameObject.SetActive(false);
            instance.blessingTooltip.gameObject.SetActive(false);
            instance.curseTooltip.gameObject.SetActive(false);
            instance.ItemToolTip.gameObject.SetActive(false);
            instance.CombatStatalueTooltipUI.gameObject.SetActive(false);
            instance.TimeOfDayTooltip.gameObject.SetActive(false);
            instance.AttributeValueTooltipUI.gameObject.SetActive(false);
        
        }
    
        public static void Show(Item item, Vector3 position, bool screenPos=false, bool exactPos=false)
        {
            Show(new StockedItem(item, 1), position, screenPos, exactPos);
        }
        public static void Show(StockedItem item, Vector3 position, bool screenPos=false, bool exactPos=false)
        {
            MyDebug.LogInput("Show StockedItem Tooltip" + item.item.Name);
            instance.tooltipShownThisFrame = true;
            CloseAllToolTips();

            instance.ItemToolTip.SetValues(item, screenPos?position:GetAnchoredPositionInTooltipCanvas(position), exactPos);
        
            instance.ItemToolTip.gameObject.SetActive(true);
        }
        public static void Show(Unit user,Weapon weapon, Vector3 position)
        {
            MyDebug.LogInput("Show Weapon Tooltip");
            instance.tooltipShownThisFrame = true;
            CloseAllToolTips();
            instance.WeaponToolTip.SetValues(user,weapon, GetAnchoredPositionInTooltipCanvas(position));
            instance.WeaponToolTip.gameObject.SetActive(true);
        }

        private static Vector2 GetAnchoredPositionInTooltipCanvas(Vector2 position)
        {
            // MyDebug.LogTest("TooltipCaller position: "+ position);
            Vector2 tooltipAnchoredPos;
            Vector2 startPos = Camera.main.WorldToScreenPoint(position);
            // MyDebug.LogTest("TooltipCaller screen position: "+ startPos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(instance.transform as RectTransform, startPos, Camera.main,
                out tooltipAnchoredPos);
            tooltipAnchoredPos += new Vector2((instance.transform as RectTransform).rect.width / 2f,(instance.transform as RectTransform).rect.height / 2f);
            MyDebug.LogTest("Tooltip new anchoredPosition: "+ tooltipAnchoredPos);

            return tooltipAnchoredPos;
        }
   
        private bool tooltipShownThisFrame = false;
        private void Update()
        {
     
            if(InputUtility.TouchEnd()&&!tooltipShownThisFrame)
                CloseAllToolTips();
        
            instance.tooltipShownThisFrame = false;
        }
        public static void Show(Blessing blessing, Vector3 position)
        {
            instance.tooltipShownThisFrame = true;
            CloseAllToolTips();
            //Debug.Log(skill.Name);
            //Debug.Log("TooltipPosition: "+GameObject.FindWithTag("UICamera").GetComponent<Camera>().WorldToScreenPoint(position));
            instance.blessingTooltip.SetValues(blessing,GetAnchoredPositionInTooltipCanvas(position));
        
            instance.blessingTooltip.gameObject.SetActive(true);
        }
        public static void Show(Curse curse, Vector3 position)
        {
            instance.tooltipShownThisFrame = true;
            CloseAllToolTips();
            //Debug.Log(skill.Name);
            //Debug.Log("TooltipPosition: "+GameObject.FindWithTag("UICamera").GetComponent<Camera>().WorldToScreenPoint(position));
            instance.curseTooltip.SetValues(curse, true, false,GetAnchoredPositionInTooltipCanvas(position));
        
            instance.curseTooltip.gameObject.SetActive(true);
        }

        public static void Show(Skill skill, bool blessed,Vector3 position, bool screenPos=false)
        {
            if (skill is Curse curse)
            {
                Show(curse, position);
                return;
            }
            
            instance.tooltipShownThisFrame = true;
            CloseAllToolTips();
            //Debug.Log(skill.Name);
            //Debug.Log("TooltipPosition: "+GameObject.FindWithTag("UICamera").GetComponent<Camera>().WorldToScreenPoint(position));
            instance.skillToolTip.SetValues(skill, blessed, false,screenPos?position:GetAnchoredPositionInTooltipCanvas(position));
        
            instance.skillToolTip.gameObject.SetActive(true);
        }
    
   

        public static void Hide()
        {
            instance.ItemToolTip.gameObject.SetActive(false);
        }
    
        public static void ShowAttributeValue(Unit unit,AttributeType attributeType, Vector3 position)
        {
            instance.tooltipShownThisFrame = true;
            CloseAllToolTips();
            // Debug.Log("transformPos: "+position+" ScreenPos"+Camera.main.WorldToScreenPoint(position));
            instance.AttributeValueTooltipUI.gameObject.SetActive(true);
            instance.AttributeValueTooltipUI.Show(unit,  attributeType, GetAnchoredPositionInTooltipCanvas(position));
        }
        public static void ShowCombatStatValue(Unit unit,CombatStats.CombatStatType combatStatType, Vector3 position)
        {
            instance.tooltipShownThisFrame = true;
            CloseAllToolTips();
            // Debug.Log("transformPos: "+position+" ScreenPos"+Camera.main.WorldToScreenPoint(position));
            instance.CombatStatalueTooltipUI.gameObject.SetActive(true);
            instance.CombatStatalueTooltipUI.Show(unit,  combatStatType, GetAnchoredPositionInTooltipCanvas(position));
        }


        public static void ShowTimeOfDay(float hour, TimeOfDayBonuses bonus)
        {
            instance.tooltipShownThisFrame = true;
            CloseAllToolTips();
            instance.TimeOfDayTooltip.Show((int) hour, bonus);
            instance.TimeOfDayTooltip.gameObject.SetActive(true);
        }

        public static void ShowMorality(float morality)
        {
            instance.tooltipShownThisFrame = true;
            CloseAllToolTips();
            instance.MoralityBarTooltip.Show(morality);
            instance.MoralityBarTooltip.gameObject.SetActive(true);
        }
    }
}