using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Monsters;
using Game.Manager;
using Game.WorldMapStuff.Systems;
using UnityEngine;
using Utility;

public class WM_ActorRenderer:MonoBehaviour
{
    
        [SerializeField] private CanvasGroup alphaCanvas;
        [SerializeField] private SpriteRenderer sprite;
        
        public IWM_Actor unit;

        private void Start()
        {
            unit.TurnStateManager.UnitWaiting += SetWaitingSprite;

        }
        public void Init()
        {
           // hpBar.GetComponent<Image>().color = ColorManager.Instance.GetFactionColor(unit.Faction.Id);
            float intensity = 2;
            sprite.material.SetColor("_OutLineColor", ColorManager.Instance.GetFactionColor(unit.Faction.Id)*intensity);
            //spBar.GetComponent<Image>().color = ColorManager.Instance.GetFactionColor(Unit.Faction.Id);
            
            if(unit.Faction.Id != GridGameManager.Instance.FactionManager.ActivePlayerNumber)
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
        void Destroy()
        {
            unit.TurnStateManager.UnitWaiting -= SetWaitingSprite;
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
 
        private void SetWaitingSprite(IActor unit, bool waiting)
        {
            if ( unit == this.unit)
            {
                GetComponentInChildren<SpriteRenderer>().color = !waiting ? Color.white : Color.grey;
            }
        }

}