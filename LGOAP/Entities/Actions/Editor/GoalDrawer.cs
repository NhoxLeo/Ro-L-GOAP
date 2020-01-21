/* * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * *  
 * *  File: GoalDrawer.cs
 * *  Type: Visual C# Source File
 * *  
 * *  Author: Rohan Jadav
 * *  
 * *  Description: Property drawer for Goal.
 * *  
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * */

using System.Linq;
using Hiralal.Boilerplate.Extensions;
using UnityEditor;
using UnityEngine;

namespace Hiralal.LGOAP.EditorAssembly
{
    [CustomPropertyDrawer(typeof(Goal))]
    public class GoalDrawer : PropertyDrawer
    {
        private const string TRANSITION_PROPERTY_NAME = nameof(Goal.transition);
        private const string ENTITIES_PROPERTY_NAME = nameof(Goal.entities);

        protected virtual int StartHeight => -5;

        private static GUIContent[] _labels;

        protected static GUIContent[] Labels
        {
            get
            {
                if (_labels == null)
                {
                    // create a new array and populate it with elements same as the popup names
                    _labels = new GUIContent[WorldStateTransitionEditor.IndexPopupNames.Length];
                    for (var i = 0; i < WorldStateTransitionEditor.IndexPopupNames.Length; i++)
                    {
                        _labels[i] = new GUIContent(WorldStateTransitionEditor.IndexPopupNames[i] + " (" +
                                                    WorldStateTransitionEditor.IndexPopup[i] + ")");
                    }
                }

                return _labels;
            }
            set => _labels = value;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // 21 if unexpanded, otherwise based on the entities array size
            var height = 21;

            if (property.isExpanded)
            {
                var transitionProperty = property.FindPropertyRelative(TRANSITION_PROPERTY_NAME);
                if (transitionProperty != null && transitionProperty.objectReferenceValue != null)
                {
                    var serializedTransition = new SerializedObject(transitionProperty.objectReferenceValue);

                    // for the conditions/effects headings
                    height += 57 + StartHeight;

                    // conditions array size, 42 for each + 5 space
                    var conditions =
                        serializedTransition.FindProperty(WorldStateTransitionEditor.CONDITIONS_PROPERTY_NAME);
                    height += conditions.arraySize * 47;

                    // effects array size, 42 for each + 5 space
                    var effects = serializedTransition.FindProperty(WorldStateTransitionEditor.EFFECTS_PROPERTY_NAME);
                    height += effects.arraySize * 47;
                }
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // get properties
            var (transition, entities) = GetProperties(property);

            EditorGUI.BeginProperty(position, label, property);

            DrawMainProperty(position, property, label, transition);

            // if the property is expanded draw serialized transition
            if (property.isExpanded && transition.objectReferenceValue != null)
            {
                DrawSerializedTransition(position.ShiftToBottomBy(StartHeight), transition, entities);
            }

            EditorGUI.EndProperty();
        }

        //populate properties
        protected static (SerializedProperty, SerializedProperty) GetProperties(SerializedProperty property)
        {
            var transition = property.FindPropertyRelative(TRANSITION_PROPERTY_NAME);
            var entities = property.FindPropertyRelative(ENTITIES_PROPERTY_NAME);

            var count = GetEntityCount(transition.objectReferenceValue as WorldStateTransition);

            // remove elements if size is bigger
            while (entities.arraySize > count)
            {
                entities.DeleteArrayElementAtIndex(entities.arraySize - 1);
                property.serializedObject.ApplyModifiedProperties();
            }

            // add elements if size is smaller
            while (entities.arraySize < count)
            {
                entities.InsertArrayElementAtIndex(entities.arraySize);
                property.serializedObject.ApplyModifiedProperties();
            }

            return (transition, entities);
        }

        // get highest number of entity index in transition
        protected static int GetEntityCount(WorldStateTransition transition)
        {
            if (transition == null) return 0;

            // entity indices start at 0
            var x = -1;

            // take max from both arrays.

            if (transition.conditions != null)
                x = transition.conditions.Aggregate(x, (current, condition) =>
                    Mathf.Max(condition.entityIndex, current));

            if (transition.effects != null)
                x = transition.effects.Aggregate(x, (current, effect) =>
                    Mathf.Max(effect.entityIndex, current));

            return x + 1;
        }

        protected static void DrawMainProperty(Rect position, SerializedProperty property, GUIContent label,
            SerializedProperty transition)
        {
            // pseudo-label
            var mainlabelrect = EditorGUI.PrefixLabel(position.KeepToTopfor(19), new GUIContent(" "));

            // transition reference
            mainlabelrect.x -= (EditorGUI.indentLevel * 15);
            mainlabelrect.width += (EditorGUI.indentLevel * 15);
            if (DrawError(transition))
            {
                // draw an empty error-box if required to draw error
                EditorGUI.HelpBox(position.KeepToTopfor(19).ShiftToRightBy((int) mainlabelrect.x - 4, 20),
                    "",
                    MessageType.Error);

                // account for the change in position
                mainlabelrect.x += 21;
                mainlabelrect.width -= 21;
            }

            // draw the main transition property
            EditorGUI.PropertyField(mainlabelrect, transition, GUIContent.none);

            // if transition is null draw an error
            if (transition.objectReferenceValue == null)
            {
                EditorGUI.HelpBox(position.KeepToTopfor(19)
                        .KeepToLeftFor((int) (position.width - mainlabelrect.width)),
                    "Non-nullable.",
                    MessageType.Error);
            }
            // otherwise draw a foldout in place of an error
            else
            {
                // foldout
                property.isExpanded = EditorGUI.Foldout(position
                        .KeepToTopfor(19)
                        .KeepToLeftFor((int) (position.width - mainlabelrect.width)),
                    property.isExpanded,
                    label,
                    EditorStyles.miniButton);
            }
        }

        protected static bool DrawError(SerializedProperty transitionProperty)
        {
            // return false right away, the error for that is drawn anyway if the transition property is null
            if (transitionProperty.objectReferenceValue == null) return false;

            var serializedTransition = new SerializedObject(transitionProperty.objectReferenceValue);
            var conditions =
                serializedTransition.FindProperty(WorldStateTransitionEditor.CONDITIONS_PROPERTY_NAME).arraySize;
            var effects =
                serializedTransition.FindProperty(WorldStateTransitionEditor.EFFECTS_PROPERTY_NAME).arraySize;

            // return true if both conditions and effects array sizes are 0
            return conditions == 0 && effects == 0;
        }

        protected static void DrawSerializedTransition(Rect totalPosition, SerializedProperty transition,
            SerializedProperty entities)
        {
            // populate variables
            var serializedTransition = new SerializedObject(transition.objectReferenceValue);

            var conditions =
                serializedTransition.FindProperty(WorldStateTransitionEditor.CONDITIONS_PROPERTY_NAME);
            var effects =
                serializedTransition.FindProperty(WorldStateTransitionEditor.EFFECTS_PROPERTY_NAME);

            // adjust indent, turn off gui
            EditorGUI.indentLevel++;
            GUI.enabled = false;
            {
                // cache indent
                var indent = EditorGUI.indentLevel;

                // if conditions array is empty, display an error
                if (conditions.arraySize == 0)
                {
                    EditorGUI.HelpBox(totalPosition.ShiftToBottomBy(31, 19),
                        "No conditions specified.", MessageType.Error);
                }
                // otherwise draw it
                else
                {
                    // heading
                    EditorGUI.LabelField(totalPosition.ShiftToBottomBy(31, 19), "Conditions",
                        EditorStyles.boldLabel);
                    // reset indent, indentation is accounted for within the rect calculation
                    EditorGUI.indentLevel = 0;

                    // elements
                    for (int i = 0; i < conditions.arraySize; i++)
                    {
                        // number label
                        EditorGUI.LabelField(totalPosition
                                .ShiftToBottomBy(52 + (47 * i), 42)
                                .ShiftToRightBy(7),
                            (i + 1) + ".");

                        // draw the whole element
                        DrawDetailedWsv(totalPosition.ShiftToRightBy(indent * 15).ShiftToBottomBy(52 + (47 * i), 42),
                            conditions.GetArrayElementAtIndex(i), entities);
                    }

                    // revert indent back to original
                    EditorGUI.indentLevel = indent;
                }

                // if effects array is empty, display an error
                if (effects.arraySize == 0)
                {
                    EditorGUI.HelpBox(totalPosition.ShiftToBottomBy(57 + (47 * conditions.arraySize), 19),
                        "No effects specified.", MessageType.Error);
                }
                // otherwise draw it
                else
                {
                    // number label
                    EditorGUI.LabelField(totalPosition.ShiftToBottomBy(57 + (47 * conditions.arraySize), 19),
                        "Effects",
                        EditorStyles.boldLabel);
                    // reset indent, indentation is accounted for within the rect calculation
                    EditorGUI.indentLevel = 0;

                    // elements
                    for (int i = 0; i < effects.arraySize; i++)
                    {
                        // number label
                        EditorGUI.LabelField(totalPosition
                                .ShiftToBottomBy(78 + (47 * conditions.arraySize) + (47 * i), 42)
                                .ShiftToRightBy(7),
                            (i + 1) + ".");

                        // draw the whole element
                        DrawDetailedWsv(totalPosition.ShiftToRightBy(indent * 15)
                                .ShiftToBottomBy(78 + (47 * conditions.arraySize) + (47 * i), 42),
                            effects.GetArrayElementAtIndex(i), entities);
                    }

                    // revert indent back to original
                    EditorGUI.indentLevel = indent;
                }
            }
            // re-enable gui, revert back indent
            GUI.enabled = true;
            EditorGUI.indentLevel--;
        }

        protected static void DrawDetailedWsv(Rect rect, SerializedProperty serializedWorldStateVariable,
            SerializedProperty entityProperty)
        {
            // EntityIndex property
            var serializedIndex = serializedWorldStateVariable
                .FindPropertyRelative(WorldStateTransitionEditor.WSV_ENTITY_INDEX_PROPERTY_NAME);

            // Key property
            var serializedKey = serializedWorldStateVariable
                .FindPropertyRelative(WorldStateTransitionEditor.WSV_KEY_PROPERTY_NAME);

            // Value property
            var serializedValue = serializedWorldStateVariable
                .FindPropertyRelative(WorldStateTransitionEditor.WSV_VALUE_PROPERTY_NAME);

            // Set up the rect
            var halfHeight = (int) (rect.height * 0.5f);
            var thirdWidth = (int) (rect.width * 0.333333f);

            // Top half
            var controlRect = rect.ShiftToBottomBy(1, halfHeight - 1);

            // Entity property
            // null because changing the transition objectreference will have a delay for the
            // max entityindex to update in the entities array
            if (serializedIndex != null && entityProperty.arraySize > serializedIndex.intValue)
            {
                GUI.enabled = true;
                EditorGUI.PropertyField(controlRect.KeepToLeftFor(thirdWidth - 1),
                    entityProperty.GetArrayElementAtIndex(serializedIndex.intValue),
                    GUIContent.none);
                GUI.enabled = false;
            }

            // Key
            EditorGUI.PropertyField(controlRect.ShiftToRightBy(thirdWidth + 1, thirdWidth - 1), serializedKey,
                GUIContent.none);

            // Value
            EditorGUI.PropertyField(controlRect.KeepToRightFor(thirdWidth - 1), serializedValue, GUIContent.none);

            // Bottom half
            controlRect = rect.ShiftToBottomBy(halfHeight + 1, halfHeight - 1);

            // Entity
            EditorGUI.LabelField(controlRect.KeepToLeftFor(thirdWidth - 1), Labels[serializedIndex.intValue]);

            // Key name
            EditorGUI.LabelField(controlRect.ShiftToRightBy(thirdWidth + 1, thirdWidth - 1),
                serializedKey.objectReferenceValue == null
                    ? ""
                    : serializedKey.objectReferenceValue.ToString());

            // Value name
            EditorGUI.LabelField(controlRect.KeepToRightFor(thirdWidth - 1),
                serializedValue.objectReferenceValue == null
                    ? ""
                    : serializedValue.objectReferenceValue.ToString());
        }
    }
}