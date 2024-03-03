using System;
using System.Collections.Generic;
using System.Linq;
using _02_Scripts.EditorScripts.Utility;
using _02_Scripts.Game.Dialog.DialogSystem;
using Game.Dialog.DialogSystem;
using UnityEditor;

namespace __2___Scripts.External.Editor.Inspectors
{
    [CustomEditor(typeof(LGDialog))]
    public class DialogInspector :UnityEditor.Editor
    {
        private SerializedProperty dialogContainerProperty;
        private SerializedProperty dialogGroupProperty;
        private SerializedProperty dialogProperty;
        
        private SerializedProperty groupedDialogsProperty;
        private SerializedProperty startingDialogsOnlyProperty;
        private SerializedProperty selectedDialogGroupProperty;
        private SerializedProperty selectedDialogIndexProperty;

        private void OnEnable()
        {
            dialogContainerProperty = serializedObject.FindProperty("dialogContainer");
            dialogGroupProperty = serializedObject.FindProperty("dialogGroup");
            dialogProperty = serializedObject.FindProperty("dialog");
            groupedDialogsProperty = serializedObject.FindProperty("groupedDialogs");
            startingDialogsOnlyProperty = serializedObject.FindProperty("startingDialogsOnly");
            
            selectedDialogGroupProperty = serializedObject.FindProperty("selectedDialogGroupIndex");
            selectedDialogIndexProperty = serializedObject.FindProperty("selectedDialogIndex");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDialogContainerArea();
            LGDialogContainerSO dialogContainer = (LGDialogContainerSO)dialogContainerProperty.objectReferenceValue;
            if (dialogContainer == null)
            {
                StopDrawing("Select a Dialog Container to see the rest of the Inspector.");
                return;
            }
            DrawFiltersArea();
            bool currentStartingDialogesOnlyFilter = startingDialogsOnlyProperty.boolValue;

            List<string> dialogNames;

            string dialogFolderPath = $"Assets/07_GameData/DialogueSystem/Dialogues/{dialogContainer.FileName}";
            string dialogInfoMessage;
            if (groupedDialogsProperty.boolValue)
            {
                List<string> groupNames = dialogContainer.GetDialogGroupNames();
                if (groupNames.Count == 0)
                {
                    StopDrawing("There are no"+(currentStartingDialogesOnlyFilter ? " Starting":"")+" Dialog Groups in this Dialog Container.");
                    return;
                }
                
                DrawDialogGroupArea(dialogContainer, groupNames);
                LGDialogGroupSO dialogGroup = (LGDialogGroupSO)dialogGroupProperty.objectReferenceValue;
                dialogNames = dialogContainer.GetGroupedDialogNames(dialogGroup,currentStartingDialogesOnlyFilter);
                dialogFolderPath += $"/Groups/{dialogGroup.GroupName}/Dialogues";
                dialogInfoMessage = "There are no "+(currentStartingDialogesOnlyFilter ? " Starting":"")+ " in this Dialogue Group.";
            }
            else
            {
                dialogNames = dialogContainer.GetUngroupedDialogNames(currentStartingDialogesOnlyFilter);
                dialogFolderPath += "Global/Dialogues";
                dialogInfoMessage = "There are no "+(currentStartingDialogesOnlyFilter ? " Starting":"")+" ungrouped Dialogs in this Dialogue Container.";
            }

            if (dialogNames.Count == 0)
            {
                StopDrawing(dialogInfoMessage);
                return;
            }
            
            DrawDialogArea(dialogNames, dialogFolderPath);
            serializedObject.ApplyModifiedProperties();
        }

      

        private void StopDrawing(string reason, MessageType messageType = MessageType.Info)
        {
            InspectorUtility.DrawHelpBox(reason,messageType);
            InspectorUtility.DrawSpace();
            InspectorUtility.DrawHelpBox("You need to select a Dialogue for this component to work properly at Runtime!", MessageType.Warning);
            serializedObject.ApplyModifiedProperties();
        }
        private void DrawDialogArea(List<string> dialogNames, string folderPath)
        {
            InspectorUtility.DrawHeader("Dialog");

            int oldSelectedDialogIndex = selectedDialogIndexProperty.intValue;
            LGDialogSO oldDialog = (LGDialogSO)dialogProperty.objectReferenceValue; 
            
            bool isOldDialogNull = oldDialog == null;
            string oldDialogName = isOldDialogNull ? "" : oldDialog.DialogName;
            
            UpdateIndexOnNamesList(dialogNames, selectedDialogIndexProperty, oldSelectedDialogIndex, oldDialogName, isOldDialogNull);
            
            selectedDialogIndexProperty.intValue= InspectorUtility.DrawPopup("Dialog", selectedDialogIndexProperty, dialogNames.ToArray());
            string selectedDialogName = dialogNames[selectedDialogIndexProperty.intValue];
            LGDialogSO selectedDialog = IOUtility.LoadAsset<LGDialogSO>(folderPath, selectedDialogName);
            dialogProperty.objectReferenceValue = selectedDialog;
            InspectorUtility.DrawDisabledField(()=> dialogProperty.DrawPropertyField());
           
            
        }

        private void DrawDialogGroupArea(LGDialogContainerSO dialogContainer, List<string> groupNames)
        {
            InspectorUtility.DrawHeader("Dialog Group");

            int oldSelectedDialogGroupIndex = selectedDialogGroupProperty.intValue;
            LGDialogGroupSO oldDialogGroup = (LGDialogGroupSO)dialogGroupProperty.objectReferenceValue;

            bool isOldDialogGroupNull = oldDialogGroup == null;
            string oldDialogGroupName = isOldDialogGroupNull ? "" : oldDialogGroup.GroupName;
            UpdateIndexOnNamesList(groupNames, selectedDialogGroupProperty, oldSelectedDialogGroupIndex, oldDialogGroupName,isOldDialogGroupNull);
            selectedDialogGroupProperty.intValue = InspectorUtility.DrawPopup("Dialog Group",selectedDialogGroupProperty.intValue, groupNames.ToArray());
            string selectedGroupName = groupNames[selectedDialogGroupProperty.intValue];
            LGDialogGroupSO selectedGroup = IOUtility.LoadAsset<LGDialogGroupSO>($"Assets/07_GameData/DialogueSystem/Dialogues/{dialogContainer.FileName}/Groups/{selectedGroupName}", selectedGroupName);
            dialogGroupProperty.objectReferenceValue = selectedGroup;
            
            InspectorUtility.DrawDisabledField(()=>dialogGroupProperty.DrawPropertyField());
            InspectorUtility.DrawSpace();

        }

        private void UpdateIndexOnNamesList(List<string> optionNames, SerializedProperty indexProperty,int oldSelectedPropertyIndex, string oldPropertyName, bool isOldPropertyNull)
        {
            if (isOldPropertyNull)
            {
                indexProperty.intValue = 0;
                return;
            }

            bool oldIndexIsOutOfBoundsOfNamesListCount = oldSelectedPropertyIndex > optionNames.Count - 1;
            bool oldNameIsDifferentThanSelectedName = oldIndexIsOutOfBoundsOfNamesListCount||oldPropertyName != optionNames[oldSelectedPropertyIndex];
            
            if (oldNameIsDifferentThanSelectedName)
            {
                if (optionNames.Contains(oldPropertyName))
                {
                    indexProperty.intValue = optionNames.IndexOf(oldPropertyName);
                }
                else
                {
                    indexProperty.intValue = 0;
                }
            }
            
        }

        private void DrawFiltersArea()
        {
            InspectorUtility.DrawHeader("Filters");

            groupedDialogsProperty.DrawPropertyField();
            startingDialogsOnlyProperty.DrawPropertyField();
            InspectorUtility.DrawSpace();

        }

        private void DrawDialogContainerArea()
        {
            InspectorUtility.DrawHeader("Dialog Container");
            dialogContainerProperty.DrawPropertyField();
            InspectorUtility.DrawSpace();
            
        }
    }
}