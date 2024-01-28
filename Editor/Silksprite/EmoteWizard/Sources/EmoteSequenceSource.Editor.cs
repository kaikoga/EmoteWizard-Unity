using System;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Sequence;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;

namespace Silksprite.EmoteWizard.Sources
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(EmoteSequenceSource))]
    public class EmoteSequenceSourceEditor : EmoteWizardEditorBase<EmoteSequenceSource>
    {
        LocalizedProperty _layerKind;
        LocalizedProperty _groupName;

        LocalizedProperty _isFixedDuration;

        LocalizedProperty _clip;
        LocalizedProperty _entryTransitionDuration;
        LocalizedProperty _exitTransitionDuration;

        LocalizedProperty _hasExitTime;
        LocalizedProperty _clipExitTime;

        LocalizedProperty _hasTimeParameter;
        LocalizedProperty _timeParameter;

        LocalizedProperty _hasEntryClip;
        LocalizedProperty _entryClip;
        LocalizedProperty _entryClipExitTime;
        LocalizedProperty _postEntryTransitionDuration;

        LocalizedProperty _hasExitClip;
        LocalizedProperty _exitClip;
        LocalizedProperty _exitClipExitTime;
        LocalizedProperty _postExitTransitionDuration;

        LocalizedProperty _serializedHasLayerBlend;
        LocalizedProperty _serializedBlendIn;
        LocalizedProperty _serializedBlendOut;

        LocalizedProperty _serializedHasTrackingOverrides;
        LocalizedProperty _serializedTrackingOverrides;

        AnimationPreview _preview;

        void OnEnable()
        {
            var serializedItem = Lop(nameof(EmoteSequenceSource.sequence), Loc("EmoteSequenceSource::sequence"));

            _layerKind = serializedItem.Lop(nameof(EmoteSequence.layerKind), Loc("EmoteSequence::layerKind"));
            _groupName = serializedItem.Lop(nameof(EmoteSequence.groupName), Loc("EmoteSequence::groupName"));

            _isFixedDuration = serializedItem.Lop(nameof(EmoteSequence.isFixedDuration), Loc("EmoteSequence::isFixedDuration"));
            
            _clip = serializedItem.Lop(nameof(EmoteSequence.clip), Loc("EmoteSequence::clip"));
            _entryTransitionDuration = serializedItem.Lop(nameof(EmoteSequence.entryTransitionDuration), Loc("EmoteSequence::entryTransitionDuration"));
            _exitTransitionDuration = serializedItem.Lop(nameof(EmoteSequence.exitTransitionDuration), Loc("EmoteSequence::exitTransitionDuration"));
            
            _hasExitTime = serializedItem.Lop(nameof(EmoteSequence.hasExitTime), Loc("EmoteSequence::hasExitTime"));
            _clipExitTime = serializedItem.Lop(nameof(EmoteSequence.clipExitTime), Loc("EmoteSequence::clipExitTime"));

            _hasTimeParameter = serializedItem.Lop(nameof(EmoteSequence.hasTimeParameter), Loc("EmoteSequence::hasTimeParameter"));
            _timeParameter = serializedItem.Lop(nameof(EmoteSequence.timeParameter), Loc("EmoteSequence::timeParameter"));

            _hasEntryClip = serializedItem.Lop(nameof(EmoteSequence.hasEntryClip), Loc("EmoteSequence::hasEntryClip"));
            _entryClip = serializedItem.Lop(nameof(EmoteSequence.entryClip), Loc("EmoteSequence::entryClip"));
            _entryClipExitTime = serializedItem.Lop(nameof(EmoteSequence.entryClipExitTime), Loc("EmoteSequence::entryClipExitTime"));
            _postEntryTransitionDuration = serializedItem.Lop(nameof(EmoteSequence.postEntryTransitionDuration), Loc("EmoteSequence::postEntryTransitionDuration"));

            _hasExitClip = serializedItem.Lop(nameof(EmoteSequence.hasExitClip), Loc("EmoteSequence::hasExitClip"));
            _exitClip = serializedItem.Lop(nameof(EmoteSequence.exitClip), Loc("EmoteSequence::exitClip"));
            _exitClipExitTime = serializedItem.Lop(nameof(EmoteSequence.exitClipExitTime), Loc("EmoteSequence::exitClipExitTime"));
            _postExitTransitionDuration = serializedItem.Lop(nameof(EmoteSequence.postExitTransitionDuration), Loc("EmoteSequence::postExitTransitionDuration"));

            _serializedHasLayerBlend = serializedItem.Lop(nameof(EmoteSequence.hasLayerBlend), Loc("EmoteSequence::hasLayerBlend"));
            _serializedBlendIn = serializedItem.Lop(nameof(EmoteSequence.blendIn), Loc("EmoteSequence::blendIn"));
            _serializedBlendOut = serializedItem.Lop(nameof(EmoteSequence.blendOut), Loc("EmoteSequence::blendOut"));

            _serializedHasTrackingOverrides = serializedItem.Lop(nameof(EmoteSequence.hasTrackingOverrides), Loc("EmoteSequence::hasTrackingOverrides"));
            _serializedTrackingOverrides = serializedItem.Lop(nameof(EmoteSequence.trackingOverrides), Loc("EmoteSequence::trackingOverrides"));

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

            var clip = _clip.Property.objectReferenceValue as AnimationClip;
            _preview.Clip = clip;
            _preview.Refresh();
        }

        void OnDisable()
        {
            _preview?.Dispose();
        }

        protected override void OnInnerInspectorGUI()
        {
            EmoteWizardGUILayout.Prop(_layerKind);
            EmoteWizardGUILayout.Prop(_groupName);

            using (new LabelWidthScope(200f))
            {
                EmoteWizardGUILayout.Header(Loc("EmoteSequence::Common Settings"));
                EmoteWizardGUILayout.Prop(_isFixedDuration);
                EditorGUI.BeginChangeCheck();
                EmoteWizardGUILayout.Prop(_clip);
                var requireRefreshPreview = EditorGUI.EndChangeCheck();
                EmoteWizardGUILayout.Prop(_entryTransitionDuration);
                EmoteWizardGUILayout.Prop(_exitTransitionDuration);

                EmoteWizardGUILayout.Header(Loc("EmoteSequence::Exit Time"));
                EmoteWizardGUILayout.PropertyFoldout(_hasExitTime, () =>
                {
                    EmoteWizardGUILayout.Prop(_clipExitTime);
                });

                EmoteWizardGUILayout.Header(Loc("EmoteSequence::Time Parameter"));
                EmoteWizardGUILayout.PropertyFoldout(_hasTimeParameter, () =>
                {
                    EmoteWizardGUILayout.Prop(_timeParameter);
                });

                EmoteWizardGUILayout.Header(Loc("EmoteSequence::Entry Clip"));
                EmoteWizardGUILayout.PropertyFoldout(_hasEntryClip, () =>
                {
                    EmoteWizardGUILayout.Prop(_entryClip);
                    EmoteWizardGUILayout.Prop(_entryClipExitTime);
                    EmoteWizardGUILayout.Prop(_postEntryTransitionDuration);
                });

                EmoteWizardGUILayout.Header(Loc("EmoteSequence::Exit Clip"));
                EmoteWizardGUILayout.PropertyFoldout(_hasExitClip, () =>
                {
                    EmoteWizardGUILayout.Prop(_exitClip);
                    EmoteWizardGUILayout.Prop(_exitClipExitTime);
                    EmoteWizardGUILayout.Prop(_postExitTransitionDuration);
                });

                EmoteWizardGUILayout.Header(Loc("EmoteSequence::Layer Blend"));
                EmoteWizardGUILayout.PropertyFoldout(_serializedHasLayerBlend, () =>
                {
                    EmoteWizardGUILayout.Prop(_serializedBlendIn);
                    EmoteWizardGUILayout.Prop(_serializedBlendOut);
                });

                EmoteWizardGUILayout.Header(Loc("EmoteSequence::Tracking Overrides"));
                EmoteWizardGUILayout.PropertyFoldout(_serializedHasTrackingOverrides, () =>
                {
                    EmoteWizardGUILayout.Prop(_serializedTrackingOverrides);
                });

                serializedObject.ApplyModifiedProperties();

                if (requireRefreshPreview) RefreshPreviewIfNeeded(CreateEnv());
                _preview?.OnInspectorGUI();
            }
        }
    }
}
