using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;

namespace Silksprite.EmoteWizard.Internal
{
    public static class DefaultEmoteItems
    {
        static IEnumerable<HandSign> HandSigns =>
            Enum.GetValues(typeof(HandSign)).OfType<HandSign>();

        public static IEnumerable<EmoteItemTemplate> EnumerateDefaultHandSigns(LayerKind layerKind)
        {
            return HandSigns.Select(handSign => EmoteItemTemplate.Builder(layerKind, $"{handSign}", EmoteWizardConstants.Defaults.Groups.HandSign)
                .AddCondition(new EmoteCondition { kind = ParameterItemKind.Int, parameter = EmoteWizardConstants.Params.Gesture, mode = EmoteConditionMode.Equals, threshold = (int)handSign })
                .AddTimeParameter(handSign == HandSign.Fist, EmoteWizardConstants.Params.GestureWeight)
                .AddFixedDuration(true)
                .AddClip(null, 0f, 0.1f)
                .ToEmoteItemTemplate());
        }

    }
}