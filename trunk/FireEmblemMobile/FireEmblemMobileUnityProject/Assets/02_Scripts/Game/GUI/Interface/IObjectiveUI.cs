using Game.EncounterAreas.Encounters.Battle;
using UnityEngine;

namespace Game.GUI.Interface
{
    public abstract class IObjectiveUI : MonoBehaviour
    {
        public abstract void Show(BattleMap chapter);
        public abstract void Hide();
    }
}