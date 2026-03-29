using System;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Preview;
using Silksprite.EmoteWizard.Sources.Sequence;
using Silksprite.EmoteWizardSupport.ClipBuilder;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.Loch;
using Silksprite.Loch.Extensions;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using UnityEngine;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard.Sources
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GenericEmoteSequenceSource))]
    public class GenericEmoteSequenceSourceEditor : EmoteWizardEditorBase<GenericEmoteSequenceSource>
    {
        LocalizedProperty _layerKind = null!;
        LocalizedProperty _groupName = null!;

        LocalizedProperty _animatedEnable = null!;
        LocalizedProperty _animatedBlendShapes = null!;

        LocalizedProperty _isFixedDuration = null!;
        LocalizedProperty _entryTransitionDuration = null!;
        LocalizedProperty _exitTransitionDuration = null!;

        LocalizedProperty _hasTimeParameter = null!;
        LocalizedProperty _timeParameter = null!;

        LocalizedProperty _hasLayerBlend = null!;
        LocalizedProperty _blendIn = null!;
        LocalizedProperty _blendOut = null!;

        LocalizedProperty _hasTrackingOverrides = null!;
        LocalizedProperty _trackingOverrides = null!;

        AnimationClip? _inputClip;

        IAnimationPreviewWrapper _previewWrapper = null!;
        AnimationClip? _temporaryClip;

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

            _hasTimeParameter = serializedItem.Lop(nameof(EmoteSequence.hasTimeParameter), Loc("EmoteSequence::hasTimeParameter"));
            _timeParameter = serializedItem.Lop(nameof(EmoteSequence.timeParameter), Loc("EmoteSequence::timeParameter"));

            _hasLayerBlend = serializedItem.Lop(nameof(GenericEmoteSequence.hasLayerBlend), Loc("GenericEmoteSequenceSource::hasLayerBlend"));
            _blendIn = serializedItem.Lop(nameof(GenericEmoteSequence.blendIn), Loc("GenericEmoteSequenceSource::blendIn"));
            _blendOut = serializedItem.Lop(nameof(GenericEmoteSequence.blendOut), Loc("GenericEmoteSequenceSource::blendOut"));

            _hasTrackingOverrides = serializedItem.Lop(nameof(GenericEmoteSequence.hasTrackingOverrides), Loc("GenericEmoteSequenceSource::hasTrackingOverrides"));
            _trackingOverrides = serializedItem.Lop(nameof(GenericEmoteSequence.trackingOverrides), Loc("GenericEmoteSequenceSource::trackingOverrides"));

            var environment = CachedEnv();
            if (environment.AvatarRoot)
            {
                _previewWrapper = AnimationPreviewWrapper.Create(environment.AvatarRoot.gameObject);
                RefreshPreviewIfNeeded(environment);
            }
            else
            {
                _previewWrapper = AnimationPreviewWrapper.Null;
            }
        }

        void RefreshPreviewIfNeeded(EmoteWizardEnvironment environment)
        {
            if (_previewWrapper.IsBlocked)
            {
                return;
            }

            if (_temporaryClip != null) DestroyImmediate(_temporaryClip);
            _temporaryClip = (AnimationClip?)soleTarget.ToEmoteFactoryTemplate().Build(environment, new ClipBuilderImpl()).clip;
            _previewWrapper.RefreshPreview(_temporaryClip);
        }

        void OnDisable()
        {
            _previewWrapper.Dispose();
            if (_temporaryClip) DestroyImmediate(_temporaryClip);
            _temporaryClip = null;
        }

        protected override void OnInnerInspectorGUI()
        {
            var environment = CachedEnv();

            if (environment.MaybeUnityPlatforms())
            {
                LEditorGUILayout.PropAsEnumPopup<LayerKind>(_layerKind);
                LEditorGUILayout.Prop(_groupName);

                LGUILayout.Heading(Loc("GenericEmoteSequence::Common Settings"));
                LEditorGUILayout.Prop(_isFixedDuration);
                LEditorGUILayout.Prop(_entryTransitionDuration);
                LEditorGUILayout.Prop(_exitTransitionDuration);

                LGUILayout.Heading(Loc("GenericEmoteSequence::Time Parameter"));
                LEditorGUILayout.PropAsFoldout(_hasTimeParameter, () =>
                {
                    LEditorGUILayout.Prop(_timeParameter);
                });

                LGUILayout.Heading(Loc("GenericEmoteSequence::Layer Blend"));
                LEditorGUILayout.PropAsFoldout(_hasLayerBlend, () =>
                {
                    LEditorGUILayout.Prop(_blendIn);
                    LEditorGUILayout.Prop(_blendOut);
                });

                LGUILayout.Heading(Loc("GenericEmoteSequence::Tracking Overrides"));
                LEditorGUILayout.PropAsFoldout(_hasTrackingOverrides, () =>
                {
                    LEditorGUILayout.Prop(_trackingOverrides);
                });
            }

            LGUILayout.Heading(Loc("GenericEmoteSequence::Animation"));
            EditorGUI.BeginChangeCheck();
            if (environment.MaybeUnityPlatforms())
            {
                LEditorGUILayout.Prop(_animatedEnable);
            }

            LEditorGUILayout.Prop(_animatedBlendShapes);
            var requireRefreshPreview = EditorGUI.EndChangeCheck();
            
            serializedObject.ApplyModifiedProperties();

            using (new GUILayout.HorizontalScope())
            {
                _inputClip = LEditorGUILayout.ObjectField(Loc("GenericEmoteSequenceSource::Clip"), _inputClip, true);
                if (LGUILayout.Button(Loc("GenericEmoteSequenceSource::Import"), Array.Empty<GUILayoutOption>()))
                {
                    var converted = _inputClip.ToGenericEmoteSequence(CachedEnv().AvatarRoot.gameObject);
                    soleTarget.sequence.animatedEnable = converted.animatedEnable;
                    soleTarget.sequence.animatedBlendShapes = converted.animatedBlendShapes;
                }
            }

            if (requireRefreshPreview) RefreshPreviewIfNeeded(CachedEnv());
            _previewWrapper.OnInspectorGUI();

            if (EmoteWizardGUILayout.Undoable(Loc("GenericEmoteSequenceSource::Explode"), "Explode Generic Emote Sequence source", out var undoable))
            {
                string GetExplodePath(GenericEmoteSequenceSource source) => string.IsNullOrWhiteSpace(source.gameObject.name)
                    ? "Assets/EW_GeneratedClip.anim"
                    : $"Assets/EW_GeneratedClip_{source.gameObject.name}.anim";

                soleTarget.ExplodeEmoteSequencesImmediate(undoable, new ClipBuilderImpl(GetExplodePath(soleTarget)));
            }
        }
    }
}
