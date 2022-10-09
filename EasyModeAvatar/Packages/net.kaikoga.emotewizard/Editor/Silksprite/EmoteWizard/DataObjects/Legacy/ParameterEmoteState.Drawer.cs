using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.PropertyDrawerUITools;

namespace Silksprite.EmoteWizard.DataObjects.Legacy
{
    [CustomPropertyDrawer(typeof(ParameterEmoteState))]
    public class ParameterEmoteStateDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label)
        {
            var serializedEnabled = serializedProperty.FindPropertyRelative(nameof(ParameterEmoteState.enabled));
            var serializedValue = serializedProperty.FindPropertyRelative(nameof(ParameterEmoteState.value));
            var serializedClip = serializedProperty.FindPropertyRelative(nameof(ParameterEmoteState.clip));
            var serializedTargets = serializedProperty.FindPropertyRelative(nameof(ParameterEmoteState.targets));
            var serializedControl = serializedProperty.FindPropertyRelative(nameof(ParameterEmoteState.control));

            const bool EditTargets = true;
            using (new EditorGUI.IndentLevelScope())
            {
                using (new HideLabelsScope())
                {
                    EditorGUI.PropertyField(position.UISlice(0.0f, 0.1f, 0), serializedEnabled);
                    EditorGUI.BeginDisabledGroup(!serializedEnabled.boolValue);
                    EditorGUI.PropertyField(position.UISlice(0.1f, 0.2f, 0), serializedValue);
                    var value = serializedValue.floatValue;
                    CustomEditorGUI.PropertyFieldWithGenerate(position.UISlice(0.3f, 0.7f, 0),
                        serializedClip,
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
                    var height = TargetListHeight(serializedTargets);
                    EditorGUI.PropertyField(position.SliceV(0f, height), serializedTargets, true);
                    position = position.InsetTop(height + EditorGUIUtility.standardVerticalSpacing);
                }
                // using (var sub = context.EmoteControlDrawerContext().StartContext())
                {
                    EditorGUI.PropertyField(position, serializedControl, true);
                }
                EditorGUI.EndDisabledGroup();
            }
        }

        public override float GetPropertyHeight(SerializedProperty serializedProperty, GUIContent label)
        {
            var serializedTargets = serializedProperty.FindPropertyRelative(nameof(ParameterEmoteState.targets));
            var serializedControl = serializedProperty.FindPropertyRelative(nameof(ParameterEmoteState.control));

            const bool EditTargets = true;

            // using (context.EmoteControlDrawerContext().StartContext())
            {
                var height = LineHeight(1f);
                if (EditTargets)
                {
                    height += TargetListHeight(serializedTargets) + EditorGUIUtility.standardVerticalSpacing;
                }

                height += EditorGUI.GetPropertyHeight(serializedControl, true) + EditorGUIUtility.standardVerticalSpacing;
                return height;
            }
        }

        const int MinimumLargeTargetList = 4;
        static float TargetListHeight(SerializedProperty serializedTargets)
        {
            return EditorGUI.GetPropertyHeight(serializedTargets, true);
        }
    }
}