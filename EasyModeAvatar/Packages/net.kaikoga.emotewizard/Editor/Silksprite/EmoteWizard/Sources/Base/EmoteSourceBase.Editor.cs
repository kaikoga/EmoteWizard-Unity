using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Sources.Impl.Base;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Base
{
    [CustomEditor(typeof(EmoteSourceBase), true)]
    public class EmoteSourceBaseEditor : Editor
    {
        const bool EditConditions = true;
        const bool EditAnimations = true;
        const bool AdvancedAnimations = true;

        public override void OnInspectorGUI()
        {
            var emoteSourceBase = (EmoteSourceBase)target;
            var layer = emoteSourceBase.LayerName;
            var emote = emoteSourceBase.emote;

            var serializedObj = serializedObject.FindProperty(nameof(EmoteSourceBase.emote));

            if (EditConditions)
            {
                using (new EditorGUI.DisabledScope(!serializedObj.FindPropertyRelative(nameof(Emote.overrideEnabled)).boolValue))
                {
                    EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(Emote.overrideIndex)));
                }
                using (new HideLabelsScope())
                {
                    EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(Emote.gesture1)));
                    EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(Emote.gesture2)));
                }

                // using (context.EmoteConditionDrawerContext().StartContext())
                {
                    EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(Emote.conditions)));
                }
            }
            else
            {
                var emoteLabel = emote.ToStateName();
                if (emote.conditions.Count > 0) emoteLabel += " *";

                GUILayout.Label(emoteLabel, new GUIStyle {fontStyle = FontStyle.Bold});
            }

            if (EditAnimations)
            {
                var fileName = emote.ToStateName(true);
                if (AdvancedAnimations)
                {
                    CustomEditorGUILayout.PropertyFieldWithGenerate(serializedObj.FindPropertyRelative(nameof(Emote.clipLeft)),
                        () =>
                        {
                            var relativePath = GeneratedAssetLocator.EmoteStateClipPath(layer, fileName, "Left");
                            return emoteSourceBase.EmoteWizardRoot.EnsureAsset<AnimationClip>(relativePath);
                        });

                    CustomEditorGUILayout.PropertyFieldWithGenerate(serializedObj.FindPropertyRelative(nameof(Emote.clipRight)),
                        () =>
                        {
                            var relativePath = GeneratedAssetLocator.EmoteStateClipPath(layer, fileName, "Right");
                            return emoteSourceBase.EmoteWizardRoot.EnsureAsset<AnimationClip>(relativePath);
                        });
                }
                else
                {
                    CustomEditorGUILayout.PropertyFieldWithGenerate(serializedObj.FindPropertyRelative(nameof(Emote.clipLeft)),
                        "Clip",
                        () =>
                        {
                            var relativePath = GeneratedAssetLocator.EmoteStateClipPath(layer, fileName);
                            return emoteSourceBase.EmoteWizardRoot.EnsureAsset<AnimationClip>(relativePath);
                        });
                }
            }

            // using (context.EmoteParameterDrawerContext().StartContext())
            {
                EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(Emote.control)));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}