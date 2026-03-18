using System;
using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Platforms.Common.Extensions
{
    public static class TrackingTargetExtension
    {
        // NOTE: isOn = true means animated (not tracking), isOn = false means tracking (default)
        public static string ToAnimatorParameterName(this TrackingTarget target, TrackingMode mode)
        {
            var modeString = mode switch
            {
                TrackingMode.Tracking => "Off",
                TrackingMode.Override => "On",
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
            return $"_EW_Tracking_{target}_{modeString}";
        }
    }
}