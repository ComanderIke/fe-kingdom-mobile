using Game.GameActors.Units;
using Game.GUI.Controller;
using UnityEngine;

namespace Game.GUI.Other
{
    [ExecuteInEditMode]
    public abstract class ICharacterUI : MonoBehaviour
    {
        public abstract void Show(Unit unit);

        public abstract void Hide();

        public abstract ExpBarController GetExpRenderer();
        
    }
}