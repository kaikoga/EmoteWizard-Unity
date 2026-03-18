using Silksprite.EmoteWizard.DataObjects;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Platforms.Common
{
    public interface IEditorPlatformFeatures
    {
        void PopulateTriggerDriver(AnimatorState state, bool isOn, TrackingTarget[] targets, TrackingTarget[] currentTrackingTargets);
        void PopulateTrackingControl(AnimatorState state, TrackingTarget target, float targetWeight);
        void PopulateLayerControl(AnimatorState state, float goalWeight, float duration);
    }
}
