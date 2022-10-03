using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.PropertyDrawerUITools;

namespace Silksprite.EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(ParameterEmoteState))]
    public class ParameterEmoteStateDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label)
        {
            const bool EditTargets = true;
            using (new EditorGUI.IndentLevelScope())
            {
                using (new HideLabelsScope())
                {
                    EditorGUI.PropertyField(position.UISlice(0.0f, 0.1f, 0), serializedProperty.FindPropertyRelative(nameof(ParameterEmoteState.enabled)));
                    EditorGUI.BeginDisabledGroup(!serializedProperty.FindPropertyRelative(nameof(ParameterEmoteState.enabled)).boolValue);
                    EditorGUI.PropertyField(position.UISlice(0.1f, 0.2f, 0), serializedProperty.FindPropertyRelative(nameof(ParameterEmoteState.value)));
                    var value = serializedProperty.FindPropertyRelative(nameof(ParameterEmoteState.value)).floatValue;
                    CustomEditorGUI.PropertyFieldWithGenerate(position.UISlice(0.3f, 0.7f, 0),
                        serializedProperty.FindPropertyRelative(nameof(ParameterEmoteState.clip)),
                        () =>
                        {
                            string contextName = null;
                            if (string.IsNullOrEmpty(contextName))
                            {
                                Debug.LogError("Emote Name is required.");
                                return null;
                            }

                            // var relativePath = GeneratedAssetLocator.ParameterEmoteStateClipPath(context.Layer, contextName, value);
                            // return context.EmoteWizardRoot.EnsureAsset<AnimationClip>(relativePath);
                            return null;
                        });
                }

                position = position.UISliceV(1);
                if (EditTargets)
                {
                    var height = TargetListHeight(serializedProperty);
                    EditorGUI.PropertyField(position.SliceV(0f, height), serializedProperty.FindPropertyRelative(nameof(ParameterEmoteState.targets)));
                    position = position.InsetTop(height + EditorGUIUtility.standardVerticalSpacing);
                }
                // using (var sub = context.EmoteControlDrawerContext().StartContext())
                {
                    EditorGUI.PropertyField(position, serializedProperty.FindPropertyRelative(nameof(ParameterEmoteState.control)));
                }
                EditorGUI.EndDisabledGroup();
            }
        }

        public override float GetPropertyHeight(SerializedProperty serializedProperty, GUIContent label)
        {
            const bool EditTargets = true;

            // using (context.EmoteControlDrawerContext().StartContext())
            {
                var height = LineHeight(1f);
                if (EditTargets)
                {
                    height += TargetListHeight(serializedProperty) + EditorGUIUtility.standardVerticalSpacing;
                }
                height += EditorGUI.GetPropertyHeight(serializedProperty.FindPropertyRelative(nameof(ParameterEmoteState.control))) + EditorGUIUtility.standardVerticalSpacing;
                return height;
            }
        }

        const int MinimumLargeTargetList = 4;
        static float TargetListHeight(SerializedProperty serializedProperty)
        {
            return EditorGUI.GetPropertyHeight(serializedProperty.FindPropertyRelative(nameof(ParameterEmoteState.targets)));
        }
    }
}