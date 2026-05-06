using System;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;

namespace Silksprite.EmoteWizard.Platforms.Common
{
    public static class GeneratedParameters
    {
        // NOTE: isOn = true means animated (not tracking), isOn = false means tracking (default)
        public static string TrackingTrigger(TrackingTarget target, TrackingMode mode)
        {
            var modeString = mode switch
            {
                TrackingMode.Tracking => "Off",
                TrackingMode.Override => "On",
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
            return $"__EW__Tracking_{target}_{modeString}";
        }

        public static string RemappedInput(ParameterInstance parameterInstance)
        {
            return $"__EW__Input_{parameterInstance.Name}";
        }
    }
}