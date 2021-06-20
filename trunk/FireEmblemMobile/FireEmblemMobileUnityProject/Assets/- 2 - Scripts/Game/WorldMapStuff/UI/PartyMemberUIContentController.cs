using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.WorldMapStuff.UI
{
    public class PartyMemberUIContentController : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                Debug.Log("DROP ON CONTENTS");
                var partyUI=eventData.pointerDrag.gameObject.GetComponent<PartyMemberUIController>();
                if (partyUI != null)
                {
                    partyUI.UpdateParent(this.transform);
                }
               
            }
        }
    }
}
