using Silksprite.EmoteWizard.DataObjects;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Platforms.Common
{
    public interface IEditorPlatformFeatures
    {
        void PopulateParameterDriver(AnimatorState state, bool isEntry, bool isPersisted, TrackingTarget[] targets, TrackingTarget[] currentTrackingTargets);
        void PopulateBodyControl(AnimatorState state, TrackingTarget target, float targetWeight, bool isPersisted);
    }
}
