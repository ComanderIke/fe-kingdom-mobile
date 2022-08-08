using Game.AI;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.Manager;
using Game.Mechanics;
using Game.WorldMapStuff.Model;
using UnityEditor;
using UnityEngine;

namespace __2___Scripts.External.Editor
{
    public class AIWindow: EditorWindow
    {
        private AISystem aiSystem;
        private BattleSystem battleSystem;
        [MenuItem("Tools/AI_Window")]
        public static void ShowMyEditor()
        {
            // This method is called when the user selects the menu item in the Editor
            EditorWindow wnd = GetWindow<AIWindow>();
            wnd.titleContent = new GUIContent("AI Viewer");
            
        }
       
        public void Update()
        {
            if (aiSystem == null)
            {
                if(GridGameManager.Instance!=null)
                    aiSystem = GridGameManager.Instance.GetSystem<AISystem>();
            }
            if (battleSystem == null)
            {
                if(GridGameManager.Instance!=null)
                    battleSystem = GridGameManager.Instance.GetSystem<BattleSystem>();
            }

        }

        private IAIAgent selectedAgent = null;
        private IAttackableTarget selectedTarget = null;
        private ICombatResult combatResult = null;
        public void OnGUI()
        {
            if (aiSystem == null)
                return;
            if (GUILayout.Button("ShowTargets"))
            {
                aiSystem.ShowInitTurnData();
            }
            if (GUILayout.Button("HideTargets"))
            {
                aiSystem.AiRenderer.Hide();
            }
            GUILayout.Label("Current Units: ");

            
            foreach (var u in aiSystem.PlayerFaction.Units)
            {
                if (u == selectedAgent)
                {
                    GUILayout.Label("Unit: " + u.name + ", ");
                    foreach (var target in u.AIComponent.AttackableTargets)
                    {
                        if (GUILayout.Button("Target: "+target.Target+ "show CombatInfo:"))
                        {
                            selectedTarget = target.Target;
                            combatResult = battleSystem.GetCombatResultAtAttackLocation(u, target.Target, target.OptimalAttackPos);
                            Debug.Log(combatResult.GetDamageRatio());
                          
                        }

                        if (selectedTarget == target.Target)
                        {
                            GUILayout.Label("Position: "+target.OptimalAttackPos);
                            GUILayout.Label("Result: "+combatResult.BattleResult);
                            GUILayout.Label("DamageRatio: "+combatResult.GetDamageRatio());
                        }
                    }
                }
                else if (GUILayout.Button("Unit: " + u.name + ", "))
                {
                    selectedAgent = u;
                    
                    aiSystem.AiRenderer.ShowAgentData(selectedAgent);
                }
            }

           
        }
    }
    
    }