/* * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * *  
 * *  File: WorldStateTransitionEditor.cs
 * *  Type: Visual C# Source File
 * *  
 * *  Author: Rohan Jadav
 * *  
 * *  Description: Custom Editor for WorldStateTransitions.
 * *  
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * */

using System.Diagnostics.CodeAnalysis;
using Hiralal.Boilerplate.Extensions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Hiralal.LGOAP.EditorAssembly
{
    [CustomEditor(typeof(WorldStateTransition))]
    [SuppressMessage("ReSharper", "DelegateSubtraction")]
    public class WorldStateTransitionEditor : Editor
    {
        // Cached constants for property names
        public const string CONDITIONS_PROPERTY_NAME = nameof(WorldStateTransition.conditions);
        public const string EFFECTS_PROPERTY_NAME = nameof(WorldStateTransition.effects);
        public const string WSV_KEY_PROPERTY_NAME = nameof(WorldStateVariable.key);
        public const string WSV_VALUE_PROPERTY_NAME = nameof(WorldStateVariable.value);
        public const string WSV_ENTITY_INDEX_PROPERTY_NAME = nameof(WorldStateVariable.entityIndex);

        // Popup options
        public static readonly string[] IndexPopup =
            { "0", "1", "2", "3", "4", "5" };
        public static readonly string[] IndexPopupNames = 
            {"AI Agent", "Global", "Smart Object", "Helper Entity 1", "Helper Entity 2", "Helper Entity 3"};

        // Building both lists
        private ReorderableList conditionsReorderableList;
        private SerializedProperty conditionsSerializedList;

        private ReorderableList effectsReorderableList;
        private SerializedProperty effectsSerializedList;

        private void OnEnable()
        {
            // Cache serialized properties
            conditionsSerializedList = serializedObject.FindProperty(CONDITIONS_PROPERTY_NAME);
            effectsSerializedList = serializedObject.FindProperty(EFFECTS_PROPERTY_NAME);

            // Local method for a reorderable list
            ReorderableList CreateList(SerializedProperty property) =>
                new ReorderableList(serializedObject, property, true, true, true, true)
                    {elementHeight = 42};
            // Create reorderable lists
            conditionsReorderableList = CreateList(conditionsSerializedList);
            effectsReorderableList = CreateList(effectsSerializedList);

            // Subscribe to callbacks
            CallbackSubscribe(conditionsReorderableList);
            CallbackSubscribe(effectsReorderableList);
        }

        private void OnDisable()
        {
            // Unsubscribe to callbacks
            CallbackUnsubscribe(conditionsReorderableList);
            CallbackUnsubscribe(effectsReorderableList);
        }

        private void CallbackSubscribe(ReorderableList list)
        {
            // Drawing header
            list.drawHeaderCallback += rect =>
                DrawHeader(rect, list == conditionsReorderableList);
            
            // Drawing a single element
            list.drawElementCallback += (rect, index, active, focused) =>
                DrawElement(rect, index, active, focused, list == conditionsReorderableList);
            
            // Upon adding an element
            list.onAddCallback += reorderableList =>
                AddToList(reorderableList, list == conditionsReorderableList);
            
            // Upon removing an element
            list.onRemoveCallback += reorderableList =>
                RemoveFromList(reorderableList, list == conditionsReorderableList);
        }

        private void CallbackUnsubscribe(ReorderableList list)
        {
            // Drawing header
            list.drawHeaderCallback -= rect => DrawHeader(rect, list == conditionsReorderableList);
            
            // Drawing a single element
            list.drawElementCallback -= (rect, index, active, focused) =>
                DrawElement(rect, index, active, focused, list == conditionsReorderableList);
            
            // Upon adding an element
            list.onAddCallback -= reorderableList => AddToList(reorderableList, list == conditionsReorderableList);
            
            // Upon removing an element
            list.onRemoveCallback -= reorderableList =>
                RemoveFromList(reorderableList, list == conditionsReorderableList);
        }

        public override void OnInspectorGUI()
        {
            // Conditions list
            conditionsReorderableList.DoLayoutList();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            // Effects list
            effectsReorderableList.DoLayoutList();
        }

        // id = true for conditions, id = false for effects
        private void DrawHeader(Rect rect, bool id) =>
            EditorGUI.LabelField(rect, id ? "Conditions" : "Effects", EditorStyles.boldLabel);

        // id = true for conditions, id = false for effects
        private void AddToList(ReorderableList list, bool id)
        {
            // Add to the serialized list and apply the properties
            
            var x = id ? conditionsSerializedList : effectsSerializedList;
            x.InsertArrayElementAtIndex(x.arraySize);
            serializedObject.ApplyModifiedProperties();
        }

        // id = true for conditions, id = false for effects
        private void RemoveFromList(ReorderableList list, bool id)
        {
            // Remove from the serialized list and apply the properties
            
            var x = id ? conditionsSerializedList : effectsSerializedList;
            x.DeleteArrayElementAtIndex(list.index);
            serializedObject.ApplyModifiedProperties();
        }

        // id = true for conditions, id = false for effects
        protected virtual void DrawElement(Rect rect, int index, bool isActive, bool isFocused, bool id)
        {
            // WorldState class
            var serializedWorldState = (id ? conditionsSerializedList : effectsSerializedList)
                .GetArrayElementAtIndex(index);
            
            // EntityIndex property
            var serializedIndex = serializedWorldState
                .FindPropertyRelative(WSV_ENTITY_INDEX_PROPERTY_NAME);
            
            // Key property
            var serializedKey = serializedWorldState
                .FindPropertyRelative(WSV_KEY_PROPERTY_NAME);
            
            // Value property
            var serializedValue = serializedWorldState
                .FindPropertyRelative(WSV_VALUE_PROPERTY_NAME);

            // Setup the rects
            var thirdWidth = (int) (rect.width * 0.333333f);
            var halfHeight = (int) (rect.height * 0.5f);

            var controlRect = rect.KeepToTopfor(halfHeight - 1).ShiftToBottomBy(1);
            
            // Display editable fields

            EditorGUI.BeginChangeCheck();
            
            // EntityIndex dropdown
            serializedIndex.intValue = EditorGUI.Popup
                (controlRect.KeepToLeftFor(thirdWidth - 1), serializedIndex.intValue, IndexPopup);
            
            // Key reference
            EditorGUI.PropertyField
                (controlRect.ShiftToRightBy(thirdWidth + 1, thirdWidth - 1), serializedKey, GUIContent.none);
            
            // Value reference
            EditorGUI.PropertyField
                (controlRect.KeepToRightFor(thirdWidth - 1), serializedValue, GUIContent.none);
            
            if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();

            controlRect = rect.ShiftToBottomBy(halfHeight + 1);
            
            // Display data with disabled GUI (for faded colour)

            GUI.enabled = false;
            
            // Entity
            EditorGUI.LabelField(controlRect.KeepToLeftFor(thirdWidth - 1),
                IndexPopupNames[serializedIndex.intValue], EditorStyles.boldLabel);
            
            // Key name
            EditorGUI.LabelField(controlRect.ShiftToRightBy(thirdWidth + 1, thirdWidth - 1),
                serializedKey.objectReferenceValue == null ? "" : serializedKey.objectReferenceValue.name, EditorStyles.boldLabel);

            // Value name
            EditorGUI.LabelField(controlRect.KeepToRightFor(thirdWidth - 1),
                serializedValue.objectReferenceValue == null ? "" : serializedValue.objectReferenceValue.name, EditorStyles.boldLabel);

            GUI.enabled = true;
        }
    }
}