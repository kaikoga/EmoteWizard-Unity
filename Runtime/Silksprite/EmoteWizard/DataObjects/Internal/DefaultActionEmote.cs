using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using Silksprite.EmoteWizard.Utils;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class DefaultActionEmote
    {
        int _index;
        string _name;
        bool _hasExitTime;
        float _exitTime = 0.75f;
        Motion _clip;
        Motion _exitClip;

        static IEnumerable<DefaultActionEmote> Defaults()
        {
            return new List<DefaultActionEmote>
            {
                new DefaultActionEmote
                {
                    _index = 1,
                    _name = "Wave",
                    _hasExitTime = true,
                    _exitTime = 0.6f,
                    _clip = VrcSdkAssetLocator.ProxyStandWave(),
                    _exitClip = null
                },
                new DefaultActionEmote
                {
                    _index = 2,
                    _name = "Clap",
                    _hasExitTime = false,
                    _clip = VrcSdkAssetLocator.ProxyStandClap(),
                    _exitClip = null
                },
                new DefaultActionEmote
                {
                    _index = 3,
                    _name = "Point",
                    _hasExitTime = true,
                    _exitTime = 0.75f,
                    _clip = VrcSdkAssetLocator.ProxyStandPoint(),
                    _exitClip = null
                },
                new DefaultActionEmote
                {
                    _index = 4,
                    _name = "Cheer",
                    _hasExitTime = false,
                    _clip = VrcSdkAssetLocator.ProxyStandCheer(),
                    _exitClip = null
                },
                new DefaultActionEmote
                {
                    _index = 5,
                    _name = "Dance",
                    _hasExitTime = false,
                    _clip = VrcSdkAssetLocator.ProxyDance(),
                    _exitClip = null
                },
                new DefaultActionEmote
                {
                    _index = 6,
                    _name = "Backflip",
                    _hasExitTime = true,
                    _exitTime = 0.8f,
                    _clip = VrcSdkAssetLocator.ProxyBackflip(),
                    _exitClip = null
                },
                new DefaultActionEmote
                {
                    _index = 7,
                    _name = "SadKick",
                    _hasExitTime = true,
                    _exitTime = 0.75f,
                    _clip = VrcSdkAssetLocator.ProxyStandSadkick(),
                    _exitClip = null
                },
                new DefaultActionEmote
                {
                    _index = 8,
                    _name = "Die",
                    _hasExitTime = false,
                    _clip = VrcSdkAssetLocator.ProxyDie(),
                    _exitClip = VrcSdkAssetLocator.ProxySupineWakeup()
                }
            };
        }

        public static IEnumerable<IEmoteTemplate> EnumerateDefaultActionEmoteItems()
        {
            var expressionItemIcon = VrcSdkAssetLocator.PersonDance();

            var actionTrackingOverrides = new[]
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

            foreach (var def in Defaults())
            {
                yield return EmoteItemTemplate.Builder(LayerKind.Action, def._name, EmoteWizardConstants.Defaults.Groups.Action)
                    .AddCondition(new EmoteCondition { kind = ParameterItemKind.Int, parameter = EmoteWizardConstants.Defaults.Params.ActionSelect, mode = EmoteConditionMode.Equals, threshold = def._index })
                    .AddFixedDuration(true)
                    .AddClip(def._clip)
                    .AddClipExitTime(def._hasExitTime, def._exitTime)
                    .AddExitClip(def._exitClip != null, def._exitClip, 0.75f, def._exitClip ? 0.4f : 0.25f)
                    .AddLayerBlend(true, 0.5f, 0.25f)
                    .AddTrackingOverrides(true, actionTrackingOverrides)
                    .AddExpressionItem(true, $"Default/{def._name}", expressionItemIcon)
                    .ToEmoteItemTemplate();
            }
            yield return EmoteItemTemplate.Builder(LayerKind.Action, "AFK", EmoteWizardConstants.Defaults.Groups.Action)
                .AddPriority(100)
                .AddCondition(new EmoteCondition { kind = ParameterItemKind.Bool, parameter = EmoteWizardConstants.Params.AFK, mode = EmoteConditionMode.If, threshold = 0 })
                .AddFixedDuration(true)
                .AddClip(VrcSdkAssetLocator.ProxyAfk(), 1f, 0.2f)
                .AddLayerBlend(true, 1f, 0.5f)
                .AddTrackingOverrides(true, actionTrackingOverrides)
                .ToEmoteItemTemplate();
        }
    }
}