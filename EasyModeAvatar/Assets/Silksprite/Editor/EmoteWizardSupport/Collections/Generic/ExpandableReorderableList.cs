using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizardSupport.Collections.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.Collections.Generic
{
    public class ExpandableReorderableList<T> : ReorderableList
    {
        public int pagerIndex = 0;

        public bool IsExpanded => IsExpandedTracker.GetIsExpanded(list);

        readonly ListDrawerBase _listDrawer;
        readonly ITypedDrawer<T> _typedDrawer;
        readonly string _headerName;
        readonly Action<int> _repopulate;

        public ExpandableReorderableList(ListDrawerBase listDrawer, ITypedDrawer<T> typedDrawer, string headerName, T[] elements) : this(listDrawer, typedDrawer, headerName, elements, true)
        {
            _repopulate = size =>
            {
                var l = (T[]) list;
                ArrayUtils.ResizeAndPopulate(ref l, size);
                list = l;
            };
        }
        
        public ExpandableReorderableList(ListDrawerBase listDrawer, ITypedDrawer<T> typedDrawer, string headerName, List<T> elements) : this(listDrawer, typedDrawer, headerName, elements, true)
        {
            _repopulate = size =>
            {
                var l = (List<T>) list;
                ListUtils.ResizeAndPopulate(ref l, size);
                list = l;
            };
        }
        
        ExpandableReorderableList(ListDrawerBase listDrawer, ITypedDrawer<T> typedDrawer, string headerName, IList elements, bool ignored) : base(elements, typeof(T))
        {
            _listDrawer = listDrawer;
            _typedDrawer = typedDrawer;
            _headerName = headerName;

            drawHeaderCallback += rect =>
            {
                var isExpanded = TypedGUI.Foldout(rect.UISliceV(0), list, _headerName);
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
                if (IsExpanded)
                {
                    var item = (T) list[index];
                    _typedDrawer.OnGUI(rect, ref item, GUIContent.none);
                    list[index] = item;
                }
            };
            elementHeightCallback += index => IsExpanded ? _typedDrawer.GetPropertyHeight((T) list[index], GUIContent.none) : 0f;

            onCanAddCallback += list => IsExpanded;
            onCanRemoveCallback += list => IsExpanded;
            showDefaultBackground = false;
        }

        public void DrawAsProperty(IList elements, ListDisplayMode displayMode)
        {
            list = elements;
            if (!ExpandableReorderableList.Enabled)
            {
                DrawAsList();
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
            var isExpanded = TypedGUILayout.Foldout(list, _headerName);
            if (!isExpanded) return;

            using (new EditorGUI.IndentLevelScope())
            {
                const int arraySizeMax = 100;
                var arraySize = list.Count;
                TypedGUILayout.DelayedIntField("Size", ref arraySize);
                if (arraySize > arraySizeMax) arraySize = arraySizeMax;
                _repopulate(arraySize);

                _listDrawer?.OnGUI(false);
                for (var i = 0; i < list.Count; i++)
                {
                    var child = (T) list[i];
                    var height = _typedDrawer.GetPropertyHeight((T) child, GUIContent.none);
                    _typedDrawer.OnGUI(EditorGUILayout.GetControlRect(false, height), ref child, GUIContent.none);
                    list[i] = child;
                }
            }
        }

        void DrawAsReorderList()
        {
            if (IsExpanded)
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
            var isExpanded = TypedGUILayout.Foldout(list, _headerName);
            if (!isExpanded) return;

            using (new EditorGUI.IndentLevelScope())
            {
                const int arraySizeMax = 100;
                var arraySize = list.Count;

                using (new GUILayout.HorizontalScope())
                using (new HideLabelsScope())
                {
                    var pagerOptions = Enumerable.Range(0, arraySize)
                        .Select(i => _typedDrawer.PagerItemName((T) list[i], i))
                        .ToArray();
                    using (new EditorGUI.DisabledScope(arraySize == 0))
                    {
                        pagerIndex = EditorGUILayout.Popup("", pagerIndex, pagerOptions, GUILayout.MinWidth(80f), GUILayout.ExpandWidth(true));
                        pagerIndex++;
                        TypedGUILayout.IntField("", ref pagerIndex, GUILayout.Width(50f));
                        pagerIndex--;
                    }
                    GUILayout.Label("/", GUILayout.Width(10f));
                    TypedGUILayout.DelayedIntField("", ref arraySize, GUILayout.Width(50f));
                    if (arraySize > arraySizeMax) arraySize = arraySizeMax;
                    _repopulate(arraySize);

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
                    var child = (T) list[pagerIndex];
                    var height = _typedDrawer.GetPropertyHeight(child, GUIContent.none);
                    _typedDrawer.OnGUI(EditorGUILayout.GetControlRect(false, height), ref child, GUIContent.none);
                    list[pagerIndex] = child;
                }
            }
        }
    }
}