using System;
using System.Collections.Generic;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameResources;
using Game.Manager;
using Game.Mechanics;
using Game.WorldMapStuff.Model;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace __2___Scripts.External.Editor
{
   
    public class ConvoyWindow: EditorWindow
    {
        private Convoy currentConvoy;
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
               
                foreach (var item in currentConvoy.Items)
                {
                    if (item != null)
                    {
                        if (item.item != null)
                        {
                            GUILayout.Label("Item: " + item.item.Name + " x" + item.stock);
                        }
                    }
                }
                GUILayout.Label("Itemcount: "+currentConvoy.Items.Count );
                if (GUILayout.Button("Add HealthPotion",GUILayout.Width(100)))
                {
                    Player.Instance.Party.Convoy.AddStockedItem(new StockedItem(GameBPData.Instance.GetItemByName("Health Potion"), 1));
                }
                if (GUILayout.Button("Add Bomb",GUILayout.Width(100)))
                {
                    Player.Instance.Party.Convoy.AddStockedItem(new StockedItem(GameBPData.Instance.GetItemByName("Gunpowder Bomb"), 1));
                }
                if (GUILayout.Button("Add Relic",GUILayout.Width(100)))
                {
                    Player.Instance.Party.Convoy.AddStockedItem(new StockedItem(GameBPData.Instance.GetRandomRelic(1), 1));
                }

                
            }
            else
            {
                GUILayout.Label("No Convoy exists!");
            }
        }
    }
}