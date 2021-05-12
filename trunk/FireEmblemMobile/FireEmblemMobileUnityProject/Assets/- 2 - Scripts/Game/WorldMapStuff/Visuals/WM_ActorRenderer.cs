using Game.GameActors.Units;
using Game.Manager;
using Game.WorldMapStuff.Model;
using UnityEngine;
using Utility;

public class WM_ActorRenderer:MonoBehaviour
{
    
        [SerializeField] private CanvasGroup alphaCanvas;
        [SerializeField] private SpriteRenderer sprite;
        
        public WM_Actor actor;

        private void Start()
        {
            actor.TurnStateManager.UnitWaiting += SetWaitingSprite;
            actor.TurnStateManager.onSelected += ShowSelected;
        }
        public void Init()
        {
           // hpBar.GetComponent<Image>().color = ColorManager.Instance.GetFactionColor(unit.Faction.Id);
            float intensity = 2;
            sprite.material.SetColor("_OutLineColor", ColorManager.Instance.GetFactionColor(actor.Faction.Id)*intensity);
            //spBar.GetComponent<Image>().color = ColorManager.Instance.GetFactionColor(Unit.Faction.Id);
            
            if(actor.Faction.Id != GridGameManager.Instance.FactionManager.ActivePlayerNumber)
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
            actor.TurnStateManager.UnitWaiting -= SetWaitingSprite;
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

        public void ShowSelected(bool selected)
        {
           
            if (selected)
            {
                float intensity = 2;
                sprite.material.SetColor("_OutLineColor",
                    ColorManager.Instance.GetFactionColor(actor.Faction.Id) * intensity);
            }
            else
            {
                float intensity = 2;
                sprite.material.SetColor("_OutLineColor",
                    ColorManager.Instance.MainRedColor*intensity);
            }
        }
 
        private void SetWaitingSprite(IActor unit, bool waiting)
        {
            if ( unit == this.actor)
            {
                GetComponentInChildren<SpriteRenderer>().color = !waiting ? Color.white : Color.grey;
            }
        }

}