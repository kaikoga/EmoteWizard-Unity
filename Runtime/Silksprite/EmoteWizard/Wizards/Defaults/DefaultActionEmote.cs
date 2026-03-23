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
    public static class DefaultActionEmote
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

        class DefaultActionEmoteDefVRChat
        {
            public DefaultActionIndex Key;
            int _index;
            bool _hasExitTime;
            float _exitTime = 0.75f;
            Motion? _clip;
            Motion? _exitClip;

            string Name => GetName(Key);
            
            public IEmoteTemplate ToEmoteItemTemplate(EmoteTemplatePath path)
            {
                if (Key == DefaultActionIndex.Afk)
                {
                    return UnpackedAfk(path);
                }

                var expressionItemIcon = VrcSdkAssetLocator.PersonDance();

                return EmoteItemTemplate.Builder(LayerKind.Action, path.Join(Name), EmoteWizardConstants.Groups.Action,
                        default,
                        EmoteItemKind.EmoteItem, EmoteSequenceFactoryKind.EmoteSequence)
                    .AddCondition(new EmoteCondition { kind = ParameterItemKind.Int, parameter = EmoteWizardConstants.Params.ActionSelect, mode = EmoteConditionMode.Equals, threshold = _index })
                    .AddFixedDuration(true)
                    .AddClip(_clip)
                    .AddClipExitTime(_hasExitTime, _exitTime)
                    .AddExitClip(_exitClip != null, _exitClip, 0.75f, _exitClip ? 0.4f : 0.25f)
                    .AddLayerBlend(true, 0.5f, 0.25f)
                    .AddTrackingOverrides(true, ActionTrackingOverrides)
                    .AddExpressionItem(true, $"Default/{Name}", expressionItemIcon)
                    .ToEmoteItemTemplate();
            }
            
            static IEmoteTemplate UnpackedAfk(EmoteTemplatePath path)
            {
                return EmoteItemTemplate.Builder(LayerKind.Action, path.Join("AFK"), EmoteWizardConstants.Groups.Action,
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


            public static DefaultActionEmoteDefVRChat? Default(DefaultActionIndex index)
            {
                return index switch
                {
                    DefaultActionIndex.Wave => new DefaultActionEmoteDefVRChat
                    {
                        _index = 1,
                        _hasExitTime = true,
                        _exitTime = 0.6f,
                        _clip = VrcSdkAssetLocator.ProxyStandWave(),
                        _exitClip = null
                    },
                    DefaultActionIndex.Clap => new DefaultActionEmoteDefVRChat
                    {
                        _index = 2,
                        _hasExitTime = false,
                        _clip = VrcSdkAssetLocator.ProxyStandClap(),
                        _exitClip = null
                    },
                    DefaultActionIndex.Point => new DefaultActionEmoteDefVRChat
                    {
                        _index = 3,
                        _hasExitTime = true,
                        _exitTime = 0.75f,
                        _clip = VrcSdkAssetLocator.ProxyStandPoint(),
                        _exitClip = null
                    },
                    DefaultActionIndex.Cheer => new DefaultActionEmoteDefVRChat
                    {
                        _index = 4,
                        _hasExitTime = false,
                        _clip = VrcSdkAssetLocator.ProxyStandCheer(),
                        _exitClip = null
                    },
                    DefaultActionIndex.Dance => new DefaultActionEmoteDefVRChat
                    {
                        _index = 5,
                        _hasExitTime = false,
                        _clip = VrcSdkAssetLocator.ProxyDance(),
                        _exitClip = null
                    },
                    DefaultActionIndex.Backflip => new DefaultActionEmoteDefVRChat
                    {
                        _index = 6,
                        _hasExitTime = true,
                        _exitTime = 0.8f,
                        _clip = VrcSdkAssetLocator.ProxyBackflip(),
                        _exitClip = null
                    },
                    DefaultActionIndex.SadKick => new DefaultActionEmoteDefVRChat
                    {
                        _index = 7,
                        _hasExitTime = true,
                        _exitTime = 0.75f,
                        _clip = VrcSdkAssetLocator.ProxyStandSadkick(),
                        _exitClip = null
                    },
                    DefaultActionIndex.Die => new DefaultActionEmoteDefVRChat
                    {
                        _index = 8,
                        _hasExitTime = false,
                        _clip = VrcSdkAssetLocator.ProxyDie(),
                        _exitClip = VrcSdkAssetLocator.ProxySupineWakeup()
                    },
                    _ => null
                };
            }
        }

        static string GetName(DefaultActionIndex index)
        {
            return index switch
            {
                DefaultActionIndex.Wave => "Wave",
                DefaultActionIndex.Clap => "Clap",
                DefaultActionIndex.Point => "Point",
                DefaultActionIndex.Cheer => "Cheer",
                DefaultActionIndex.Dance => "Dance",
                DefaultActionIndex.Backflip => "Backflip",
                DefaultActionIndex.SadKick => "SadKick",
                DefaultActionIndex.Die => "Die",
                DefaultActionIndex.Afk => "AFK",
                _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
            };
        }

        public static IEnumerable<IEmoteTemplate> UnpackDefaultAction(EmoteTemplatePath path, DefaultActionIndex index)
        {
            if (DefaultActionEmoteDefVRChat.Default(index) is { } defaultActionEmote)
            {
                yield return defaultActionEmote.ToEmoteItemTemplate(path);
            }
        }

        public static IEnumerable<IEmoteTemplate> EnumerateDefaultActionEmoteItems(EmoteTemplatePath path)
        {
            return Enum.GetValues(typeof(DefaultActionIndex)).OfType<DefaultActionIndex>()
                .Select(index => new DefaultActionEmoteItemTemplate(path.Join(GetName(index)), index));
        }
    }
}