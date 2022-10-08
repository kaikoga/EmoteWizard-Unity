using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Internal.LayerBuilders2;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Internal
{
    public class AnimatorLayerBuilder
    {
        public AnimatorLayerWizardBase Wizard;
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
            foreach (var emoteItemGroup in emoteItems.GroupBy(item => item.trigger.groupName))
            {
                var groupName = emoteItemGroup.Key;
                var layer = PopulateLayer(groupName);
                new EmoteLayerBuilder2(this, layer, emoteItemGroup).Build();
            }
        }

        public void BuildTrackingControlLayers()
        {
            foreach (var kv in _overriders)
            {
                var trackingTarget = kv.Key;
                var trackingControlLayer = PopulateLayer($"TrackingControl {trackingTarget}");
                new TrackingControlLayerBuilder2(this, trackingControlLayer, trackingTarget, kv.Value).Build();
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
            foreach (var trackingTarget in _overriders.Keys)
            {
                AnimatorController.AddParameter(trackingTarget.ToAnimatorParameterName(false), AnimatorControllerParameterType.Trigger);
                AnimatorController.AddParameter(trackingTarget.ToAnimatorParameterName(true), AnimatorControllerParameterType.Trigger);
            }
        }
    }
}