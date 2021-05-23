using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.WorldMapStuff.Manager
{
    public abstract class IPartySelectionRenderer : MonoBehaviour
    {
        public abstract void Hide();


        public abstract void Show(WM_Actor selectedActor);

    }
}