using System;
using System.IO;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Sources.Impl;
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

        public override void OnInspectorGUI()
        {
            var serializedObj = serializedObject.FindProperty(nameof(ExpressionItemSource.expressionItem));

            EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(ExpressionItem.icon)));
            EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(ExpressionItem.path)));
            EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(ExpressionItem.parameter)));
            EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(ExpressionItem.value)));
            EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(ExpressionItem.itemKind)));

            var y = 2;

            void DrawSubParameters(int subParametersCount, int labelsCount)
            {
                serializedObj.FindPropertyRelative(nameof(ExpressionItem.subParameters)).arraySize = subParametersCount;
                if (subParametersCount > 0)
                {
                    GUILayout.Label("Puppet Parameters");
                    for (var i = 0; i < subParametersCount; i++)
                    {
                        GUILayout.Label(SubParameterLabels[subParametersCount][i]);
                        EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(ExpressionItem.subParameters)).GetArrayElementAtIndex(i));
                    }
                }

                serializedObj.FindPropertyRelative(nameof(ExpressionItem.labels)).arraySize = labelsCount;
                serializedObj.FindPropertyRelative(nameof(ExpressionItem.labelIcons)).arraySize = labelsCount;
                if (labelsCount > 0)
                {
                    GUILayout.Label("Puppet Labels");
                    for (var i = 0; i < labelsCount; i++)
                    {
                        GUILayout.Label(SubParameterLabels[labelsCount][i]);
                        EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(ExpressionItem.labels)).GetArrayElementAtIndex(i));
                        EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(ExpressionItem.labelIcons)).GetArrayElementAtIndex(i));
                    }
                }
            }

            if (serializedObj.FindPropertyRelative(nameof(ExpressionItem.itemKind)).hasMultipleDifferentValues) return;
            switch ((ExpressionItemKind)serializedObj.FindPropertyRelative(nameof(ExpressionItem.itemKind)).intValue)
            {
                case ExpressionItemKind.Button:
                case ExpressionItemKind.Toggle:
                    break;
                case ExpressionItemKind.SubMenu:
                    CustomEditorGUILayout.PropertyFieldWithGenerate(
                        serializedObj.FindPropertyRelative(nameof(ExpressionItem.subMenu)),
                        () =>
                        {
                            var name = serializedObj.FindPropertyRelative(nameof(ExpressionItem.subMenu)).stringValue;
                            if (string.IsNullOrEmpty(name))
                            {
                                Debug.LogError("Expression name is required.");
                                return null;
                            }

                            var relativePath =
                                $"Expressions/@@@Generated@@@ExprSubmenu_{Path.GetFileName(name)}.anim";
                            return ((ExpressionItemSource)target).EmoteWizardRoot.EnsureAsset<VRCExpressionsMenu>(relativePath);
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
        }
    }
}
