using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.AdLib.ChilloutVR;
using Silksprite.AdLib.ChilloutVR.Access;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Platforms.Common.Extensions;
using Silksprite.EmoteWizard.Platforms.Common.Internal.Extensions;
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
