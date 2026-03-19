using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using Silksprite.EmoteWizard.Utils;
using UnityEngine;

namespace Silksprite.EmoteWizard.Wizards.Defaults
{
    public class DefaultActionEmote
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
        
        int _index;
        string _name;
        bool _hasExitTime;
        float _exitTime = 0.75f;
        Motion _clip;
        Motion _exitClip;

        static DefaultActionEmote Default(DefaultActionIndex index)
        {
            return index switch
            {
                DefaultActionIndex.Wave => new DefaultActionEmote
                {
                    _index = 1,
                    _name = "Wave",
                    _hasExitTime = true,
                    _exitTime = 0.6f,
                    _clip = VrcSdkAssetLocator.ProxyStandWave(),
                    _exitClip = null
                },
                DefaultActionIndex.Clap => new DefaultActionEmote
                {
                    _index = 2,
                    _name = "Clap",
                    _hasExitTime = false,
                    _clip = VrcSdkAssetLocator.ProxyStandClap(),
                    _exitClip = null
                },
                DefaultActionIndex.Point => new DefaultActionEmote
                {
                    _index = 3,
                    _name = "Point",
                    _hasExitTime = true,
                    _exitTime = 0.75f,
                    _clip = VrcSdkAssetLocator.ProxyStandPoint(),
                    _exitClip = null
                },
                DefaultActionIndex.Cheer => new DefaultActionEmote
                {
                    _index = 4,
                    _name = "Cheer",
                    _hasExitTime = false,
                    _clip = VrcSdkAssetLocator.ProxyStandCheer(),
                    _exitClip = null
                },
                DefaultActionIndex.Dance => new DefaultActionEmote
                {
                    _index = 5,
                    _name = "Dance",
                    _hasExitTime = false,
                    _clip = VrcSdkAssetLocator.ProxyDance(),
                    _exitClip = null
                },
                DefaultActionIndex.Backflip => new DefaultActionEmote
                {
                    _index = 6,
                    _name = "Backflip",
                    _hasExitTime = true,
                    _exitTime = 0.8f,
                    _clip = VrcSdkAssetLocator.ProxyBackflip(),
                    _exitClip = null
                },
                DefaultActionIndex.SadKick => new DefaultActionEmote
                {
                    _index = 7,
                    _name = "SadKick",
                    _hasExitTime = true,
                    _exitTime = 0.75f,
                    _clip = VrcSdkAssetLocator.ProxyStandSadkick(),
                    _exitClip = null
                },
                DefaultActionIndex.Die => new DefaultActionEmote
                {
                    _index = 8,
                    _name = "Die",
                    _hasExitTime = false,
                    _clip = VrcSdkAssetLocator.ProxyDie(),
                    _exitClip = VrcSdkAssetLocator.ProxySupineWakeup()
                },
                _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
            };
        }

        IEmoteTemplate ToEmoteItemTemplate()
        {
            var expressionItemIcon = VrcSdkAssetLocator.PersonDance();

            return EmoteItemTemplate.Builder(LayerKind.Action, _name, EmoteWizardConstants.Defaults.Groups.Action,
                    default,
                    EmoteItemKind.EmoteItem, EmoteSequenceFactoryKind.EmoteSequence)
                .AddCondition(new EmoteCondition { kind = ParameterItemKind.Int, parameter = EmoteWizardConstants.Defaults.Params.ActionSelect, mode = EmoteConditionMode.Equals, threshold = _index })
                .AddFixedDuration(true)
                .AddClip(_clip)
                .AddClipExitTime(_hasExitTime, _exitTime)
                .AddExitClip(_exitClip != null, _exitClip, 0.75f, _exitClip ? 0.4f : 0.25f)
                .AddLayerBlend(true, 0.5f, 0.25f)
                .AddTrackingOverrides(true, ActionTrackingOverrides)
                .AddExpressionItem(true, $"Default/{_name}", expressionItemIcon)
                .ToEmoteItemTemplate();
        }

        public static IEmoteTemplate DefaultAction(DefaultActionIndex index)
        {
            return Default(index).ToEmoteItemTemplate();
        }

        public static IEnumerable<IEmoteTemplate> EnumerateDefaultActionEmoteItems()
        {
            return Enum.GetValues(typeof(DefaultActionIndex)).OfType<DefaultActionIndex>()
                .Select(index => index switch
                {
                    DefaultActionIndex.Afk => Afk(),
                    _ => DefaultAction(index)
                });
        }

        static IEmoteTemplate Afk()
        {
            return EmoteItemTemplate.Builder(LayerKind.Action, "AFK", EmoteWizardConstants.Defaults.Groups.Action,
                    default,
                    EmoteItemKind.EmoteItem, EmoteSequenceFactoryKind.EmoteSequence)
                .AddPriority(100)
                .AddCondition(new EmoteCondition { kind = ParameterItemKind.Bool, parameter = EmoteWizardConstants.Params.AFK, mode = EmoteConditionMode.If, threshold = 0 })
                .AddFixedDuration(true)
                .AddClip(VrcSdkAssetLocator.ProxyAfk(), 1f, 0.2f)
                .AddLayerBlend(true, 1f, 0.5f)
                .AddTrackingOverrides(true, ActionTrackingOverrides)
                .ToEmoteItemTemplate();
        }
    }
}