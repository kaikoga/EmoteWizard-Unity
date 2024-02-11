using System;
using Silksprite.AdLib.Utils.VRM0;
using Silksprite.EmoteWizardSupport.Undoable;
using VRM;

namespace Silksprite.EmoteWizard.Contexts.Extensions
{
    public static class EmoteWizardEnvironmentExtension
    {
        public static void BuildVrm0Avatar(this EmoteWizardEnvironment environment, IUndoable undoable, bool manualBuild)
        {
            if (manualBuild) throw new InvalidOperationException("manual build is not supported");

            var vrmMeta = environment.AvatarRoot.GetComponent<VRMMeta>();
            if (!vrmMeta) return;

            var blendShapeProxy = vrmMeta.GetComponent<VRMBlendShapeProxy>();
            if (!blendShapeProxy) return;

            var blendShapeAvatar = blendShapeProxy.BlendShapeAvatar;
            if (!blendShapeAvatar) return;

            blendShapeAvatar = new CustomCloneBlendShapeAvatar().Clone(blendShapeAvatar).mainAsset;
        }
    }
}