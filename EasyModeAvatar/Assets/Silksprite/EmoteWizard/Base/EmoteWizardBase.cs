using UnityEngine;

namespace Silksprite.EmoteWizard.Base
{
    [RequireComponent(typeof(EmoteWizardRoot))]
    public abstract class EmoteWizardBase : MonoBehaviour
    {
        public EmoteWizardRoot EmoteWizardRoot => GetComponent<EmoteWizardRoot>();
        protected T GetWizard<T>() where T : EmoteWizardBase => EmoteWizardRoot.GetComponent<T>();

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