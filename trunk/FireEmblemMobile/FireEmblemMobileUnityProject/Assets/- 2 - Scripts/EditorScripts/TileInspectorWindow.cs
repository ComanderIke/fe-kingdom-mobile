using System.Collections.Generic;
using Game.Grid;
using Game.Manager;
using Game.Map;
using GameEngine;
using GameEngine.GameStates;
using UnityEditor;
using UnityEngine;

namespace __2___Scripts.External.Editor
{
    public class TileInspectorWindow: EditorWindow
    {
        private GridSystem gridSystem;
        private Tile selectedTile;
        private int selectedTileIndex = 0;

        public TileInspectorWindow()
        {
            
        }
        [MenuItem("Tools/TileInspectorWindow")]
        public static void ShowMyEditor()
        {
            // This method is called when the user selects the menu item in the Editor
            EditorWindow wnd = GetWindow<TileInspectorWindow>();
          
            wnd.titleContent = new GUIContent("TileInspector");
        }
       
        public void Update()
        {
            if (GridGameManager.Instance == null)
                return;
            gridSystem = GridGameManager.Instance.GetSystem<GridSystem>();
            if (gridSystem != null)
            {
                var newSelectedTile=gridSystem.cursor.GetCurrentTile();
                
                if (newSelectedTile!=null && selectedTile != newSelectedTile)
                {
                    selectedTile = newSelectedTile;
                    Repaint();
              
                }
               
            }

           
        }

        public void OnGUI()
        {
            if (selectedTile != null)
            {

                GUILayout.Label("Selected Tile: "+selectedTile);
                GUILayout.Label("X: "+selectedTile.X+ " Y: "+selectedTile.Y);
                if (selectedTile.GridObject != null)
                {
                    GUILayout.Label("Actor: "+selectedTile.GridObject);
                }
                else
                {
                    GUILayout.Label("No Actor!");
                }
            }
            else
            {
                GUILayout.Label("No Tile Selected!");
            }
        }
    }
}