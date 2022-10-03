using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Sources.Impl.Base;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Base
{
    [CustomEditor(typeof(EmoteSourceBase), true)]
    public class EmoteSourceBaseEditor : Editor
    {
        SerializedProperty _serializedOverrideEnabled;
        SerializedProperty _serializedOverrideIndex;
        SerializedProperty _serializedGesture1;
        SerializedProperty _serializedGesture2;
        SerializedProperty _serializedConditions;
        SerializedProperty _serializedClipLeft;
        SerializedProperty _serializedClipRight;
        SerializedProperty _serializedControl;
        const bool EditConditions = true;
        const bool EditAnimations = true;
        const bool AdvancedAnimations = true;

        void OnEnable()
        {
            var serializedItem = serializedObject.FindProperty(nameof(EmoteSourceBase.emote));

            _serializedOverrideEnabled = serializedItem.FindPropertyRelative(nameof(Emote.overrideEnabled));
            _serializedOverrideIndex = serializedItem.FindPropertyRelative(nameof(Emote.overrideIndex));
            _serializedGesture1 = serializedItem.FindPropertyRelative(nameof(Emote.gesture1));
            _serializedGesture2 = serializedItem.FindPropertyRelative(nameof(Emote.gesture2));
            _serializedConditions = serializedItem.FindPropertyRelative(nameof(Emote.conditions));
            _serializedClipLeft = serializedItem.FindPropertyRelative(nameof(Emote.clipLeft));
            _serializedClipRight = serializedItem.FindPropertyRelative(nameof(Emote.clipRight));
            _serializedControl = serializedItem.FindPropertyRelative(nameof(Emote.control));
        }

        public override void OnInspectorGUI()
        {
            var emoteSourceBase = (EmoteSourceBase)target;
            var layer = emoteSourceBase.LayerName;
            var emote = emoteSourceBase.emote;

            if (EditConditions)
            {
                using (new EditorGUI.DisabledScope(!_serializedOverrideEnabled.boolValue))
                {
                    EditorGUILayout.PropertyField(_serializedOverrideIndex);
                }
                // using (new HideLabelsScope())
                {
                    EditorGUILayout.PropertyField(_serializedGesture1);
                    EditorGUILayout.PropertyField(_serializedGesture2);
                }

                // using (context.EmoteConditionDrawerContext().StartContext())
                {
                    EditorGUILayout.PropertyField(_serializedConditions);
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
                    CustomEditorGUILayout.PropertyFieldWithGenerate(_serializedClipLeft,
                        () =>
                        {
                            var relativePath = GeneratedAssetLocator.EmoteStateClipPath(layer, fileName, "Left");
                            return emoteSourceBase.EmoteWizardRoot.EnsureAsset<AnimationClip>(relativePath);
                        });

                    CustomEditorGUILayout.PropertyFieldWithGenerate(_serializedClipRight,
                        () =>
                        {
                            var relativePath = GeneratedAssetLocator.EmoteStateClipPath(layer, fileName, "Right");
                            return emoteSourceBase.EmoteWizardRoot.EnsureAsset<AnimationClip>(relativePath);
                        });
                }
                else
                {
                    CustomEditorGUILayout.PropertyFieldWithGenerate(_serializedClipLeft,
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
                EditorGUILayout.PropertyField(_serializedControl);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}