using System;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.Undoable;
using Silksprite.EmoteWizardSupport.Utils;
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

        static readonly GUIStyle HeaderFoldoutStyle = GetHeaderFoldoutStyle();

        static GUIStyle GetHeaderFoldoutStyle()
        {
            return new GUIStyle(EditorStyles.foldout)
            {
                fontStyle = FontStyle.Bold
            };
        } 
        
        static readonly Color ConfigUIColor = new Color(0.80f, 0.80f, 0.82f);
        static readonly Color OutputUIColor = new Color(0.3f, 1.0f, 0.9f);

        public static void Header(LocalizedContent loc) => EditorGUILayout.LabelField(loc.Tr, HeaderStyle);
        public static bool HeaderFoldout(bool foldout, LocalizedContent loc) => EditorGUILayout.Foldout(foldout, loc.GUIContent, HeaderFoldoutStyle);

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

        public static void Undoable(LocalizedContent loc, Action<IUndoable> onClick, params GUILayoutOption[] options) => Undoable(loc, loc.Tr, onClick);

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
        public static void PropAsEnumPopup<TEnum>(LocalizedProperty lop)
        where TEnum : Enum
        {
            EditorGUI.BeginChangeCheck();
            if (lop.Property.hasMultipleDifferentValues)
            {
                EditorGUI.showMixedValue = true;
            }
            var newValue = (TEnum)EditorGUILayout.EnumPopup(lop.GUIContent, (TEnum)(object)lop.Property.intValue);
            EditorGUI.showMixedValue = false;

            if (!EditorGUI.EndChangeCheck()) return;
            lop.Property.intValue = Convert.ToInt32(newValue);
        }

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

        public static void Label(LocalizedContent loc) => GUILayout.Label(loc.GUIContent);

        public static bool Foldout(bool foldout, LocalizedContent loc) => EditorGUILayout.Foldout(foldout, loc.GUIContent);

        public static TEnum EnumPopup<TEnum>(LocalizedContent loc, TEnum value) where TEnum : Enum => (TEnum)EditorGUILayout.EnumPopup(loc.GUIContent, value);
        
        public static void HelpBox(LocalizedContent loc, MessageType type) => EditorGUILayout.HelpBox(loc.LongTr, type);
        public static void HelpBox(LocalizedContent loc, MessageType type, Substitution substitution) => EditorGUILayout.HelpBox(loc.LongTrFormat(substitution), type);
    }
}