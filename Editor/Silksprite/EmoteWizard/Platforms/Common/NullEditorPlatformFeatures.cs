using System;
using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Platforms.Common
{
    public class NullEditorPlatformFeatures : IEditorPlatformFeatures
    {
        public static readonly IEditorPlatformFeatures Instance = new NullEditorPlatformFeatures();

        void IEditorPlatformFeatures.PopulateParameterDriver(AnimatorState state, string parameterName, float parameterValue)
        {
            throw new NotImplementedException();
        }

        void IEditorPlatformFeatures.PopulateTriggerDriver(AnimatorState state, IEnumerable<(TrackingTarget target, TrackingMode mode)> settings)
        {
            throw new NotImplementedException();
        }

        void IEditorPlatformFeatures.PopulateTrackingControl(AnimatorState state, TrackingTarget target, TrackingMode mode)
        {
            throw new NotImplementedException();
        }

        void IEditorPlatformFeatures.PopulateLayerControl(AnimatorState state, float goalWeight, float duration)
        {
            throw new NotImplementedException();
        }
    }
}
