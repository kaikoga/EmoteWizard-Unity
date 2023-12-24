using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class SetupWizard : EmoteWizardBase
    {
        public override IBehaviourContext ToContext(EmoteWizardEnvironment env) => new SetupContext(env, this);
    }
}