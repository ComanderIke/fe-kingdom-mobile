using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using UnityEngine;

namespace LostGrace
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
