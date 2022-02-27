using Game.GameActors.Units;
using UnityEngine;

namespace Game.GUI
{
    [ExecuteInEditMode]
    public abstract class ICharacterUI : MonoBehaviour
    {
        public abstract void Show(Unit unit);

        public abstract void Hide();

        public abstract ExpBarController GetExpRenderer();
        
    }
}