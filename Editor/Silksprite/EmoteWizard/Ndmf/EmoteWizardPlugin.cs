using System;
using nadena.dev.ndmf;
using Silksprite.EmoteWizard.Ndmf;
using Silksprite.EmoteWizard.Ndmf.Passes;
using UnityEngine;

#if EW_ABLET_SUPPORT
using Ablet.API;
#endif

[assembly: ExportsPlugin(typeof(EmoteWizardPlugin))]

namespace Silksprite.EmoteWizard.Ndmf
{
    // runs independently of NDMF platform
    [RunsOnAllPlatforms]
    class EmoteWizardPlugin : Plugin<EmoteWizardPlugin>
    {
        public override string QualifiedName => "net.kaikoga.emotewizard";
        public override string DisplayName => "Emote Wizard";

        protected override void OnUnhandledException(Exception e)
        {
            Debug.LogException(e);
        }

        protected override void Configure()
        {
#if EW_ABLET_SUPPORT
            if (AbletSymbols.PreferAblet) return;
#endif
            var platformInit = InPhase(BuildPhase.PlatformInit);
            platformInit.Run(InitEmoteWizardPass.Instance);
            var resolving = InPhase(BuildPhase.Resolving);
            resolving.BeforePlugin("nadena.dev.modular-avatar").Run(PreEmoteWizardPass.Instance);
            var generating = InPhase(BuildPhase.Generating);
            generating.Run(EmoteWizardPass.Instance);
            generating.Run(PostEmoteWizardPass.Instance);
        }
    }

}
