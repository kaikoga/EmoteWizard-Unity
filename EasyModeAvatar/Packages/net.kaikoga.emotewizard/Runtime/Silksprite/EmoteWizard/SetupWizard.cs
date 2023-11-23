using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class SetupWizard : EmoteWizardBase, ISetupWizardContext
    {
        public bool isSetupMode = true;

        public override void DisconnectOutputAssets()
        {
        }

        public override IBehaviourContext ToContext() => this;

        Component IBehaviourContext.Component => this;
    }
}