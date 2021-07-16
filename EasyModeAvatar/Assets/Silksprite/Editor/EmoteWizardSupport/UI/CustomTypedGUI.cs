using System;
using System.Collections;
using System.Collections.Generic;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static class CustomTypedGUI
    {
        public static void HorizontalListUntypedField(Rect position, GUIContent label, IList list)
        {
            var labelWidth = EditorGUIUtility.labelWidth;
            var sizeWidth = labelWidth * 0.25f;
            labelWidth *= 0.75f;
            GUI.Label(new Rect(position.x, position.y, labelWidth, EditorGUIUtility.singleLineHeight), label);
            using (new HideLabelsScope())
            {
                var arraySize = list.Count;
                TypedGUI.DelayedIntField(new Rect(position.x + labelWidth, position.y, sizeWidth, EditorGUIUtility.singleLineHeight), "Size", ref arraySize);
                ListUtils.ResizeAndPopulate(ref list, arraySize);
                position = position.Inset(labelWidth + sizeWidth, 0f, 0f, 0f);
                if (arraySize > 0)
                {
                    for (var i = 0; i < arraySize; i++)
                    {
                        var width = position.width;
                        var item = list[i];
                        TypedGUI.UntypedField(position.SliceH(width * i, width), ref item, GUIContent.none);
                        list[i] = item;
                    }
                }
            }
        }

        public static void AssetFieldWithGenerate<T>(Rect position, string label, ref T value, Func<T> generate)
        where T : Object
        {
            const float buttonWidth = 60;
            var fieldPosition = position;
            if (value == null) fieldPosition.width -= buttonWidth;

            TypedGUI.AssetField(fieldPosition, label, ref value);
            if (value != null) return;
            var buttonPosition = new Rect(position.xMax - buttonWidth, position.y, buttonWidth, position.height);
            if (GUI.Button(buttonPosition, "Generate"))
            {
                value = generate();
            }
        }
    }
}