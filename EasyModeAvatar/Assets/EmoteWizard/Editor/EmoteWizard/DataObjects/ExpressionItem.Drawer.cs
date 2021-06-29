using System;
using System.IO;
using EmoteWizard.Base;
using EmoteWizard.Extensions;
using EmoteWizard.Scopes;
using EmoteWizard.UI;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;
using static EmoteWizard.Tools.PropertyDrawerUITools;

namespace EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(ExpressionItem))]
    public class ExpressionItemDrawer : PropertyDrawerWithContext<ExpressionItemDrawer.Context>
    {
        static readonly string[][] SubParameterLabels = {
            null,
            new[] { "Rotation" },
            new[] { "Horizontal", "Vertical" },
            null,
            new[] { "Up", "Right", "Down", "Left" }
        };

        public static Context StartContext(EmoteWizardRoot emoteWizardRoot) => PropertyDrawerWithContext<Context>.StartContext(new Context(emoteWizardRoot));

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var context = EnsureContext(property);

            GUI.Box(position, GUIContent.none);
            position = position.InsideBox();
            using (new EditorGUI.PropertyScope(position, label, property))
            using (new EditorGUI.IndentLevelScope())
            using (new HideLabelsScope())
            {
                EditorGUI.PropertyField(position.Slice(0.0f, 0.4f, 0), property.FindPropertyRelative("icon"), new GUIContent(" "));
                EditorGUI.PropertyField(position.Slice(0.4f, 0.6f, 0), property.FindPropertyRelative("path"), new GUIContent(" "));

                EditorGUI.PropertyField(position.Slice(0.0f, 0.4f, 1), property.FindPropertyRelative("parameter"), new GUIContent(" "));
                EditorGUI.PropertyField(position.Slice(0.4f, 0.2f, 1), property.FindPropertyRelative("value"), new GUIContent(" "));
                EditorGUI.PropertyField(position.Slice(0.6f, 0.4f, 1), property.FindPropertyRelative("controlType"), new GUIContent(" "));

                var y = 2;
                void DrawSubParameters(int subParametersCount, int labelsCount)
                {
                    var subParameters = property.FindPropertyRelative("subParameters");
                    var labels = property.FindPropertyRelative("labels");
                    var labelIcons = property.FindPropertyRelative("labelIcons");
                    
                    subParameters.arraySize = subParametersCount;
                    if (subParametersCount > 0)
                    {
                        GUI.Label(position.SliceV(y++), "Puppet Parameters");
                        for (var i = 0; i < subParametersCount; i++)
                        {
                            GUI.Label(position.Slice(0.1f, 0.3f, y), SubParameterLabels[subParametersCount][i]);
                            EditorGUI.PropertyField(position.Slice(0.4f, 0.6f, y++), subParameters.GetArrayElementAtIndex(i));
                        }
                    }
                    
                    labels.arraySize = labelsCount;
                    labelIcons.arraySize = labelsCount;
                    if (labelsCount > 0)
                    {
                        GUI.Label(position.SliceV(y++), "Puppet Labels");
                        for (var i = 0; i < labelsCount; i++)
                        {
                            GUI.Label(position.Slice(0.1f, 0.4f, y), SubParameterLabels[labelsCount][i]);
                            EditorGUI.PropertyField(position.Slice(0.4f, 0.3f, y), labels.GetArrayElementAtIndex(i));
                            EditorGUI.PropertyField(position.Slice(0.7f, 0.3f, y++), labelIcons.GetArrayElementAtIndex(i));
                        }
                    }
                }

                switch ((VRCExpressionsMenu.Control.ControlType)property.FindPropertyRelative("controlType").intValue)
                {
                    case VRCExpressionsMenu.Control.ControlType.Button:
                    case VRCExpressionsMenu.Control.ControlType.Toggle:
                        break;
                    case VRCExpressionsMenu.Control.ControlType.SubMenu:
                        EmoteWizardGUI.PropertyFieldWithGenerate(
                            position.SliceV(y),
                            property.FindPropertyRelative("subMenu"),
                            () =>
                            {
                                var name = property.FindPropertyRelative("path").stringValue;
                                if (string.IsNullOrEmpty(name))
                                {
                                    Debug.LogError("Expression name is required.");
                                    return null;
                                }

                                var relativePath =
                                    $"Expressions/@@@Generated@@@ExprSubmenu_{Path.GetFileName(name)}.anim";
                                return context.EmoteWizardRoot.EnsureAsset<VRCExpressionsMenu>(relativePath);
                            });
                        break;
                    case VRCExpressionsMenu.Control.ControlType.TwoAxisPuppet:
                        DrawSubParameters(2, 4);
                        break;
                    case VRCExpressionsMenu.Control.ControlType.FourAxisPuppet:
                        DrawSubParameters(4, 4);
                        break;
                    case VRCExpressionsMenu.Control.ControlType.RadialPuppet:
                        DrawSubParameters(1, 0);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var lineHeight = 2f;
            switch ((VRCExpressionsMenu.Control.ControlType)property.FindPropertyRelative("controlType").intValue)
            {
                case VRCExpressionsMenu.Control.ControlType.Button:
                case VRCExpressionsMenu.Control.ControlType.Toggle:
                    break;
                case VRCExpressionsMenu.Control.ControlType.SubMenu:
                    lineHeight += 1f;
                    break;
                case VRCExpressionsMenu.Control.ControlType.TwoAxisPuppet:
                    lineHeight += 8f;
                    break;
                case VRCExpressionsMenu.Control.ControlType.FourAxisPuppet:
                    lineHeight += 10f;
                    break;
                case VRCExpressionsMenu.Control.ControlType.RadialPuppet:
                    lineHeight += 2f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return BoxHeight(LineHeight(lineHeight));
        }
        
        public class Context : ContextBase
        {
            public Context() : base(null) { }
            public Context(EmoteWizardRoot emoteWizardRoot) : base(emoteWizardRoot) { }
        }
    }
}