using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Utils;
using UnityEngine;

namespace Silksprite.EmoteWizard.Internal
{
    public class DefaultActionEmote
    {
        int _index;
        string _name;
        ExpressionItemKind _itemKind;
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
                    _itemKind = ExpressionItemKind.Button,
                    _clip = VrcSdkAssetLocator.ProxyStandWave(),
                    _exitClip = null
                },
                new DefaultActionEmote
                {
                    _index = 2,
                    _name = "Clap",
                    _itemKind = ExpressionItemKind.Toggle,
                    _clip = VrcSdkAssetLocator.ProxyStandClap(),
                    _exitClip = null
                },
                new DefaultActionEmote
                {
                    _index = 3,
                    _name = "Point",
                    _itemKind = ExpressionItemKind.Button,
                    _clip = VrcSdkAssetLocator.ProxyStandPoint(),
                    _exitClip = null
                },
                new DefaultActionEmote
                {
                    _index = 4,
                    _name = "Cheer",
                    _itemKind = ExpressionItemKind.Toggle,
                    _clip = VrcSdkAssetLocator.ProxyStandCheer(),
                    _exitClip = null
                },
                new DefaultActionEmote
                {
                    _index = 5,
                    _name = "Dance",
                    _itemKind = ExpressionItemKind.Toggle,
                    _clip = VrcSdkAssetLocator.ProxyDance(),
                    _exitClip = null
                },
                new DefaultActionEmote
                {
                    _index = 6,
                    _name = "Backflip",
                    _itemKind = ExpressionItemKind.Button,
                    _clip = VrcSdkAssetLocator.ProxyBackflip(),
                    _exitClip = null
                },
                new DefaultActionEmote
                {
                    _index = 7,
                    _name = "SadKick",
                    _itemKind = ExpressionItemKind.Button,
                    _clip = VrcSdkAssetLocator.ProxyStandSadkick(),
                    _exitClip = null
                },
                new DefaultActionEmote
                {
                    _index = 8,
                    _name = "Die",
                    _itemKind = ExpressionItemKind.Toggle,
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
                    itemKind = def._itemKind,
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
                    clip = def._clip,
                    exitClip = def._exitClip
                }).Concat(oldItems ?? Enumerable.Empty<ActionEmote>())
                .DistinctBy(actionEmote => actionEmote.emoteIndex)
                .OrderBy(actionEmote => actionEmote.emoteIndex)
                .ToList();
        }
        
        public static List<ActionEmote> PopulateDefaultAfkEmotes()
        {
            return new List<ActionEmote>
            {
                new ActionEmote
                {
                    name = "AFK",
                    emoteIndex = 0,
                    entryClip = VrcSdkAssetLocator.ProxyAfk(),
                    clip = VrcSdkAssetLocator.ProxyAfk(),
                    exitClip = VrcSdkAssetLocator.ProxyAfk()
                }
            };
        }
    }
}