using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Legacy;

namespace Silksprite.EmoteWizard.Internal
{
    public static class DefaultEmoteItems
    {
        public static IEnumerable<EmoteItem> EnumerateDefaultHandSigns(LayerKind layerKind)
        {
            return Emote.HandSigns.Select(handSign => EmoteItem.Builder(layerKind, $"{handSign}", EmoteWizardConstants.Defaults.Groups.HandSign)
                .AddCondition(new EmoteCondition { kind = ParameterItemKind.Int, parameter = EmoteWizardConstants.Params.Gesture, mode = EmoteConditionMode.Equals, threshold = (int)handSign })
                .AddTimeParameter(handSign == HandSign.Fist, EmoteWizardConstants.Params.GestureWeight)
                .ToEmoteItem());
        }

    }
}