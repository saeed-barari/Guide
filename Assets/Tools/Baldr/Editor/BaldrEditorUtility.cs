using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Baldr.Editor
{
    public static class BaldrEditorUtility
    {
        public static FieldInfo[] GetAllFields(this Type type)
        {
            return type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        public static object DrawUnknownType(FieldInfo fieldInfo, object obj)
        {
            try
            {
                EditorGUI.indentLevel++;
                foreach (var field in fieldInfo.GetType().GetAllFields())
                {
                    DrawUnknownType(field, fieldInfo.GetValue(obj) as Object);
                }
                EditorGUI.indentLevel--;
                
                if (fieldInfo.FieldType == typeof(int))
                {
                    return EditorGUILayout.IntField(ObjectNames.NicifyVariableName(fieldInfo.Name),
                        (int) fieldInfo.GetValue(obj));
                }

                if (fieldInfo.FieldType == typeof(float))
                {
                    return EditorGUILayout.FloatField(ObjectNames.NicifyVariableName(fieldInfo.Name),
                        (int) fieldInfo.GetValue(obj));
                }

                if (fieldInfo.FieldType == typeof(bool))
                {
                    return EditorGUILayout.Toggle(ObjectNames.NicifyVariableName(fieldInfo.Name),
                        (bool) fieldInfo.GetValue(obj));
                }

                if (fieldInfo.FieldType == typeof(string))
                {
                    return EditorGUILayout.TextField(ObjectNames.NicifyVariableName(fieldInfo.Name),
                        (string) fieldInfo.GetValue(obj));
                }

                if (fieldInfo.FieldType == typeof(Vector3))
                {
                    return EditorGUILayout.Vector3Field(ObjectNames.NicifyVariableName(fieldInfo.Name),
                        (Vector3) fieldInfo.GetValue(obj));
                }

                if (fieldInfo.FieldType == typeof(Vector2))
                {
                    return EditorGUILayout.Vector2Field(ObjectNames.NicifyVariableName(fieldInfo.Name),
                        (Vector2) fieldInfo.GetValue(obj));
                }

                if (fieldInfo.FieldType == typeof(Vector4))
                {
                    return EditorGUILayout.Vector4Field(ObjectNames.NicifyVariableName(fieldInfo.Name),
                        (Vector4) fieldInfo.GetValue(obj));
                }

                if (fieldInfo.FieldType == typeof(Color))
                {
                    return EditorGUILayout.ColorField(ObjectNames.NicifyVariableName(fieldInfo.Name),
                        (Color) fieldInfo.GetValue(obj));
                }

                if (fieldInfo.FieldType == typeof(Rect))
                {
                    return EditorGUILayout.RectField(ObjectNames.NicifyVariableName(fieldInfo.Name),
                        (Rect) fieldInfo.GetValue(obj));
                }

                if (fieldInfo.FieldType == typeof(AnimationCurve))
                {
                    return EditorGUILayout.CurveField(ObjectNames.NicifyVariableName(fieldInfo.Name),
                        (AnimationCurve) fieldInfo.GetValue(obj));
                }

                if (fieldInfo.FieldType == typeof(Bounds))
                {
                    return EditorGUILayout.BoundsField(ObjectNames.NicifyVariableName(fieldInfo.Name),
                        (Bounds) fieldInfo.GetValue(obj));
                }

                if (fieldInfo.FieldType == typeof(LayerMask))
                {
                    return EditorGUILayout.LayerField(ObjectNames.NicifyVariableName(fieldInfo.Name),
                        (LayerMask) fieldInfo.GetValue(obj));
                }

                if (fieldInfo.FieldType == typeof(Object))
                {
                    return EditorGUILayout.ObjectField(ObjectNames.NicifyVariableName(fieldInfo.Name),
                        (Object) fieldInfo.GetValue(obj),
                        fieldInfo.FieldType, true);
                }

                if (fieldInfo.FieldType.IsArray)
                {
                    foreach (var ob in (IEnumerable) fieldInfo.GetValue(obj))
                    {
                        
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            EditorGUILayout.HelpBox($"Type {fieldInfo.FieldType} of field {fieldInfo.Name} is not supported", MessageType.Error);
            return null;
        }
    }
}