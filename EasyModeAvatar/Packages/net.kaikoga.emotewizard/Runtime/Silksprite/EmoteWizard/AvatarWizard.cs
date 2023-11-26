using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class AvatarWizard : EmoteWizardBase
    {
        public override IBehaviourContext ToContext(EmoteWizardEnvironment env) => GetContext(env);
        public AvatarContext GetContext(EmoteWizardEnvironment env) => new AvatarContext(env, this);

    }
}