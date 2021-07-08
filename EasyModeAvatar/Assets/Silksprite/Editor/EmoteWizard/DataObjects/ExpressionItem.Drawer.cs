using System;
using System.IO;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizardSupport.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;
using static Silksprite.EmoteWizardSupport.Tools.PropertyDrawerUITools;

namespace Silksprite.EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(ExpressionItem))]
    public class ExpressionItemDrawer : PropertyDrawerWithContext<ExpressionItemDrawerContext>
    {
        static readonly string[][] SubParameterLabels = {
            null,
            new[] { "Rotation" },
            new[] { "Horizontal", "Vertical" },
            null,
            new[] { "Up", "Right", "Down", "Left" }
        };

        public static ExpressionItemDrawerContext StartContext(EmoteWizardRoot emoteWizardRoot) => StartContext(new ExpressionItemDrawerContext(emoteWizardRoot));

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var context = EnsureContext(property);

            GUI.Box(position, GUIContent.none);
            position = position.InsideBox();
            using (new UnityEditor.EditorGUI.PropertyScope(position, label, property))
            using (new UnityEditor.EditorGUI.IndentLevelScope())
            using (new HideLabelsScope())
            {
                EditorGUI.PropertyField(position.UISlice(0.0f, 0.4f, 0), property.FindPropertyRelative("icon"), new GUIContent(" "));
                EditorGUI.PropertyField(position.UISlice(0.4f, 0.6f, 0), property.FindPropertyRelative("path"), new GUIContent(" "));

                EditorGUI.PropertyField(position.UISlice(0.0f, 0.4f, 1), property.FindPropertyRelative("parameter"), new GUIContent(" "));
                EditorGUI.PropertyField(position.UISlice(0.4f, 0.2f, 1), property.FindPropertyRelative("value"), new GUIContent(" "));
                EditorGUI.PropertyField(position.UISlice(0.6f, 0.4f, 1), property.FindPropertyRelative("controlType"), new GUIContent(" "));

                var y = 2;
                void DrawSubParameters(int subParametersCount, int labelsCount)
                {
                    var subParameters = property.FindPropertyRelative("subParameters");
                    var labels = property.FindPropertyRelative("labels");
                    var labelIcons = property.FindPropertyRelative("labelIcons");
                    
                    subParameters.arraySize = subParametersCount;
                    if (subParametersCount > 0)
                    {
                        GUI.Label(position.UISliceV(y++), "Puppet Parameters");
                        for (var i = 0; i < subParametersCount; i++)
                        {
                            GUI.Label(position.UISlice(0.1f, 0.3f, y), SubParameterLabels[subParametersCount][i]);
                            EditorGUI.PropertyField(position.UISlice(0.4f, 0.6f, y++), subParameters.GetArrayElementAtIndex(i));
                        }
                    }
                    
                    labels.arraySize = labelsCount;
                    labelIcons.arraySize = labelsCount;
                    if (labelsCount > 0)
                    {
                        GUI.Label(position.UISliceV(y++), "Puppet Labels");
                        for (var i = 0; i < labelsCount; i++)
                        {
                            GUI.Label(position.UISlice(0.1f, 0.4f, y), SubParameterLabels[labelsCount][i]);
                            EditorGUI.PropertyField(position.UISlice(0.4f, 0.3f, y), labels.GetArrayElementAtIndex(i));
                            EditorGUI.PropertyField(position.UISlice(0.7f, 0.3f, y++), labelIcons.GetArrayElementAtIndex(i));
                        }
                    }
                }

                switch ((VRCExpressionsMenu.Control.ControlType)property.FindPropertyRelative("controlType").intValue)
                {
                    case VRCExpressionsMenu.Control.ControlType.Button:
                    case VRCExpressionsMenu.Control.ControlType.Toggle:
                        break;
                    case VRCExpressionsMenu.Control.ControlType.SubMenu:
                        CustomEditorGUI.PropertyFieldWithGenerate(
                            position.UISliceV(y),
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
    }
}