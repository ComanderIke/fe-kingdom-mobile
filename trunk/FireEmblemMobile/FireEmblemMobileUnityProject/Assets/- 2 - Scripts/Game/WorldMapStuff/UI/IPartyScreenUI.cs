using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.WorldMapStuff.UI
{
    public abstract class IPartyScreenUI : MonoBehaviour
    {
        public abstract void Show(Party party);
        public abstract void Hide();
    }
}