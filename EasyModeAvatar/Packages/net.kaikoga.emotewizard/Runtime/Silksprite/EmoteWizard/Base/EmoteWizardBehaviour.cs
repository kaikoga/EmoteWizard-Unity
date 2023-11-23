using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using UnityEngine;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class EmoteWizardBehaviour : MonoBehaviour
    {
        public IEmoteWizardEnvironment Environment => GetComponentsInParent<EmoteWizardRoot>(true).FirstOrDefault();
        protected T GetWizard<T>() where T : EmoteWizardBase => Environment.GetWizard<T>();

        public bool IsSetupMode
        {
            get
            {
                var setupWizard = GetWizard<SetupWizard>();
                return setupWizard != null && setupWizard.isSetupMode;
            }
        }
    }
}