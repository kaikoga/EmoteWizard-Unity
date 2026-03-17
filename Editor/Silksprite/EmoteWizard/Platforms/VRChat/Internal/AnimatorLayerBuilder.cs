using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.DataObjects.Platforms.Extensions;
using Silksprite.EmoteWizard.Platforms.VRChat.Extensions;
using Silksprite.EmoteWizard.Platforms.VRChat.Internal.LayerBuilders;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.ClipBuilder;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Platforms.VRChat.Internal
{
    public class AnimatorLayerBuilder
    {
        public readonly EmoteWizardEnvironment Environment;
        public readonly ParametersSnapshot ParametersSnapshot;
        readonly AnimatorController _animatorController;
        readonly string _assetPath;

        public bool IsPersistedAsset => !string.IsNullOrEmpty(_assetPath);
        
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

        readonly HashSet<TrackingTarget> _referencedTrackingTargets = new HashSet<TrackingTarget>();
        public void MarkTrackingTarget(TrackingTarget target) => _referencedTrackingTargets.Add(target);


        public AnimatorLayerBuilder(EmoteWizardEnvironment environment, ParametersSnapshot parametersSnapshot, AnimatorController animatorController)
        {
            Environment = environment;
            ParametersSnapshot = parametersSnapshot;
            _animatorController = animatorController;
            _assetPath = AssetDatabase.GetAssetPath(_animatorController);
        }

        AnimatorControllerLayer PopulateLayer(string layerName, AvatarMask avatarMask = null)
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

            if (IsPersistedAsset)
            {
                AssetDatabase.AddObjectToAsset(layer.stateMachine, _assetPath);
            }
            _animatorController.AddLayer(layer);
            return layer;
        }

        public void BuildStaticLayer(string layerName, AnimationClip clip, AvatarMask defaultAvatarMask)
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

                AvatarMask avatarMask = null;
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

        public void BuildParameters()
        {
            var platformFeatures = Environment.GetPlatformFeatures();
            MarkParameter(platformFeatures.ParameterForAlwaysTrue); // for AlwaysTrueCondition
            foreach (var parameter in ParametersSnapshot.AllParameters)
            {
                var parameterName = parameter.name;
                if (!_referencedParameters.Contains(parameterName)) continue;
                _animatorController.AddParameter(parameterName, parameter.GetParameterType());
            }
            foreach (var trackingTarget in _referencedTrackingTargets)
            {
                _animatorController.AddParameter(trackingTarget.ToAnimatorParameterName(false), AnimatorControllerParameterType.Trigger);
                _animatorController.AddParameter(trackingTarget.ToAnimatorParameterName(true), AnimatorControllerParameterType.Trigger);
            }
        }
    }
}