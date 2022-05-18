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

        readonly HashSet<string> _referencedParameters = new HashSet<string>();

        public void MarkParameter(string name)
        {
            _referencedParameters.Add(name);
        }

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
            new StaticLayerBuilder(this, resetLayer, layerName, clip).Build();
        }

        public void BuildHandSignLayer(string layerName, bool isLeft, bool advancedAnimations)
        {
            var avatarMask = isLeft ? VrcSdkAssetLocator.HandLeft() : VrcSdkAssetLocator.HandRight(); 
            var handLayer = PopulateLayer(layerName, avatarMask);
            new HandSignLayerBuilder(this, handLayer, isLeft, advancedAnimations).Build();
        }

        public void BuildParameterLayers(IEnumerable<ParameterEmote> parameterEmotes)
        {
            foreach (var parameterEmote in parameterEmotes)
            {
                if (parameterEmote.emoteKind == ParameterEmoteKind.Unused) continue;
                if (parameterEmote.states.All(state => state.clip == null)) continue;

                var parameterLayer = PopulateLayer(parameterEmote.name);
                new ParameterLayerBuilder(this, parameterLayer, parameterEmote).Build();
            }
        }

        public void BuildMixinLayers(IEnumerable<AnimationMixin> mixins)
        {
            foreach (var mixin in mixins)
            {
                if (mixin.Motion == null) continue;

                var mixinLayer = PopulateLayer(mixin.name);
                new MixinLayerBuilder(this, mixinLayer, mixin).Build();
            }
        }

        public void BuildTrackingControlLayers()
        {
            foreach (var kv in _overriders.Where(kv => kv.Key != TrackingTarget.None))
            {
                var trackingTarget = kv.Key;
                var trackingControlLayer = PopulateLayer($"TrackingControl {trackingTarget}");
                new TrackingControlLayerBuilder(this, trackingControlLayer, trackingTarget, kv.Value).Build();
            }
        }

        public void BuildParameters()
        {
            MarkParameter("Viseme"); // for AlwaysTrueCondition
            foreach (var parameter in ParametersWizard.AllParameterItems)
            {
                var parameterName = parameter.name;
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
        }
    }
}