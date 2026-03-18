using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Platforms.Common.Extensions
{
    public static class TrackingTargetExtension
    {
        // NOTE: isOn = true means animated (not tracking), isOn = false means tracking (default)
        public static string ToAnimatorParameterName(this TrackingTarget target, bool isOn)
        {
            return $"_EW_Tracking_{target}_{(isOn ? "On" : "Off")}";
        }
    }
}