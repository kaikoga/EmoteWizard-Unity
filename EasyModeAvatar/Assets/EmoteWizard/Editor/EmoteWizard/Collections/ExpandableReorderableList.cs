using EmoteWizard.Collections.Base;
using EmoteWizard.Extensions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static EmoteWizard.Tools.PropertyDrawerUITools;

namespace EmoteWizard.Collections
{
    public class ExpandableReorderableList : ReorderableList
    {
        readonly ListHeaderDrawer _listHeaderDrawer;

        public ExpandableReorderableList(SerializedObject serializedObject, SerializedProperty elements, string headerName, ListHeaderDrawer headerDrawer) : base(serializedObject, elements)
        {
            _listHeaderDrawer = headerDrawer;

            drawHeaderCallback += rect =>
            {
                var isExpanded = EditorGUI.Foldout(rect.SliceV(0), serializedProperty.isExpanded, headerName);
                serializedProperty.isExpanded = isExpanded;
                draggable = isExpanded;
                displayAdd = isExpanded;
                displayRemove = isExpanded;
                
                if (isExpanded) _listHeaderDrawer.OnGUI(rect.SliceV(1, -1), true);
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
            showDefaultBackground = false;
        }

        public void DrawAsProperty(bool useReorderUI)
        {
            if (useReorderUI)
            {
                if (serializedProperty.isExpanded)
                {
                    headerHeight = 16f + _listHeaderDrawer.GetHeaderHeight();
                    footerHeight = 12f;
                }
                else
                {
                    headerHeight = 16f;
                    footerHeight = 0f;
                }

                using (new EditorGUI.IndentLevelScope()) DoLayoutList();
            }
            else
            {
                if (serializedProperty.isExpanded) _listHeaderDrawer.OnGUI(false);
                EditorGUILayout.PropertyField(serializedProperty, true);
            }
        }
    }
}