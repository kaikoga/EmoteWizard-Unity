using System;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Preview;
using Silksprite.EmoteWizard.Sources.Sequence;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.ClipBuilder;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.Undoable;
using Silksprite.Loch;
using Silksprite.Loch.Extensions;
using Silksprite.Loch.UI;
using UnityEditor;
using UnityEngine;
using static Silksprite.Loch.Tools.LochTool;

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

        IAnimationPreviewWrapper _previewWrapper;
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

            if (_temporaryClip) DestroyImmediate(_temporaryClip);
            _temporaryClip = (AnimationClip)soleTarget.ToEmoteFactoryTemplate().Build(environment, new ClipBuilderImpl()).clip;
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
            var environment = CreateEnv();

            if (environment.MaybeVRChat())
            {
                LEditorGUILayout.Prop(_layerKind);
                LEditorGUILayout.Prop(_groupName);

                LGUILayout.Header(Loc("GenericEmoteSequence::Common Settings"));
                LEditorGUILayout.Prop(_isFixedDuration);
                LEditorGUILayout.Prop(_entryTransitionDuration);
                LEditorGUILayout.Prop(_exitTransitionDuration);

                LGUILayout.Header(Loc("GenericEmoteSequence::Time Parameter"));
                Action content = () =>
                {
                    LEditorGUILayout.Prop(_timeParameter);
                };
                LEditorGUILayout.PropAsFoldout(_hasTimeParameter, content);

                LGUILayout.Header(Loc("GenericEmoteSequence::Layer Blend"));
                Action content1 = () =>
                {
                    LEditorGUILayout.Prop(_blendIn);
                    LEditorGUILayout.Prop(_blendOut);
                };
                LEditorGUILayout.PropAsFoldout(_hasLayerBlend, content1);

                LGUILayout.Header(Loc("GenericEmoteSequence::Tracking Overrides"));
                Action content2 = () =>
                {
                    LEditorGUILayout.Prop(_trackingOverrides);
                };
                LEditorGUILayout.PropAsFoldout(_hasTrackingOverrides, content2);
            }

            LGUILayout.Header(Loc("GenericEmoteSequence::Animation"));
            EditorGUI.BeginChangeCheck();
            if (environment.MaybeVRChat())
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
                    var converted = _inputClip.ToGenericEmoteSequence(CreateEnv().AvatarRoot.gameObject);
                    soleTarget.sequence.animatedEnable = converted.animatedEnable;
                    soleTarget.sequence.animatedBlendShapes = converted.animatedBlendShapes;
                }
            }

            if (requireRefreshPreview) RefreshPreviewIfNeeded(CreateEnv());
            _previewWrapper.OnInspectorGUI();

            if (EmoteWizardGUILayout.Undoable(Loc("GenericEmoteSequenceSource::Explode"), "Explode Generic Emote Sequence source") is IUndoable undoable)
            {
                SourceExploder.ExplodeEmoteSequencesImmediate(undoable, soleTarget);
            }
        }
    }
}
