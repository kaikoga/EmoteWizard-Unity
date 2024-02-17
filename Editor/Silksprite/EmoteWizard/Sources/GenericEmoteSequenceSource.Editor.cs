using System;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Sources.Sequence;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.ClipBuilder;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;

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

        LocalizedProperty _hasTimeParameter;
        LocalizedProperty _timeParameter;

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

            _hasTimeParameter = serializedItem.Lop(nameof(EmoteSequence.hasTimeParameter), Loc("EmoteSequence::hasTimeParameter"));
            _timeParameter = serializedItem.Lop(nameof(EmoteSequence.timeParameter), Loc("EmoteSequence::timeParameter"));

            _hasLayerBlend = serializedItem.Lop(nameof(GenericEmoteSequence.hasLayerBlend), Loc("GenericEmoteSequenceSource::hasLayerBlend"));
            _blendIn = serializedItem.Lop(nameof(GenericEmoteSequence.blendIn), Loc("GenericEmoteSequenceSource::blendIn"));
            _blendOut = serializedItem.Lop(nameof(GenericEmoteSequence.blendOut), Loc("GenericEmoteSequenceSource::blendOut"));

            _hasTrackingOverrides = serializedItem.Lop(nameof(GenericEmoteSequence.hasTrackingOverrides), Loc("GenericEmoteSequenceSource::hasTrackingOverrides"));
            _trackingOverrides = serializedItem.Lop(nameof(GenericEmoteSequence.trackingOverrides), Loc("GenericEmoteSequenceSource::trackingOverrides"));

            var environment = CreateEnv();
            if (environment?.AvatarRoot)
            {
                // TODO: prevent multiple previews
                _preview = new AnimationPreview(environment.AvatarRoot.gameObject);
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
            var environment = CreateEnv();

            if (environment.Platform.IsVRChat())
            {
                EmoteWizardGUILayout.Prop(_layerKind);
                EmoteWizardGUILayout.Prop(_groupName);

                EmoteWizardGUILayout.Header(Loc("GenericEmoteSequence::Common Settings"));
                EmoteWizardGUILayout.Prop(_isFixedDuration);
                EmoteWizardGUILayout.Prop(_entryTransitionDuration);
                EmoteWizardGUILayout.Prop(_exitTransitionDuration);

                EmoteWizardGUILayout.Header(Loc("GenericEmoteSequence::Time Parameter"));
                EmoteWizardGUILayout.PropertyFoldout(_hasTimeParameter, () =>
                {
                    EmoteWizardGUILayout.Prop(_timeParameter);
                });

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
            }

            EmoteWizardGUILayout.Header(Loc("GenericEmoteSequence::Animation"));
            EditorGUI.BeginChangeCheck();
            if (environment.Platform.IsVRChat())
            {
                EmoteWizardGUILayout.Prop(_animatedEnable);
            }

            EmoteWizardGUILayout.Prop(_animatedBlendShapes);
            var requireRefreshPreview = EditorGUI.EndChangeCheck();
            
            serializedObject.ApplyModifiedProperties();

            using (new GUILayout.HorizontalScope())
            {
                _inputClip = EmoteWizardGUILayout.ObjectField(Loc("GenericEmoteSequenceSource::Clip"), _inputClip, true);
                if (EmoteWizardGUILayout.Button(Loc("GenericEmoteSequenceSource::Import"), Array.Empty<GUILayoutOption>()))
                {
                    var converted = _inputClip.ToGenericEmoteSequence(CreateEnv().AvatarRoot.gameObject);
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
    }
}
