using UnityEditor;
using UnityEngine.UIElements;

namespace __2___Scripts.External.Editor.Utility
{
    public static class StyleUtility
    {
        public static VisualElement AddClasses(this VisualElement element, params string[] classNames)
        {
            foreach (string className in classNames)
            {
               element.AddToClassList(className);
            }

            return element;

        }
        public static VisualElement AddStyleSheets(this VisualElement element, params string[] styleSheetNames)
        {
            foreach (string styleSheetName in styleSheetNames)
            {
                StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load(styleSheetName);
                element.styleSheets.Add(styleSheet);
            }

            return element;
        }
    }
}