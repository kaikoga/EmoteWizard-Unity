using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using UnityEngine;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class EmoteWizardBehaviour : MonoBehaviour
    {
        public IEmoteWizardEnvironment Environment => GetComponentsInParent<EmoteWizardRoot>(true).FirstOrDefault()?.ToEnv((this as IContextProvider)?.ToContext());
        protected T GetContext<T>() where T : IBehaviourContext => Environment.GetContext<T>();

        public bool IsSetupMode
        {
            get
            {
                var setupWizard = GetContext<SetupWizard>();
                return setupWizard != null && setupWizard.isSetupMode;
            }
        }
    }
}