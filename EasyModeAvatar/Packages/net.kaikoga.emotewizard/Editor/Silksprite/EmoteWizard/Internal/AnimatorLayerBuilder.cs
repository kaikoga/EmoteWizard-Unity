using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Internal.LayerBuilders;
using Silksprite.EmoteWizard.Utils;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Internal
{
    public class AnimatorLayerBuilder
    {
        public readonly IAnimatorLayerWizardContext Context;
        public readonly ParametersSnapshot ParametersSnapshot;
        readonly AnimatorController _animatorController;

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


        public AnimatorLayerBuilder(IAnimatorLayerWizardContext context, ParametersSnapshot parametersSnapshot, AnimatorController animatorController)
        {
            Context = context;
            ParametersSnapshot = parametersSnapshot;
            _animatorController = animatorController;
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

            if (Context.Context.PersistGeneratedAssets)
            {
                AssetDatabase.AddObjectToAsset(layer.stateMachine, AssetDatabase.GetAssetPath(_animatorController));
            }
            _animatorController.AddLayer(layer);
            return layer;
        }

        public void BuildStaticLayer(string layerName, AnimationClip clip, AvatarMask defaultAvatarMask)
        {
            var resetLayer = PopulateLayer(layerName, defaultAvatarMask);
            new StaticLayerBuilder(this, resetLayer, layerName, clip).Build();
        }

        public void BuildEmoteLayers(IEnumerable<EmoteItem> emoteItems)
        {
            foreach (var emoteItemGroup in emoteItems.GroupBy(item => item.Group))
            {
                var groupName = emoteItemGroup.Key;
                var mirror = emoteItemGroup.Any(item => item.IsMirrorItem);

                if (mirror)
                {
                    var avatarMaskLeft = Context.LayerKind == LayerKind.Gesture ? VrcSdkAssetLocator.HandLeft() : null;
                    var layerLeft = PopulateLayer($"{groupName} ({EmoteHand.Left})", avatarMaskLeft);
                    new EmoteLayerBuilder(this, layerLeft, emoteItemGroup.Select(item => item.Mirror(EmoteHand.Left))).Build();

                    var avatarMaskRight = Context.LayerKind == LayerKind.Gesture ? VrcSdkAssetLocator.HandRight() : null;
                    var layerRight = PopulateLayer($"{groupName} ({EmoteHand.Right})", avatarMaskRight);
                    new EmoteLayerBuilder(this, layerRight, emoteItemGroup.Select(item => item.Mirror(EmoteHand.Right))).Build();
                }
                else
                {
                    var layer = PopulateLayer(emoteItemGroup.Key);
                    new EmoteLayerBuilder(this, layer, emoteItemGroup.Select(item => item.ToEmoteInstance())).Build();
                }
            }
        }

        public void BuildTrackingControlLayers(IEnumerable<EmoteItem> allEmoteItems)
        {
            var overriders = allEmoteItems
                .OrderBy(item => item.Trigger.priority)
                .Where(item => item.Sequence.hasTrackingOverrides)
                .SelectMany(item => item.Sequence.trackingOverrides.Select(trackingOverride => (item, trackingOverride.target)))
                .GroupBy(pair => pair.target)
                .ToDictionary(group => group.Key, group => group.Select(pair => pair.item).ToList());

            foreach (var kv in overriders)
            {
                var trackingTarget = kv.Key;
                var trackingControlLayer = PopulateLayer($"TrackingControl ({trackingTarget})");
                new TrackingControlLayerBuilder(this, trackingControlLayer, trackingTarget, kv.Value).Build();
            }
        }

        public void BuildParameters()
        {
            MarkParameter(EmoteWizardConstants.Params.Viseme); // for AlwaysTrueCondition
            foreach (var parameter in ParametersSnapshot.AllParameters)
            {
                var parameterName = parameter.Name;
                if (!_referencedParameters.Contains(parameterName)) continue;

                AnimatorControllerParameterType parameterType;
                switch (parameter.ValueKind)
                {
                    case ParameterValueKind.Int:
                        parameterType = AnimatorControllerParameterType.Int;
                        break;
                    case ParameterValueKind.Float:
                        parameterType = AnimatorControllerParameterType.Float;
                        break;
                    case ParameterValueKind.Bool:
                        parameterType = AnimatorControllerParameterType.Bool;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                _animatorController.AddParameter(parameterName, parameterType);
            }
            foreach (var trackingTarget in _referencedTrackingTargets)
            {
                _animatorController.AddParameter(trackingTarget.ToAnimatorParameterName(false), AnimatorControllerParameterType.Trigger);
                _animatorController.AddParameter(trackingTarget.ToAnimatorParameterName(true), AnimatorControllerParameterType.Trigger);
            }
        }
    }
}