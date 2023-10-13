using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using __2___Scripts.External.Editor;
using __2___Scripts.External.Editor.Data.Save;
using _02_Scripts.EditorScripts.DialogueSystem.Elements;
using _02_Scripts.Game.GUI.Utility;
using Game.GameActors.Items;
using Game.GameActors.Units;
using LostGrace;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace _02_Scripts.Game.Dialog.DialogSystem
{
    public class IOUtility
    {
        private static string graphFileName;
        private static string containerFolderPath;
        private static LGGraphView graphView;
        private static List<DialogGroup> groups;
        private static List<DialogNode> nodes;

        private static Dictionary<string, LGDialogGroupSO> createdDialogGroups;
        private static Dictionary<string, LGDialogSO> createdDialogs;
        
        private static Dictionary<string, DialogGroup> loadedGroups;
        private static Dictionary<string, DialogNode> loadedNodes;
        public IOUtility()
        {
        }

        public static void Initialize(LGGraphView graph, string graphName)
        {
            graphView = graph;
            graphFileName = graphName;
            containerFolderPath = $"Assets/07_GameData/DialogueSystem/Dialogues/{graphFileName}";

            nodes = new List<DialogNode>();
            groups = new List<DialogGroup>();
            createdDialogGroups = new Dictionary<string, LGDialogGroupSO>();
            createdDialogs = new Dictionary<string, LGDialogSO>();
            loadedGroups = new Dictionary<string, DialogGroup>();
            loadedNodes = new Dictionary<string, DialogNode>();
        }

        public static void Load()
        {
            LgGraphSaveData graphData =
                LoadAsset<LgGraphSaveData>("Assets/02_Scripts/EditorScripts/DialogueSystem/Graphs", graphFileName);
            if (graphData == null)
            {
                EditorUtility.DisplayDialog("Couldn't load file!",
                    "The file at the following path could not be found:\n\n" +
                    $"Assets/02_Scripts/EditorScripts/DialogueSystem/Graphs/{graphFileName}\n\n", "OK");
                return;
            }
            EventWindow.UpdateFileName(graphData.FileName);

            LoadGroups(graphData.Groups);
            LoadNodes(graphData.Nodes);
            LoadEventNodes(graphData.EventNodes);
            LoadFightNodes(graphData.FightNodes);
            LoadBattleNodes(graphData.BattleNodes);
            LoadNodesConnections();
        }
        private static void LoadBattleNodes(List<LGBattleNodeSaveData> nodes)
        {
          
            foreach (LGBattleNodeSaveData nodeData in nodes)
            {
                DialogNode node = graphView.CreateNode(nodeData.Name,nodeData.DialgueType, nodeData.Position, false);

                
                List<LGChoiceSaveData> choices = CloneNodeChoices(nodeData.Choices);
               
                node.ID = nodeData.ID;
                node.Choices = choices;
                node.Text = nodeData.Text;
                node.DialogActor = nodeData.DialogActor;
              
                if (node is EventNode eventNode)
                {
               
                    List<ResourceEntry> resources = CloneNodeResources(nodeData.RewardResources);
                    List<ItemBP> items = new List<ItemBP>(nodeData.RewardItems);
                    List<DialogEvent> events = new List<DialogEvent>(nodeData.Events);
                    Debug.Log("HÄH?");
                    eventNode.ResourceRewards = resources;
                    eventNode.ItemRewards = items;
                    eventNode.Events = events;
                    eventNode.Headline = nodeData.Headline;
                    if (node is BattleNode fightNode)
                    {
                        fightNode.EnemyArmy = nodeData.EnemyArmy;
                    }
                   
                }
                
                node.Draw();
                
                graphView.AddElement(node);
                loadedNodes.Add(node.ID, node);
                if (string.IsNullOrEmpty(nodeData.GroupID))
                {
                    continue;
                }

                DialogGroup group = loadedGroups[nodeData.GroupID];
                node.Group = group;
                group.AddElement(node);
            }
        }
        private static void LoadFightNodes(List<LGFightNodeSaveData> nodes)
        {
          
            foreach (LGFightNodeSaveData nodeData in nodes)
            {
                DialogNode node = graphView.CreateNode(nodeData.Name,nodeData.DialgueType, nodeData.Position, false);

                
                List<LGChoiceSaveData> choices = CloneNodeChoices(nodeData.Choices);
               
                node.ID = nodeData.ID;
                node.Choices = choices;
                node.Text = nodeData.Text;
                node.DialogActor = nodeData.DialogActor;
              
                if (node is EventNode eventNode)
                {
               
                    List<ResourceEntry> resources = CloneNodeResources(nodeData.RewardResources);
                    List<ItemBP> items = new List<ItemBP>(nodeData.RewardItems);
                    List<DialogEvent> events = new List<DialogEvent>(nodeData.Events);
                    eventNode.ResourceRewards = resources;
                    eventNode.ItemRewards = items;
                    eventNode.Events = events;
                    eventNode.Headline = nodeData.Headline;
                    if (node is FightNode fightNode)
                    {
                        fightNode.Enemy = nodeData.Enemy;
                    }
                   
                }
                
                node.Draw();
                
                graphView.AddElement(node);
                loadedNodes.Add(node.ID, node);
                if (string.IsNullOrEmpty(nodeData.GroupID))
                {
                    continue;
                }

                DialogGroup group = loadedGroups[nodeData.GroupID];
                node.Group = group;
                group.AddElement(node);
            }
        }

        private static void LoadNodesConnections()
        {
            foreach (KeyValuePair<string, DialogNode> loadedNode in loadedNodes)
            {
               
                foreach (Port choicePort in loadedNode.Value.outputContainer.Children().Where(n=> n is Port))
                {
                    LGChoiceSaveData choiceData = (LGChoiceSaveData)choicePort.userData;
                    if (string.IsNullOrEmpty(choiceData.NodeID))
                    {
                        continue;
                    }

                    DialogNode nextNode = loadedNodes[choiceData.NodeID];
                    Port nextNodeInputPort = (Port)nextNode.inputContainer.Children().First();
                    Edge edge=choicePort.ConnectTo(nextNodeInputPort);
                    graphView.AddElement(edge);

                    loadedNode.Value.RefreshPorts();
                }

                List<LGChoiceSaveData> loadedChoiceData = new List<LGChoiceSaveData>();
                foreach (VisualElement container in loadedNode.Value.outputContainer.Children())
                {
                    foreach (Port choicePort in container.Children().Where(n=> n is Port))
                    {
                        LGChoiceSaveData choiceData = (LGChoiceSaveData)choicePort.userData;
                        if (string.IsNullOrEmpty(choiceData.NodeID))
                        {
                            continue;
                        }
                        DialogNode nextNode = loadedNodes[choiceData.NodeID];
                        if (loadedChoiceData.Contains(choiceData)&&!string.IsNullOrEmpty(choiceData.NodeFailID))
                        {
                            nextNode= loadedNodes[choiceData.NodeFailID];
                        }
                        loadedChoiceData.Add(choiceData);
                       
                        Port nextNodeInputPort = (Port)nextNode.inputContainer.Children().First();
                        Edge edge = choicePort.ConnectTo(nextNodeInputPort);
                        graphView.AddElement(edge);

                        loadedNode.Value.RefreshPorts();
                    }
                }
            }
        }

        private static void LoadNodes(List<LGNodeSaveData> nodes)
        {
            foreach (LGNodeSaveData nodeData in nodes)
            {
                DialogNode node = graphView.CreateNode(nodeData.Name,nodeData.DialgueType, nodeData.Position, false);

                
                List<LGChoiceSaveData> choices = CloneNodeChoices(nodeData.Choices);
               
                node.ID = nodeData.ID;
                node.Choices = choices;
                node.Text = nodeData.Text;
                node.DialogActor = nodeData.DialogActor;

                node.Draw();
                
                graphView.AddElement(node);
                loadedNodes.Add(node.ID, node);
                if (string.IsNullOrEmpty(nodeData.GroupID))
                {
                    continue;
                }

                DialogGroup group = loadedGroups[nodeData.GroupID];
                node.Group = group;
                group.AddElement(node);
            }
        }

        private static void LoadEventNodes(List<LGEventNodeSaveData> nodes)
        {
          
            foreach (LGEventNodeSaveData nodeData in nodes)
            {
                DialogNode node = graphView.CreateNode(nodeData.Name,nodeData.DialgueType, nodeData.Position, false);

                
                List<LGChoiceSaveData> choices = CloneNodeChoices(nodeData.Choices);
               
                node.ID = nodeData.ID;
                node.Choices = choices;
                node.Text = nodeData.Text;
                node.DialogActor = nodeData.DialogActor;
              
                if (node is EventNode eventNode)
                {
               
                    List<ResourceEntry> resources = CloneNodeResources(nodeData.RewardResources);
                    List<ItemBP> items = new List<ItemBP>(nodeData.RewardItems);
                    List<DialogEvent> events = new List<DialogEvent>(nodeData.Events);
                    eventNode.ResourceRewards = resources;
                    eventNode.ItemRewards = items;
                    eventNode.Events = events;
                    eventNode.Headline = nodeData.Headline;
                    

                }
                
                node.Draw();
                
                graphView.AddElement(node);
                loadedNodes.Add(node.ID, node);
                if (string.IsNullOrEmpty(nodeData.GroupID))
                {
                    continue;
                }

                DialogGroup group = loadedGroups[nodeData.GroupID];
                node.Group = group;
                group.AddElement(node);
            }
        }

        private static void LoadGroups(List<LGGroupSaveData> groups)
        {
            foreach (LGGroupSaveData groupData in groups)
            {
                DialogGroup group = graphView.CreateGroup(groupData.Name, groupData.Position);

                group.ID = groupData.ID;
                loadedGroups.Add(group.ID,group);
            }
        }

        public static void Save()
        {
            CreateStaticFolders();
            GetElementsFromGraphView();
            LgGraphSaveData graphSaveData = CreateAsset<LgGraphSaveData>("Assets//02_Scripts/EditorScripts/DialogueSystem/Graphs", $"{graphFileName}Graph");
            graphSaveData.Initialize(graphFileName);
            LGDialogContainerSO dialogContainer = CreateAsset<LGDialogContainerSO>(containerFolderPath, graphFileName);
            dialogContainer.Initialize(graphFileName);
            

            SaveGroups(graphSaveData, dialogContainer);
            SaveNodes(graphSaveData, dialogContainer);
            
            SaveAsset(graphSaveData);
            SaveAsset(dialogContainer);

        }

        private static void SaveNodes(LgGraphSaveData graphSaveData, LGDialogContainerSO dialogContainer)
        {
            SerializableDictionary<string, List<string>> groupNodeNames =
                new SerializableDictionary<string, List<string>>();
            List<string> ungroupedNodeNames = new List<string>();
            foreach (DialogNode node in nodes)
            {
               
                
                if (node is FightNode fightNode)
                {
                    SaveNodeToGraph(fightNode, graphSaveData);
                    SaveNodeToScriptableObject(fightNode, dialogContainer);
                }
                else if (node is BattleNode battleNode)
                {
                    SaveNodeToGraph(battleNode, graphSaveData);
                    SaveNodeToScriptableObject(battleNode, dialogContainer);
                }
                else if (node is EventNode eventNode)
                {
                    SaveNodeToGraph(eventNode, graphSaveData);
                    SaveNodeToScriptableObject(eventNode, dialogContainer);
                }
                else
                {
                    SaveNodeToGraph(node, graphSaveData);
                    SaveNodeToScriptableObject(node, dialogContainer);
                }
                
                if (node.Group != null)
                {
                    groupNodeNames.AddItem(node.Group.title, node.DialogueName);
                    continue;
                }
                
                ungroupedNodeNames.Add(node.DialogueName);
            }

            UpdateDialoguesChoicesConnections();

            UpdateOldGroupedNodes(groupNodeNames, graphSaveData);
            UpdateOldUngroupedNodes(ungroupedNodeNames, graphSaveData);
        }

        private static void UpdateOldGroupedNodes(SerializableDictionary<string, List<string>> currentGroupNodeNames, LgGraphSaveData graphSaveData)
        {
            if (graphSaveData.OldGroupedNodeNames != null && graphSaveData.OldGroupedNodeNames.Count != 0)
            {
                foreach (KeyValuePair<string, List<string>> oldGroupedNode in graphSaveData.OldGroupedNodeNames)
                {
                    List<string> nodesToRemove = new List<string>();

                    if (currentGroupNodeNames.ContainsKey(oldGroupedNode.Key))
                    {
                        nodesToRemove = oldGroupedNode.Value.Except(currentGroupNodeNames[oldGroupedNode.Key]).ToList();
                    }

                    foreach (string nodeToRemove in nodesToRemove)
                    {
                        RemoveAsset($"{containerFolderPath}/Groups/{oldGroupedNode.Key}/Dialogues", nodeToRemove);
                    }
                }
            }

            graphSaveData.OldGroupedNodeNames = new SerializableDictionary<string, List<string>>(currentGroupNodeNames);
        }

        private static void UpdateOldUngroupedNodes(List<string> currentUngroupedNodeNames, LgGraphSaveData graphSaveData)
        {
            if (graphSaveData.OldUngroupedNodeNames != null && graphSaveData.OldUngroupedNodeNames.Count != 0)
            {
                List<string> nodesToRemove =
                    graphSaveData.OldUngroupedNodeNames.Except(currentUngroupedNodeNames).ToList();

                foreach (string nodeToRemove in nodesToRemove)
                {
                    RemoveAsset($"{containerFolderPath}/Global/Dialogues", nodeToRemove);
                }
            }

            graphSaveData.OldUngroupedNodeNames = new List<string>(currentUngroupedNodeNames);
        }

        private static void RemoveAsset(string path, string assetName)
        {
            AssetDatabase.DeleteAsset($"{path}/{assetName}.asset");
        }

        private static void UpdateDialoguesChoicesConnections()
        {
            foreach (DialogNode node in nodes)
            {
                LGDialogSO dialog = createdDialogs[node.ID];

                for (int choiceIndex = 0; choiceIndex < node.Choices.Count; ++choiceIndex)
                {
                    LGChoiceSaveData nodeChoice = node.Choices[choiceIndex];
                    if(string.IsNullOrEmpty(nodeChoice.NodeID))
                        continue;
                    dialog.Choices[choiceIndex].NextDialogue = createdDialogs[nodeChoice.NodeID];
                    if (!string.IsNullOrEmpty(nodeChoice.NodeFailID))
                    {
                        dialog.Choices[choiceIndex].NextDialogueFail = createdDialogs[nodeChoice.NodeFailID];
                    }
                    SaveAsset(dialog);
                    
                }
            }
        }

        private static void SaveNodeToScriptableObject(DialogNode node, LGDialogContainerSO dialogContainer)
        {
            LGDialogSO dialog;
            if (node.Group != null)
            {
                dialog = CreateAsset<LGDialogSO>($"{containerFolderPath}/Groups/{node.Group.title}/Dialogues", node.DialogueName);
                dialogContainer.DialogueGroupes.AddItem(createdDialogGroups[node.Group.ID], dialog);
            }
            else
            {
                dialog = CreateAsset<LGDialogSO>($"{containerFolderPath}/Global/Dialogues", node.DialogueName);
                dialogContainer.UngroupedDialogs.Add(dialog);
            }
            dialog.Initialize(
                node.DialogueName, 
                node.DialogActor,
                node.Text,
                ConvertNodeChoicesToDialogueChoices(node.Choices),
                node.DialogType, 
                node.PortraitLeft,
                node.IsStartingNode()
                );
            createdDialogs.Add(node.ID, dialog);
            SaveAsset(dialog);
        }
        private static void SaveNodeToScriptableObject(EventNode node, LGDialogContainerSO dialogContainer)
        {
            LGEventDialogSO dialog;
            if (node.Group != null)
            {
                dialog = CreateAsset<LGEventDialogSO>($"{containerFolderPath}/Groups/{node.Group.title}/Dialogues", node.DialogueName);
                dialogContainer.DialogueGroupes.AddItem(createdDialogGroups[node.Group.ID], dialog);
            }
            else
            {
                dialog = CreateAsset<LGEventDialogSO>($"{containerFolderPath}/Global/Dialogues", node.DialogueName);
                dialogContainer.UngroupedDialogs.Add(dialog);
            }

           
       
            dialog.Initialize(
                node.DialogueName, 
                node.DialogActor,
                node.Text,
                ConvertNodeChoicesToDialogueChoices(node.Choices),
                node.DialogType, 
                node.PortraitLeft,
                node.IsStartingNode(),
                node.Headline,
                CopyResourceRewards(node.ResourceRewards), 
                new List<ItemBP>(node.ItemRewards), 
                new List<DialogEvent>(node.Events)
            );
            createdDialogs.Add(node.ID, dialog);
            SaveAsset(dialog);
        }
        private static void SaveNodeToScriptableObject(FightNode node, LGDialogContainerSO dialogContainer)
        {
            LGFightEventDialogSO dialog;
            if (node.Group != null)
            {
                dialog = CreateAsset<LGFightEventDialogSO>($"{containerFolderPath}/Groups/{node.Group.title}/Dialogues", node.DialogueName);
                dialogContainer.DialogueGroupes.AddItem(createdDialogGroups[node.Group.ID], dialog);
            }
            else
            {
                dialog = CreateAsset<LGFightEventDialogSO>($"{containerFolderPath}/Global/Dialogues", node.DialogueName);
                dialogContainer.UngroupedDialogs.Add(dialog);
            }

         
   
            dialog.Initialize(
                node.DialogueName, 
                node.DialogActor,
                node.Text,
                ConvertNodeChoicesToDialogueChoices(node.Choices),
                node.DialogType, 
                node.PortraitLeft,
                node.IsStartingNode(),
                node.Headline,
                CopyResourceRewards(node.ResourceRewards), 
                new List<ItemBP>(node.ItemRewards), 
                new List<DialogEvent>(node.Events),
                node.Enemy
            );
            createdDialogs.Add(node.ID, dialog);
            SaveAsset(dialog);
        }
        private static void SaveNodeToScriptableObject(BattleNode node, LGDialogContainerSO dialogContainer)
        {
            LGBattleEventDialogSO dialog;
            if (node.Group != null)
            {
                dialog = CreateAsset<LGBattleEventDialogSO>($"{containerFolderPath}/Groups/{node.Group.title}/Dialogues", node.DialogueName);
                dialogContainer.DialogueGroupes.AddItem(createdDialogGroups[node.Group.ID], dialog);
            }
            else
            {
                dialog = CreateAsset<LGBattleEventDialogSO>($"{containerFolderPath}/Global/Dialogues", node.DialogueName);
                dialogContainer.UngroupedDialogs.Add(dialog);
            }

            Debug.Log("Save To SO " + node.ItemRewards.Count);
            if(node.ItemRewards.Count>=1)
                Debug.Log("Item: " + node.ItemRewards[0].name);
            dialog.Initialize(
                node.DialogueName, 
                node.DialogActor,
                node.Text,
                ConvertNodeChoicesToDialogueChoices(node.Choices),
                node.DialogType, 
                node.PortraitLeft,
                node.IsStartingNode(),
                node.Headline,
                CopyResourceRewards(node.ResourceRewards), 
                new List<ItemBP>(node.ItemRewards), 
                new List<DialogEvent>(node.Events),
                node.EnemyArmy
            );
            createdDialogs.Add(node.ID, dialog);
            SaveAsset(dialog);
        }

        private static List<ResourceEntry> CopyResourceRewards(List<ResourceEntry> nodeResourceRewards)
        {
            List<ResourceEntry> ret = new List<ResourceEntry>();
            foreach (var res in nodeResourceRewards)
            {
                ret.Add(new ResourceEntry(res.Amount, res.ResourceType));
            }
            return ret;
        }

        private static List<LGDialogChoiceData> ConvertNodeChoicesToDialogueChoices(List<LGChoiceSaveData> nodeChoices)
        {
            List<LGDialogChoiceData> dialogChoices = new List<LGDialogChoiceData>();
            foreach (var nodeChoice in nodeChoices)
            {
                LGDialogChoiceData choiceData = new LGDialogChoiceData()
                {
                    Text = nodeChoice.Text,
                    ItemRequirements = new List<ItemBP>(nodeChoice.ItemRequirements),
                    ResourceRequirements = new List<ResourceEntry>(nodeChoice.ResourceRequirements),
                    CharacterRequirements = new List<UnitBP>(nodeChoice.CharacterRequirements),
                    AttributeRequirements = new List<ResponseStatRequirement>(nodeChoice.AttributeRequirements),
                    BlessingRequirements = new List<BlessingBP>(nodeChoice.BlessingRequirements)
                  
                };
                dialogChoices.Add(choiceData);
            }

            return dialogChoices;
        }

        private static List<ResourceEntry> CloneNodeResources(List<ResourceEntry> entries)
        {
            List<ResourceEntry> choices =new List<ResourceEntry>();
            foreach (ResourceEntry choice in entries)
            {
                ResourceEntry choiceSaveData = new ResourceEntry(choice.Amount, choice.ResourceType);
                choices.Add(choiceSaveData);
            }

            return choices;
        }
        private static List<LGChoiceSaveData> CloneNodeChoices(List<LGChoiceSaveData> nodeChoices)
        {
            List<LGChoiceSaveData> choices =new List<LGChoiceSaveData>();
            foreach (LGChoiceSaveData choice in nodeChoices)
            {
                LGChoiceSaveData choiceSaveData = new LGChoiceSaveData()
                {
                    Text = choice.Text,
                    NodeID = choice.NodeID,
                    NodeFailID = choice.NodeFailID,
                    ItemRequirements = new List<ItemBP>(choice.ItemRequirements),
                    ResourceRequirements = new List<ResourceEntry>(choice.ResourceRequirements),
                    CharacterRequirements= new List<UnitBP>(choice.CharacterRequirements),
                    AttributeRequirements = new List<ResponseStatRequirement>(choice.AttributeRequirements)
                    
                };
                choices.Add(choiceSaveData);
            }

            return choices;
        }
        private static void SaveNodeToGraph(DialogNode node, LgGraphSaveData graphSaveData)
        {
            List<LGChoiceSaveData> choices = CloneNodeChoices(node.Choices);
            LGNodeSaveData nodeData = new LGNodeSaveData()
            {
                ID = node.ID,
                Name = node.DialogueName,
                Choices = choices,
                Text = node.Text,
                IsPortraitLeft = node.PortraitLeft,
                DialogActor = node.DialogActor,
                GroupID = node.Group?.ID,
                DialgueType = node.DialogType,
                Position = node.GetPosition().position
            };
            graphSaveData.Nodes.Add(nodeData);
        }
        private static void SaveNodeToGraph(EventNode node, LgGraphSaveData graphSaveData)
        {
            List<LGChoiceSaveData> choices = CloneNodeChoices(node.Choices);
            LGEventNodeSaveData nodeData = new LGEventNodeSaveData()
            {
                ID = node.ID,
                Name = node.DialogueName,
                Choices = choices,
                Text = node.Text,
                IsPortraitLeft = node.PortraitLeft,
                DialogActor = node.DialogActor,
                GroupID = node.Group?.ID,
                DialgueType = node.DialogType,
                Position = node.GetPosition().position,
                Headline = node.Headline,
                RewardResources = node.ResourceRewards,
                RewardItems = node.ItemRewards,
                Events = node.Events,
            };
            graphSaveData.EventNodes.Add(nodeData);
        }
        private static void SaveNodeToGraph(FightNode node, LgGraphSaveData graphSaveData)
        {
            List<LGChoiceSaveData> choices = CloneNodeChoices(node.Choices);
            LGFightNodeSaveData nodeData = new LGFightNodeSaveData()
            {
                ID = node.ID,
                Name = node.DialogueName,
                Choices = choices,
                Text = node.Text,
                IsPortraitLeft = node.PortraitLeft,
                DialogActor = node.DialogActor,
                GroupID = node.Group?.ID,
                DialgueType = node.DialogType,
                Position = node.GetPosition().position,
                Headline = node.Headline,
                RewardResources = node.ResourceRewards,
                RewardItems = node.ItemRewards,
                Events = node.Events,
                Enemy = node.Enemy
            };
            graphSaveData.FightNodes.Add(nodeData);
        }
        private static void SaveNodeToGraph(BattleNode node, LgGraphSaveData graphSaveData)
        {
            List<LGChoiceSaveData> choices = CloneNodeChoices(node.Choices);
            LGBattleNodeSaveData nodeData = new LGBattleNodeSaveData()
            {
                ID = node.ID,
                Name = node.DialogueName,
                Choices = choices,
                Text = node.Text,
                IsPortraitLeft = node.PortraitLeft,
                DialogActor = node.DialogActor,
                GroupID = node.Group?.ID,
                DialgueType = node.DialogType,
                Position = node.GetPosition().position,
                Headline = node.Headline,
                RewardResources = node.ResourceRewards,
                RewardItems = node.ItemRewards,
                Events = node.Events,
                EnemyArmy = node.EnemyArmy
            };
            graphSaveData.BattleNodes.Add(nodeData);
        }



        private static void SaveGroups(LgGraphSaveData graphSaveData, LGDialogContainerSO dialogContainer)
        {
            List<string> groupNames = new List<string>();
            foreach (DialogGroup group in groups)
            {
                SaveGroupToGraph(group, graphSaveData);
                SaveGroupToScriptableObject(group, dialogContainer);
                groupNames.Add(group.title);
            }

            UpdateOldGroups(groupNames,graphSaveData);
        }

        private static void UpdateOldGroups(List<string> currentGroupNames,LgGraphSaveData graphSaveData)
        {
            if (graphSaveData.OldGroupNames != null && graphSaveData.OldGroupNames.Count != 0)
            {
                List<string> groupsToRemove = graphSaveData.OldGroupNames.Except(currentGroupNames).ToList();

                foreach (string groupToRemove in groupsToRemove)
                {
                    RemoveFolder($"{containerFolderPath}/Groups/{groupToRemove}");
                }
            }

            graphSaveData.OldGroupNames = new List<string>(currentGroupNames);
        }

        private static void RemoveFolder(string fullPath)
        {
            FileUtil.DeleteFileOrDirectory($"{fullPath}.meta");
            FileUtil.DeleteFileOrDirectory($"{fullPath}/");
           
        }

        private static void SaveGroupToScriptableObject(DialogGroup group, LGDialogContainerSO dialogContainer)
        {
            string groupName = group.title;
            CreateFolder($"{containerFolderPath}/Groups", groupName);
            CreateFolder($"{containerFolderPath}/Groups/{groupName}", "Dialogues");

            LGDialogGroupSO dialogGroup =
                CreateAsset<LGDialogGroupSO>($"{containerFolderPath}/Groups/{groupName}", groupName);
            dialogGroup.Initialize(groupName);
            createdDialogGroups.Add(group.ID, dialogGroup);
            dialogContainer.DialogueGroupes.Add(dialogGroup, new List<LGDialogSO>());

            SaveAsset(dialogGroup);
        }

        public static void SaveAsset(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void SaveGroupToGraph(DialogGroup group, LgGraphSaveData graphSaveData)
        {
            LGGroupSaveData groupdata = new LGGroupSaveData()
            {
                ID = group.ID,
                Name = group.title,
                Position = group.GetPosition().position
            };
            graphSaveData.Groups.Add(groupdata);
        }

        public static T CreateAsset<T>(string path, string assetName) where T:ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";
            T asset = LoadAsset<T>(path, assetName);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, fullPath);
            }

            return asset;
        }

        public static T LoadAsset<T>(string path, string assetName) where T:ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";
            return AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }

        private static void GetElementsFromGraphView()
        {
            Type groupType = typeof(DialogGroup);
            graphView.graphElements.ForEach(graphElement =>
            {
                if (graphElement is DialogNode node)
                {
                    nodes.Add(node);
                    return;
                }

                if (graphElement.GetType() == groupType)
                {
                    DialogGroup group = (DialogGroup)graphElement;
                    groups.Add(group);
                    return;
                }
            });
        }

        private static void CreateStaticFolders()
        {
            CreateFolder("Assets/02_Scripts/EditorScripts/DialogueSystem","Graphs");
            CreateFolder("Assets/07_GameData","DialogueSystem");
            CreateFolder("Assets/07_GameData/DialogueSystem","Dialogues");
            CreateFolder("Assets/07_GameData/DialogueSystem/Dialogues", graphFileName);
            CreateFolder(containerFolderPath, "Global");
            CreateFolder(containerFolderPath, "Groups");
            CreateFolder($"{containerFolderPath}/Global", "Dialogues");
        }

        private static void CreateFolder(string path, string folderName)
        {
            if (AssetDatabase.IsValidFolder($"{path}/{folderName}"))
            {
                return;
            }

            AssetDatabase.CreateFolder(path,folderName);
        }
    }
}