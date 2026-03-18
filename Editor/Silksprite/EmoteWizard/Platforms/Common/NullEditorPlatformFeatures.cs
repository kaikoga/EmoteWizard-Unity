using System;
using Silksprite.EmoteWizard.DataObjects;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Platforms.Common
{
    public class NullEditorPlatformFeatures : IEditorPlatformFeatures
    {
        public static readonly IEditorPlatformFeatures Instance = new NullEditorPlatformFeatures();

        void IEditorPlatformFeatures.PopulateTriggerDriver(AnimatorState state, bool isEntry, TrackingTarget[] targets, TrackingTarget[] currentTrackingTargets)
        {
            throw new NotImplementedException();
        }

        void IEditorPlatformFeatures.PopulateTrackingControl(AnimatorState state, TrackingTarget target, float targetWeight)
        {
            throw new NotImplementedException();
        }

        void IEditorPlatformFeatures.PopulateLayerControl(AnimatorState state, float goalWeight, float duration)
        {
            throw new NotImplementedException();
        }
    }
}
