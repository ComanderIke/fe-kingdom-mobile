using System.Collections.Generic;
using System.Linq;
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
        private Dictionary<IAIAgent, List<ICombatResult>> agentCombatResults;

        private void UpdateCombatResults()
        {
            agentCombatResults = new Dictionary<IAIAgent, List<ICombatResult>>();
            foreach (var u in aiSystem.PlayerFaction.Units)
            {
                List<ICombatResult> results = new List<ICombatResult>();
                foreach (var target in u.AIComponent.AttackableTargets)
                {
                    results.Add(battleSystem.GetCombatResultAtAttackLocation(u, target.Target, target.OptimalAttackPos));

                }
                agentCombatResults.Add(u, results);
            }
        }
        public void OnGUI()
        {
            if (aiSystem == null)
                return;
            if(agentCombatResults==null)
                UpdateCombatResults();
            if (GUILayout.Button("Update"))
            {
                UpdateCombatResults();
            }
            if (GUILayout.Button("ShowTargets"))
            {
                aiSystem.ShowInitTurnData();
            }
            if (GUILayout.Button("HideTargets"))
            {
                aiSystem.AiRenderer.Hide();
            }
            GUILayout.Label("Current Units: ");

            
            foreach (var u in aiSystem.GetMoveOrderList())
            {
                if (u == selectedAgent)
                {
                    GUILayout.Label("Unit: " + u + ", ");
                }
                else if (GUILayout.Button("Unit: " + u + ", "))
                {
                    selectedAgent = u;
                    
                    aiSystem.AiRenderer.ShowAgentData(selectedAgent);
                }
            }
            GUILayout.Label("AttackerList: ");
            foreach (var u in aiSystem.GetAttackerList())
            {
                if (u == selectedAgent)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Unit: " + u + ", ");
                    int index = 0;
                    foreach (var target in u.AIComponent.AttackableTargets)
                    {

                        string BattleResultString = "W";
                        if(!agentCombatResults.ContainsKey(u))
                            UpdateCombatResults();
                        if (!agentCombatResults.ContainsKey(u))
                        {
                            continue;
                        }
                        if(index>= agentCombatResults[u].Count())
                            continue;
                        var result = agentCombatResults[u][index];
                            switch (result.BattleResult)
                            {
                                case BattleResult.Draw:
                                    BattleResultString = "D";
                                    break;
                                case BattleResult.Loss:
                                    BattleResultString = "L";
                                    break;
                            }

                            if (GUILayout.Button("Target: " + target.Target + " " + BattleResultString + " " +
                                                 result.GetDamageRatio()))
                            {
                                selectedTarget = target.Target;
                            }
                        

                        index++;
                        // if (selectedTarget == target.Target)
                        // {
                        //     GUILayout.Label("Position: "+target.OptimalAttackPos);
                        //     GUILayout.Label("Result: "+combatResult.BattleResult);
                        //     GUILayout.Label("DamageRatio: "+combatResult.GetDamageRatio());
                        // }
                    }
                    GUILayout.EndHorizontal();
                }
                else if (GUILayout.Button("Unit: " + u+ ", "))
                {
                    selectedAgent = u;
                    
                    aiSystem.AiRenderer.ShowAgentData(selectedAgent);
                }
            }

           
        }
    }
    
    }