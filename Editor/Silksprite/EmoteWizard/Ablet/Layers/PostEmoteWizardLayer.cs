using Ablet.API;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.API.V1.Building;
using Ablet.Builtin.Utils;
using Silksprite.EmoteWizard.Base;

namespace Silksprite.EmoteWizard.Ablet.Layers
{
    [AbletLayer]
    class PostEmoteWizardLayer : IAbletLayer
    {
        string IAbletDefinition.Id => "net.kaikoga.emotewizard.post";
        string IAbletDefinition.DisplayName => "EmoteWizard Post";
        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<AfterLayer<EmoteWizardLayer>>();
        }
        IAbletProcedure IAbletLayer.ToProcedure(IBuildArgument argument)
        {
            if (!AbletSymbols.PreferAblet) return null;

            return new AbletBuildProcedure(context =>
            {
                foreach (var ewComponent in context.CurrentRootObject.GetComponentsInChildren<EmoteWizardBehaviour>(true))
                {
                    UnityEngine.Object.DestroyImmediate(ewComponent);
                }
            });
        }
    }
}
