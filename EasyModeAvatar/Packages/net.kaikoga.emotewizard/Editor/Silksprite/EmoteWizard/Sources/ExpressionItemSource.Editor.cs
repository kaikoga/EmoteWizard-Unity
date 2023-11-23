using System;
using System.IO;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.Sources
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ExpressionItemSource))]
    public class ExpressionItemSourceEditor : Editor
    {
        static readonly string[][] SubParameterLabels =
        {
            null,
            new[] { "Rotation" },
            new[] { "Horizontal", "Vertical" },
            null,
            new[] { "Up", "Right", "Down", "Left" }
        };

        SerializedProperty _serializedIcon;
        SerializedProperty _serializedPath;
        SerializedProperty _serializedParameter;
        SerializedProperty _serializedValue;
        SerializedProperty _serializedItemKind;
        SerializedProperty _serializedSubParameters;
        SerializedProperty _serializedLabels;
        SerializedProperty _serializedLabelIcons;
        SerializedProperty _serializedSubMenu;

        void OnEnable()
        {
            var serializedItem = serializedObject.FindProperty(nameof(ExpressionItemSource.expressionItem));

            _serializedIcon = serializedItem.FindPropertyRelative(nameof(ExpressionItem.icon));
            _serializedPath = serializedItem.FindPropertyRelative(nameof(ExpressionItem.path));
            _serializedParameter = serializedItem.FindPropertyRelative(nameof(ExpressionItem.parameter));
            _serializedValue = serializedItem.FindPropertyRelative(nameof(ExpressionItem.value));
            _serializedItemKind = serializedItem.FindPropertyRelative(nameof(ExpressionItem.itemKind));
            _serializedSubParameters = serializedItem.FindPropertyRelative(nameof(ExpressionItem.subParameters));
            _serializedLabels = serializedItem.FindPropertyRelative(nameof(ExpressionItem.labels));
            _serializedLabelIcons = serializedItem.FindPropertyRelative(nameof(ExpressionItem.labelIcons));
            _serializedSubMenu = serializedItem.FindPropertyRelative(nameof(ExpressionItem.subMenu));
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_serializedIcon);
            EditorGUILayout.PropertyField(_serializedPath);
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.PropertyField(_serializedParameter);
                using (new HideLabelsScope())
                {
                    GUILayout.Label("=", GUILayout.ExpandWidth(false));
                    EditorGUILayout.PropertyField(_serializedValue);
                }
            }
            EditorGUILayout.PropertyField(_serializedItemKind);

            void DrawSubParameters(int subParametersCount, int labelsCount)
            {
                _serializedSubParameters.arraySize = subParametersCount;
                if (subParametersCount > 0)
                {
                    GUILayout.Label("Puppet Parameters");
                    for (var i = 0; i < subParametersCount; i++)
                    {
                        GUILayout.Label(SubParameterLabels[subParametersCount][i]);
                        EditorGUILayout.PropertyField(_serializedSubParameters.GetArrayElementAtIndex(i));
                    }
                }

                _serializedLabels.arraySize = labelsCount;
                _serializedLabelIcons.arraySize = labelsCount;
                if (labelsCount > 0)
                {
                    GUILayout.Label("Puppet Labels");
                    for (var i = 0; i < labelsCount; i++)
                    {
                        GUILayout.Label(SubParameterLabels[labelsCount][i]);
                        EditorGUILayout.PropertyField(_serializedLabels.GetArrayElementAtIndex(i));
                        EditorGUILayout.PropertyField(_serializedLabelIcons.GetArrayElementAtIndex(i));
                    }
                }
            }

            if (_serializedItemKind.hasMultipleDifferentValues) return;
            switch ((ExpressionItemKind)_serializedItemKind.intValue)
            {
                case ExpressionItemKind.Button:
                case ExpressionItemKind.Toggle:
                    break;
                case ExpressionItemKind.SubMenu:
                    CustomEditorGUILayout.PropertyFieldWithGenerate(
                        _serializedSubMenu,
                        () =>
                        {
                            var name = _serializedPath.stringValue;
                            if (string.IsNullOrEmpty(name))
                            {
                                Debug.LogError("Expression name is required.");
                                return null;
                            }

                            var relativePath =
                                $"Expressions/@@@Generated@@@ExprSubmenu_{Path.GetFileName(name)}.anim";
                            return ((ExpressionItemSource)target).Environment.EnsureAsset<VRCExpressionsMenu>(relativePath);
                        });
                    break;
                case ExpressionItemKind.TwoAxisPuppet:
                    DrawSubParameters(2, 4);
                    break;
                case ExpressionItemKind.FourAxisPuppet:
                    DrawSubParameters(4, 4);
                    break;
                case ExpressionItemKind.RadialPuppet:
                    DrawSubParameters(1, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            serializedObject.ApplyModifiedProperties();
            
            EmoteWizardGUILayout.Tutorial(((ExpressionItemSource)target).Environment, Tutorial);
        }

        static string Tutorial =>
            string.Join("\n",
                "Expression Menuを登録します。");
    }
}
