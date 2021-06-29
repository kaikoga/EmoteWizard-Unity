using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EmoteWizard.Collections
{
    public class ExpandableReorderableList : ReorderableList
    {
        public ExpandableReorderableList(SerializedObject serializedObject, SerializedProperty elements, string headerName) : base(serializedObject, elements)
        {
            drawHeaderCallback += rect =>
            {
                var isExpanded = EditorGUI.Foldout(rect, serializedProperty.isExpanded, headerName);
                serializedProperty.isExpanded = isExpanded;
                draggable = isExpanded;
                displayAdd = isExpanded;
                displayRemove = isExpanded;
            };

            drawElementCallback += (rect, index, selected, focused) =>
            {
                if (serializedProperty.isExpanded)
                {
                    EditorGUI.PropertyField(rect, serializedProperty.GetArrayElementAtIndex(index), GUIContent.none);
                }
            };
            elementHeightCallback += index => serializedProperty.isExpanded
                ? EditorGUI.GetPropertyHeight(serializedProperty.GetArrayElementAtIndex(index),
                    GUIContent.none)
                : 0f;

            onCanAddCallback += list => serializedProperty.isExpanded;
            onCanRemoveCallback += list => serializedProperty.isExpanded;
        }

        public void DrawAsProperty(bool useReorderUI)
        {
            if (useReorderUI)
            {
                using (new EditorGUI.IndentLevelScope()) DoLayoutList();
            }
            else
            {
                EditorGUILayout.PropertyField(serializedProperty, true);
            }
        }
    }
}