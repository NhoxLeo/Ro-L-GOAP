/* * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * *  
 * *  File: WSVKDatabaseEditor.cs
 * *  Type: Visual C# Source File
 * *  
 * *  Author: Rohan Jadav
 * *  
 * *  Description: Custom editor for WorldStateVariableKeyList.
 * *  
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * */

using Hiralal.AdvancedPatterns.SOList;
using Hiralal.Boilerplate.Extensions;
using UnityEditor;
using UnityEngine;

namespace Hiralal.LGOAP.EditorAssembly
{
    [CustomEditor(typeof(WorldStateVariableKeyList))]
    public class WSVKDatabaseEditor : ScriptableObjectListInspector<WorldStateVariableKey>
    {
        protected override void DrawPropertyAfterName(Rect rect, int index, bool isActive, bool isFocused)
        {
            var currentObject = serializedList.GetArrayElementAtIndex(index).objectReferenceValue as WorldStateVariableKey;

            var currentSerializedProperty = serializedList.GetArrayElementAtIndex(index);

            GUI.enabled = false;
            EditorGUI.PropertyField(rect.KeepToLeftFor(100), currentSerializedProperty, GUIContent.none);
            
            if (currentObject != null)
                EditorGUI.IntField(rect.ShiftToRightBy(100), GUIContent.none, currentObject.InstanceID);
            
            GUI.enabled = true;
        }
    }
}