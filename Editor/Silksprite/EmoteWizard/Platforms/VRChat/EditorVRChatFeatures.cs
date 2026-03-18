using System;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Platforms.Common;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Platforms.VRChat
{
    public class EditorVRChatFeatures : IEditorPlatformFeatures
    {
        public static readonly IEditorPlatformFeatures Instance = new EditorVRChatFeatures();

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
