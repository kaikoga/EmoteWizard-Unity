using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Internal.LayerBuilders2;
using Silksprite.EmoteWizard.Utils;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Internal
{
    public class AnimatorLayerBuilder
    {
        public AnimatorLayerWizardBase Wizard;
        public ParametersSnapshot ParametersSnapshot;
        public string DefaultRelativePath;

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

        AnimatorController _animatorController;
        AnimatorController AnimatorController
        {
            get
            {
                if (_animatorController) return _animatorController;
                return _animatorController = Wizard.ReplaceOrCreateOutputAsset(ref Wizard.outputAsset, DefaultRelativePath);
            }
        }

        AnimatorControllerLayer PopulateLayer(string layerName, AvatarMask avatarMask = null)
        {
            layerName = AnimatorController.MakeUniqueLayerName(layerName);
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
                    exitPosition = new Vector3(1200f, 0f, 0f)
                }
            };

            AssetDatabase.AddObjectToAsset(layer.stateMachine, AssetDatabase.GetAssetPath(AnimatorController));
            AnimatorController.AddLayer(layer);
            return layer;
        }

        public void BuildStaticLayer(string layerName, AnimationClip clip, AvatarMask defaultAvatarMask)
        {
            var resetLayer = PopulateLayer(layerName, defaultAvatarMask);
            new StaticLayerBuilder2(this, resetLayer, layerName, clip).Build();
        }

        public void BuildEmoteLayers(IEnumerable<EmoteItem> emoteItems)
        {
            foreach (var emoteItemGroup in emoteItems.GroupBy(item => item.GroupInstance))
            {
                var groupInstance = emoteItemGroup.Key;
                AvatarMask avatarMask = null;
                if (Wizard.LayerKind == LayerKind.Gesture)
                {
                    switch (groupInstance.Hand)
                    {
                        case EmoteHand.Left: avatarMask = VrcSdkAssetLocator.HandLeft(); break;
                        case EmoteHand.Right: avatarMask = VrcSdkAssetLocator.HandRight(); break;
                    }
                }
                var layer = PopulateLayer(groupInstance.Name, avatarMask);
                new EmoteLayerBuilder2(this, layer, emoteItemGroup).Build();
            }
        }

        public void BuildTrackingControlLayers(IEnumerable<EmoteItem> allEmoteItems)
        {
            var overriders = allEmoteItems
                .OrderBy(item => item.trigger.priority)
                .Where(item => item.sequence.hasTrackingOverrides)
                .SelectMany(item => item.sequence.trackingOverrides.Select(trackingOverride => (item, trackingOverride.target)))
                .GroupBy(pair => pair.target)
                .ToDictionary(group => group.Key, group => group.Select(pair => pair.item).ToList());

            foreach (var kv in overriders)
            {
                var trackingTarget = kv.Key;
                var trackingControlLayer = PopulateLayer($"TrackingControl ({trackingTarget})");
                new TrackingControlLayerBuilder2(this, trackingControlLayer, trackingTarget, kv.Value).Build();
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
                AnimatorController.AddParameter(parameterName, parameterType);
            }
            foreach (var trackingTarget in _referencedTrackingTargets)
            {
                AnimatorController.AddParameter(trackingTarget.ToAnimatorParameterName(false), AnimatorControllerParameterType.Trigger);
                AnimatorController.AddParameter(trackingTarget.ToAnimatorParameterName(true), AnimatorControllerParameterType.Trigger);
            }
        }
    }
}