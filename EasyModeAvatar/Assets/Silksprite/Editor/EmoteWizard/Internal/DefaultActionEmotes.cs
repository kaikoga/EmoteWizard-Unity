using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Utils;

namespace Silksprite.EmoteWizard.Internal
{
    public static class DefaultActionEmotes
    {
        static string NameForDefaultEmote(int value)
        {
            switch (value)
            {
                case 1: return "Wave";
                case 2: return "Clap";
                case 3: return "Point";
                case 4: return "Cheer";
                case 5: return "Dance";
                case 6: return "Backflip";
                case 7: return "SadKick";
                case 8: return "Die";
                default: throw new ArgumentOutOfRangeException();
            }
        }

        static ExpressionItemKind ItemKindForDefaultEmote(int value)
        {
            switch (value)
            {
                case 2:
                case 4:
                case 5:
                case 8:
                    return ExpressionItemKind.Toggle;
                default:
                    return ExpressionItemKind.Button;
            }
        }

        public static List<ExpressionItem> PopulateDefaultExpressionItems(string defaultPrefix, IEnumerable<ExpressionItem> oldItems)
        {
            var icon = VrcSdkAssetLocator.PersonDance();
            var expressionItems = Enumerable.Range(1, 8)
                .Select(i => new ExpressionItem
                {
                    icon = icon,
                    path = $"{defaultPrefix}{NameForDefaultEmote(i)}",
                    parameter = "VRCEmote",
                    value = i,
                    itemKind = ItemKindForDefaultEmote(i),
                });
            return oldItems.Concat(expressionItems)
                .DistinctBy(item => item.path)
                .ToList();
        }

        public static List<ActionEmote> PopulateDefaultActionEmotes(IEnumerable<ActionEmote> oldItems = null)
        {
            return Enumerable.Range(1, 8)
                .Select(index => new ActionEmote
                {
                    name = NameForDefaultEmote(index),
                    emoteIndex = index
                }).Concat(oldItems ?? Enumerable.Empty<ActionEmote>())
                .DistinctBy(actionEmote => actionEmote.emoteIndex)
                .OrderBy(actionEmote => actionEmote.emoteIndex)
                .ToList();
        }
    }
}