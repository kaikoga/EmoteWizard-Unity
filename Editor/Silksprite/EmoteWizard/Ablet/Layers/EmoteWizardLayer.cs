using System.Collections.Generic;
using Ablet.API;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.API.V1.Building;
using Ablet.Builtin;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Builder;
using Silksprite.EmoteWizardSupport.Undoable;

#if EW_VRCSDK3_AVATARS
using Silksprite.EmoteWizard.Platforms.VRChat.Contexts.Builder;
#endif

#if CVR_CCK_EXISTS || ADLIB_CVR_CCK_STUBBED
using Silksprite.EmoteWizard.Platforms.ChilloutVR.Contexts.Builder;
#endif

#if ATIV_DETECTED_VRM0
using Silksprite.EmoteWizard.Platforms.VRM0.Contexts.Builder;
#endif

#if ATIV_DETECTED_VRM1
using Silksprite.EmoteWizard.Platforms.VRM1.Contexts.Builder;
#endif

namespace Silksprite.EmoteWizard.Ablet.Layers
{
    [AbletLayer]
    class EmoteWizardLayer : IAbletLayer
    {
        public string Id => "Silksprite.EmoteWizard";
        public string DisplayName => "EmoteWizard";
        public void Configure(IDependencyConfigurator config)
        {
            config.AddDependency<GeneratingPhase>();
        }
        public AbletProcedure? ToProcedure(IBuildArgument argument)
        {
            if (!AbletSymbols.PreferAblet) return null;

            return AbletBuildProcedure.Create(context =>
            {
                var undoable = new EditorUndoable("Build Emote Wizard from Ablet");

                foreach (var root in context.CurrentRootObject.GetComponentsInChildren<EmoteWizardRoot>(true))
                {
                    var env = root.ToEnv();
                    env.PersistGeneratedAssets = false;
                    env.AvatarRoot = context.CurrentRootTransform;
                    foreach (var builder in GuessBuilder(env))
                    {
                        builder.BuildAvatar(undoable, false);
                    }
                }
            });

            static IEnumerable<AvatarBuilderContextBase> GuessBuilder(EmoteWizardEnvironment env)
            {
#if EW_VRCSDK3_AVATARS
                yield return env.GetContext<VRChatAvatarBuilderContext>();
#endif
#if CVR_CCK_EXISTS || ADLIB_CVR_CCK_STUBBED
                yield return env.GetContext<ChilloutVRAvatarBuilderContext>();
#endif
#if ATIV_DETECTED_VRM0
                yield return env.GetContext<VRM0AvatarBuilderContext>();
#endif
#if ATIV_DETECTED_VRM1
                yield return env.GetContext<VRM1AvatarBuilderContext>();
#endif
            }

        }
    }
}
