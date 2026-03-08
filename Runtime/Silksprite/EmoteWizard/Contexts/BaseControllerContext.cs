using JetBrains.Annotations;
using Silksprite.EmoteWizard.Configs;
using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Contexts
{
    public class BaseControllerContext : AnimatorControllerContextBase
    {
        [UsedImplicitly]
        public BaseControllerContext(EmoteWizardEnvironment env) : base(env) { }

        public BaseControllerContext(EmoteWizardEnvironment env, BaseControllerConfig config) : base(env, config) { }
    }
}