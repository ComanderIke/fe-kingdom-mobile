using System.Collections.Generic;
using UnityEngine;

namespace Game.Dialog
{
    [CreateAssetMenu(fileName="Conversation", menuName="GameData/Dialog/Conversation")]
    public class Conversation : ScriptableObject
    {
        public List<Line> lines;


    }
}