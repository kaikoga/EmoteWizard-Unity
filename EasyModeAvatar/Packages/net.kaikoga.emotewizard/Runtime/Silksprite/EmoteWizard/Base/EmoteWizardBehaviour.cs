using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using UnityEngine;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class EmoteWizardBehaviour : MonoBehaviour
    {
        public EmoteWizardEnvironment CreateEnv()
        {
            return GetComponentsInParent<EmoteWizardRoot>(true).FirstOrDefault()?.ToEnv();
        }

        public bool IsSetupMode
        {
            get
            {
                var setupContext = CreateEnv().GetContext<SetupContext>();
                return setupContext != null && setupContext.IsSetupMode;
            }
        }
    }
}