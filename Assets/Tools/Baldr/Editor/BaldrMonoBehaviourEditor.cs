#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BaldrAttributes;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

#endregion

namespace Baldr.Editor
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class BaldrMonoBehaviourEditor : UnityEditor.Editor
    {
        private static int _showDefault = 1;

        private static object _drawingtarget;

        private static string _lastHeaderGroupName = string.Empty;


        private static readonly Dictionary<string, AnimBool> _headerGroupsExpanded = new();
        private static bool _insideHeaderGroup;
        
        private Dictionary<string, List<ToDraw>> _toDrawList = new();

        public override void OnInspectorGUI()
        {
            _showDefault = GUILayout.Toolbar(_showDefault, new[] {"Default", "Baldr"});

            if (_showDefault == 0)
            {
                base.OnInspectorGUI();
                return;
            }

            serializedObject.Update();


            var iter = serializedObject.GetIterator();
            iter.NextVisible(true); // get inside object
            iter.NextVisible(false); // pass through the disabled script reference
            DrawDisabledScriptField(); // render the disabled reference

            try
            {
                DrawBaldrMonoBehaviourInspector(iter);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message + "\n" + e.StackTrace);
                if (_insideHeaderGroup)
                    EditorGUILayout.EndVertical();
                EditorGUILayout.HelpBox(e.Message, MessageType.Error);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawBaldrMonoBehaviourInspector(SerializedProperty serializedProperty)
        {
            _insideHeaderGroup = false;
            _drawingtarget = target;

            _toDrawList = new Dictionary<string, List<ToDraw>>();


            FillDrawListWithProps(serializedProperty);
            FillDrawListWithMethods();


            foreach (var label in _toDrawList.Keys)
            {
                // properties without group name will be drawn in the traditional way
                if (label == string.Empty)
                {
                    foreach (var toDraw in _toDrawList[label])
                        if (toDraw.property is not null)
                            EditorGUILayout.PropertyField(toDraw.property, true);
                    continue;
                }

                BeginHeaderGroup(label);
                GUILayout.BeginVertical(BaldrEditorStyles.InnerBox);
                if (EditorGUILayout.BeginFadeGroup(_headerGroupsExpanded[label].faded))
                {
                    foreach (var toDraw in _toDrawList[label])
                        if (toDraw.property is not null)
                            EditorGUILayout.PropertyField(toDraw.property, true);
                        else if (toDraw.methodButton is not null)
                            DrawButtonIfPossible(toDraw.methodButton);
                }
                EditorGUILayout.EndFadeGroup();
                GUILayout.EndVertical();

                EndHeaderGroup();
            }

            _drawingtarget = null;
        }

        private void FillDrawListWithProps(SerializedProperty serializedProperty)
        {
            string lastGroup = string.Empty;
            
            foreach (var prop in serializedProperty.GetChildren())
            {
                var field = _drawingtarget.GetType().GetField(prop.name,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (field == null) continue;

                var groupAttr =
                    field.GetCustomAttributes(typeof(GroupAttribute), true).FirstOrDefault() as GroupAttribute;

                if (groupAttr is not null)
                {
                    lastGroup = groupAttr.label;
                }

                if (_toDrawList.ContainsKey(lastGroup) is false)
                    _toDrawList.Add(lastGroup, new List<ToDraw>());

                _toDrawList[lastGroup].Add(new ToDraw
                {
                    targetObject = _drawingtarget, methodButton = null, property = prop
                });
            }
        }

        private void FillDrawListWithMethods()
        {
            foreach (var methodInfo in target.GetType()
                         .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var btnAttr =
                    methodInfo.GetCustomAttributes(typeof(ButtonAttribute), true).FirstOrDefault() as ButtonAttribute;
                if (btnAttr is null) continue;

                if (_toDrawList.ContainsKey(btnAttr.groupName) is false)
                    _toDrawList.Add(btnAttr.groupName, new List<ToDraw>());
                _toDrawList[btnAttr.groupName].Add(new ToDraw
                {
                    targetObject = _drawingtarget, methodButton = new ButtonMethod(methodInfo), property = null
                });
            }
        }

        private static bool CanDrawProperty()
        {
            return _lastHeaderGroupName == string.Empty || !_headerGroupsExpanded.ContainsKey(_lastHeaderGroupName) ||
                   _headerGroupsExpanded[_lastHeaderGroupName].target;
        }


        private void BeginHeaderGroup(string label)
        {
            _lastHeaderGroupName = label;
            GUILayout.BeginVertical(EditorStyles.helpBox);

            if (!_headerGroupsExpanded.ContainsKey(label))
            {
                var animBool = new AnimBool(true);
                animBool.valueChanged.AddListener(Repaint);
                _headerGroupsExpanded.Add(label, animBool);
            }
            
            if (GUILayout.Button(label, BaldrEditorStyles.HeaderButton))
            {
                _headerGroupsExpanded[label].target = !_headerGroupsExpanded[label].target;
            }

            GUILayout.Space(2);
            _insideHeaderGroup = true;
        }

        private static void EndHeaderGroup()
        {
            GUILayout.Space(2);

            GUILayout.EndVertical();
            _lastHeaderGroupName = string.Empty;
        }

        private void DrawDisabledScriptField()
        {
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            }
        }


        private static void DrawButtonIfPossible(ButtonMethod buttonMethod)
        {
            // if has arguments, don't draw button
            if (buttonMethod.attr is null || buttonMethod.methodInfo is null) return;

            // main draw
            GUI.color = buttonMethod.attr.color;
            if (GUILayout.Button(ObjectNames.NicifyVariableName(buttonMethod.methodInfo.Name), GUILayout.Height(buttonMethod.attr.height)))
                buttonMethod.methodInfo.Invoke(_drawingtarget, null);
        }
    }

    internal class ToDraw
    {
        public ButtonMethod methodButton;
        public SerializedProperty property;
        public object targetObject;
    }

    internal class ButtonMethod
    {
        public readonly ButtonAttribute attr;
        public readonly MethodInfo methodInfo;

        public ButtonMethod(MethodInfo methodInfo)
        {
            this.methodInfo = methodInfo;
            attr =
                methodInfo.GetCustomAttributes(true).FirstOrDefault(attribute => attribute is ButtonAttribute) as
                    ButtonAttribute;
        }
    }
}