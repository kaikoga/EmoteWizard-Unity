using Ablet.API;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.API.V1.Building;
using Ablet.Builtin;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizardSupport.Undoable;

#if EW_UNIVRM_VRM0
using VRM;
#endif

#if EW_UNIVRM_VRM1
using UniVRM10;
#endif

namespace Silksprite.EmoteWizard.Ablet.Layers
{
    [AbletLayer]
    class InitEmoteWizardPass : IAbletLayer
    {
        string IAbletDefinition.Id => "net.kaikoga.emotewizard.init";
        string IAbletDefinition.DisplayName => "EmoteWizard Init";
        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<ImportingPhase>();
        }
        AbletProcedure IAbletLayer.ToProcedure(IBuildArgument argument)
        {
            if (!AbletSymbols.PreferAblet) return null;

            return AbletBuildProcedure.Create((IBuildContext context) =>
            {
                var undoable = new EditorUndoable("Prepare Emote Wizard from ndmf");

                var avatarRootTransform = context.CurrentRootTransform;

                foreach (var root in avatarRootTransform.GetComponentsInChildren<EmoteWizardRoot>(true))
                {
#if EW_UNIVRM_VRM0
                    if (avatarRootTransform.TryGetComponent<VRMMeta>(out var meta))
                    {
                        meta.EnsureVRM0Components(avatarRootTransform, root, undoable);
                    }

#endif
#if EW_UNIVRM_VRM1
                    if (avatarRootTransform.TryGetComponent<Vrm10Instance>(out var instance))
                    {
                        instance.EnsureVRM1Components(avatarRootTransform, root, undoable);
                    }
#endif
                }
            });
        }
    }
}
