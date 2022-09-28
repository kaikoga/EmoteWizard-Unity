using Silksprite.EmoteWizard.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class SetupWizard : EmoteWizardBase
    {
        public bool isSetupMode = true;

        public override void DisconnectOutputAssets()
        {
        }
    }
}