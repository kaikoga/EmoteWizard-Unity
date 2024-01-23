using System;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static class EmoteWizardGUILayout
    {
        const float GenerateButtonWidth = 64f; 

        static readonly GUIStyle HeaderStyle = new GUIStyle
        {
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleLeft,
            padding = new RectOffset(-4, 0, 4, 0)
        };

        static readonly Color ConfigUIColor = new Color(0.80f, 0.80f, 0.82f);
        static readonly Color OutputUIColor = new Color(0.3f, 1.0f, 0.9f);

        public static void Header(LocalizedContent loc) => EditorGUILayout.LabelField(loc.Tr, HeaderStyle);

        public static void PropertyFoldout(LocalizedProperty lop, Action content = null)
        {
            var rect = EditorGUILayout.GetControlRect();
            using (new EditorGUI.PropertyScope(rect, lop.GUIContent, lop.Property))
            {
                lop.Property.boolValue = EditorGUI.ToggleLeft(rect, lop.GUIContent, lop.Property.boolValue);
            }

            if (lop.Property.boolValue)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    content?.Invoke();
                }
            }
        }

        public static IUndoable Undoable(LocalizedContent loc) => Undoable(loc, loc.Tr);

        public static IUndoable Undoable(LocalizedContent loc, string undoLabel) => GUILayout.Button(loc.GUIContent) ? new EditorUndoable(undoLabel) : null;

        public static void Undoable(LocalizedContent loc, Action<IUndoable> onClick) => Undoable(loc, loc.Tr, onClick);

        public static void Undoable(LocalizedContent loc, string undoLabel, Action<IUndoable> onClick)
        {
            if (GUILayout.Button(loc.GUIContent)) onClick(new EditorUndoable(undoLabel)); 
        }

        public static void ConfigUIArea(Action action)
        {
            using (new BoxLayoutScope(ConfigUIColor))
            {
                action();
            }
        }

        public static void OutputUIArea(bool persistGeneratedAssets, Action action) => OutputUIArea(persistGeneratedAssets, Loc("EWS::Output zone"), action);

        public static void OutputUIArea(bool persistGeneratedAssets, LocalizedContent loc, Action action)
        {
            if (!persistGeneratedAssets) return;
            using (new BoxLayoutScope(OutputUIColor))
            {
                if (!loc.IsNullOrEmpty()) GUILayout.Label(loc.GUIContent);
                action();
            }
        }

        public static void Prop(LocalizedProperty lop) => EditorGUILayout.PropertyField(lop.Property, lop.Loc.GUIContent);

        public static void PropertyFieldWithGenerate(LocalizedProperty lop, Func<Object> generate)
        {
            using (new GUILayout.HorizontalScope())
            {
                Prop(lop);
                if (lop.Property.objectReferenceValue == null && Button(Loc("EWS::Generate"), GUILayout.Width(GenerateButtonWidth)))
                {
                    lop.Property.objectReferenceValue = generate();
                }
            }
        }

        public static T ObjectField<T>(LocalizedContent loc, T obj, bool allowSceneObjects)
            where T : Object
        {
            return (T) EditorGUILayout.ObjectField(loc.GUIContent, obj, typeof(T), allowSceneObjects);
        }

        public static bool Button(LocalizedContent loc, params GUILayoutOption[] options) => GUILayout.Button(loc.GUIContent, options);

        public static void Label(LocalizedContent loc, GUILayoutOption[] options) => GUILayout.Label(loc.GUIContent, options);

        public static bool Foldout(bool foldout, LocalizedContent loc) => EditorGUILayout.Foldout(foldout, loc.GUIContent);
        
        public static void HelpBox(LocalizedContent loc, MessageType type) => EditorGUILayout.HelpBox(loc.LongTr, type);
    }
}