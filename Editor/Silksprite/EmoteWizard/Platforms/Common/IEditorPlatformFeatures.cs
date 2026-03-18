using Silksprite.EmoteWizard.DataObjects;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Platforms.Common
{
    public interface IEditorPlatformFeatures
    {
        void PopulateParameterDriver(AnimatorState state, bool isEntry, TrackingTarget[] targets, TrackingTarget[] currentTrackingTargets);
        void PopulateBodyControl(AnimatorState state, TrackingTarget target, float targetWeight);
        void PopulatePlayableLayerControl(AnimatorState state, float goalWeight, float duration);
    }
}
