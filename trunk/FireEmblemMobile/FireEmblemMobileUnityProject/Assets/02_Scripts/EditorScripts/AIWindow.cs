using System.Collections.Generic;
using System.Linq;
using Game.AI;
using Game.AI.DecisionMaking;
using Game.GameActors.InteractableGridObjects;
using Game.GameActors.Units;
using Game.GameActors.Units.Interfaces;
using Game.Manager;
using Game.States.Mechanics;
using Game.Systems;
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

            aiSystem.StopAIActions = GUILayout.Toggle(aiSystem.StopAIActions,"StopAIActions");
            
            if (GUILayout.Button("ShowTargets"))
            {
                aiSystem.ShowInitTurnData();
            }
            if (GUILayout.Button("HideTargets"))
            {
                aiSystem.AiRenderer.Hide();
            }
            GUILayout.Label("Current Units: ");

            var list = aiSystem.GetMoveOrderList();
            if (list == null)
                return;
            foreach (var u in list)
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
            var alist = aiSystem.GetAttackerList();
            if (alist == null)
                return;
            foreach (var u in alist)
            {
                if (u == selectedAgent)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Unit: " + u + ", ");
                    int index = 0;
                    foreach (var target in u.AIComponent.AttackableTargets)
                    {
                       // Debug.Log("Target: "+ (index+1)  +" "+target.Target);
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
                            switch (result.AttackResult)
                            {
                                case AttackResult.Draw:
                                    BattleResultString = "D";
                                    break;
                                case AttackResult.Loss:
                                    BattleResultString = "L";
                                    break;
                            }

                            if (GUILayout.Button("Target: " + target.Target + " " + BattleResultString + " " +
                                                 result.GetDamageRatio()+ " "+result.GetAttackPosition()))
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