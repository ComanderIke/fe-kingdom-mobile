using System.Collections.Generic;
using UnityEngine;

//Needed for Build only otherwise unreferenced ScriptableObjects will not be loaded
namespace Game.GUI.Other
{
    public class SingletonReferences : MonoBehaviour
    {
        public List<ScriptableObject> SingletonScriptableObjects;
        // Start is called before the first frame update
   
    }
}
