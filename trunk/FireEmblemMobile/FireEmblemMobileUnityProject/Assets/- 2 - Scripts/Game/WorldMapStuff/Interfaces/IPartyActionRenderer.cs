using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.WorldMapStuff.Interfaces
{
    public abstract class IPartyActionRenderer : MonoBehaviour
    {
        public abstract void Show(Party party);
        public abstract void Hide();
    }
}
