using UnityEngine;

namespace EmoteWizard.Base
{
    [RequireComponent(typeof(EmoteWizardRoot))]
    public abstract class EmoteWizardBase : MonoBehaviour
    {
        public EmoteWizardRoot EmoteWizardRoot => GetComponent<EmoteWizardRoot>();
        public ParametersWizard ParametersWizard => GetComponent<ParametersWizard>();

        public bool IsSetupMode
        {
            get
            {
                var setupWizard = GetComponent<SetupWizard>();
                return setupWizard != null && setupWizard.isSetupMode;
            }
        }
    }
}