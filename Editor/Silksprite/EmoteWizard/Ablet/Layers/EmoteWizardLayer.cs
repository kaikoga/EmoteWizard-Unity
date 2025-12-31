using Ablet.API;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.API.V1.Building;
using Ablet.Builtin;

#if EW_VRCSDK3_AVATARS
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizardSupport.Undoable;
#endif

#if EW_UNIVRM_VRM0 || EW_UNIVRM_VRM1
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizardSupport.Undoable;
#endif

namespace Silksprite.EmoteWizard.Ablet.Layers
{
    [AbletLayer]
    class EmoteWizardLayer : IAbletLayer
    {
        public string Id => "net.kaikoga.emotewizard";
        public string DisplayName => "EmoteWizard";
        public void Configure(IDependencyConfigurator config)
        {
            config.AddDependency<GeneratingPhase>();
        }
        public AbletProcedure ToProcedure(IBuildArgument argument)
        {
            if (!AbletSymbols.PreferAblet) return null;

            return AbletBuildProcedure.Create((IBuildContext context) =>
            {
                foreach (var root in context.CurrentRootObject.GetComponentsInChildren<EmoteWizardRoot>(true))
                {
                    var env = root.ToEnv();
                    env.PersistGeneratedAssets = false;
                    env.AvatarRoot = context.CurrentRootTransform;
#if EW_VRCSDK3_AVATARS
                    env.BuildVrcAvatar(new EditorUndoable("Build Emote Wizard from Ablet"), false);
#endif
#if EW_UNIVRM_VRM0
                    env.BuildVrm0Avatar(new EditorUndoable("Build Emote Wizard from Ablet"), false);
#endif
#if EW_UNIVRM_VRM1
                    env.BuildVrm1Avatar(new EditorUndoable("Build Emote Wizard from Ablet"), false);
#endif
                }
                
            });
        }
    }
}
