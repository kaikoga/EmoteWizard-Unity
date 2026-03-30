using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Silksprite.AdLib.Utils.VRM1;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Builder;
using Silksprite.EmoteWizard.Contexts.Ephemeral;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Platforms.VRM1.Extensions;
using Silksprite.EmoteWizard.Scopes;
using Silksprite.EmoteWizardSupport.Undoable;
using UniVRM10;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizard.Platforms.VRM1.Contexts.Builder
{
    public class VRM1AvatarBuilderContext : AvatarBuilderContextBase
    {
        [UsedImplicitly]
        public VRM1AvatarBuilderContext(EmoteWizardEnvironment environment) : base(environment) { }

        public override void CleanupAvatar()
        {
            // nop
        }

        public override void BuildAvatar(IUndoable undoable, bool manualBuild)
        {
            if (manualBuild) throw new InvalidOperationException("manual build is not supported");

            var vrm10Instance = Environment.AvatarRoot.GetComponent<Vrm10Instance>();
            if (!vrm10Instance) return;

            var vrm10Object = vrm10Instance.Vrm;
            if (!vrm10Object) return;

            using (new ManualBundleGeneratedAssetsScope(Environment, manualBuild))
            {
                vrm10Object = new CustomCloneVRM10Object().Clone(vrm10Object).mainAsset;
                vrm10Instance.Vrm = vrm10Object;

                var expression = vrm10Object.Expression;

                foreach (var genericEmoteItem in Environment.GetContext<GenericEmoteItemContext>().GenericEmoteItems(GenericEmotePlatform.VRM1))
                {
                    var clip = genericEmoteItem.ToVRM10Expression(Environment, out var expressionPreset);
                    expression.AddClip(expressionPreset, clip);
                }
            }
            
        }

        class ManualBundleGeneratedAssetsScope : ManualBundleGeneratedAssetsScopeBase
        {
            public ManualBundleGeneratedAssetsScope(EmoteWizardEnvironment environment, bool manualBuild) : base(environment, manualBuild)
            {
            }

            protected override IEnumerable<Object> CollectVolatileAssets(EmoteWizardEnvironment environment)
            {
                var vrm10Instance = environment.AvatarRoot.GetComponent<Vrm10Instance>();
                yield return vrm10Instance.Vrm;
                foreach (var clip in vrm10Instance.Vrm.Expression.Clips) yield return clip.Clip;
            }
        }
    }
}
