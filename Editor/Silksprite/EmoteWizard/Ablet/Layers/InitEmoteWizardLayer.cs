using Ablet.API;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.API.V1.Building;
using Ablet.Builtin;
using Silksprite.EmoteWizardSupport.Undoable;

#if ATIV_DETECTED_VRM0
using Silksprite.EmoteWizard.Platforms.VRM0.Extensions;
using VRM;
#endif

#if ATIV_DETECTED_VRM1
using Silksprite.EmoteWizard.Platforms.VRM1.Extensions;
using UniVRM10;
#endif

namespace Silksprite.EmoteWizard.Ablet.Layers
{
    [AbletLayer]
    class InitEmoteWizardPass : IAbletLayer
    {
        string IAbletDefinition.Id => "Silksprite.EmoteWizard.Init";
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
#if ATIV_DETECTED_VRM0
                    if (avatarRootTransform.TryGetComponent<VRMMeta>(out var meta))
                    {
                        meta.EnsureVRM0Components(avatarRootTransform, root, undoable);
                    }

#endif
#if ATIV_DETECTED_VRM1
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
