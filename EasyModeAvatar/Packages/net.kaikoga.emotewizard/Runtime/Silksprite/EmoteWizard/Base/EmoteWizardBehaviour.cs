using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using UnityEngine;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class EmoteWizardBehaviour : MonoBehaviour
    {
        public IEmoteWizardEnvironment Environment => GetComponentsInParent<EmoteWizardRoot>(true).FirstOrDefault()?.ToEnv((this as IContextProvider)?.ToContext());

        public bool IsSetupMode
        {
            get
            {
                var setupContext = Environment.GetContext<SetupContext>();
                return setupContext != null && setupContext.IsSetupMode;
            }
        }
    }
}