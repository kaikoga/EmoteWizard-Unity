using System;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Preview;
using Silksprite.EmoteWizard.Sources.Sequence;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.Loch;
using Silksprite.Loch.Extensions;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using UnityEngine;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard.Sources
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(EmoteSequenceSource))]
    public class EmoteSequenceSourceEditor : EmoteWizardEditorBase<EmoteSequenceSource>
    {
        protected override DetectedPlatform SupportedPlatforms => DetectedPlatform.VRChat;

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

        IAnimationPreviewWrapper _previewWrapper;

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
            if (environment?.AvatarRoot)
            {
                _previewWrapper = AnimationPreviewWrapper.Create(environment.AvatarRoot.gameObject);
                RefreshPreviewIfNeeded();
            }
            else
            {
                _previewWrapper = AnimationPreviewWrapper.Null;
            }
        }

        void RefreshPreviewIfNeeded()
        {
            if (_previewWrapper.IsBlocked)
            {
                return;
            }

            var clip = _clip.Property.objectReferenceValue as AnimationClip;
            _previewWrapper.RefreshPreview(clip);
        }

        void OnDisable()
        {
            _previewWrapper.Dispose();
        }

        protected override void OnInnerInspectorGUI()
        {
            LEditorGUILayout.Prop(_layerKind);
            LEditorGUILayout.Prop(_groupName);

            using (new LabelWidthScope(200f))
            {
                LGUILayout.Heading(Loc("EmoteSequence::Common Settings"));
                LEditorGUILayout.Prop(_isFixedDuration);
                EditorGUI.BeginChangeCheck();
                LEditorGUILayout.Prop(_clip);
                var requireRefreshPreview = EditorGUI.EndChangeCheck();
                LEditorGUILayout.Prop(_entryTransitionDuration);
                LEditorGUILayout.Prop(_exitTransitionDuration);

                LGUILayout.Heading(Loc("EmoteSequence::Exit Time"));
                Action content = () =>
                {
                    LEditorGUILayout.Prop(_clipExitTime);
                };
                LEditorGUILayout.PropAsFoldout(_hasExitTime, content);

                LGUILayout.Heading(Loc("EmoteSequence::Time Parameter"));
                Action content1 = () =>
                {
                    LEditorGUILayout.Prop(_timeParameter);
                };
                LEditorGUILayout.PropAsFoldout(_hasTimeParameter, content1);

                LGUILayout.Heading(Loc("EmoteSequence::Entry Clip"));
                Action content2 = () =>
                {
                    LEditorGUILayout.Prop(_entryClip);
                    LEditorGUILayout.Prop(_entryClipExitTime);
                    LEditorGUILayout.Prop(_postEntryTransitionDuration);
                };
                LEditorGUILayout.PropAsFoldout(_hasEntryClip, content2);

                LGUILayout.Heading(Loc("EmoteSequence::Exit Clip"));
                Action content3 = () =>
                {
                    LEditorGUILayout.Prop(_exitClip);
                    LEditorGUILayout.Prop(_exitClipExitTime);
                    LEditorGUILayout.Prop(_postExitTransitionDuration);
                };
                LEditorGUILayout.PropAsFoldout(_hasExitClip, content3);

                LGUILayout.Heading(Loc("EmoteSequence::Layer Blend"));
                Action content4 = () =>
                {
                    LEditorGUILayout.Prop(_serializedBlendIn);
                    LEditorGUILayout.Prop(_serializedBlendOut);
                };
                LEditorGUILayout.PropAsFoldout(_serializedHasLayerBlend, content4);

                LGUILayout.Heading(Loc("EmoteSequence::Tracking Overrides"));
                Action content5 = () =>
                {
                    LEditorGUILayout.Prop(_serializedTrackingOverrides);
                };
                LEditorGUILayout.PropAsFoldout(_serializedHasTrackingOverrides, content5);

                serializedObject.ApplyModifiedProperties();

                if (requireRefreshPreview) RefreshPreviewIfNeeded();
                _previewWrapper.OnInspectorGUI();
            }
        }
    }
}
