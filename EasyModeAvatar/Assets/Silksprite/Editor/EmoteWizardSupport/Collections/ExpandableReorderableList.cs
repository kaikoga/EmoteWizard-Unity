using System;
using System.Linq;
using Silksprite.EmoteWizardSupport.Collections.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.Collections
{
    public class ExpandableReorderableList : ReorderableList
    {
        public static bool Enabled = true;

        public int pagerIndex = 0;

        readonly ListDrawerBase _listDrawer;

        public ExpandableReorderableList(ListDrawerBase listDrawer, SerializedProperty elements) : base(elements.serializedObject, elements)
        {
            _listDrawer = listDrawer;

            drawHeaderCallback += rect =>
            {
                var isExpanded = EditorGUI.Foldout(rect.UISliceV(0), serializedProperty.isExpanded, serializedProperty.displayName);
                serializedProperty.isExpanded = isExpanded;
                draggable = isExpanded;
                displayAdd = isExpanded;
                displayRemove = isExpanded;

                if (isExpanded)
                {
                    _listDrawer?.OnGUI(rect.UISliceV(1, -1), true);
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
                    case ListDisplayMode.Pager:
                        DrawAsPager();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(displayMode), displayMode, null);
                }
            }
        }

        void DrawAsList()
        {
            var isExpanded = EditorGUILayout.Foldout(serializedProperty.isExpanded, serializedProperty.displayName);
            serializedProperty.isExpanded = isExpanded;
            if (!isExpanded) return;

            using (new EditorGUI.IndentLevelScope())
            {
                const int arraySizeMax = 100;
                var arraySize = EditorGUILayout.DelayedIntField("Size", serializedProperty.arraySize);
                if (arraySize > arraySizeMax) arraySize = arraySizeMax;
                serializedProperty.arraySize = arraySize;

                _listDrawer?.OnGUI(false);
                foreach (var child in serializedProperty) 
                { 
                    EditorGUILayout.PropertyField((SerializedProperty) child);
                }
            }
        }

        void DrawAsReorderList()
        {
            if (serializedProperty.isExpanded)
            {
                headerHeight = 16f + (_listDrawer?.GetHeaderHeight() ?? 0f);
                footerHeight = 12f;
            }
            else
            {
                headerHeight = 16f;
                footerHeight = 0f;
            }
            using (new EditorGUI.IndentLevelScope()) DoLayoutList();
        }
        
        void DrawAsPager()
        {
            var isExpanded = EditorGUILayout.Foldout(serializedProperty.isExpanded, serializedProperty.displayName);
            serializedProperty.isExpanded = isExpanded;
            if (!isExpanded) return;

            using (new EditorGUI.IndentLevelScope())
            {
                const int arraySizeMax = 100;
                var arraySize = serializedProperty.arraySize;

                using (new GUILayout.HorizontalScope())
                using (new HideLabelsScope())
                {
                    var pagerOptions = Enumerable.Range(0, arraySize)
                        .Select(i => _listDrawer.PagerItemName(serializedProperty.GetArrayElementAtIndex(i), i))
                        .ToArray();
                    using (new EditorGUI.DisabledScope(arraySize == 0))
                    {
                        pagerIndex = EditorGUILayout.Popup("", pagerIndex, pagerOptions, GUILayout.MinWidth(80f), GUILayout.ExpandWidth(true));
                        pagerIndex = EditorGUILayout.IntField("", pagerIndex + 1, GUILayout.Width(50f)) - 1;
                    }
                    GUILayout.Label("/", GUILayout.Width(10f));
                    arraySize = EditorGUILayout.DelayedIntField("", arraySize, GUILayout.Width(50f));
                    if (arraySize > arraySizeMax) arraySize = arraySizeMax;
                    serializedProperty.arraySize = arraySize;

                    using (new EditorGUI.DisabledScope(arraySize == 0))
                    {
                        if (GUILayout.Button("<<", GUILayout.Width(24f))) pagerIndex = 0;
                        if (GUILayout.Button("<", GUILayout.Width(24f))) pagerIndex--;
                        if (GUILayout.Button(">", GUILayout.Width(24f))) pagerIndex++;
                        if (GUILayout.Button(">>", GUILayout.Width(24f))) pagerIndex = int.MaxValue;
                    }

                    if (pagerIndex >= arraySize) pagerIndex = arraySize - 1;
                    if (pagerIndex < 0) pagerIndex = 0;
                }

                _listDrawer?.OnGUI(false);
                if (pagerIndex < arraySize)
                {
                    EditorGUILayout.PropertyField(serializedProperty.GetArrayElementAtIndex(pagerIndex));
                }
            }
        }
    }
}