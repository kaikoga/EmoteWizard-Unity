using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Silksprite.AdLib.Utils.VRM0;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Builder;
using Silksprite.EmoteWizard.Contexts.Ephemeral;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Platforms.VRM0.Extensions;
using Silksprite.EmoteWizard.Scopes;
using Silksprite.EmoteWizardSupport.Undoable;
using Silksprite.EmoteWizardSupport.Utils;
using VRM;

namespace Silksprite.EmoteWizard.Platforms.VRM0.Contexts.Builder
{
    public class VRM0AvatarBuilderContext : AvatarBuilderContextBase
    {
        [UsedImplicitly]
        public VRM0AvatarBuilderContext(EmoteWizardEnvironment environment) : base(environment) { }

        public override void CleanupAvatar()
        {
        }

        public override void BuildAvatar(IUndoable undoable, bool manualBuild)
        {
            if (manualBuild) throw new InvalidOperationException("manual build is not supported");

            var vrmMeta = Environment.AvatarRoot.GetComponent<VRMMeta>();
            if (!vrmMeta) return;

            var blendShapeProxy = vrmMeta.GetComponent<VRMBlendShapeProxy>();
            if (!blendShapeProxy) return;

            var blendShapeAvatar = blendShapeProxy.BlendShapeAvatar;
            if (!blendShapeAvatar) return;

            using (new ManualBundleGeneratedAssetsScope(Environment, manualBuild))
            {
                blendShapeAvatar = EditorUtil.ToEphemeralClone(blendShapeAvatar, b => new CustomCloneBlendShapeAvatar().Clone(b).mainAsset);
                blendShapeProxy.BlendShapeAvatar = blendShapeAvatar;

                foreach (var genericEmoteItem in Environment.GetContext<GenericEmoteItemContext>().GenericEmoteItems(GenericEmotePlatform.VRM0))
                {
                    var clip = genericEmoteItem.ToBlendShapeClip(Environment);
                    blendShapeAvatar.SetClip(clip.Key, clip);
                }
            }
        }

        class ManualBundleGeneratedAssetsScope : ManualBundleGeneratedAssetsScopeBase
        {
            public ManualBundleGeneratedAssetsScope(EmoteWizardEnvironment environment, bool manualBuild) : base(environment, manualBuild)
            {
            }

            protected override IEnumerable<UnityEngine.Object> CollectVolatileAssets(EmoteWizardEnvironment environment)
            {
                var vrmBlendShapeProxy = environment.AvatarRoot.GetComponent<VRMBlendShapeProxy>();
                yield return vrmBlendShapeProxy.BlendShapeAvatar;
                foreach (var clip in vrmBlendShapeProxy.BlendShapeAvatar.Clips) yield return clip;
            }
        }
    }
}
