using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Internal.LayerBuilders;
using Silksprite.EmoteWizard.Utils;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Internal
{
    public class AnimationControllerBuilder
    {
        public AnimationWizardBase AnimationWizardBase;
        public ParametersWizard ParametersWizard;
        public string DefaultRelativePath;

        readonly Dictionary<TrackingTarget, List<AnimatorStateTransition>> _overriders = new Dictionary<TrackingTarget, List<AnimatorStateTransition>>();

        public void RegisterOverrider(TrackingTarget target, AnimatorStateTransition transition)
        {
            if (!_overriders.TryGetValue(target, out var transitions))
            {
                transitions = new List<AnimatorStateTransition>();
                _overriders[target] = transitions;
            }
            transitions.Add(transition);
        }

        AnimatorController _animatorController;
        AnimatorController AnimatorController
        {
            get
            {
                if (_animatorController) return _animatorController;
                return _animatorController = AnimationWizardBase.ReplaceOrCreateOutputAsset(ref AnimationWizardBase.outputAsset, DefaultRelativePath);
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
                    anyStatePosition = new Vector3(0, 0, 0),
                    entryPosition = new Vector3(0, 100, 0),
                    exitPosition = new Vector3(0, 200, 0)
                }
            };

            AssetDatabase.AddObjectToAsset(layer.stateMachine, AssetDatabase.GetAssetPath(AnimatorController));
            AnimatorController.AddLayer(layer);
            return layer;
        }

        public void BuildStaticLayer(string layerName, AnimationClip clip, AvatarMask defaultAvatarMask)
        {
            var resetLayer = PopulateLayer(layerName, defaultAvatarMask);
            var layerBuilder = new StaticLayerBuilder(this, resetLayer);
            layerBuilder.Build(layerName, clip);
        }

        public void BuildHandSignLayer(string layerName, bool isLeft, bool advancedAnimations)
        {
            var avatarMask = isLeft ? VrcSdkAssetLocator.HandLeft() : VrcSdkAssetLocator.HandRight(); 
            var handLayer = PopulateLayer(layerName, avatarMask);
            var layerBuilder = new HandSignLayerBuilder(this, handLayer);
            layerBuilder.Build(isLeft, advancedAnimations);
        }

        public void BuildParameterLayers(IEnumerable<ParameterEmote> parameterEmotes)
        {
            foreach (var parameterEmote in parameterEmotes)
            {
                var parameterLayer = PopulateLayer(parameterEmote.name);
                var layerBuilder = new ParameterLayerBuilder(this, parameterLayer);
                layerBuilder.Build(parameterEmote);
            }
        }

        public void BuildMixinLayers(IEnumerable<AnimationMixin> mixins)
        {
            foreach (var mixin in mixins.Where(mixin => mixin.Motion != null))
            {
                var mixinLayer = PopulateLayer(mixin.name);
                var layerBuilder = new MixinLayerBuilder(this, mixinLayer);
                layerBuilder.Build(mixin);
            }
        }

        public void BuildTrackingControlLayers()
        {
            foreach (var kv in _overriders.Where(kv => kv.Key != TrackingTarget.None))
            {
                var trackingTarget = kv.Key;
                var trackingControlLayer = PopulateLayer($"TrackingControl {trackingTarget}");
                var layerBuilder = new TrackingControlLayerBuilder(this, trackingControlLayer);
                layerBuilder.Build(trackingTarget, kv.Value);
            }
        }

        public void BuildParameters()
        {
            foreach (var parameter in ParametersWizard.AllParameterItems)
            {
                var parameterName = parameter.name;
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
        }
    }
}