using System;
using EmoteWizard.Collections.Base;
using EmoteWizard.Extensions;
using EmoteWizard.UI;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EmoteWizard.Collections
{
    public class ExpandableReorderableList : ReorderableList
    {
        public static bool Enabled = true;
        
        readonly string _headerName;
        readonly ListHeaderDrawer _listHeaderDrawer;

        public ExpandableReorderableList(SerializedObject serializedObject, SerializedProperty elements, string headerName, ListHeaderDrawer headerDrawer) : base(serializedObject, elements)
        {
            _headerName = headerName;
            _listHeaderDrawer = headerDrawer;

            drawHeaderCallback += rect =>
            {
                var isExpanded = EditorGUI.Foldout(rect.UISliceV(0), serializedProperty.isExpanded, headerName);
                serializedProperty.isExpanded = isExpanded;
                draggable = isExpanded;
                displayAdd = isExpanded;
                displayRemove = isExpanded;

                if (isExpanded)
                {
                    _listHeaderDrawer?.OnGUI(rect.UISliceV(1, -1), true);
                }
                else
                {
                    index = -1;
                }
            };

            drawNoneElementCallback += _ => { };
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

        public void DrawAsProperty(ListDisplayMode displayMode)
        {
            if (!Enabled)
            {
                EditorGUILayout.PropertyField(serializedProperty, true);
            }
            else
            {
                switch (displayMode)
                {
                    case ListDisplayMode.List:
                        DrawAsList();
                        break;
                    case ListDisplayMode.ReorderList:
                        DrawAsReorderList();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(displayMode), displayMode, null);
                }
            }
        }

        void DrawAsList()
        {
            var isExpanded = EditorGUILayout.Foldout(serializedProperty.isExpanded, _headerName);
            serializedProperty.isExpanded = isExpanded;
            if (!isExpanded) return;

            using (new EditorGUI.IndentLevelScope())
            {
                const int arraySizeMax = 100;
                var arraySize = EditorGUILayout.DelayedIntField("Size", serializedProperty.arraySize);
                if (arraySize > arraySizeMax) arraySize = arraySizeMax;
                serializedProperty.arraySize = arraySize;

                if (serializedProperty.isExpanded) _listHeaderDrawer?.OnGUI(false);
                for (var i = 0; i < arraySize; i++)
                {
                    EditorGUILayout.PropertyField(serializedProperty.GetArrayElementAtIndex(i));
                }
            }
        }

        void DrawAsReorderList()
        {
            if (serializedProperty.isExpanded)
            {
                headerHeight = 16f + (_listHeaderDrawer?.GetHeaderHeight() ?? 0f);
                footerHeight = 12f;
            }
            else
            {
                headerHeight = 16f;
                footerHeight = 0f;
            }
            using (new EditorGUI.IndentLevelScope()) DoLayoutList();
        }
    }
}