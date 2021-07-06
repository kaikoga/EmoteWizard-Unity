using System;
using System.Linq;
using EmoteWizard.Collections.Base;
using EmoteWizard.Extensions;
using EmoteWizard.Scopes;
using EmoteWizard.UI;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EmoteWizard.Collections
{
    public class ExpandableReorderableList : ReorderableList
    {
        public static bool Enabled = true;

        public int pagerIndex = 0;

        readonly string _headerName;
        readonly ListHeaderDrawer _listHeaderDrawer;
        readonly Func<SerializedProperty, int, string> _pagerNameGenerator = (_, i) => $"Item {i + 1}";

        public ExpandableReorderableList(SerializedObject serializedObject, SerializedProperty elements, string headerName, ListHeaderDrawer headerDrawer, Func<SerializedProperty, int, string> pagerNameGenerator) : base(serializedObject, elements)
        {
            _headerName = headerName;
            _listHeaderDrawer = headerDrawer;
            if (pagerNameGenerator != null) _pagerNameGenerator = pagerNameGenerator;

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
            var isExpanded = EditorGUILayout.Foldout(serializedProperty.isExpanded, _headerName);
            serializedProperty.isExpanded = isExpanded;
            if (!isExpanded) return;

            using (new EditorGUI.IndentLevelScope())
            {
                const int arraySizeMax = 100;
                var arraySize = EditorGUILayout.DelayedIntField("Size", serializedProperty.arraySize);
                if (arraySize > arraySizeMax) arraySize = arraySizeMax;
                serializedProperty.arraySize = arraySize;

                _listHeaderDrawer?.OnGUI(false);
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
        
        void DrawAsPager()
        {
            var isExpanded = EditorGUILayout.Foldout(serializedProperty.isExpanded, _headerName);
            serializedProperty.isExpanded = isExpanded;

            using (new EditorGUI.IndentLevelScope())
            {
                const int arraySizeMax = 100;
                var arraySize = serializedProperty.arraySize;

                using (new GUILayout.HorizontalScope())
                using (new HideLabelsScope())
                {
                    var pagerOptions = Enumerable.Range(0, arraySize).Select(i => _pagerNameGenerator(serializedProperty.GetArrayElementAtIndex(i), i)).ToArray();
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

                if (serializedProperty.isExpanded)
                {
                    _listHeaderDrawer?.OnGUI(false);
                }

                if (pagerIndex < arraySize)
                {
                    EditorGUILayout.PropertyField(serializedProperty.GetArrayElementAtIndex(pagerIndex));
                }
            }
        }
    }
}