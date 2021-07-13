using UnityEngine;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class EmoteWizardBase : MonoBehaviour
    {
        public EmoteWizardRoot EmoteWizardRoot => GetComponentInParent<EmoteWizardRoot>();
        protected T GetWizard<T>() where T : EmoteWizardBase => EmoteWizardRoot.GetWizard<T>();

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