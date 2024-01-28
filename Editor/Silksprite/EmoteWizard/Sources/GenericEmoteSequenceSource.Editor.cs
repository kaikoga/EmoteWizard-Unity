using System;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Sources.Sequence;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.ClipBuilder;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;
using Object = System.Object;

namespace Silksprite.EmoteWizard.Sources
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GenericEmoteSequenceSource))]
    public class GenericEmoteSequenceSourceEditor : EmoteWizardEditorBase<GenericEmoteSequenceSource>
    {
        LocalizedProperty _layerKind;
        LocalizedProperty _groupName;

        LocalizedProperty _animatedEnable;
        LocalizedProperty _animatedBlendShapes;

        LocalizedProperty _isFixedDuration;
        LocalizedProperty _entryTransitionDuration;
        LocalizedProperty _exitTransitionDuration;

        LocalizedProperty _hasLayerBlend;
        LocalizedProperty _blendIn;
        LocalizedProperty _blendOut;

        LocalizedProperty _hasTrackingOverrides;
        LocalizedProperty _trackingOverrides;

        AnimationClip _inputClip;

        AnimationPreview _preview;

        AnimationClip _temporaryClip;

        void OnEnable()
        {
            var serializedItem = Lop(nameof(GenericEmoteSequenceSource.sequence), Loc("GenericEmoteSequenceSource::sequence"));

            _layerKind = serializedItem.Lop(nameof(GenericEmoteSequence.layerKind), Loc("GenericEmoteSequenceSource::layerKind"));
            _groupName = serializedItem.Lop(nameof(GenericEmoteSequence.groupName), Loc("GenericEmoteSequenceSource::groupName"));

            _animatedEnable = serializedItem.Lop(nameof(GenericEmoteSequence.animatedEnable), Loc("GenericEmoteSequenceSource::animatedEnable"));
            _animatedBlendShapes = serializedItem.Lop(nameof(GenericEmoteSequence.animatedBlendShapes), Loc("GenericEmoteSequenceSource::animatedBlendShapes"));

            _isFixedDuration = serializedItem.Lop(nameof(GenericEmoteSequence.isFixedDuration), Loc("GenericEmoteSequenceSource::isFixedDuration"));
            _entryTransitionDuration = serializedItem.Lop(nameof(GenericEmoteSequence.entryTransitionDuration), Loc("GenericEmoteSequenceSource::entryTransitionDuration"));
            _exitTransitionDuration = serializedItem.Lop(nameof(GenericEmoteSequence.exitTransitionDuration), Loc("GenericEmoteSequenceSource::exitTransitionDuration"));
            
            _hasLayerBlend = serializedItem.Lop(nameof(GenericEmoteSequence.hasLayerBlend), Loc("GenericEmoteSequenceSource::hasLayerBlend"));
            _blendIn = serializedItem.Lop(nameof(GenericEmoteSequence.blendIn), Loc("GenericEmoteSequenceSource::blendIn"));
            _blendOut = serializedItem.Lop(nameof(GenericEmoteSequence.blendOut), Loc("GenericEmoteSequenceSource::blendOut"));

            _hasTrackingOverrides = serializedItem.Lop(nameof(GenericEmoteSequence.hasTrackingOverrides), Loc("GenericEmoteSequenceSource::hasTrackingOverrides"));
            _trackingOverrides = serializedItem.Lop(nameof(GenericEmoteSequence.trackingOverrides), Loc("GenericEmoteSequenceSource::trackingOverrides"));

            var environment = CreateEnv();
            if (environment?.AvatarDescriptor)
            {
                // TODO: prevent multiple previews
                _preview = new AnimationPreview(environment.AvatarDescriptor);
                RefreshPreviewIfNeeded(environment);
            }
        }

        void RefreshPreviewIfNeeded(EmoteWizardEnvironment environment)
        {
            if (_preview == null) return;

            if (_temporaryClip) DestroyImmediate(_temporaryClip);
            _temporaryClip = (AnimationClip)soleTarget.ToEmoteFactoryTemplate().Build(environment, new ClipBuilderImpl()).clip;
            _preview.Clip = _temporaryClip;
            _preview.Refresh();
        }

        void OnDisable()
        {
            _preview?.Dispose();
            if (_temporaryClip) DestroyImmediate(_temporaryClip);
            _temporaryClip = null;
        }

        protected override void OnInnerInspectorGUI()
        {
            EmoteWizardGUILayout.Prop(_layerKind);
            EmoteWizardGUILayout.Prop(_groupName);

            EmoteWizardGUILayout.Header(Loc("GenericEmoteSequence::Common Settings"));
            EmoteWizardGUILayout.Prop(_isFixedDuration);
            EmoteWizardGUILayout.Prop(_entryTransitionDuration);
            EmoteWizardGUILayout.Prop(_exitTransitionDuration);

            EmoteWizardGUILayout.Header(Loc("GenericEmoteSequence::Layer Blend"));
            EmoteWizardGUILayout.PropertyFoldout(_hasLayerBlend, () =>
            {
                EmoteWizardGUILayout.Prop(_blendIn);
                EmoteWizardGUILayout.Prop(_blendOut);
            });

            EmoteWizardGUILayout.Header(Loc("GenericEmoteSequence::Tracking Overrides"));
            EmoteWizardGUILayout.PropertyFoldout(_hasTrackingOverrides, () =>
            {
                EmoteWizardGUILayout.Prop(_trackingOverrides);
            });

            EmoteWizardGUILayout.Header(Loc("GenericEmoteSequence::Animation"));
            EditorGUI.BeginChangeCheck();
            EmoteWizardGUILayout.Prop(_animatedEnable);
            EmoteWizardGUILayout.Prop(_animatedBlendShapes);
            var requireRefreshPreview = EditorGUI.EndChangeCheck();
            
            serializedObject.ApplyModifiedProperties();

            using (new GUILayout.HorizontalScope())
            {
                _inputClip = EmoteWizardGUILayout.ObjectField(Loc("GenericEmoteSequenceSource::Clip"), _inputClip, true);
                if (EmoteWizardGUILayout.Button(Loc("GenericEmoteSequenceSource::Import"), Array.Empty<GUILayoutOption>()))
                {
                    var converted = _inputClip.ToGenericEmoteSequence(CreateEnv().AvatarDescriptor.gameObject);
                    soleTarget.sequence.animatedEnable = converted.animatedEnable;
                    soleTarget.sequence.animatedBlendShapes = converted.animatedBlendShapes;
                }
            }

            if (requireRefreshPreview) RefreshPreviewIfNeeded(CreateEnv());
            _preview?.OnInspectorGUI();

            if (EmoteWizardGUILayout.Undoable(Loc("GenericEmoteSequenceSource::Explode"), "Explode Generic Emote Sequence source") is IUndoable undoable)
            {
                SourceExploder.ExplodeEmoteSequencesImmediate(undoable, soleTarget);
            }
        }

        [CustomPropertyDrawer(typeof(GenericEmoteSequence.AnimatedEnable))]
        public class AnimatedEnableDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label)
            {
                using (new LabelWidthScope(100f))
                {
                    var target = serializedProperty.Lop(nameof(GenericEmoteSequence.AnimatedEnable.target), Loc("AnimatedEnable::target"));
                    var isEnable = serializedProperty.Lop(nameof(GenericEmoteSequence.AnimatedEnable.isEnable), Loc("AnimatedEnable::isEnable"));

                    EditorGUI.PropertyField(position.UISliceV(0), target.Property, target.GUIContent);
                    EditorGUI.PropertyField(position.UISliceV(1), isEnable.Property, isEnable.GUIContent);
                }
            }

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing * 1;
            }
        }

        [CustomPropertyDrawer(typeof(GenericEmoteSequence.AnimatedBlendShape))]
        public class AnimatedBlendShapeDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label)
            {
                using (new LabelWidthScope(100f))
                {
                    var target = serializedProperty.Lop(nameof(GenericEmoteSequence.AnimatedBlendShape.target),
                        Loc("AnimatedBlendShape::target"));
                    var blendShapeName = serializedProperty.Lop(
                        nameof(GenericEmoteSequence.AnimatedBlendShape.blendShapeName),
                        Loc("AnimatedBlendShape::blendShapeName"));
                    var value = serializedProperty.Lop(nameof(GenericEmoteSequence.AnimatedBlendShape.value),
                        Loc("AnimatedBlendShape::value"));

                    EditorGUI.PropertyField(position.UISliceV(0), target.Property, target.GUIContent);

                    var skinnedMeshRenderer = (SkinnedMeshRenderer)target.Property.objectReferenceValue;
                    if (skinnedMeshRenderer && skinnedMeshRenderer.sharedMesh is Mesh sharedMesh)
                    {
                        EditorGUI.BeginChangeCheck();
                        var options = Enumerable.Range(0, sharedMesh.blendShapeCount)
                            .Select(i => sharedMesh.GetBlendShapeName(i))
                            .ToArray();
                        var newBlendShapeNameValue = EditorGUI.Popup(
                            position.UISliceV(1),
                            blendShapeName.Loc.Tr,
                            Array.IndexOf(options, blendShapeName.Property.stringValue),
                            options
                        );
                        if (EditorGUI.EndChangeCheck())
                        {
                            blendShapeName.Property.stringValue = options[newBlendShapeNameValue];
                        }
                    }

                    EditorGUI.PropertyField(position.UISliceV(2), value.Property, value.GUIContent);
                }
            }

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                return EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing * 2;
            }
        }
    }
}
