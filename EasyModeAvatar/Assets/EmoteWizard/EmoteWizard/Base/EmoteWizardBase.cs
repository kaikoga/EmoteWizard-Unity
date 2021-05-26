using UnityEngine;

namespace EmoteWizard.Base
{
    [RequireComponent(typeof(EmoteWizardRoot))]
    public abstract class EmoteWizardBase : MonoBehaviour
    {
        public EmoteWizardRoot EmoteWizardRoot => GetComponent<EmoteWizardRoot>();
        public ExpressionWizard ExpressionWizard => GetComponent<ExpressionWizard>();

        public bool IsSetupMode => GetComponent<SetupWizard>();
    }
}