using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CartoonCoffee
{
    public class ParticleShaderGUI : ShaderGUI
    {
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.richText = true;

            EditorGUILayout.BeginVertical("Helpbox");
            EditorGUILayout.LabelField("<b><size=14>Textures</size></b>", style);

            bool vertical = true;
            bool shaderEnabled = true;

            for(int n = 0; n < properties.Length; n++)
            {
                MaterialProperty prop = properties[n];

                string displayName = prop.displayName;

                if (displayName == "Tint Color" || displayName == "Soft Particles Factor" || prop.name == "_texcoord") continue;
                if (displayName.EndsWith("Space"))
                {
                    prop.floatValue = 0;
                    continue;
                }

                string[] split = prop.displayName.Split(':');
                if(split.Length == 2)
                {
                    displayName = split[1].Substring(1);
                }

                if (displayName.StartsWith("Enable"))
                {
                    if (vertical)
                    {
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.Space();
                    }

                    vertical = true;
                    EditorGUILayout.BeginVertical("Helpbox");
                    EditorGUILayout.LabelField("<b><size=14>" + displayName.Replace("Enable ","") + "</size></b>", style);

                    shaderEnabled = prop.floatValue > 0.5f;
                }
                else
                {
                    if(shaderEnabled == false)
                    {
                        continue;
                    }
                }

                if (displayName.EndsWith("Texture") || displayName.EndsWith("Mask"))
                {
                    prop.textureValue = (Texture)EditorGUILayout.ObjectField(displayName, prop.textureValue, typeof(Texture), false);
                }
                else
                {
                    materialEditor.ShaderProperty(prop, displayName);
                }
            }

            if (vertical)
            {
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
                vertical = false;
            }
        }
    }
}