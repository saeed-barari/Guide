using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Baldr.Editor
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class BaldrMonoBehaviourEditor : UnityEditor.Editor
    {
        private static Dictionary<FieldInfo?, bool> isExpanded = new Dictionary<FieldInfo?, bool>();
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            foreach (var fieldInfo in target.GetType().GetFields(BindingFlags.Instance| BindingFlags.Public | BindingFlags.NonPublic))
            {
                //
            }
            
            foreach (var methodInfo in target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var oldCol = GUI.color;
                DrawButtonIfPossible(methodInfo);
                GUI.color = oldCol;
            }
            
        }

       

        private void DrawButtonIfPossible(MethodInfo methodInfo)
        {
            var attrs = methodInfo.GetCustomAttributes(typeof(BaldrAttributes.ButtonAttribute), true);
            if (attrs.Length <= 0) return;
            
            // if has arguments, don't draw button
            if (methodInfo.GetGenericArguments().Length > 0) return;

            var attr = attrs[0] as BaldrAttributes.ButtonAttribute;
            
            // main draw
            GUI.color = attr.color;
            if (GUILayout.Button(ObjectNames.NicifyVariableName(methodInfo.Name), GUILayout.Height(attr.height)))
            {
                methodInfo.Invoke(target, null);
            }
        }
    }
}