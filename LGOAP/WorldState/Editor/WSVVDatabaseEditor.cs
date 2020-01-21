/* * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * *  
 * *  File: WSVVDatabaseEditor.cs
 * *  Type: Visual C# Source File
 * *  
 * *  Author: Rohan Jadav
 * *  
 * *  Description: Custom editor for WorldStateVariableValueList.
 * *  
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * */

using Hiralal.AdvancedPatterns.SOList;
using Hiralal.Boilerplate.Extensions;
using UnityEditor;
using UnityEngine;

namespace Hiralal.LGOAP.EditorAssembly
{
    [CustomEditor(typeof(WorldStateVariableValueList))]
    public class WSVVDatabaseEditor : ScriptableObjectListInspector<WorldStateVariableValue>
    {
        protected override void DrawPropertyAfterName(Rect rect, int index, bool isActive, bool isFocused)
        {
            var currentSerializedObject = new SerializedObject(serializedList.GetArrayElementAtIndex(index).objectReferenceValue);

            var currentSerializedProperty = currentSerializedObject.FindProperty("index");

            currentSerializedProperty.intValue = index + 1;
            currentSerializedObject.ApplyModifiedProperties();

            GUI.enabled = false;
            EditorGUI.PropertyField(rect.KeepToLeftFor(30), currentSerializedProperty, GUIContent.none);
            EditorGUI.PropertyField(rect.ShiftToRightBy(30), serializedList.GetArrayElementAtIndex(index),
                GUIContent.none);
            GUI.enabled = true;
        }
    }
}