using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Platforms.Common
{
    public interface IEditorPlatformFeatures
    {
        void PopulateParameterDriver(AnimatorState state, string parameterName, float parameterValue);
        void PopulateTriggerDriver(AnimatorState state, IEnumerable<(TrackingTarget target, TrackingMode mode)> settings);
        void PopulateTrackingControl(AnimatorState state, TrackingTarget target, TrackingMode mode);
        void PopulateLayerControl(AnimatorState state, float goalWeight, float duration);
    }
}
