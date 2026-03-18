using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Platforms.Common
{
    public interface IEditorPlatformFeatures
    {
        void PopulateTriggerDriver(AnimatorState state, IEnumerable<(TrackingTarget target, bool isOn)> settings);
        void PopulateTrackingControl(AnimatorState state, TrackingTarget target, float targetWeight);
        void PopulateLayerControl(AnimatorState state, float goalWeight, float duration);
    }
}
