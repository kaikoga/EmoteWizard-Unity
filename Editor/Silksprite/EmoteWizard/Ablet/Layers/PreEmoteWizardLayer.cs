using Ablet.API;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.API.V1.Building;
using Ablet.Builtin;
using Silksprite.EmoteWizard.Extensions;

#if EW_VRCSDK3_AVATARS
using VRC.SDK3.Avatars.Components;
#endif

namespace Silksprite.EmoteWizard.Ablet.Layers
{
    [AbletLayer]
    class PreEmoteWizardLayer : IAbletLayer
    {
        string IAbletDefinition.Id => "net.kaikoga.emotewizard.pre";
        string IAbletDefinition.DisplayName => "EmoteWizard Pre";
        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<PruningPhase>();
        }

        IAbletProcedure IAbletLayer.ToProcedure(IBuildArgument argument)
        {
            if (!AbletSymbols.PreferAblet) return null;

            return new AbletBuildProcedure(context =>
            {
                var avatarRootTransform = context.CurrentRootTransform;

                foreach (var root in avatarRootTransform.GetComponentsInChildren<EmoteWizardRoot>(true))
                {
#if EW_VRCSDK3_AVATARS
                    if (context.CurrentRootObject.TryGetComponent<VRCAvatarDescriptor>(out var avatarDescriptor))
                    {
                        avatarDescriptor.DeleteVrcLayers(root);
                    }
#endif
                }
                
            });
        }
    }
}
