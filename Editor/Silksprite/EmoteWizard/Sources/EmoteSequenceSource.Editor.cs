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
        protected override DetectedPlatform SupportedPlatforms => DetectedPlatform.VRCPlatforms;

        LocalizedProperty _layerKind = null!;
        LocalizedProperty _groupName = null!;

        LocalizedProperty _isFixedDuration = null!;

        LocalizedProperty _clip = null!;
        LocalizedProperty _mirroredClip = null!;
        LocalizedProperty _entryTransitionDuration = null!;
        LocalizedProperty _exitTransitionDuration = null!;

        LocalizedProperty _hasExitTime = null!;
        LocalizedProperty _clipExitTime = null!;

        LocalizedProperty _hasTimeParameter = null!;
        LocalizedProperty _timeParameter = null!;

        LocalizedProperty _hasEntryClip = null!;
        LocalizedProperty _entryClip = null!;
        LocalizedProperty _mirroredEntryClip = null!;
        LocalizedProperty _entryClipExitTime = null!;
        LocalizedProperty _postEntryTransitionDuration = null!;

        LocalizedProperty _hasExitClip = null!;
        LocalizedProperty _exitClip = null!;
        LocalizedProperty _mirroredExitClip = null!;
        LocalizedProperty _exitClipExitTime = null!;
        LocalizedProperty _postExitTransitionDuration = null!;

        LocalizedProperty _serializedHasLayerBlend = null!;
        LocalizedProperty _serializedBlendIn = null!;
        LocalizedProperty _serializedBlendOut = null!;

        LocalizedProperty _serializedHasTrackingOverrides = null!;
        LocalizedProperty _serializedTrackingOverrides = null!;

        IAnimationPreviewWrapper _previewWrapper = null!;

        void OnEnable()
        {
            var serializedItem = Lop(nameof(EmoteSequenceSource.sequence), Loc("EmoteSequenceSource::sequence"));

            _layerKind = serializedItem.Lop(nameof(EmoteSequence.layerKind), Loc("EmoteSequence::layerKind"));
            _groupName = serializedItem.Lop(nameof(EmoteSequence.groupName), Loc("EmoteSequence::groupName"));

            _isFixedDuration = serializedItem.Lop(nameof(EmoteSequence.isFixedDuration), Loc("EmoteSequence::isFixedDuration"));
            
            _clip = serializedItem.Lop(nameof(EmoteSequence.clip), Loc("EmoteSequence::clip"));
            _mirroredClip = serializedItem.Lop(nameof(EmoteSequence.mirroredClip), Loc("EmoteSequence::mirroredClip"));
            _entryTransitionDuration = serializedItem.Lop(nameof(EmoteSequence.entryTransitionDuration), Loc("EmoteSequence::entryTransitionDuration"));
            _exitTransitionDuration = serializedItem.Lop(nameof(EmoteSequence.exitTransitionDuration), Loc("EmoteSequence::exitTransitionDuration"));
            
            _hasExitTime = serializedItem.Lop(nameof(EmoteSequence.hasExitTime), Loc("EmoteSequence::hasExitTime"));
            _clipExitTime = serializedItem.Lop(nameof(EmoteSequence.clipExitTime), Loc("EmoteSequence::clipExitTime"));

            _hasTimeParameter = serializedItem.Lop(nameof(EmoteSequence.hasTimeParameter), Loc("EmoteSequence::hasTimeParameter"));
            _timeParameter = serializedItem.Lop(nameof(EmoteSequence.timeParameter), Loc("EmoteSequence::timeParameter"));

            _hasEntryClip = serializedItem.Lop(nameof(EmoteSequence.hasEntryClip), Loc("EmoteSequence::hasEntryClip"));
            _entryClip = serializedItem.Lop(nameof(EmoteSequence.mirroredEntryClip), Loc("EmoteSequence::entryClip"));
            _mirroredEntryClip = serializedItem.Lop(nameof(EmoteSequence.entryClip), Loc("EmoteSequence::mirroredEntryClip"));
            _entryClipExitTime = serializedItem.Lop(nameof(EmoteSequence.entryClipExitTime), Loc("EmoteSequence::entryClipExitTime"));
            _postEntryTransitionDuration = serializedItem.Lop(nameof(EmoteSequence.postEntryTransitionDuration), Loc("EmoteSequence::postEntryTransitionDuration"));

            _hasExitClip = serializedItem.Lop(nameof(EmoteSequence.hasExitClip), Loc("EmoteSequence::hasExitClip"));
            _exitClip = serializedItem.Lop(nameof(EmoteSequence.exitClip), Loc("EmoteSequence::exitClip"));
            _mirroredExitClip = serializedItem.Lop(nameof(EmoteSequence.mirroredExitClip), Loc("EmoteSequence::mirroredExitClip"));
            _exitClipExitTime = serializedItem.Lop(nameof(EmoteSequence.exitClipExitTime), Loc("EmoteSequence::exitClipExitTime"));
            _postExitTransitionDuration = serializedItem.Lop(nameof(EmoteSequence.postExitTransitionDuration), Loc("EmoteSequence::postExitTransitionDuration"));

            _serializedHasLayerBlend = serializedItem.Lop(nameof(EmoteSequence.hasLayerBlend), Loc("EmoteSequence::hasLayerBlend"));
            _serializedBlendIn = serializedItem.Lop(nameof(EmoteSequence.blendIn), Loc("EmoteSequence::blendIn"));
            _serializedBlendOut = serializedItem.Lop(nameof(EmoteSequence.blendOut), Loc("EmoteSequence::blendOut"));

            _serializedHasTrackingOverrides = serializedItem.Lop(nameof(EmoteSequence.hasTrackingOverrides), Loc("EmoteSequence::hasTrackingOverrides"));
            _serializedTrackingOverrides = serializedItem.Lop(nameof(EmoteSequence.trackingOverrides), Loc("EmoteSequence::trackingOverrides"));

            var environment = CachedEnv();
            if (environment.AvatarRoot)
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
            LEditorGUILayout.PropAsEnumPopup<LayerKind>(_layerKind);
            LEditorGUILayout.Prop(_groupName);

            using (new LabelWidthScope(200f))
            {
                LGUILayout.Heading(Loc("EmoteSequence::Common Settings"));
                LEditorGUILayout.Prop(_isFixedDuration);
                EditorGUI.BeginChangeCheck();
                LEditorGUILayout.Prop(_clip);
                LEditorGUILayout.Prop(_mirroredClip);
                var requireRefreshPreview = EditorGUI.EndChangeCheck();
                LEditorGUILayout.Prop(_entryTransitionDuration);
                LEditorGUILayout.Prop(_exitTransitionDuration);

                LGUILayout.Heading(Loc("EmoteSequence::Exit Time"));
                LEditorGUILayout.PropAsFoldout(_hasExitTime, () =>
                {
                    LEditorGUILayout.Prop(_clipExitTime);
                });

                LGUILayout.Heading(Loc("EmoteSequence::Time Parameter"));
                LEditorGUILayout.PropAsFoldout(_hasTimeParameter, () =>
                {
                    LEditorGUILayout.Prop(_timeParameter);
                });

                LGUILayout.Heading(Loc("EmoteSequence::Entry Clip"));
                LEditorGUILayout.PropAsFoldout(_hasEntryClip, () =>
                {
                    LEditorGUILayout.Prop(_entryClip);
                    LEditorGUILayout.Prop(_mirroredEntryClip);
                    LEditorGUILayout.Prop(_entryClipExitTime);
                    LEditorGUILayout.Prop(_postEntryTransitionDuration);
                });

                LGUILayout.Heading(Loc("EmoteSequence::Exit Clip"));
                LEditorGUILayout.PropAsFoldout(_hasExitClip, () =>
                {
                    LEditorGUILayout.Prop(_exitClip);
                    LEditorGUILayout.Prop(_mirroredExitClip);
                    LEditorGUILayout.Prop(_exitClipExitTime);
                    LEditorGUILayout.Prop(_postExitTransitionDuration);
                });

                LGUILayout.Heading(Loc("EmoteSequence::Layer Blend"));
                LEditorGUILayout.PropAsFoldout(_serializedHasLayerBlend, () =>
                {
                    LEditorGUILayout.Prop(_serializedBlendIn);
                    LEditorGUILayout.Prop(_serializedBlendOut);
                });

                LGUILayout.Heading(Loc("EmoteSequence::Tracking Overrides"));
                LEditorGUILayout.PropAsFoldout(_serializedHasTrackingOverrides, () =>
                {
                    LEditorGUILayout.Prop(_serializedTrackingOverrides);
                });

                serializedObject.ApplyModifiedProperties();

                if (requireRefreshPreview) RefreshPreviewIfNeeded();
                _previewWrapper.OnInspectorGUI();
            }
        }
    }
}
