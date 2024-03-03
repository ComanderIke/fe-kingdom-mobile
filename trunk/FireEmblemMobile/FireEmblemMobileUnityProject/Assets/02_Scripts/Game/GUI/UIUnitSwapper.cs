using Game.GameActors.Player;
using UnityEngine;

namespace Game.GUI
{
    public class UIUnitSwapper : MonoBehaviour
    {
        public void NextClicked()
        {
           
            Player.Instance.Party.ActiveUnitIndex++;
        }

        public void PrevClicked()
        {
           
            Player.Instance.Party.ActiveUnitIndex--;
        }
    }
}
