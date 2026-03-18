using System;
using Silksprite.EmoteWizard.DataObjects;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Platforms.Common
{
    public class NullEditorPlatformFeatures : IEditorPlatformFeatures
    {
        public static readonly IEditorPlatformFeatures Instance = new NullEditorPlatformFeatures();

        void IEditorPlatformFeatures.PopulateParameterDriver(AnimatorState state, bool isEntry, bool isPersisted, TrackingTarget[] targets, TrackingTarget[] currentTrackingTargets)
        {
            throw new NotImplementedException();
        }

        void IEditorPlatformFeatures.PopulateBodyControl(AnimatorState state, TrackingTarget target, float targetWeight, bool isPersisted)
        {
            throw new NotImplementedException();
        }
    }
}
