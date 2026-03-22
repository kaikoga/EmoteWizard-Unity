using Ablet.API;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.API.V1.Building;
using Ablet.Builtin;

#if EW_VRCSDK3_AVATARS
using Silksprite.EmoteWizard.Platforms.VRChat.Extensions;
using VRC.SDK3.Avatars.Components;
#endif

namespace Silksprite.EmoteWizard.Ablet.Layers
{
    [AbletLayer]
    class PreEmoteWizardLayer : IAbletLayer
    {
        string IAbletDefinition.Id => "Silksprite.EmoteWizard.Pre";
        string IAbletDefinition.DisplayName => "EmoteWizard Pre";
        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<PruningPhase>();
        }

        AbletProcedure? IAbletLayer.ToProcedure(IBuildArgument argument)
        {
            if (!AbletSymbols.PreferAblet) return null;

            return AbletBuildProcedure.Create(context =>
            {
#if EW_VRCSDK3_AVATARS
                var avatarRootTransform = context.CurrentRootTransform;

                foreach (var root in avatarRootTransform.GetComponentsInChildren<EmoteWizardRoot>(true))
                {
                    if (context.CurrentRootObject.TryGetComponent<VRCAvatarDescriptor>(out var avatarDescriptor))
                    {
                        avatarDescriptor.DeleteVrcLayers(root);
                    }
                }
#endif
                
            });
        }
    }
}
