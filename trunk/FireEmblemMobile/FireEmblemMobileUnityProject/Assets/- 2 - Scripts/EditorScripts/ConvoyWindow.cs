using System;
using System.Collections.Generic;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameResources;
using Game.Manager;
using Game.Mechanics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace __2___Scripts.External.Editor
{
   
    public class ConvoyWindow: EditorWindow
    {
        private List<StockedItem> currentConvoy;
        [MenuItem("Tools/Convoey_Window")]
        public static void ShowMyEditor()
        {
            // This method is called when the user selects the menu item in the Editor
            EditorWindow wnd = GetWindow<ConvoyWindow>();
            wnd.titleContent = new GUIContent("Convoy Viewer");
        }
       
        public void Update()
        {
            if (Player.Instance == null)
                return;
            if (Player.Instance.Party != null)
            {
                if (Player.Instance.Party.Convoy != currentConvoy)
                {
                    currentConvoy = Player.Instance.Party.Convoy;
                    
                    Repaint();
                    //Debug.Log("Repaint!");
                }
            }

           
        }

        public void OnGUI()
        {
            //Debug.Log("OnGUI");
            
            if (currentConvoy != null&& Player.Instance!=null&&Player.Instance.Party!=null&&Player.Instance.Party.Convoy!=null)
            {
              
                GUILayout.Label("Items in Convoy: " );
               
                foreach (var item in currentConvoy)
                {
                    if (item != null)
                    {
                        if (item.item != null)
                        {
                            GUILayout.Label("Item: " + item.item.Name + " x" + item.stock);
                        }
                    }
                }
                GUILayout.Label("Itemcount: "+currentConvoy.Count );
                if (GUILayout.Button("Add HealthPotion",GUILayout.Width(100)))
                {
                    Player.Instance.Party.Convoy.Add(new StockedItem(GameData.Instance.GetHealthPotion(), 1));
                }

                
            }
            else
            {
                GUILayout.Label("No Convoy exists!");
            }
        }
    }
}