using System;
using EmoteWizard.Extensions;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;
using static EmoteWizard.Extensions.PropertyDrawerUITools;

namespace EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(ExpressionItem))]
    public class ExpressionItemDrawer : PropertyDrawer
    {
        static readonly string[][] SubParameterLabels = {
            null,
            new[] { "Rotation" },
            new[] { "Horizontal", "Vertical" },
            null,
            new[] { "Up", "Right", "Down", "Left" }
        };

        public static void DrawHeader(bool useReorderUI)
        {
            var position = GUILayoutUtility.GetRect(0, BoxHeight(LineHeight(2f)));
            GUI.Box(position, GUIContent.none);
            position = position.InsideBox();
            position.xMin += useReorderUI ? 20f : 6f;
            position.xMax -= 6f;
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
                GUI.Label(position.Slice(0.0f, 0.4f, 0), "Icon");
                GUI.Label(position.Slice(0.4f, 0.6f, 0), "Path");
                
                GUI.Label(position.Slice(0.0f, 0.4f, 1), "Parameter");
                GUI.Label(position.Slice(0.4f, 0.2f, 1), "Value");
                GUI.Label(position.Slice(0.6f, 0.4f, 1), "ControlType");
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.Box(position, GUIContent.none);
            position = position.InsideBox();
            using (new EditorGUI.PropertyScope(position, label, property))
            using (new EditorGUI.IndentLevelScope())
            {
                var labelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 1f;
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
                    case VRCExpressionsMenu.Control.ControlType.SubMenu:
                        DrawSubParameters(0, 0);
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

                EditorGUIUtility.labelWidth = labelWidth;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var lineHeight = 2f;
            switch ((VRCExpressionsMenu.Control.ControlType)property.FindPropertyRelative("controlType").intValue)
            {
                case VRCExpressionsMenu.Control.ControlType.Button:
                case VRCExpressionsMenu.Control.ControlType.Toggle:
                case VRCExpressionsMenu.Control.ControlType.SubMenu:
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