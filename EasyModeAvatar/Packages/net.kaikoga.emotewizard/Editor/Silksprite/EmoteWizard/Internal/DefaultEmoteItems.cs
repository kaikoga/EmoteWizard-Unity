using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Internal
{
    public static class DefaultEmoteItems
    {
        public static IEnumerable<EmoteItem> EnumerateDefaultHandSigns(string layerName)
        {
            return Emote.HandSigns.Select(handSign => EmoteItem.Builder(layerName, $"{handSign}", "HandSign")
                .AddCondition(new EmoteCondition { kind = ParameterItemKind.Int, parameter = "GestureRight", mode = EmoteConditionMode.Equals, threshold = (int)handSign })
                .AddTimeParameter(handSign == HandSign.Fist, "GestureRightWeight")
                .ToEmoteItem());
        }

    }
}