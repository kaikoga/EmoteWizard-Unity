using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Extensions;
using Silksprite.EmoteWizard.Platforms.Utils;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using Silksprite.EmoteWizard.Wizards;
using UnityEngine;

namespace Silksprite.EmoteWizard.Platforms.Defaults
{
    class DefaultActionEmoteVRChat
    {
        static readonly List<TrackingOverride> ActionTrackingOverrides = new[]
        {
            TrackingTarget.Head,
            TrackingTarget.LeftHand,
            TrackingTarget.RightHand,
            TrackingTarget.Hip,
            TrackingTarget.LeftFoot,
            TrackingTarget.RightFoot,
            TrackingTarget.LeftFingers,
            TrackingTarget.RightFingers
        }.Select(target => new TrackingOverride { target = target }).ToList();

        DefaultActionIndex _key;
        int _index;
        bool _hasExitTime;
        float _exitTime = 0.75f;
        Motion? _clip;
        Motion? _exitClip;

        public IEmoteTemplate ToEmoteItemTemplate(IPlatformFeatures platformFeatures, EmoteTemplatePath path)
        {
            if (_key == DefaultActionIndex.Afk)
            {
                return UnpackAsAfk(path);
            }

            var expressionItemIcon = VrcSdkAssetLocator.PersonDance();

            return EmoteItemTemplate.Builder(LayerKind.Action, path, EmoteWizardConstants.Groups.Action,
                    default,
                    EmoteItemKind.EmoteItem, EmoteSequenceFactoryKind.EmoteSequence)
                .AddCondition(new EmoteCondition { kind = ParameterItemKind.Int, parameter = platformFeatures.ParameterReferenceForActionSelect, mode = EmoteConditionMode.Equals, threshold = _index })
                .AddFixedDuration(true)
                .AddClip(_clip)
                .AddClipExitTime(_hasExitTime, _exitTime)
                .AddExitClip(_exitClip != null, _exitClip, 0.75f, _exitClip ? 0.4f : 0.25f)
                .AddLayerBlend(true, 0.5f, 0.25f)
                .AddTrackingOverrides(true, ActionTrackingOverrides)
                .AddExpressionItem(true, $"Default/{_key.Name()}", expressionItemIcon)
                .ToEmoteItemTemplate();
        }
            
        static IEmoteTemplate UnpackAsAfk(EmoteTemplatePath path)
        {
            return EmoteItemTemplate.Builder(LayerKind.Action, path, EmoteWizardConstants.Groups.Action,
                    default,
                    EmoteItemKind.EmoteItem, EmoteSequenceFactoryKind.EmoteSequence)
                .AddPriority(100)
                .AddCondition(new EmoteCondition { kind = ParameterItemKind.Bool, parameter = EmoteWizardConstants.Params.Afk, mode = EmoteConditionMode.If, threshold = 0 })
                .AddFixedDuration(true)
                .AddClip(VrcSdkAssetLocator.ProxyAfk(), 1f, 0.2f)
                .AddLayerBlend(true, 1f, 0.5f)
                .AddTrackingOverrides(true, ActionTrackingOverrides)
                .ToEmoteItemTemplate();
        }


        public static DefaultActionEmoteVRChat? Default(DefaultActionIndex index)
        {
            return index switch
            {
                DefaultActionIndex.Wave => new DefaultActionEmoteVRChat
                {
                    _key = index,
                    _index = 1,
                    _hasExitTime = true,
                    _exitTime = 0.6f,
                    _clip = VrcSdkAssetLocator.ProxyStandWave(),
                    _exitClip = null
                },
                DefaultActionIndex.Clap => new DefaultActionEmoteVRChat
                {
                    _key = index,
                    _index = 2,
                    _hasExitTime = false,
                    _clip = VrcSdkAssetLocator.ProxyStandClap(),
                    _exitClip = null
                },
                DefaultActionIndex.Point => new DefaultActionEmoteVRChat
                {
                    _key = index,
                    _index = 3,
                    _hasExitTime = true,
                    _exitTime = 0.75f,
                    _clip = VrcSdkAssetLocator.ProxyStandPoint(),
                    _exitClip = null
                },
                DefaultActionIndex.Cheer => new DefaultActionEmoteVRChat
                {
                    _key = index,
                    _index = 4,
                    _hasExitTime = false,
                    _clip = VrcSdkAssetLocator.ProxyStandCheer(),
                    _exitClip = null
                },
                DefaultActionIndex.Dance => new DefaultActionEmoteVRChat
                {
                    _key = index,
                    _index = 5,
                    _hasExitTime = false,
                    _clip = VrcSdkAssetLocator.ProxyDance(),
                    _exitClip = null
                },
                DefaultActionIndex.Backflip => new DefaultActionEmoteVRChat
                {
                    _key = index,
                    _index = 6,
                    _hasExitTime = true,
                    _exitTime = 0.8f,
                    _clip = VrcSdkAssetLocator.ProxyBackflip(),
                    _exitClip = null
                },
                DefaultActionIndex.SadKick => new DefaultActionEmoteVRChat
                {
                    _key = index,
                    _index = 7,
                    _hasExitTime = true,
                    _exitTime = 0.75f,
                    _clip = VrcSdkAssetLocator.ProxyStandSadkick(),
                    _exitClip = null
                },
                DefaultActionIndex.Die => new DefaultActionEmoteVRChat
                {
                    _key = index,
                    _index = 8,
                    _hasExitTime = false,
                    _clip = VrcSdkAssetLocator.ProxyDie(),
                    _exitClip = VrcSdkAssetLocator.ProxySupineWakeup()
                },
                DefaultActionIndex.Afk => new DefaultActionEmoteVRChat
                {
                    _key = index,
                    _index = int.MinValue,
                },
                _ => null
            };
        }
    }
}
