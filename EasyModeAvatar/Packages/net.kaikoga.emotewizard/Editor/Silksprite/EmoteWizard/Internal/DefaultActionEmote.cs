using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Legacy;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Utils;
using UnityEngine;

namespace Silksprite.EmoteWizard.Internal
{
    public class DefaultActionEmote
    {
        int _index;
        string _name;
        bool _hasExitTime;
        float _exitTime = 0.75f;
        Motion _clip;
        Motion _exitClip;

        static List<DefaultActionEmote> _defaults;
        static IEnumerable<DefaultActionEmote> Defaults
        {
            get
            {
                return _defaults = _defaults ?? PopulateDefaults();
            }
        }

        static List<DefaultActionEmote> PopulateDefaults()
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

        public static List<ExpressionItem> PopulateDefaultExpressionItems(string defaultPrefix, IEnumerable<ExpressionItem> oldItems)
        {
            var icon = VrcSdkAssetLocator.PersonDance();
            var expressionItems = Defaults
                .Select(def => new ExpressionItem
                {
                    icon = icon,
                    path = $"{defaultPrefix}{def._name}",
                    parameter = "VRCEmote",
                    value = def._index,
                    itemKind = def._hasExitTime ? ExpressionItemKind.Button : ExpressionItemKind.Toggle,
                });
            return oldItems.Concat(expressionItems)
                .DistinctBy(item => item.path)
                .ToList();
        }

        public static List<ActionEmote> PopulateDefaultActionEmotes(IEnumerable<ActionEmote> oldItems = null)
        {
            return Defaults
                .Select(def => new ActionEmote
                {
                    name = def._name,
                    emoteIndex = def._index,
                    hasExitTime = def._hasExitTime,
                    clipExitTime = def._exitTime,
                    clip = def._clip,
                    exitClip = def._exitClip,
                    exitTransitionDuration = 0.25f,
                    postExitTransitionDuration = def._exitClip ? 0.4f : 0.25f,
                    blendIn = 0.5f
                }).Concat(oldItems ?? Enumerable.Empty<ActionEmote>())
                .DistinctBy(actionEmote => actionEmote.emoteIndex)
                .OrderBy(actionEmote => actionEmote.emoteIndex)
                .ToList();
        }
        
        public static ActionEmote PopulateDefaultAfkEmote()
        {
            return new ActionEmote
            {
                name = "AFK",
                emoteIndex = 0,
                hasExitTime = false,
                clip = VrcSdkAssetLocator.ProxyAfk(),
                entryTransitionDuration = 1f,
                exitTransitionDuration = 0.2f,
                blendIn = 1f,
                blendOut = 0.5f
            };
        }

        public static IEnumerable<EmoteItem> EnumerateDefaultActionEmoteItems()
        {
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

            foreach (var def in Defaults)
            {
                yield return EmoteItem.Builder(LayerKind.Action, def._name, EmoteWizardConstants.Defaults.Groups.Action)
                    .AddCondition(new EmoteCondition { kind = ParameterItemKind.Int, parameter = EmoteWizardConstants.Defaults.Params.ActionSelect, mode = EmoteConditionMode.Equals, threshold = def._index })
                    .AddClip(def._clip)
                    .AddClipExitTime(def._hasExitTime, def._exitTime)
                    .AddExitClip(def._exitClip != null, def._exitClip, 0.25f, def._exitClip ? 0.4f : 0.25f)
                    .AddLayerBlend(true, 0.5f, 0.25f)
                    .AddTrackingOverrides(true, actionTrackingOverrides)
                    .ToEmoteItem();
            }
            yield return EmoteItem.Builder(LayerKind.Action, "AFK", EmoteWizardConstants.Defaults.Groups.Action)
                .AddPriority(100)
                .AddCondition(new EmoteCondition { kind = ParameterItemKind.Bool, parameter = EmoteWizardConstants.Params.AFK, mode = EmoteConditionMode.If, threshold = 0 })
                .AddClip(VrcSdkAssetLocator.ProxyAfk(), 1f, 0.2f)
                .AddLayerBlend(true, 1f, 0.5f)
                .AddTrackingOverrides(true, actionTrackingOverrides)
                .ToEmoteItem();
        }
    }
}