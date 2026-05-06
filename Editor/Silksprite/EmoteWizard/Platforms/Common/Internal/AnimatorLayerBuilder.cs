using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Platforms.Common.Extensions;
using Silksprite.EmoteWizard.Platforms.Common.Internal.LayerBuilders;
using Silksprite.EmoteWizard.Platforms.Extensions;
using Silksprite.EmoteWizardSupport.ClipBuilder;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Platforms.Common.Internal
{
    public class AnimatorLayerBuilder
    {
        public readonly EmoteWizardEnvironment Environment;
        public readonly IEditorPlatformFeatures EditorFeatures;
        public readonly ParametersSnapshot ParametersSnapshot;
        readonly AnimatorController _animatorController;

        readonly Dictionary<string, AnimatorControllerParameterType> _rawParameters = new Dictionary<string, AnimatorControllerParameterType>();

        readonly HashSet<string> _referencedParameters = new HashSet<string>();

        public void MarkParameter(string name) => _referencedParameters.Add(name);

        public void MarkParameter(Motion motion)
        {
            if (!(motion is BlendTree blendTree)) return;
            _referencedParameters.Add(blendTree.blendParameter);
            _referencedParameters.Add(blendTree.blendParameterY);
            foreach (var childMotion in blendTree.children)
            {
                _referencedParameters.Add(childMotion.directBlendParameter);
                MarkParameter(childMotion.motion);
            }
        }

        public void MarkDefaultParameters()
        {
            // TODO: do we need this on VRChat?
            foreach (var parameter in ParametersSnapshot.DefaultParameterItems)
            {
                _referencedParameters.Add(parameter.Name);
            }
        }

        readonly HashSet<TrackingTarget> _referencedTrackingTargets = new HashSet<TrackingTarget>();
        public void MarkTrackingTarget(TrackingTarget target) => _referencedTrackingTargets.Add(target);

        public void MarkRawParameter(string name, AnimatorControllerParameterType parameterType)
        {
            _rawParameters.Add(name, parameterType);            
        }

        public AnimatorLayerBuilder(
            EmoteWizardEnvironment environment,
            IEditorPlatformFeatures editorFeatures,
            ParametersSnapshot parametersSnapshot,
            AnimatorController animatorController)
        {
            Environment = environment;
            EditorFeatures = editorFeatures;
            ParametersSnapshot = parametersSnapshot;
            _animatorController = animatorController;
        }

        public void AddExternalLayer(AnimatorControllerLayer layer)
        {
            _animatorController.AddLayer(layer);
        }

        AnimatorControllerLayer PopulateLayer(string layerName, AvatarMask? avatarMask = null)
        {
            layerName = _animatorController.MakeUniqueLayerName(layerName);
            var layer = new AnimatorControllerLayer
            {
                name = layerName,
                defaultWeight = 1.0f,
                avatarMask = avatarMask,
                stateMachine = new AnimatorStateMachine
                {
                    name = layerName,
                    hideFlags = HideFlags.HideInHierarchy,
                    anyStatePosition = new Vector3(-300f, 100f, 0f),
                    entryPosition = new Vector3(-300f, 0f, 0f),
                    exitPosition = new Vector3(1500f, 0f, 0f)
                }
            };

            if (EditorUtility.IsPersistent(_animatorController))
            {
                AssetDatabase.AddObjectToAsset(layer.stateMachine, _animatorController);
            }
            _animatorController.AddLayer(layer);
            return layer;
        }

        public void BuildStaticLayer(string layerName, Motion? clip, AvatarMask? defaultAvatarMask)
        {
            var resetLayer = PopulateLayer(layerName, defaultAvatarMask);
            new StaticLayerBuilder(this, resetLayer, layerName, clip).Build();
        }

        public void BuildEmoteLayers(IEnumerable<EmoteItem> mirroredEmoteItems, LayerKind layerKind)
        {
            var clipBuilder = new ClipBuilderImpl();
            var emoteInstances = mirroredEmoteItems.Select(emote => emote.ToEmoteInstance(Environment, clipBuilder));
            var platformFeatures = Environment.GetPlatformFeatures();

            foreach (var mirroredEmoteGroup in emoteInstances.GroupBy(item => (item.GroupName, item.Hand)))
            {
                var (groupName, hand) = mirroredEmoteGroup.Key;

                AvatarMask? avatarMask = null;
                if (layerKind == LayerKind.Gesture)
                {
                    switch (hand)
                    {
                        case EmoteHand.Neither:
                            break;
                        case EmoteHand.Left:
                            avatarMask = platformFeatures.HandLeft;
                            break;
                        case EmoteHand.Right:
                            avatarMask = platformFeatures.HandRight;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                var layer = PopulateLayer(groupName, avatarMask);
                new EmoteLayerBuilder(this, layer, mirroredEmoteGroup).Build();
            }
        }

        public void BuildMixinLayer(MixinInstance mixin)
        {
            if (mixin.SourceClip is { } clip)
            {
                BuildStaticLayer(mixin.Name, clip, null);
            }

            if (mixin.SourceController is AnimatorController animatorController)
            {
                foreach (var layer in animatorController.layers)
                {
                    _animatorController.AddLayer(layer);
                }
            }
        }

        public void BuildTrackingControlLayers(IEnumerable<EmoteItem> allMirroredEmoteItems)
        {
            var overriders = allMirroredEmoteItems
                .OrderBy(item => item.Trigger.Priority)
                .SelectMany(item => item.TrackingOverrides().Select(trackingOverride => (item, trackingOverride.target)))
                .GroupBy(pair => pair.target)
                .Where(group => group.Key != TrackingTarget.None && Enum.IsDefined(typeof(TrackingTarget), group.Key))
                .ToDictionary(group => group.Key, group => group.Select(pair => pair.item).ToList());

            foreach (var kv in overriders)
            {
                var trackingTarget = kv.Key;
                var trackingControlLayer = PopulateLayer($"TrackingControl ({trackingTarget})");
                new TrackingControlLayerBuilder(this, trackingControlLayer, trackingTarget, kv.Value).Build();
            }
        }

        public void BuildEditorLayer(IEnumerable<EmoteItem> allEmoteItems)
        {
            var clips = allEmoteItems
                .SelectMany(item => item.AllClipRefs())
                .Distinct();
            
            var editorLayer = PopulateLayer("Editor");
            new EditorLayerBuilder(this, editorLayer, clips).Build();
        }

        public void BuildActionSelectDriverLayer(string layerName, int[] actions)
        {
            var platformFeatures = Environment.GetPlatformFeatures();
            var actionSelectDriverLayer = PopulateLayer(layerName);
            ParameterRemapDriverLayerBuilder.Create(
                this,
                actionSelectDriverLayer,
                actions,
                platformFeatures.ParameterForPlatformActionSelect,
                platformFeatures.ParameterForPlatformCancelAction,
                platformFeatures.ParameterForActionSelect
            ).Build();
        }

        public void BuildParameterRemapDriverLayer(string layerName, IReadOnlyDictionary<int, int> actions, string remapFrom, string remapTo)
        {
            var parameterRemapDriverLayer = PopulateLayer(layerName);
            ParameterRemapDriverLayerBuilder.Create(
                this,
                parameterRemapDriverLayer,
                actions,
                remapFrom,
                null,
                remapTo
            ).Build();
        }

        public void BuildParameters()
        {
            var platformFeatures = Environment.GetPlatformFeatures();
            MarkParameter(platformFeatures.ParameterForAlwaysTrue); // for AlwaysTrueCondition

            foreach (var parameter in ParametersSnapshot.AllParameters)
            {
                var parameterName = parameter.Name;
                if (!_referencedParameters.Contains(parameterName)) continue;
                _animatorController.AddParameter(parameterName, parameter.GetParameterType());
            }
            foreach (var trackingTarget in _referencedTrackingTargets)
            {
                _animatorController.AddParameter(trackingTarget.ToAnimatorParameterName(TrackingMode.Tracking), AnimatorControllerParameterType.Trigger);
                _animatorController.AddParameter(trackingTarget.ToAnimatorParameterName(TrackingMode.Override), AnimatorControllerParameterType.Trigger);
            }
            foreach (var rawParameter in _rawParameters)
            {
                _animatorController.AddParameter(rawParameter.Key, rawParameter.Value);
            }
        }
    }
}